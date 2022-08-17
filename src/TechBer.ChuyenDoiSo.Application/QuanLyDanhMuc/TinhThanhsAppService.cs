

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
	[AbpAuthorize(AppPermissions.Pages_TinhThanhs)]
    public class TinhThanhsAppService : ChuyenDoiSoAppServiceBase, ITinhThanhsAppService
    {
		 private readonly IRepository<TinhThanh> _tinhThanhRepository;
		 private readonly ITinhThanhsExcelExporter _tinhThanhsExcelExporter;
		 

		  public TinhThanhsAppService(IRepository<TinhThanh> tinhThanhRepository, ITinhThanhsExcelExporter tinhThanhsExcelExporter ) 
		  {
			_tinhThanhRepository = tinhThanhRepository;
			_tinhThanhsExcelExporter = tinhThanhsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetTinhThanhForViewDto>> GetAll(GetAllTinhThanhsInput input)
         {
			
			var filteredTinhThanhs = _tinhThanhRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Ma.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaFilter),  e => e.Ma == input.MaFilter);

			var pagedAndFilteredTinhThanhs = filteredTinhThanhs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var tinhThanhs = from o in pagedAndFilteredTinhThanhs
                         select new GetTinhThanhForViewDto() {
							TinhThanh = new TinhThanhDto
							{
                                Name = o.Name,
                                Ma = o.Ma,
                                Id = o.Id
							}
						};

            var totalCount = await filteredTinhThanhs.CountAsync();

            return new PagedResultDto<GetTinhThanhForViewDto>(
                totalCount,
                await tinhThanhs.ToListAsync()
            );
         }
		 
		 public async Task<GetTinhThanhForViewDto> GetTinhThanhForView(int id)
         {
            var tinhThanh = await _tinhThanhRepository.GetAsync(id);

            var output = new GetTinhThanhForViewDto { TinhThanh = ObjectMapper.Map<TinhThanhDto>(tinhThanh) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TinhThanhs_Edit)]
		 public async Task<GetTinhThanhForEditOutput> GetTinhThanhForEdit(EntityDto input)
         {
            var tinhThanh = await _tinhThanhRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTinhThanhForEditOutput {TinhThanh = ObjectMapper.Map<CreateOrEditTinhThanhDto>(tinhThanh)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTinhThanhDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TinhThanhs_Create)]
		 protected virtual async Task Create(CreateOrEditTinhThanhDto input)
         {
            var tinhThanh = ObjectMapper.Map<TinhThanh>(input);

			

            await _tinhThanhRepository.InsertAsync(tinhThanh);
         }

		 [AbpAuthorize(AppPermissions.Pages_TinhThanhs_Edit)]
		 protected virtual async Task Update(CreateOrEditTinhThanhDto input)
         {
            var tinhThanh = await _tinhThanhRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, tinhThanh);
         }

		 [AbpAuthorize(AppPermissions.Pages_TinhThanhs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _tinhThanhRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTinhThanhsToExcel(GetAllTinhThanhsForExcelInput input)
         {
			
			var filteredTinhThanhs = _tinhThanhRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Ma.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaFilter),  e => e.Ma == input.MaFilter);

			var query = (from o in filteredTinhThanhs
                         select new GetTinhThanhForViewDto() { 
							TinhThanh = new TinhThanhDto
							{
                                Name = o.Name,
                                Ma = o.Ma,
                                Id = o.Id
							}
						 });


            var tinhThanhListDtos = await query.ToListAsync();

            return _tinhThanhsExcelExporter.ExportToFile(tinhThanhListDtos);
         }


    }
}