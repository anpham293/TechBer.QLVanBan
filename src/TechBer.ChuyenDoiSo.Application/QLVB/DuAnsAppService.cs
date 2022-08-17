

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
		 

		  public DuAnsAppService(IRepository<DuAn> duAnRepository, IDuAnsExcelExporter duAnsExcelExporter ) 
		  {
			_duAnRepository = duAnRepository;
			_duAnsExcelExporter = duAnsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetDuAnForViewDto>> GetAll(GetAllDuAnsInput input)
         {
			
			var filteredDuAns = _duAnRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),  e => e.Descriptions == input.DescriptionsFilter);

			var pagedAndFilteredDuAns = filteredDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var duAns = from o in pagedAndFilteredDuAns
                         select new GetDuAnForViewDto() {
							DuAn = new DuAnDto
							{
                                Name = o.Name,
                                Descriptions = o.Descriptions,
                                Id = o.Id
							}
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
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DuAns_Edit)]
		 public async Task<GetDuAnForEditOutput> GetDuAnForEdit(EntityDto input)
         {
            var duAn = await _duAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDuAnForEditOutput {DuAn = ObjectMapper.Map<CreateOrEditDuAnDto>(duAn)};
			
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
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),  e => e.Descriptions == input.DescriptionsFilter);

			var query = (from o in filteredDuAns
                         select new GetDuAnForViewDto() { 
							DuAn = new DuAnDto
							{
                                Name = o.Name,
                                Descriptions = o.Descriptions,
                                Id = o.Id
							}
						 });


            var duAnListDtos = await query.ToListAsync();

            return _duAnsExcelExporter.ExportToFile(duAnListDtos);
         }


    }
}