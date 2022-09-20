using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB;
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
using Abp.UI;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using Z.EntityFramework.Plus;

namespace TechBer.ChuyenDoiSo.QLVB
{
    [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
    public class QuyTrinhDuAnAssignedsAppService : ChuyenDoiSoAppServiceBase, IQuyTrinhDuAnAssignedsAppService
    {
        private readonly IRepository<QuyTrinhDuAnAssigned, long> _quyTrinhDuAnAssignedRepository;
        private readonly IQuyTrinhDuAnAssignedsExcelExporter _quyTrinhDuAnAssignedsExcelExporter;
        private readonly IRepository<LoaiDuAn, int> _lookup_loaiDuAnRepository;
        private readonly IRepository<QuyTrinhDuAn, int> _lookup_quyTrinhDuAnRepository;
        private readonly IRepository<QuyTrinhDuAnAssigned, long> _quyTrinhDuAnRepository;
        private readonly IRepository<DuAn, int> _lookup_duAnRepository;


        public QuyTrinhDuAnAssignedsAppService(IRepository<QuyTrinhDuAnAssigned, long> quyTrinhDuAnAssignedRepository,
            IQuyTrinhDuAnAssignedsExcelExporter quyTrinhDuAnAssignedsExcelExporter,
            IRepository<LoaiDuAn, int> lookup_loaiDuAnRepository,
            IRepository<QuyTrinhDuAn, int> lookup_quyTrinhDuAnRepository,
            IRepository<QuyTrinhDuAnAssigned, long> lookup_quyTrinhDuAnAssignedRepository,
            IRepository<DuAn, int> lookup_duAnRepository)
        {
            _quyTrinhDuAnAssignedRepository = quyTrinhDuAnAssignedRepository;
            _quyTrinhDuAnAssignedsExcelExporter = quyTrinhDuAnAssignedsExcelExporter;
            _lookup_loaiDuAnRepository = lookup_loaiDuAnRepository;
            _lookup_quyTrinhDuAnRepository = lookup_quyTrinhDuAnRepository;
            _quyTrinhDuAnRepository = lookup_quyTrinhDuAnAssignedRepository;
            _lookup_duAnRepository = lookup_duAnRepository;
        }

        public async Task<PagedResultDto<GetQuyTrinhDuAnAssignedForViewDto>> GetAll(
            GetAllQuyTrinhDuAnAssignedsInput input)
        {
            var filteredQuyTrinhDuAnAssigneds = _quyTrinhDuAnAssignedRepository.GetAll()
                .Include(e => e.LoaiDuAnFk)
                .Include(e => e.QuyTrinhDuAnFk)
                .Include(e => e.ParentFk)
                .Include(e => e.DuAnFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter) ||
                         e.MaQuyTrinh.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),
                    e => e.Descriptions == input.DescriptionsFilter)
                .WhereIf(input.MinSTTFilter != null, e => e.STT >= input.MinSTTFilter)
                .WhereIf(input.MaxSTTFilter != null, e => e.STT <= input.MaxSTTFilter)
                .WhereIf(input.MinSoVanBanQuyDinhFilter != null,
                    e => e.SoVanBanQuyDinh >= input.MinSoVanBanQuyDinhFilter)
                .WhereIf(input.MaxSoVanBanQuyDinhFilter != null,
                    e => e.SoVanBanQuyDinh <= input.MaxSoVanBanQuyDinhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaQuyTrinhFilter),
                    e => e.MaQuyTrinh == input.MaQuyTrinhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter),
                    e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnNameFilter),
                    e => e.QuyTrinhDuAnFk != null && e.QuyTrinhDuAnFk.Name == input.QuyTrinhDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnAssignedNameFilter),
                    e => e.ParentFk != null && e.ParentFk.Name == input.QuyTrinhDuAnAssignedNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter),
                    e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter);

            var pagedAndFilteredQuyTrinhDuAnAssigneds = filteredQuyTrinhDuAnAssigneds
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var quyTrinhDuAnAssigneds = from o in pagedAndFilteredQuyTrinhDuAnAssigneds
                join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_quyTrinhDuAnRepository.GetAll() on o.QuyTrinhDuAnId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _quyTrinhDuAnRepository.GetAll() on o.ParentId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o4.Id into j4
                from s4 in j4.DefaultIfEmpty()
                select new GetQuyTrinhDuAnAssignedForViewDto()
                {
                    QuyTrinhDuAnAssigned = new QuyTrinhDuAnAssignedDto
                    {
                        Name = o.Name,
                        Descriptions = o.Descriptions,
                        STT = o.STT,
                        SoVanBanQuyDinh = o.SoVanBanQuyDinh,
                        MaQuyTrinh = o.MaQuyTrinh,
                        Id = o.Id
                    },
                    LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    QuyTrinhDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                    QuyTrinhDuAnAssignedName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                    DuAnName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                };

            var totalCount = await filteredQuyTrinhDuAnAssigneds.CountAsync();

            return new PagedResultDto<GetQuyTrinhDuAnAssignedForViewDto>(
                totalCount,
                await quyTrinhDuAnAssigneds.ToListAsync()
            );
        }

        public async Task<GetQuyTrinhDuAnAssignedForViewDto> GetQuyTrinhDuAnAssignedForView(long id)
        {
            var quyTrinhDuAnAssigned = await _quyTrinhDuAnAssignedRepository.GetAsync(id);

            var output = new GetQuyTrinhDuAnAssignedForViewDto
                {QuyTrinhDuAnAssigned = ObjectMapper.Map<QuyTrinhDuAnAssignedDto>(quyTrinhDuAnAssigned)};

            if (output.QuyTrinhDuAnAssigned.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn =
                    await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAnAssigned.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }

            if (output.QuyTrinhDuAnAssigned.QuyTrinhDuAnId != null)
            {
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAnAssigned
                        .QuyTrinhDuAnId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
            }

            if (output.QuyTrinhDuAnAssigned.ParentId != null)
            {
                var _lookupQuyTrinhDuAnAssigned =
                    await _quyTrinhDuAnRepository.FirstOrDefaultAsync(
                        (long) output.QuyTrinhDuAnAssigned.ParentId);
                output.QuyTrinhDuAnAssignedName = _lookupQuyTrinhDuAnAssigned?.Name?.ToString();
            }

            if (output.QuyTrinhDuAnAssigned.DuAnId != null)
            {
                var _lookupDuAn =
                    await _lookup_duAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAnAssigned.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public async Task<GetQuyTrinhDuAnAssignedForEditOutput> GetQuyTrinhDuAnAssignedForEdit(EntityDto<long> input)
        {
            var quyTrinhDuAnAssigned = await _quyTrinhDuAnAssignedRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetQuyTrinhDuAnAssignedForEditOutput
                {QuyTrinhDuAnAssigned = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnAssignedDto>(quyTrinhDuAnAssigned)};

            if (output.QuyTrinhDuAnAssigned.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn =
                    await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAnAssigned.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }

            if (output.QuyTrinhDuAnAssigned.QuyTrinhDuAnId != null)
            {
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAnAssigned
                        .QuyTrinhDuAnId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
            }

            if (output.QuyTrinhDuAnAssigned.ParentId != null)
            {
                var _lookupQuyTrinhDuAnAssigned =
                    await _quyTrinhDuAnRepository.FirstOrDefaultAsync(
                        (long) output.QuyTrinhDuAnAssigned.ParentId);
                output.QuyTrinhDuAnAssignedName = _lookupQuyTrinhDuAnAssigned?.Name?.ToString();
            }

            if (output.QuyTrinhDuAnAssigned.DuAnId != null)
            {
                var _lookupDuAn =
                    await _lookup_duAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAnAssigned.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }

            return output;
        }

        public async Task<GetQuyTrinhDuAnAssignedForEditOutput> CreateOrEdit(CreateOrEditQuyTrinhDuAnAssignedDto input)
        {
            if (input.Id == null)
            {
                return await Create(input);
            }
            else
            {
                return await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create)]
        protected virtual async Task<GetQuyTrinhDuAnAssignedForEditOutput> Create(
            CreateOrEditQuyTrinhDuAnAssignedDto input)
        {
            input.QuyTrinhDuAnId = null;
            var quyTrinhDuAnAssigned = ObjectMapper.Map<QuyTrinhDuAnAssigned>(input);


            if (AbpSession.TenantId != null)
            {
                quyTrinhDuAnAssigned.TenantId = (int?) AbpSession.TenantId;
            }

            var quytrinhlist = await _quyTrinhDuAnRepository.GetAll().WhereIf(true, p =>
                    p.LoaiDuAnId == input.LoaiDuAnId && p.ParentId == input.ParentId).OrderByDescending(p => p.STT)
                .ToListAsync();
            if (quytrinhlist.IsNullOrEmpty())
            {
                quyTrinhDuAnAssigned.STT = 1;
            }
            else
            {
                quyTrinhDuAnAssigned.STT = quytrinhlist[0].STT + 1;
            }

            long id = await _quyTrinhDuAnRepository.InsertAndGetIdAsync(quyTrinhDuAnAssigned);
            quyTrinhDuAnAssigned.Id = id;
            var output = new GetQuyTrinhDuAnAssignedForEditOutput
                {QuyTrinhDuAnAssigned = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnAssignedDto>(quyTrinhDuAnAssigned)};
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        protected virtual async Task<GetQuyTrinhDuAnAssignedForEditOutput> Update(
            CreateOrEditQuyTrinhDuAnAssignedDto input)
        {
            var quyTrinhDuAnAssigned = await _quyTrinhDuAnRepository.FirstOrDefaultAsync((int) input.Id);
            quyTrinhDuAnAssigned.Name = input.Name;
            quyTrinhDuAnAssigned.Descriptions = input.Descriptions;
            // quyTrinhDuAnAssigned.STT = input.STT;
            quyTrinhDuAnAssigned.SoVanBanQuyDinh = input.SoVanBanQuyDinh;
            quyTrinhDuAnAssigned.MaQuyTrinh = input.MaQuyTrinh;
            var output = new GetQuyTrinhDuAnAssignedForEditOutput
                {QuyTrinhDuAnAssigned = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnAssignedDto>(quyTrinhDuAnAssigned)};
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            QuyTrinhDuAnAssigned quyTrinhDuAnAssigned = await _quyTrinhDuAnRepository.FirstOrDefaultAsync(input.Id);
            if (quyTrinhDuAnAssigned.QuyTrinhDuAnId.HasValue)
            {
                throw new UserFriendlyException("Quy trình gốc không thể xóa!");
            }

            await _quyTrinhDuAnAssignedRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetQuyTrinhDuAnAssignedsToExcel(GetAllQuyTrinhDuAnAssignedsForExcelInput input)
        {
            var filteredQuyTrinhDuAnAssigneds = _quyTrinhDuAnAssignedRepository.GetAll()
                .Include(e => e.LoaiDuAnFk)
                .Include(e => e.QuyTrinhDuAnFk)
                .Include(e => e.ParentFk)
                .Include(e => e.DuAnFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter) ||
                         e.MaQuyTrinh.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),
                    e => e.Descriptions == input.DescriptionsFilter)
                .WhereIf(input.MinSTTFilter != null, e => e.STT >= input.MinSTTFilter)
                .WhereIf(input.MaxSTTFilter != null, e => e.STT <= input.MaxSTTFilter)
                .WhereIf(input.MinSoVanBanQuyDinhFilter != null,
                    e => e.SoVanBanQuyDinh >= input.MinSoVanBanQuyDinhFilter)
                .WhereIf(input.MaxSoVanBanQuyDinhFilter != null,
                    e => e.SoVanBanQuyDinh <= input.MaxSoVanBanQuyDinhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaQuyTrinhFilter),
                    e => e.MaQuyTrinh == input.MaQuyTrinhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter),
                    e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnNameFilter),
                    e => e.QuyTrinhDuAnFk != null && e.QuyTrinhDuAnFk.Name == input.QuyTrinhDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnAssignedNameFilter),
                    e => e.ParentFk != null && e.ParentFk.Name == input.QuyTrinhDuAnAssignedNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter),
                    e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter);

            var query = (from o in filteredQuyTrinhDuAnAssigneds
                join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_quyTrinhDuAnRepository.GetAll() on o.QuyTrinhDuAnId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _quyTrinhDuAnRepository.GetAll() on o.ParentId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o4.Id into j4
                from s4 in j4.DefaultIfEmpty()
                select new GetQuyTrinhDuAnAssignedForViewDto()
                {
                    QuyTrinhDuAnAssigned = new QuyTrinhDuAnAssignedDto
                    {
                        Name = o.Name,
                        Descriptions = o.Descriptions,
                        STT = o.STT,
                        SoVanBanQuyDinh = o.SoVanBanQuyDinh,
                        MaQuyTrinh = o.MaQuyTrinh,
                        Id = o.Id
                    },
                    LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    QuyTrinhDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                    QuyTrinhDuAnAssignedName = s3 == null || s3.Name == null ? "" : s3.Name.ToString(),
                    DuAnName = s4 == null || s4.Name == null ? "" : s4.Name.ToString()
                });


            var quyTrinhDuAnAssignedListDtos = await query.ToListAsync();

            return _quyTrinhDuAnAssignedsExcelExporter.ExportToFile(quyTrinhDuAnAssignedListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
        public async Task<PagedResultDto<QuyTrinhDuAnAssignedLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(
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

            var lookupTableDtoList = new List<QuyTrinhDuAnAssignedLoaiDuAnLookupTableDto>();
            foreach (var loaiDuAn in loaiDuAnList)
            {
                lookupTableDtoList.Add(new QuyTrinhDuAnAssignedLoaiDuAnLookupTableDto
                {
                    Id = loaiDuAn.Id,
                    DisplayName = loaiDuAn.Name?.ToString()
                });
            }

            return new PagedResultDto<QuyTrinhDuAnAssignedLoaiDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
        public async Task<PagedResultDto<QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableDto>>
            GetAllQuyTrinhDuAnForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _lookup_quyTrinhDuAnRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var quyTrinhDuAnList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableDto>();
            foreach (var quyTrinhDuAn in quyTrinhDuAnList)
            {
                lookupTableDtoList.Add(new QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableDto
                {
                    Id = quyTrinhDuAn.Id,
                    DisplayName = quyTrinhDuAn.Name?.ToString()
                });
            }

            return new PagedResultDto<QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
        public async Task<PagedResultDto<QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableDto>>
            GetAllQuyTrinhDuAnAssignedForLookupTable(GetAllForLookupTableInput input)
        {
            var query = _quyTrinhDuAnRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var quyTrinhDuAnAssignedList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableDto>();
            foreach (var quyTrinhDuAnAssigned in quyTrinhDuAnAssignedList)
            {
                lookupTableDtoList.Add(new QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableDto
                {
                    Id = quyTrinhDuAnAssigned.Id,
                    DisplayName = quyTrinhDuAnAssigned.Name?.ToString()
                });
            }

            return new PagedResultDto<QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
        public async Task<PagedResultDto<QuyTrinhDuAnAssignedDuAnLookupTableDto>> GetAllDuAnForLookupTable(
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

            var lookupTableDtoList = new List<QuyTrinhDuAnAssignedDuAnLookupTableDto>();
            foreach (var duAn in duAnList)
            {
                lookupTableDtoList.Add(new QuyTrinhDuAnAssignedDuAnLookupTableDto
                {
                    Id = duAn.Id,
                    DisplayName = duAn.Name?.ToString()
                });
            }

            return new PagedResultDto<QuyTrinhDuAnAssignedDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<GetQuyTrinhDuAnAssignedForView2Dto>> GetDataForTree(int duanid)
        {
            var filtered = _quyTrinhDuAnRepository.GetAll().WhereIf(true, p => p.DuAnId == duanid)
                ;

            var query = from o in filtered
                select new GetQuyTrinhDuAnAssignedForView2Dto()
                {
                    QuyTrinhDuAn = new QuyTrinhDuAnAssignedHasMemberCountDto()
                    {
                        Name = o.Name,
                        SoVanBanQuyDinh = o.SoVanBanQuyDinh,
                        STT = o.STT,
                        Id = o.Id,
                        ParentId = o.ParentId,
                        Descriptions = o.Descriptions,
                        MaQuyTrinh = o.MaQuyTrinh,
                        LoaiDuAnId = o.LoaiDuAnId,
                        DuAnId = o.DuAnId,
                        QuyTrinhDuAnId = o.QuyTrinhDuAnId
                    },
                };
            var t = await query.ToListAsync();
            foreach (var VARIABLE in t)
            {
                VARIABLE.QuyTrinhDuAn.MemberCount =
                    await _quyTrinhDuAnRepository.CountAsync(p => p.ParentId == VARIABLE.QuyTrinhDuAn.Id);
            }

            return t;
        }
        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Delete)]
        public async Task<int> XoaTieuChi(long id)
        {
            try
            {
                var query = _quyTrinhDuAnAssignedRepository.GetAll().Where(p => p.ParentId == id);

                if(await query.CountAsync() > 0)
                {
                    return (int)XoaTieuChiState.CHUA_XOA_HET_CON;
                }

                await _quyTrinhDuAnAssignedRepository.DeleteAsync(id);

                return (int)XoaTieuChiState.XOA_THANH_CONG;
            }
            catch (Exception ex)
            {
                return (int)XoaTieuChiState.LOI_KHAC;
            }
        }
        public async Task<int> MoveTieuChi(MoveTreeDto input)
        {
            try
            {
                var quyTrinhDuAn = await _quyTrinhDuAnRepository.FirstOrDefaultAsync(input.Id);
                int old_position = quyTrinhDuAn.STT;
                int new_position = input.Position + 1;


                if (quyTrinhDuAn.ParentId == input.NewParentId)
                {
                    if (old_position > new_position)
                    {
                        await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
                            p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId
                                 && p.ParentId == quyTrinhDuAn.ParentId
                                 && p.STT >= new_position
                                 && p.STT < old_position
                        ).UpdateAsync(quytrinh => new QuyTrinhDuAnAssigned()
                        {
                            STT = quytrinh.STT + 1
                        });
                    }
                    else
                    {
                        await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
                            p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId
                                 && p.ParentId == quyTrinhDuAn.ParentId
                                 && p.STT <= new_position
                                 && p.STT > old_position
                        ).UpdateAsync(quytrinh => new QuyTrinhDuAnAssigned()
                        {
                            STT = quytrinh.STT - 1
                        });
                    }
                }
                else
                {
                    //update new parent
                    await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
                        p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId
                             && p.ParentId == input.NewParentId
                             && p.STT >= new_position
                    ).UpdateAsync(quytrinh => new QuyTrinhDuAnAssigned()
                    {
                        STT = quytrinh.STT + 1
                    });

                    //update old parent
                    await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
                        p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId
                             && p.ParentId == quyTrinhDuAn.ParentId
                             && p.STT > new_position
                    ).UpdateAsync(quytrinh => new QuyTrinhDuAnAssigned()
                    {
                        STT = quytrinh.STT - 1
                    });
                }

                quyTrinhDuAn.ParentId = input.NewParentId;
                quyTrinhDuAn.STT = new_position;
                await _quyTrinhDuAnRepository.UpdateAsync(quyTrinhDuAn);
                return (int) DiChuyenTreeState.DI_CHUYEN_OK;
            }
            catch (Exception ex)
            {
                return (int) DiChuyenTreeState.DI_CHUYEN_THAT_BAI;
            }
        }
    }
}