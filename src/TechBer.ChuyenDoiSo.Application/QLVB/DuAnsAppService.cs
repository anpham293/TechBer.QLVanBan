using TechBer.ChuyenDoiSo.QLVB;
using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QLVB.Exporting;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;

namespace TechBer.ChuyenDoiSo.QLVB
{
    [AbpAuthorize(AppPermissions.Pages_DuAns)]
    public class DuAnsAppService : ChuyenDoiSoAppServiceBase, IDuAnsAppService
    {
        private readonly IRepository<DuAn> _duAnRepository;
        private readonly IDuAnsExcelExporter _duAnsExcelExporter;
        private readonly IRepository<LoaiDuAn, int> _lookup_loaiDuAnRepository;
        private readonly IRepository<Chuong, int> _lookup_chuongRepository;
        private readonly IRepository<LoaiKhoan, int> _lookup_loaiKhoanRepository;
        private readonly IRepository<QuyTrinhDuAnAssigned, long> _quyTrinhDuAnAssignedRepository;
        private readonly IRepository<QuyTrinhDuAn, int> _quyTrinhDuAnRepository;
        private readonly IRepository<VanBanDuAn, int> _vanBanRepository;
        private readonly IRepository<UserOrganizationUnit, long> _userOrganizationUnitRepository;


        public DuAnsAppService(IRepository<DuAn> duAnRepository, IDuAnsExcelExporter duAnsExcelExporter,
            IRepository<LoaiDuAn, int> lookup_loaiDuAnRepository,
            IRepository<Chuong, int> lookup_chuongRepository,
            IRepository<LoaiKhoan, int> lookup_loaiKhoanRepository,
            IRepository<QuyTrinhDuAnAssigned, long> quyTrinhDuAnAssignedRepository,
            IRepository<QuyTrinhDuAn, int> quyTrinhDuAnRepository,
            IRepository<VanBanDuAn, int> vanBanRepository,
            IRepository<UserOrganizationUnit, long> userOrganizationUnitRepository
        )
        {
            _duAnRepository = duAnRepository;
            _duAnsExcelExporter = duAnsExcelExporter;
            _lookup_loaiDuAnRepository = lookup_loaiDuAnRepository;
            _lookup_chuongRepository = lookup_chuongRepository;
            _lookup_loaiKhoanRepository = lookup_loaiKhoanRepository;
            _quyTrinhDuAnAssignedRepository = quyTrinhDuAnAssignedRepository;
            _quyTrinhDuAnRepository = quyTrinhDuAnRepository;
            _userOrganizationUnitRepository = userOrganizationUnitRepository;
        }

        public async Task<PagedResultDto<GetDuAnForViewDto>> GetAll(GetAllDuAnsInput input)
        {
            var userId = AbpSession.UserId;
            var listUserOrganizationUnit =  _userOrganizationUnitRepository.GetAll()
                .WhereIf(true, p => p.UserId == userId)
                .Select(uou => uou.OrganizationUnitId);
            var filteredDuAns = _duAnRepository.GetAll()
                    .Include(e => e.LoaiDuAnFk)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                        e => false || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
                    .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),
                        e => e.Descriptions == input.DescriptionsFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter),
                        e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter)
                ;
            var listDuAnLoaiDuAnOrganizationUnit = await (from o in filteredDuAns
                join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetDuAnForViewDto()
                {
                    DuAn = new DuAnDto
                    {
                        Name = o.Name,
                        Descriptions = o.Descriptions,
                        Id = o.Id
                    },
                    LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    LoaiDuAn = new LoaiDuAnDto()
                    {
                        Id = s1.Id,
                        Name = s1.Name,
                        OrganizationUnitId = s1.OrganizationUnitId
                    }
                }).ToListAsync();
            // foreach (var VARIABLE in listUserOrganizationUnit)
            // {
            //     foreach (var VARIABLE1 in listDuAnLoaiDuAnOrganizationUnit)
            //     {
            //         if (VARIABLE1.LoaiDuAn.OrganizationUnitId == VARIABLE.OrganizationUnitId)
            //         {
            //             VARIABLE1.UserId = (long)userId;
            //         }
            //     }
            // }

            



            

            // var pagedAndFilteredDuAns = filteredDuAns
            //     .OrderBy(input.Sorting ?? "id asc")
            //     .PageBy(input);

            // var duAns = from o in pagedAndFilteredDuAns
            //     join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
            //     from s1 in j1.DefaultIfEmpty()
            //     select new GetDuAnForViewDto()
            //     {
            //         DuAn = new DuAnDto
            //         {
            //             Name = o.Name,
            //             Descriptions = o.Descriptions,
            //             Id = o.Id
            //         },
            //         LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
            //     };

            
            var duAns =  listDuAnLoaiDuAnOrganizationUnit.WhereIf(true, e => listUserOrganizationUnit.Contains((long)e.LoaiDuAn.OrganizationUnitId));

            var totalCount = await filteredDuAns.CountAsync();

            return new PagedResultDto<GetDuAnForViewDto>(
                totalCount,
                 duAns.ToList()
            );
        }

        public async Task<GetDuAnForViewDto> GetDuAnForView(int id)
        {
            var duAn = await _duAnRepository.GetAsync(id);

            var output = new GetDuAnForViewDto {DuAn = ObjectMapper.Map<DuAnDto>(duAn)};

            if (output.DuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn =
                    await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int) output.DuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DuAns_Edit)]
        public async Task<GetDuAnForEditOutput> GetDuAnForEdit(EntityDto input)
        {
            var duAn = await _duAnRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDuAnForEditOutput {DuAn = ObjectMapper.Map<CreateOrEditDuAnDto>(duAn)};

            if (output.DuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn =
                    await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int) output.DuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }
            if (output.DuAn.ChuongId != null)
            {
                var _lookupChuong =
                    await _lookup_chuongRepository.FirstOrDefaultAsync((int) output.DuAn.ChuongId);
                    output.ChuongName = _lookupChuong?.MaSo + " - " + _lookupChuong?.Ten;
            }
            if (output.DuAn.LoaiKhoanId != null)
            {
                var _lookupLoaiKhoan =
                    await _lookup_loaiKhoanRepository.FirstOrDefaultAsync((int) output.DuAn.LoaiKhoanId);
                output.LoaiKhoanName = _lookupLoaiKhoan?.MaSo + " - " + _lookupLoaiKhoan?.Ten;
            }
            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDuAnDto input)
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
        
        [AbpAuthorize(AppPermissions.Pages_DuAns_Create)]
        protected virtual async Task Create(CreateOrEditDuAnDto input)
        {
            var duAn = ObjectMapper.Map<DuAn>(input);


            if (AbpSession.TenantId != null)
            {
                duAn.TenantId = (int?) AbpSession.TenantId;
            }

            int duanid = await _duAnRepository.InsertAndGetIdAsync(duAn);

            LoaiDuAn loaiDuAn = await _lookup_loaiDuAnRepository.FirstOrDefaultAsync(duAn.LoaiDuAnId.Value);

            if (!loaiDuAn.IsNullOrDeleted())
            {
                List<QuyTrinhDuAn> quyTrinhDuAns =
                    await _quyTrinhDuAnRepository.GetAllListAsync(p => p.LoaiDuAnId == loaiDuAn.Id);
                
                
                foreach (QuyTrinhDuAn quyTrinhDuAn in quyTrinhDuAns)
                {
                    CreateOrEditQuyTrinhDuAnAssignedDto createOrEditQuyTrinhDuAnAssignedDto =
                        new CreateOrEditQuyTrinhDuAnAssignedDto()
                        {

                            Descriptions = quyTrinhDuAn.Descriptions,
                            Name = quyTrinhDuAn.Name,
                            ParentId = null,
                            MaQuyTrinh = quyTrinhDuAn.MaQuyTrinh,
                            STT = quyTrinhDuAn.STT,
                            LoaiDuAnId = quyTrinhDuAn.LoaiDuAnId,
                            QuyTrinhDuAnId = quyTrinhDuAn.Id,
                            DuAnId = duanid,
                            SoVanBanQuyDinh = 0
                        };
                    await _quyTrinhDuAnAssignedRepository.InsertAsync(ObjectMapper.Map<QuyTrinhDuAnAssigned>(createOrEditQuyTrinhDuAnAssignedDto));
                }

                await CurrentUnitOfWork.SaveChangesAsync();
                List<QuyTrinhDuAnAssigned> quyTrinhDuAnAssigneds = await 
                    _quyTrinhDuAnAssignedRepository.GetAllListAsync(p => p.DuAnId == duanid);
                foreach (var VARIABLE in quyTrinhDuAnAssigneds)
                {
                    QuyTrinhDuAn quyTrinhDuAn =
                        await _quyTrinhDuAnRepository.FirstOrDefaultAsync(VARIABLE.QuyTrinhDuAnId.Value);
                    
                    if (quyTrinhDuAn.ParentId.HasValue)
                    {
                        var variable_update = await _quyTrinhDuAnAssignedRepository.FirstOrDefaultAsync(VARIABLE.Id);
                        QuyTrinhDuAnAssigned findParent =
                            quyTrinhDuAnAssigneds.FirstOrDefault(p => p.QuyTrinhDuAnId == quyTrinhDuAn.ParentId);
                        if (!findParent.IsNullOrDeleted())
                        {
                            variable_update.ParentId = findParent.Id;
                        }
                        else
                        {
                            variable_update.ParentId = null;
                        }

                        await _quyTrinhDuAnAssignedRepository.UpdateAsync(variable_update);
                    }
                }
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DuAns_Edit)]
        protected virtual async Task Update(CreateOrEditDuAnDto input)
        {
            var duAn = await _duAnRepository.FirstOrDefaultAsync((int) input.Id);
            duAn.Descriptions = input.Descriptions;
            duAn.Name = input.Name;
            duAn.ChuongId = input.ChuongId;
            duAn.LoaiKhoanId = input.LoaiKhoanId;
            duAn.MaDVQHNS = input.MaDVQHNS;
            duAn.NgayBatDau = input.NgayBatDau;
            duAn.NgayKetThuc = input.NgayKetThuc;
            await _duAnRepository.UpdateAsync(duAn);
        }

        [AbpAuthorize(AppPermissions.Pages_DuAns_Delete)]
        public async Task Delete(EntityDto input)
        {
            List<long> idlist = await _quyTrinhDuAnAssignedRepository.GetAll().WhereIf(true, p => p.DuAnId == input.Id)
                .Select(p => p.Id).ToListAsync();
            await _vanBanRepository.DeleteAsync(p => idlist.Contains(p.QuyTrinhDuAnAssignedId.Value));
            await _quyTrinhDuAnAssignedRepository.DeleteAsync(p => idlist.Contains(p.Id));
            await _duAnRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDuAnsToExcel(GetAllDuAnsForExcelInput input)
        {
            var filteredDuAns = _duAnRepository.GetAll()
                .Include(e => e.LoaiDuAnFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),
                    e => e.Descriptions == input.DescriptionsFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter),
                    e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter);

            var query = (from o in filteredDuAns
                join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                select new GetDuAnForViewDto()
                {
                    DuAn = new DuAnDto
                    {
                        Name = o.Name,
                        Descriptions = o.Descriptions,
                        Id = o.Id
                    },
                    LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
                });


            var duAnListDtos = await query.ToListAsync();

            return _duAnsExcelExporter.ExportToFile(duAnListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_DuAns)]
        public async Task<PagedResultDto<DuAnLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_loaiDuAnRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var loaiDuAnList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DuAnLoaiDuAnLookupTableDto>();
            foreach (var loaiDuAn in loaiDuAnList)
            {
                lookupTableDtoList.Add(new DuAnLoaiDuAnLookupTableDto
                {
                    Id = loaiDuAn.Id,
                    DisplayName = loaiDuAn.Name?.ToString()
                });
            }

            return new PagedResultDto<DuAnLoaiDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        
        [AbpAuthorize(AppPermissions.Pages_DuAns)]
        public async Task<PagedResultDto<DuAnChuongLookupTableDto>> GetAllChuongForLookupTable(
            ChuongLookupTableInput input)
        {
            var query = _lookup_chuongRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Ten != null && e.Ten.Contains(input.Filter)
                    || e.MaSo != null && e.MaSo.Contains(input.Filter))
                .WhereIf(true, e => e.CapQuanLyId == input.CapQuanLyFilterId)
                ;

            var totalCount = await query.CountAsync();

            var chuongList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DuAnChuongLookupTableDto>();
            foreach (var chuong in chuongList)
            {
                lookupTableDtoList.Add(new DuAnChuongLookupTableDto
                {
                    Id = chuong.Id,
                    MaSo = chuong.MaSo,
                    Ten = chuong.Ten,
                    CapQuanLyId = (int)chuong.CapQuanLyId
                });
            }

            return new PagedResultDto<DuAnChuongLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
        [AbpAuthorize(AppPermissions.Pages_DuAns)]
        public async Task<PagedResultDto<DuAnLoaiKhoanLookupTableDto>> GetAllLoaiKhoanForLookupTable(
            LoaiKhoanLookupTableInput input)
        {
            var query = _lookup_loaiKhoanRepository.GetAll()
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => e.Ten != null && e.Ten.Contains(input.Filter)
                                                                            || e.MaSo != null && e.MaSo.Contains(input.Filter))
                ;

            var totalCount = await query.CountAsync();

            var loaiKhoanList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<DuAnLoaiKhoanLookupTableDto>();
            foreach (var loaiKhoan in loaiKhoanList)
            {
                lookupTableDtoList.Add(new DuAnLoaiKhoanLookupTableDto
                {
                    Id = loaiKhoan.Id,
                    MaSo = loaiKhoan.MaSo,
                    Ten = loaiKhoan.Ten
                });
            }

            return new PagedResultDto<DuAnLoaiKhoanLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }
    }
}