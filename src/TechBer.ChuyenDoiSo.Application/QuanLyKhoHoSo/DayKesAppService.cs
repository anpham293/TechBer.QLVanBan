using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;


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
	[AbpAuthorize(AppPermissions.Pages_DayKes)]
    public class DayKesAppService : ChuyenDoiSoAppServiceBase, IDayKesAppService
    {
		 private readonly IRepository<DayKe> _dayKeRepository;
		 private readonly IDayKesExcelExporter _dayKesExcelExporter;
		 private readonly IRepository<PhongKho,int> _lookup_phongKhoRepository;
		 

		  public DayKesAppService(IRepository<DayKe> dayKeRepository, IDayKesExcelExporter dayKesExcelExporter , IRepository<PhongKho, int> lookup_phongKhoRepository) 
		  {
			_dayKeRepository = dayKeRepository;
			_dayKesExcelExporter = dayKesExcelExporter;
			_lookup_phongKhoRepository = lookup_phongKhoRepository;
		
		  }

		 public async Task<PagedResultDto<GetDayKeForViewDto>> GetAll(GetAllDayKesInput input)
         {
			
			var filteredDayKes = _dayKeRepository.GetAll()
						.Include( e => e.PhongKhoFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhongKhoMaSoFilter), e => e.PhongKhoFk != null && e.PhongKhoFk.MaSo == input.PhongKhoMaSoFilter);

			var pagedAndFilteredDayKes = filteredDayKes
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var dayKes = from o in pagedAndFilteredDayKes
                         join o1 in _lookup_phongKhoRepository.GetAll() on o.PhongKhoId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDayKeForViewDto() {
							DayKe = new DayKeDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                Id = o.Id
							},
                         	PhongKhoMaSo = s1 == null || s1.MaSo == null ? "" : s1.MaSo.ToString()
						};

            var totalCount = await filteredDayKes.CountAsync();

            return new PagedResultDto<GetDayKeForViewDto>(
                totalCount,
                await dayKes.ToListAsync()
            );
         }
		 
		 public async Task<GetDayKeForViewDto> GetDayKeForView(int id)
         {
            var dayKe = await _dayKeRepository.GetAsync(id);

            var output = new GetDayKeForViewDto { DayKe = ObjectMapper.Map<DayKeDto>(dayKe) };

		    if (output.DayKe.PhongKhoId != null)
            {
                var _lookupPhongKho = await _lookup_phongKhoRepository.FirstOrDefaultAsync((int)output.DayKe.PhongKhoId);
                output.PhongKhoMaSo = _lookupPhongKho?.MaSo?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DayKes_Edit)]
		 public async Task<GetDayKeForEditOutput> GetDayKeForEdit(EntityDto input)
         {
            var dayKe = await _dayKeRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDayKeForEditOutput {DayKe = ObjectMapper.Map<CreateOrEditDayKeDto>(dayKe)};

		    if (output.DayKe.PhongKhoId != null)
            {
                var _lookupPhongKho = await _lookup_phongKhoRepository.FirstOrDefaultAsync((int)output.DayKe.PhongKhoId);
                output.PhongKhoMaSo = _lookupPhongKho?.MaSo?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDayKeDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DayKes_Create)]
		 protected virtual async Task Create(CreateOrEditDayKeDto input)
         {
            var dayKe = ObjectMapper.Map<DayKe>(input);

			
			if (AbpSession.TenantId != null)
			{
				dayKe.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _dayKeRepository.InsertAsync(dayKe);
         }

		 [AbpAuthorize(AppPermissions.Pages_DayKes_Edit)]
		 protected virtual async Task Update(CreateOrEditDayKeDto input)
         {
            var dayKe = await _dayKeRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, dayKe);
         }

		 [AbpAuthorize(AppPermissions.Pages_DayKes_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _dayKeRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDayKesToExcel(GetAllDayKesForExcelInput input)
         {
			
			var filteredDayKes = _dayKeRepository.GetAll()
						.Include( e => e.PhongKhoFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhongKhoMaSoFilter), e => e.PhongKhoFk != null && e.PhongKhoFk.MaSo == input.PhongKhoMaSoFilter);

			var query = (from o in filteredDayKes
                         join o1 in _lookup_phongKhoRepository.GetAll() on o.PhongKhoId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDayKeForViewDto() { 
							DayKe = new DayKeDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                Id = o.Id
							},
                         	PhongKhoMaSo = s1 == null || s1.MaSo == null ? "" : s1.MaSo.ToString()
						 });


            var dayKeListDtos = await query.ToListAsync();

            return _dayKesExcelExporter.ExportToFile(dayKeListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_DayKes)]
         public async Task<PagedResultDto<DayKePhongKhoLookupTableDto>> GetAllPhongKhoForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_phongKhoRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.MaSo != null && e.MaSo.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var phongKhoList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DayKePhongKhoLookupTableDto>();
			foreach(var phongKho in phongKhoList){
				lookupTableDtoList.Add(new DayKePhongKhoLookupTableDto
				{
					Id = phongKho.Id,
					DisplayName = phongKho.MaSo?.ToString()
				});
			}

            return new PagedResultDto<DayKePhongKhoLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}