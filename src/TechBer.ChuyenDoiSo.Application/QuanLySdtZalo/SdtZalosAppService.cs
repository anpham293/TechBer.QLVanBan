

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLySdtZalo.Exporting;
using TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo
{
	[AbpAuthorize(AppPermissions.Pages_SdtZalos)]
    public class SdtZalosAppService : ChuyenDoiSoAppServiceBase, ISdtZalosAppService
    {
		 private readonly IRepository<SdtZalo, long> _sdtZaloRepository;
		 private readonly ISdtZalosExcelExporter _sdtZalosExcelExporter;
		 

		  public SdtZalosAppService(IRepository<SdtZalo, long> sdtZaloRepository, ISdtZalosExcelExporter sdtZalosExcelExporter ) 
		  {
			_sdtZaloRepository = sdtZaloRepository;
			_sdtZalosExcelExporter = sdtZalosExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetSdtZaloForViewDto>> GetAll(GetAllSdtZalosInput input)
         {
			
			var filteredSdtZalos = _sdtZaloRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter) || e.Sdt.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SdtFilter),  e => e.Sdt == input.SdtFilter);

			var pagedAndFilteredSdtZalos = filteredSdtZalos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var sdtZalos = from o in pagedAndFilteredSdtZalos
                         select new GetSdtZaloForViewDto() {
							SdtZalo = new SdtZaloDto
							{
                                Ten = o.Ten,
                                Sdt = o.Sdt,
                                Id = o.Id
							}
						};

            var totalCount = await filteredSdtZalos.CountAsync();

            return new PagedResultDto<GetSdtZaloForViewDto>(
                totalCount,
                await sdtZalos.ToListAsync()
            );
         }
		 
		 public async Task<GetSdtZaloForViewDto> GetSdtZaloForView(long id)
         {
            var sdtZalo = await _sdtZaloRepository.GetAsync(id);

            var output = new GetSdtZaloForViewDto { SdtZalo = ObjectMapper.Map<SdtZaloDto>(sdtZalo) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_SdtZalos_Edit)]
		 public async Task<GetSdtZaloForEditOutput> GetSdtZaloForEdit(EntityDto<long> input)
         {
            var sdtZalo = await _sdtZaloRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetSdtZaloForEditOutput {SdtZalo = ObjectMapper.Map<CreateOrEditSdtZaloDto>(sdtZalo)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditSdtZaloDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_SdtZalos_Create)]
		 protected virtual async Task Create(CreateOrEditSdtZaloDto input)
         {
            var sdtZalo = ObjectMapper.Map<SdtZalo>(input);

			

            await _sdtZaloRepository.InsertAsync(sdtZalo);
         }

		 [AbpAuthorize(AppPermissions.Pages_SdtZalos_Edit)]
		 protected virtual async Task Update(CreateOrEditSdtZaloDto input)
         {
            var sdtZalo = await _sdtZaloRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, sdtZalo);
         }

		 [AbpAuthorize(AppPermissions.Pages_SdtZalos_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _sdtZaloRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetSdtZalosToExcel(GetAllSdtZalosForExcelInput input)
         {
			
			var filteredSdtZalos = _sdtZaloRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Ten.Contains(input.Filter) || e.Sdt.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SdtFilter),  e => e.Sdt == input.SdtFilter);

			var query = (from o in filteredSdtZalos
                         select new GetSdtZaloForViewDto() { 
							SdtZalo = new SdtZaloDto
							{
                                Ten = o.Ten,
                                Sdt = o.Sdt,
                                Id = o.Id
							}
						 });


            var sdtZaloListDtos = await query.ToListAsync();

            return _sdtZalosExcelExporter.ExportToFile(sdtZaloListDtos);
         }


    }
}