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
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[AbpAuthorize(AppPermissions.Pages_DuAns)]
    public class DuAnsAppService : ChuyenDoiSoAppServiceBase, IDuAnsAppService
    {
		 private readonly IRepository<DuAn> _duAnRepository;
		 private readonly IDuAnsExcelExporter _duAnsExcelExporter;
		 private readonly IRepository<LoaiDuAn,int> _lookup_loaiDuAnRepository;
		 

		  public DuAnsAppService(IRepository<DuAn> duAnRepository, IDuAnsExcelExporter duAnsExcelExporter , IRepository<LoaiDuAn, int> lookup_loaiDuAnRepository) 
		  {
			_duAnRepository = duAnRepository;
			_duAnsExcelExporter = duAnsExcelExporter;
			_lookup_loaiDuAnRepository = lookup_loaiDuAnRepository;
		
		  }

		 public async Task<PagedResultDto<GetDuAnForViewDto>> GetAll(GetAllDuAnsInput input)
         {
			
			var filteredDuAns = _duAnRepository.GetAll()
						.Include( e => e.LoaiDuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),  e => e.Descriptions == input.DescriptionsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter), e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter);

			var pagedAndFilteredDuAns = filteredDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var duAns = from o in pagedAndFilteredDuAns
                         join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDuAnForViewDto() {
							DuAn = new DuAnDto
							{
                                Name = o.Name,
                                Descriptions = o.Descriptions,
                                Id = o.Id
							},
                         	LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredDuAns.CountAsync();

            return new PagedResultDto<GetDuAnForViewDto>(
                totalCount,
                await duAns.ToListAsync()
            );
         }
		 
		 public async Task<GetDuAnForViewDto> GetDuAnForView(int id)
         {
            var duAn = await _duAnRepository.GetAsync(id);

            var output = new GetDuAnForViewDto { DuAn = ObjectMapper.Map<DuAnDto>(duAn) };

		    if (output.DuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn = await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int)output.DuAn.LoaiDuAnId);
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
                var _lookupLoaiDuAn = await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int)output.DuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDuAnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
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
		

            await _duAnRepository.InsertAsync(duAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_DuAns_Edit)]
		 protected virtual async Task Update(CreateOrEditDuAnDto input)
         {
            var duAn = await _duAnRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, duAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_DuAns_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _duAnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDuAnsToExcel(GetAllDuAnsForExcelInput input)
         {
			
			var filteredDuAns = _duAnRepository.GetAll()
						.Include( e => e.LoaiDuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),  e => e.Descriptions == input.DescriptionsFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter), e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter);

			var query = (from o in filteredDuAns
                         join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDuAnForViewDto() { 
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
         public async Task<PagedResultDto<DuAnLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_loaiDuAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var loaiDuAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DuAnLoaiDuAnLookupTableDto>();
			foreach(var loaiDuAn in loaiDuAnList){
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
    }
}