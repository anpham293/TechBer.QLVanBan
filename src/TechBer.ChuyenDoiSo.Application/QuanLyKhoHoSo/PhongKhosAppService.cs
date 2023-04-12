

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
	[AbpAuthorize(AppPermissions.Pages_PhongKhos)]
    public class PhongKhosAppService : ChuyenDoiSoAppServiceBase, IPhongKhosAppService
    {
		 private readonly IRepository<PhongKho> _phongKhoRepository;
		 private readonly IPhongKhosExcelExporter _phongKhosExcelExporter;
		 

		  public PhongKhosAppService(IRepository<PhongKho> phongKhoRepository, IPhongKhosExcelExporter phongKhosExcelExporter ) 
		  {
			_phongKhoRepository = phongKhoRepository;
			_phongKhosExcelExporter = phongKhosExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetPhongKhoForViewDto>> GetAll(GetAllPhongKhosInput input)
         {
			
			var filteredPhongKhos = _phongKhoRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter);

			var pagedAndFilteredPhongKhos = filteredPhongKhos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var phongKhos = from o in pagedAndFilteredPhongKhos
                         select new GetPhongKhoForViewDto() {
							PhongKho = new PhongKhoDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                Id = o.Id
							}
						};

            var totalCount = await filteredPhongKhos.CountAsync();

            return new PagedResultDto<GetPhongKhoForViewDto>(
                totalCount,
                await phongKhos.ToListAsync()
            );
         }
		 
		 public async Task<GetPhongKhoForViewDto> GetPhongKhoForView(int id)
         {
            var phongKho = await _phongKhoRepository.GetAsync(id);

            var output = new GetPhongKhoForViewDto { PhongKho = ObjectMapper.Map<PhongKhoDto>(phongKho) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_PhongKhos_Edit)]
		 public async Task<GetPhongKhoForEditOutput> GetPhongKhoForEdit(EntityDto input)
         {
            var phongKho = await _phongKhoRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetPhongKhoForEditOutput {PhongKho = ObjectMapper.Map<CreateOrEditPhongKhoDto>(phongKho)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditPhongKhoDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_PhongKhos_Create)]
		 protected virtual async Task Create(CreateOrEditPhongKhoDto input)
         {
            var phongKho = ObjectMapper.Map<PhongKho>(input);

			
			if (AbpSession.TenantId != null)
			{
				phongKho.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _phongKhoRepository.InsertAsync(phongKho);
         }

		 [AbpAuthorize(AppPermissions.Pages_PhongKhos_Edit)]
		 protected virtual async Task Update(CreateOrEditPhongKhoDto input)
         {
            var phongKho = await _phongKhoRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, phongKho);
         }

		 [AbpAuthorize(AppPermissions.Pages_PhongKhos_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _phongKhoRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetPhongKhosToExcel(GetAllPhongKhosForExcelInput input)
         {
			
			var filteredPhongKhos = _phongKhoRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter);

			var query = (from o in filteredPhongKhos
                         select new GetPhongKhoForViewDto() { 
							PhongKho = new PhongKhoDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                Id = o.Id
							}
						 });


            var phongKhoListDtos = await query.ToListAsync();

            return _phongKhosExcelExporter.ExportToFile(phongKhoListDtos);
         }


    }
}