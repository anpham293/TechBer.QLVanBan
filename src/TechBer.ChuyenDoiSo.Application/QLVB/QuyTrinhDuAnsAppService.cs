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
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using Z.EntityFramework.Plus;

namespace TechBer.ChuyenDoiSo.QLVB
{
    [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
    public class QuyTrinhDuAnsAppService : ChuyenDoiSoAppServiceBase, IQuyTrinhDuAnsAppService
    {
        private readonly IRepository<QuyTrinhDuAn> _quyTrinhDuAnRepository;
        private readonly IQuyTrinhDuAnsExcelExporter _quyTrinhDuAnsExcelExporter;
        private readonly IRepository<LoaiDuAn, int> _lookup_loaiDuAnRepository;
        private readonly IRepository<QuyTrinhDuAn, int> _lookup_quyTrinhDuAnRepository;


        public QuyTrinhDuAnsAppService(IRepository<QuyTrinhDuAn> quyTrinhDuAnRepository,
            IQuyTrinhDuAnsExcelExporter quyTrinhDuAnsExcelExporter,
            IRepository<LoaiDuAn, int> lookup_loaiDuAnRepository,
            IRepository<QuyTrinhDuAn, int> lookup_quyTrinhDuAnRepository)
        {
            _quyTrinhDuAnRepository = quyTrinhDuAnRepository;
            _quyTrinhDuAnsExcelExporter = quyTrinhDuAnsExcelExporter;
            _lookup_loaiDuAnRepository = lookup_loaiDuAnRepository;
            _lookup_quyTrinhDuAnRepository = lookup_quyTrinhDuAnRepository;
        }

        public async Task<PagedResultDto<GetQuyTrinhDuAnForViewDto>> GetAll(GetAllQuyTrinhDuAnsInput input)
        {
            var filteredQuyTrinhDuAns = _quyTrinhDuAnRepository.GetAll()
                .Include(e => e.LoaiDuAnFk)
                .Include(e => e.ParentFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter) ||
                         e.MaQuyTrinh.Contains(input.Filter) )
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),
                    e => e.Descriptions == input.DescriptionsFilter)
                .WhereIf(input.MinSTTFilter != null, e => e.STT >= input.MinSTTFilter)
                .WhereIf(input.MaxSTTFilter != null, e => e.STT <= input.MaxSTTFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaQuyTrinhFilter),
                    e => e.MaQuyTrinh == input.MaQuyTrinhFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter),
                    e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnNameFilter),
                    e => e.ParentFk != null && e.ParentFk.Name == input.QuyTrinhDuAnNameFilter);

            var pagedAndFilteredQuyTrinhDuAns = filteredQuyTrinhDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var quyTrinhDuAns = from o in pagedAndFilteredQuyTrinhDuAns
                join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_quyTrinhDuAnRepository.GetAll() on o.ParentId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetQuyTrinhDuAnForViewDto()
                {
                    QuyTrinhDuAn = new QuyTrinhDuAnDto
                    {
                        Name = o.Name,
                        Descriptions = o.Descriptions,
                        STT = o.STT,
                        MaQuyTrinh = o.MaQuyTrinh,
                        SoVanBanQuyDinh = o.SoVanBanQuyDinh,
                        Id = o.Id
                    },
                    LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    QuyTrinhDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                };

            var totalCount = await filteredQuyTrinhDuAns.CountAsync();

            return new PagedResultDto<GetQuyTrinhDuAnForViewDto>(
                totalCount,
                await quyTrinhDuAns.ToListAsync()
            );
        }

        public async Task<GetQuyTrinhDuAnForViewDto> GetQuyTrinhDuAnForView(int id)
        {
            var quyTrinhDuAn = await _quyTrinhDuAnRepository.GetAsync(id);

            var output = new GetQuyTrinhDuAnForViewDto {QuyTrinhDuAn = ObjectMapper.Map<QuyTrinhDuAnDto>(quyTrinhDuAn)};

            if (output.QuyTrinhDuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn =
                    await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }

            if (output.QuyTrinhDuAn.ParentId != null)
            {
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAn.ParentId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Edit)]
        public async Task<GetQuyTrinhDuAnForEditOutput> GetQuyTrinhDuAnForEdit(EntityDto input)
        {
            var quyTrinhDuAn = await _quyTrinhDuAnRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetQuyTrinhDuAnForEditOutput
                {QuyTrinhDuAn = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnDto>(quyTrinhDuAn)};

            if (output.QuyTrinhDuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn =
                    await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }

            if (output.QuyTrinhDuAn.ParentId != null)
            {
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnRepository.FirstOrDefaultAsync((int) output.QuyTrinhDuAn.ParentId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
            }

            return output;
        }

        public async Task<GetQuyTrinhDuAnForEditOutput> CreateOrEdit(CreateOrEditQuyTrinhDuAnDto input)
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

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Create)]
        protected virtual async Task<GetQuyTrinhDuAnForEditOutput> Create(CreateOrEditQuyTrinhDuAnDto input)
        {
            var quyTrinhDuAn = ObjectMapper.Map<QuyTrinhDuAn>(input);


            if (AbpSession.TenantId != null)
            {
                quyTrinhDuAn.TenantId = (int?) AbpSession.TenantId;
            }

            var quytrinhlist = await _quyTrinhDuAnRepository.GetAll().WhereIf(true, p =>
                    p.LoaiDuAnId == input.LoaiDuAnId && p.ParentId == input.ParentId).OrderByDescending(p => p.STT)
                .ToListAsync();
            if (quytrinhlist.IsNullOrEmpty())
            {
                quyTrinhDuAn.STT = 1;
            }
            else
            {
                quyTrinhDuAn.STT = quytrinhlist[0].STT + 1;
            }

            int id = await _quyTrinhDuAnRepository.InsertAndGetIdAsync(quyTrinhDuAn);
            quyTrinhDuAn.Id = id;
            var output = new GetQuyTrinhDuAnForEditOutput
                {QuyTrinhDuAn = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnDto>(quyTrinhDuAn)};
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Edit)]
        protected virtual async Task<GetQuyTrinhDuAnForEditOutput> Update(CreateOrEditQuyTrinhDuAnDto input)
        {
            var quyTrinhDuAn = await _quyTrinhDuAnRepository.FirstOrDefaultAsync((int) input.Id);
            quyTrinhDuAn.Name = input.Name;
            quyTrinhDuAn.Descriptions = input.Descriptions;
            quyTrinhDuAn.STT = input.STT;
            quyTrinhDuAn.SoVanBanQuyDinh = input.SoVanBanQuyDinh;
            quyTrinhDuAn.MaQuyTrinh = input.MaQuyTrinh;
            var output = new GetQuyTrinhDuAnForEditOutput
                {QuyTrinhDuAn = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnDto>(quyTrinhDuAn)};
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _quyTrinhDuAnRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetQuyTrinhDuAnsToExcel(GetAllQuyTrinhDuAnsForExcelInput input)
        {
            var filteredQuyTrinhDuAns = _quyTrinhDuAnRepository.GetAll()
                .Include(e => e.LoaiDuAnFk)
                .Include(e => e.ParentFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter) ||
                         e.MaQuyTrinh.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter), e => e.Name == input.NameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),
                    e => e.Descriptions == input.DescriptionsFilter)
                .WhereIf(input.MinSTTFilter != null, e => e.STT >= input.MinSTTFilter)
                .WhereIf(input.MaxSTTFilter != null, e => e.STT <= input.MaxSTTFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaQuyTrinhFilter),
                    e => e.MaQuyTrinh == input.MaQuyTrinhFilter)
                
                .WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter),
                    e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.QuyTrinhDuAnNameFilter),
                    e => e.ParentFk != null && e.ParentFk.Name == input.QuyTrinhDuAnNameFilter);

            var query = (from o in filteredQuyTrinhDuAns
                join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_quyTrinhDuAnRepository.GetAll() on o.ParentId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetQuyTrinhDuAnForViewDto()
                {
                    QuyTrinhDuAn = new QuyTrinhDuAnDto
                    {
                        Name = o.Name,
                        Descriptions = o.Descriptions,
                        STT = o.STT,
                        MaQuyTrinh = o.MaQuyTrinh,
                        SoVanBanQuyDinh = o.SoVanBanQuyDinh,
                        Id = o.Id
                    },
                    LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    QuyTrinhDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
                });


            var quyTrinhDuAnListDtos = await query.ToListAsync();

            return _quyTrinhDuAnsExcelExporter.ExportToFile(quyTrinhDuAnListDtos);
        }


        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
        public async Task<PagedResultDto<QuyTrinhDuAnLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(
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

            var lookupTableDtoList = new List<QuyTrinhDuAnLoaiDuAnLookupTableDto>();
            foreach (var loaiDuAn in loaiDuAnList)
            {
                lookupTableDtoList.Add(new QuyTrinhDuAnLoaiDuAnLookupTableDto
                {
                    Id = loaiDuAn.Id,
                    DisplayName = loaiDuAn.Name?.ToString()
                });
            }

            return new PagedResultDto<QuyTrinhDuAnLoaiDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
        public async Task<PagedResultDto<QuyTrinhDuAnQuyTrinhDuAnLookupTableDto>> GetAllQuyTrinhDuAnForLookupTable(
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

            var lookupTableDtoList = new List<QuyTrinhDuAnQuyTrinhDuAnLookupTableDto>();
            foreach (var quyTrinhDuAn in quyTrinhDuAnList)
            {
                lookupTableDtoList.Add(new QuyTrinhDuAnQuyTrinhDuAnLookupTableDto
                {
                    Id = quyTrinhDuAn.Id,
                    DisplayName = quyTrinhDuAn.Name?.ToString()
                });
            }

            return new PagedResultDto<QuyTrinhDuAnQuyTrinhDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
        }

        public async Task<List<GetQuyTrinhDuAnForView2Dto>> GetDataForTree(int loaiId)
        {
            var filtered = _quyTrinhDuAnRepository.GetAll().WhereIf(true, p => p.LoaiDuAnId == loaiId)
                ;

            var query = from o in filtered
                select new GetQuyTrinhDuAnForView2Dto()
                {
                    QuyTrinhDuAn = new QuyTrinhDuAnHasMemberCountDto()
                    {
                        Name = o.Name,
                        SoVanBanQuyDinh = o.SoVanBanQuyDinh,
                        STT = o.STT,
                        Id = o.Id,
                        ParentId = o.ParentId,
                        Descriptions = o.Descriptions,
                        MaQuyTrinh = o.MaQuyTrinh,
                        LoaiDuAnId = o.LoaiDuAnId
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
                        ).UpdateAsync(quytrinh => new QuyTrinhDuAn()
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
                        ).UpdateAsync(quytrinh => new QuyTrinhDuAn()
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
                    ).UpdateAsync(quytrinh => new QuyTrinhDuAn()
                    {
                        STT = quytrinh.STT + 1
                    });
                    
                    //update old parent
                    await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
                        p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId
                             && p.ParentId == quyTrinhDuAn.ParentId
                             && p.STT > new_position
                    ).UpdateAsync(quytrinh => new QuyTrinhDuAn()
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