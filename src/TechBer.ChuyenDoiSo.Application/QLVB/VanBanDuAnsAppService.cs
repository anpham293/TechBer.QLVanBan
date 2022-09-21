using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QLVB.Exporting;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using TechBer.ChuyenDoiSo.Storage;
using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.QLVB
{
    [AbpAuthorize(AppPermissions.Pages_VanBanDuAns)]
    public class VanBanDuAnsAppService : ChuyenDoiSoAppServiceBase, IVanBanDuAnsAppService
    {
        private const int MaxFileBytes = 524288000; //500MB
        private readonly IRepository<VanBanDuAn> _vanBanDuAnRepository;
        private readonly IVanBanDuAnsExcelExporter _vanBanDuAnsExcelExporter;
        private readonly IRepository<DuAn, int> _lookup_duAnRepository;
        private readonly IRepository<QuyTrinhDuAnAssigned, long> _lookup_quyTrinhDuAnRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;


        public VanBanDuAnsAppService(IRepository<VanBanDuAn> vanBanDuAnRepository,
            IVanBanDuAnsExcelExporter vanBanDuAnsExcelExporter, IRepository<DuAn, int> lookup_duAnRepository,
            IRepository<QuyTrinhDuAnAssigned, long> lookup_quyTrinhDuAnRepository,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _vanBanDuAnRepository = vanBanDuAnRepository;
            _vanBanDuAnsExcelExporter = vanBanDuAnsExcelExporter;
            _lookup_duAnRepository = lookup_duAnRepository;
            _lookup_quyTrinhDuAnRepository = lookup_quyTrinhDuAnRepository;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<PagedResultDto<GetVanBanDuAnForViewDto>> GetAll(GetAllVanBanDuAnsInput input)
        {
            var filteredVanBanDuAns = _vanBanDuAnRepository.GetAll()
                .Include(e => e.DuAnFk)
                .Include(e => e.QuyTrinhDuAnAssignedFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.KyHieuVanBan.Contains(input.Filter) ||
                         e.FileVanBan.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.KyHieuVanBanFilter),
                    e => e.KyHieuVanBan == input.KyHieuVanBanFilter)
                .WhereIf(input.MinNgayBanHanhFilter != null, e => e.NgayBanHanh >= input.MinNgayBanHanhFilter)
                .WhereIf(input.MaxNgayBanHanhFilter != null, e => e.NgayBanHanh <= input.MaxNgayBanHanhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.FileVanBanFilter),
                    e => e.FileVanBan == input.FileVanBanFilter)
                .WhereIf(input.DuAnNameFilter.HasValue, e => e.DuAnId == input.DuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnNameFilter),
                    e => e.QuyTrinhDuAnAssignedId == Int32.Parse(input.QuyTrinhDuAnNameFilter));

            var pagedAndFilteredVanBanDuAns = filteredVanBanDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var vanBanDuAns = from o in pagedAndFilteredVanBanDuAns
                join o1 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_quyTrinhDuAnRepository.GetAll() on o.QuyTrinhDuAnAssignedId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetVanBanDuAnForViewDto()
                {
                    VanBanDuAn = new VanBanDuAnDto
                    {
                        Name = o.Name,
                        KyHieuVanBan = o.KyHieuVanBan,
                        NgayBanHanh = o.NgayBanHanh,
                        FileVanBan = (o.FileVanBan.IsNullOrEmpty()
                            ? o.FileVanBan
                            : JsonConvert.DeserializeObject<FileMauSerializeObj>(o.FileVanBan).FileName),
                        Id = o.Id,
                        ViTriLuuTru = o.ViTriLuuTru
                    },
                    DuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    QuyTrinhDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                };

            var totalCount = await filteredVanBanDuAns.CountAsync();

            return new PagedResultDto<GetVanBanDuAnForViewDto>(
                totalCount,
                await vanBanDuAns.ToListAsync()
            );
        }

        public async Task<GetVanBanDuAnForViewDto> GetVanBanDuAnForView(int id)
        {
            var vanBanDuAn = await _vanBanDuAnRepository.GetAsync(id);

            var output = new GetVanBanDuAnForViewDto {VanBanDuAn = ObjectMapper.Map<VanBanDuAnDto>(vanBanDuAn)};

            if (output.VanBanDuAn.DuAnId != null)
            {
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int) output.VanBanDuAn.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }

            if (output.VanBanDuAn.QuyTrinhDuAnId != null)
            {
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnRepository.FirstOrDefaultAsync((int) output.VanBanDuAn.QuyTrinhDuAnId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_VanBanDuAns_Edit)]
        public async Task<GetVanBanDuAnForEditOutput> GetVanBanDuAnForEdit(EntityDto input)
        {
            var vanBanDuAn = await _vanBanDuAnRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetVanBanDuAnForEditOutput
                {VanBanDuAn = ObjectMapper.Map<CreateOrEditVanBanDuAnDto>(vanBanDuAn)};

            if (output.VanBanDuAn.DuAnId != null)
            {
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int) output.VanBanDuAn.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }

            if (output.VanBanDuAn.QuyTrinhDuAnId != null)
            {
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnRepository.FirstOrDefaultAsync((int) output.VanBanDuAn.QuyTrinhDuAnId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditVanBanDuAnDto input)
        {
            try
            {
                if (input.Id == null)
                {
                    await Create(input);
                }
                else
                {
                    await Update(input);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_VanBanDuAns_Create)]
        protected virtual async Task Create(CreateOrEditVanBanDuAnDto input)
        {
            var vanBanDuAn = ObjectMapper.Map<VanBanDuAn>(input);

            vanBanDuAn.QuyTrinhDuAnAssignedId = input.QuyTrinhDuAnId;
            if (AbpSession.TenantId != null)
            {
                vanBanDuAn.TenantId = (int?) AbpSession.TenantId;
            }

            var fileMau = "";
            if (!string.IsNullOrEmpty(input.UploadedFileToken))
            {
                byte[] byteArray;
                var fileBytes = _tempFileCacheManager.GetFile(input.UploadedFileToken);

                if (fileBytes == null)
                {
                    return;
                }

                using (var stream = new MemoryStream(fileBytes))
                {
                    byteArray = stream.ToArray();
                }

                if (byteArray.Length > MaxFileBytes)
                {
                    return;
                }

                var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
                var fileMauObj = new FileMauSerializeObj()
                {
                    FileName = input.FileName,
                    Guid = storedFile.Id.ToString(),
                    ContentType = input.ContentType
                };

                await _binaryObjectManager.SaveAsync(storedFile);
                fileMau = JsonConvert.SerializeObject(fileMauObj);
            }

            vanBanDuAn.FileVanBan = fileMau;
            await _vanBanDuAnRepository.InsertAsync(vanBanDuAn);
        }

        [AbpAuthorize(AppPermissions.Pages_VanBanDuAns_Edit)]
        protected virtual async Task Update(CreateOrEditVanBanDuAnDto input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input.UploadedFileToken))
                {
                    byte[] byteArray;
                    var fileBytes = _tempFileCacheManager.GetFile(input.UploadedFileToken);

                    if (fileBytes == null)
                    {
                        return;
                    }

                    using (var stream = new MemoryStream(fileBytes))
                    {
                        byteArray = stream.ToArray();
                    }

                    if (byteArray.Length > MaxFileBytes)
                    {
                        return;
                    }

                    var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
                    var fileMauObj = new FileMauSerializeObj
                    {
                        FileName = input.FileName,
                        Guid = storedFile.Id.ToString(),
                        ContentType = input.ContentType
                    };


                    var fileMau = JsonConvert.SerializeObject(fileMauObj);
                    var vanban = await _vanBanDuAnRepository.FirstOrDefaultAsync((int) input.Id);
                    if (!string.IsNullOrWhiteSpace(vanban.FileVanBan))
                    {
                        var oldFileMau = JsonConvert.DeserializeObject<FileMauSerializeObj>(vanban.FileVanBan);
                        await _binaryObjectManager.DeleteAsync(Guid.Parse(oldFileMau.Guid));
                        await _binaryObjectManager.SaveAsync(storedFile);
                    }
                    else
                    {
                        await _binaryObjectManager.SaveAsync(storedFile);
                    }

                    ObjectMapper.Map(input, vanban);
                    vanban.FileVanBan = fileMau;
                }
                else
                {
                    var vanBanDuAn = await _vanBanDuAnRepository.FirstOrDefaultAsync((int) input.Id);
                    vanBanDuAn.Name = input.Name;
                    vanBanDuAn.KyHieuVanBan = input.KyHieuVanBan;
                    vanBanDuAn.NgayBanHanh = input.NgayBanHanh;
                    vanBanDuAn.ViTriLuuTru = input.ViTriLuuTru;
                    await _vanBanDuAnRepository.UpdateAsync(vanBanDuAn);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_VanBanDuAns_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _vanBanDuAnRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetVanBanDuAnsToExcel(GetAllVanBanDuAnsForExcelInput input)
        {
            var filteredVanBanDuAns = _vanBanDuAnRepository.GetAll()
                .Include(e => e.DuAnFk)
                .Include(e => e.QuyTrinhDuAnAssignedFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.KyHieuVanBan.Contains(input.Filter) ||
                         e.FileVanBan.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.KyHieuVanBanFilter),
                    e => e.KyHieuVanBan == input.KyHieuVanBanFilter)
                .WhereIf(input.MinNgayBanHanhFilter != null, e => e.NgayBanHanh >= input.MinNgayBanHanhFilter)
                .WhereIf(input.MaxNgayBanHanhFilter != null, e => e.NgayBanHanh <= input.MaxNgayBanHanhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.FileVanBanFilter),
                    e => e.FileVanBan == input.FileVanBanFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter),
                    e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnNameFilter),
                    e => e.QuyTrinhDuAnAssignedFk != null &&
                         e.QuyTrinhDuAnAssignedFk.Name == input.QuyTrinhDuAnNameFilter);

            var query = (from o in filteredVanBanDuAns
                join o1 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_quyTrinhDuAnRepository.GetAll() on o.QuyTrinhDuAnAssignedId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetVanBanDuAnForViewDto()
                {
                    VanBanDuAn = new VanBanDuAnDto
                    {
                        Name = o.Name,
                        KyHieuVanBan = o.KyHieuVanBan,
                        NgayBanHanh = o.NgayBanHanh,
                        FileVanBan = o.FileVanBan,
                        Id = o.Id
                    },
                    DuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    QuyTrinhDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                });


            var vanBanDuAnListDtos = await query.ToListAsync();

            return _vanBanDuAnsExcelExporter.ExportToFile(vanBanDuAnListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_VanBanDuAns)]
        public async Task<PagedResultDto<VanBanDuAnDuAnLookupTableDto>> GetAllDuAnForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_duAnRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var duAnList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<VanBanDuAnDuAnLookupTableDto>();
            foreach (var duAn in duAnList)
            {
                lookupTableDtoList.Add(new VanBanDuAnDuAnLookupTableDto
                {
                    Id = duAn.Id,
                    DisplayName = duAn.Name?.ToString()
                });
            }

            return new PagedResultDto<VanBanDuAnDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_VanBanDuAns)]
        public async Task<PagedResultDto<VanBanDuAnQuyTrinhDuAnLookupTableDto>> GetAllQuyTrinhDuAnForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_quyTrinhDuAnRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var quyTrinhDuAnList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<VanBanDuAnQuyTrinhDuAnLookupTableDto>();
            foreach (var quyTrinhDuAn in quyTrinhDuAnList)
            {
                lookupTableDtoList.Add(new VanBanDuAnQuyTrinhDuAnLookupTableDto
                {
                    Id = quyTrinhDuAn.Id,
                    DisplayName = quyTrinhDuAn.Name?.ToString()
                });
            }

            return new PagedResultDto<VanBanDuAnQuyTrinhDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}