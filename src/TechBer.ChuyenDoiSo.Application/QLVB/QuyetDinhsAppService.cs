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

namespace TechBer.ChuyenDoiSo.QLVB
{
    [AbpAuthorize(AppPermissions.Pages_QuyetDinhs)]
    public class QuyetDinhsAppService : ChuyenDoiSoAppServiceBase, IQuyetDinhsAppService
    {
        private const int MaxFileBytes = 524288000; //500MB
        private readonly IRepository<QuyetDinh> _quyetDinhRepository;
        private readonly IQuyetDinhsExcelExporter _quyetDinhsExcelExporter;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public QuyetDinhsAppService(IRepository<QuyetDinh> quyetDinhRepository,
            ITempFileCacheManager tempFileCacheManager,
            IQuyetDinhsExcelExporter quyetDinhsExcelExporter,
            IBinaryObjectManager binaryObjectManager)
        {
            _quyetDinhRepository = quyetDinhRepository;
            _quyetDinhsExcelExporter = quyetDinhsExcelExporter;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
        }

        public async Task<PagedResultDto<GetQuyetDinhForViewDto>> GetAll(GetAllQuyetDinhsInput input)
        {
            var filteredQuyetDinhs = _quyetDinhRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.So.Contains(input.Filter) || e.Ten.Contains(input.Filter) ||
                         e.FileQuyetDinh.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SoFilter), e => e.So == input.SoFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(input.MinNgayBanHanhFilter != null, e => e.NgayBanHanh >= input.MinNgayBanHanhFilter)
                .WhereIf(input.MaxNgayBanHanhFilter != null, e => e.NgayBanHanh <= input.MaxNgayBanHanhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.FileQuyetDinhFilter),
                    e => e.FileQuyetDinh == input.FileQuyetDinhFilter)
                .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

            var pagedAndFilteredQuyetDinhs = filteredQuyetDinhs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var quyetDinhs = from o in pagedAndFilteredQuyetDinhs
                select new GetQuyetDinhForViewDto()
                {
                    QuyetDinh = new QuyetDinhDto
                    {
                        So = o.So,
                        Ten = o.Ten,
                        NgayBanHanh = o.NgayBanHanh,
                        FileQuyetDinh = o.FileQuyetDinh,
                        TrangThai = o.TrangThai,
                        Id = o.Id
                    }
                };

            var totalCount = await filteredQuyetDinhs.CountAsync();

            return new PagedResultDto<GetQuyetDinhForViewDto>(
                totalCount,
                await quyetDinhs.ToListAsync()
            );
        }

        public async Task<GetQuyetDinhForViewDto> GetQuyetDinhForView(int id)
        {
            var quyetDinh = await _quyetDinhRepository.GetAsync(id);

            var output = new GetQuyetDinhForViewDto {QuyetDinh = ObjectMapper.Map<QuyetDinhDto>(quyetDinh)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Edit)]
        public async Task<GetQuyetDinhForEditOutput> GetQuyetDinhForEdit(EntityDto input)
        {
            var quyetDinh = await _quyetDinhRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetQuyetDinhForEditOutput
                {QuyetDinh = ObjectMapper.Map<CreateOrEditQuyetDinhDto>(quyetDinh)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditQuyetDinhDto input)
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

        [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Create)]
        protected virtual async Task Create(CreateOrEditQuyetDinhDto input)
        {
            var quyetDinh = ObjectMapper.Map<QuyetDinh>(input);

            if (AbpSession.TenantId != null)
            {
                quyetDinh.TenantId = (int?) AbpSession.TenantId;
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
            quyetDinh.FileQuyetDinh= fileMau;
            await _quyetDinhRepository.InsertAsync(quyetDinh);
        }

        [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Edit)]
        protected virtual async Task Update(CreateOrEditQuyetDinhDto input)
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
                    var quyeDinh = await _quyetDinhRepository.FirstOrDefaultAsync((int) input.Id);
                    if (!string.IsNullOrWhiteSpace(quyeDinh.FileQuyetDinh))
                    {
                        var oldFileMau = JsonConvert.DeserializeObject<FileMauSerializeObj>(quyeDinh.FileQuyetDinh);
                        await _binaryObjectManager.DeleteAsync(Guid.Parse(oldFileMau.Guid));
                        await _binaryObjectManager.SaveAsync(storedFile);
                    }
                    else
                    {
                        await _binaryObjectManager.SaveAsync(storedFile);
                    }

                    ObjectMapper.Map(input, quyeDinh);
                    quyeDinh.FileQuyetDinh = fileMau;
                }
                else
                {
                    var quyetDinh = await _quyetDinhRepository.FirstOrDefaultAsync((int) input.Id);
                    quyetDinh.So = input.So;
                    quyetDinh.Ten = input.Ten;
                    quyetDinh.NgayBanHanh = input.NgayBanHanh;
                    quyetDinh.TrangThai = input.TrangThai;
                    await _quyetDinhRepository.UpdateAsync(quyetDinh);

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _quyetDinhRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetQuyetDinhsToExcel(GetAllQuyetDinhsForExcelInput input)
        {
            var filteredQuyetDinhs = _quyetDinhRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.So.Contains(input.Filter) || e.Ten.Contains(input.Filter) ||
                         e.FileQuyetDinh.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.SoFilter), e => e.So == input.SoFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(input.MinNgayBanHanhFilter != null, e => e.NgayBanHanh >= input.MinNgayBanHanhFilter)
                .WhereIf(input.MaxNgayBanHanhFilter != null, e => e.NgayBanHanh <= input.MaxNgayBanHanhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.FileQuyetDinhFilter),
                    e => e.FileQuyetDinh == input.FileQuyetDinhFilter)
                .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

            var query = (from o in filteredQuyetDinhs
                select new GetQuyetDinhForViewDto()
                {
                    QuyetDinh = new QuyetDinhDto
                    {
                        So = o.So,
                        Ten = o.Ten,
                        NgayBanHanh = o.NgayBanHanh,
                        FileQuyetDinh = o.FileQuyetDinh,
                        TrangThai = o.TrangThai,
                        Id = o.Id
                    }
                });


            var quyetDinhListDtos = await query.ToListAsync();

            return _quyetDinhsExcelExporter.ExportToFile(quyetDinhListDtos);
        }
    }
}