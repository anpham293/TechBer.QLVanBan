using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QLVB;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Exporting;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo
{
	[AbpAuthorize(AppPermissions.Pages_ThungHoSos)]
    public class ThungHoSosAppService : ChuyenDoiSoAppServiceBase, IThungHoSosAppService
    {
		 private readonly IRepository<ThungHoSo> _thungHoSoRepository;
		 private readonly IThungHoSosExcelExporter _thungHoSosExcelExporter;
		 private readonly IRepository<DayKe,int> _lookup_dayKeRepository;
		 private readonly IRepository<DuAn,int> _lookup_duAnRepository;
		 

		  public ThungHoSosAppService(IRepository<ThungHoSo> thungHoSoRepository, IThungHoSosExcelExporter thungHoSosExcelExporter , IRepository<DayKe, int> lookup_dayKeRepository, IRepository<DuAn, int> lookup_duAnRepository) 
		  {
			_thungHoSoRepository = thungHoSoRepository;
			_thungHoSosExcelExporter = thungHoSosExcelExporter;
			_lookup_dayKeRepository = lookup_dayKeRepository;
		_lookup_duAnRepository = lookup_duAnRepository;
		
		  }

		 public async Task<PagedResultDto<GetThungHoSoForViewDto>> GetAll(GetAllThungHoSosInput input)
         {
			
			var filteredThungHoSos = _thungHoSoRepository.GetAll()
						.Include( e => e.DayKeFk)
						.Include( e => e.DuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter),  e => e.MoTa == input.MoTaFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DayKeMaSoFilter), e => e.DayKeFk != null && e.DayKeFk.MaSo == input.DayKeMaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter), e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter);

			var pagedAndFilteredThungHoSos = filteredThungHoSos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var thungHoSos = from o in pagedAndFilteredThungHoSos
                         join o1 in _lookup_dayKeRepository.GetAll() on o.DayKeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetThungHoSoForViewDto() {
							ThungHoSo = new ThungHoSoDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                MoTa = o.MoTa,
                                TrangThai = o.TrangThai,
                                Id = o.Id
							},
                         	DayKeMaSo = s1 == null || s1.MaSo == null ? "" : s1.MaSo.ToString(),
                         	DuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredThungHoSos.CountAsync();

            return new PagedResultDto<GetThungHoSoForViewDto>(
                totalCount,
                await thungHoSos.ToListAsync()
            );
         }
		 
		 public async Task<GetThungHoSoForViewDto> GetThungHoSoForView(int id)
         {
            var thungHoSo = await _thungHoSoRepository.GetAsync(id);

            var output = new GetThungHoSoForViewDto { ThungHoSo = ObjectMapper.Map<ThungHoSoDto>(thungHoSo) };

		    if (output.ThungHoSo.DayKeId != null)
            {
                var _lookupDayKe = await _lookup_dayKeRepository.FirstOrDefaultAsync((int)output.ThungHoSo.DayKeId);
                output.DayKeMaSo = _lookupDayKe?.MaSo?.ToString();
            }

		    if (output.ThungHoSo.DuAnId != null)
            {
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int)output.ThungHoSo.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ThungHoSos_Edit)]
		 public async Task<GetThungHoSoForEditOutput> GetThungHoSoForEdit(EntityDto input)
         {
            var thungHoSo = await _thungHoSoRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetThungHoSoForEditOutput {ThungHoSo = ObjectMapper.Map<CreateOrEditThungHoSoDto>(thungHoSo)};

		    if (output.ThungHoSo.DayKeId != null)
            {
                var _lookupDayKe = await _lookup_dayKeRepository.FirstOrDefaultAsync((int)output.ThungHoSo.DayKeId);
                output.DayKeMaSo = _lookupDayKe?.MaSo?.ToString();
            }

		    if (output.ThungHoSo.DuAnId != null)
            {
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int)output.ThungHoSo.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditThungHoSoDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ThungHoSos_Create)]
		 protected virtual async Task Create(CreateOrEditThungHoSoDto input)
         {
            var thungHoSo = ObjectMapper.Map<ThungHoSo>(input);

			
			if (AbpSession.TenantId != null)
			{
				thungHoSo.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _thungHoSoRepository.InsertAsync(thungHoSo);
         }

		 [AbpAuthorize(AppPermissions.Pages_ThungHoSos_Edit)]
		 protected virtual async Task Update(CreateOrEditThungHoSoDto input)
         {
            var thungHoSo = await _thungHoSoRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, thungHoSo);
         }

		 [AbpAuthorize(AppPermissions.Pages_ThungHoSos_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _thungHoSoRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetThungHoSosToExcel(GetAllThungHoSosForExcelInput input)
         {
			
			var filteredThungHoSos = _thungHoSoRepository.GetAll()
						.Include( e => e.DayKeFk)
						.Include( e => e.DuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.MoTa.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaFilter),  e => e.MoTa == input.MoTaFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DayKeMaSoFilter), e => e.DayKeFk != null && e.DayKeFk.MaSo == input.DayKeMaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter), e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter);

			var query = (from o in filteredThungHoSos
                         join o1 in _lookup_dayKeRepository.GetAll() on o.DayKeId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetThungHoSoForViewDto() { 
							ThungHoSo = new ThungHoSoDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                MoTa = o.MoTa,
                                TrangThai = o.TrangThai,
                                Id = o.Id
							},
                         	DayKeMaSo = s1 == null || s1.MaSo == null ? "" : s1.MaSo.ToString(),
                         	DuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var thungHoSoListDtos = await query.ToListAsync();

            return _thungHoSosExcelExporter.ExportToFile(thungHoSoListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ThungHoSos)]
         public async Task<PagedResultDto<ThungHoSoDayKeLookupTableDto>> GetAllDayKeForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_dayKeRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.MaSo != null && e.MaSo.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var dayKeList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ThungHoSoDayKeLookupTableDto>();
			foreach(var dayKe in dayKeList){
				lookupTableDtoList.Add(new ThungHoSoDayKeLookupTableDto
				{
					Id = dayKe.Id,
					DisplayName = dayKe.MaSo?.ToString()
				});
			}

            return new PagedResultDto<ThungHoSoDayKeLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ThungHoSos)]
         public async Task<PagedResultDto<ThungHoSoDuAnLookupTableDto>> GetAllDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_duAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var duAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ThungHoSoDuAnLookupTableDto>();
			foreach(var duAn in duAnList){
				lookupTableDtoList.Add(new ThungHoSoDuAnLookupTableDto
				{
					Id = duAn.Id,
					DisplayName = duAn.Name?.ToString()
				});
			}

            return new PagedResultDto<ThungHoSoDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}