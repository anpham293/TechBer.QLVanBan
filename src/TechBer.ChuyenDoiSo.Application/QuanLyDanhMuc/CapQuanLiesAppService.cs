

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_CapQuanLies)]
    public class CapQuanLiesAppService : ChuyenDoiSoAppServiceBase, ICapQuanLiesAppService
    {
		 private readonly IRepository<CapQuanLy> _capQuanLyRepository;
		 private readonly ICapQuanLiesExcelExporter _capQuanLiesExcelExporter;
		 

		  public CapQuanLiesAppService(IRepository<CapQuanLy> capQuanLyRepository, ICapQuanLiesExcelExporter capQuanLiesExcelExporter ) 
		  {
			_capQuanLyRepository = capQuanLyRepository;
			_capQuanLiesExcelExporter = capQuanLiesExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetCapQuanLyForViewDto>> GetAll(GetAllCapQuanLiesInput input)
         {
			
			var filteredCapQuanLies = _capQuanLyRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter);

			var pagedAndFilteredCapQuanLies = filteredCapQuanLies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var capQuanLies = from o in pagedAndFilteredCapQuanLies
                         select new GetCapQuanLyForViewDto() {
							CapQuanLy = new CapQuanLyDto
							{
                                Ten = o.Ten,
                                Id = o.Id
							}
						};

            var totalCount = await filteredCapQuanLies.CountAsync();

            return new PagedResultDto<GetCapQuanLyForViewDto>(
                totalCount,
                await capQuanLies.ToListAsync()
            );
         }
		 
		 public async Task<GetCapQuanLyForViewDto> GetCapQuanLyForView(int id)
         {
            var capQuanLy = await _capQuanLyRepository.GetAsync(id);

            var output = new GetCapQuanLyForViewDto { CapQuanLy = ObjectMapper.Map<CapQuanLyDto>(capQuanLy) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_CapQuanLies_Edit)]
		 public async Task<GetCapQuanLyForEditOutput> GetCapQuanLyForEdit(EntityDto input)
         {
            var capQuanLy = await _capQuanLyRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetCapQuanLyForEditOutput {CapQuanLy = ObjectMapper.Map<CreateOrEditCapQuanLyDto>(capQuanLy)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditCapQuanLyDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_CapQuanLies_Create)]
		 protected virtual async Task Create(CreateOrEditCapQuanLyDto input)
         {
            var capQuanLy = ObjectMapper.Map<CapQuanLy>(input);

			
			if (AbpSession.TenantId != null)
			{
				capQuanLy.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _capQuanLyRepository.InsertAsync(capQuanLy);
         }

		 [AbpAuthorize(AppPermissions.Pages_CapQuanLies_Edit)]
		 protected virtual async Task Update(CreateOrEditCapQuanLyDto input)
         {
            var capQuanLy = await _capQuanLyRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, capQuanLy);
         }

		 [AbpAuthorize(AppPermissions.Pages_CapQuanLies_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _capQuanLyRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetCapQuanLiesToExcel(GetAllCapQuanLiesForExcelInput input)
         {
			
			var filteredCapQuanLies = _capQuanLyRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter);

			var query = (from o in filteredCapQuanLies
                         select new GetCapQuanLyForViewDto() { 
							CapQuanLy = new CapQuanLyDto
							{
                                Ten = o.Ten,
                                Id = o.Id
							}
						 });


            var capQuanLyListDtos = await query.ToListAsync();

            return _capQuanLiesExcelExporter.ExportToFile(capQuanLyListDtos);
         }


    }
}