

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
	[AbpAuthorize(AppPermissions.Pages_LoaiDuAns)]
    public class LoaiDuAnsAppService : ChuyenDoiSoAppServiceBase, ILoaiDuAnsAppService
    {
		 private readonly IRepository<LoaiDuAn> _loaiDuAnRepository;
		 private readonly ILoaiDuAnsExcelExporter _loaiDuAnsExcelExporter;
		 

		  public LoaiDuAnsAppService(IRepository<LoaiDuAn> loaiDuAnRepository, ILoaiDuAnsExcelExporter loaiDuAnsExcelExporter ) 
		  {
			_loaiDuAnRepository = loaiDuAnRepository;
			_loaiDuAnsExcelExporter = loaiDuAnsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetLoaiDuAnForViewDto>> GetAll(GetAllLoaiDuAnsInput input)
         {
			
			var filteredLoaiDuAns = _loaiDuAnRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var pagedAndFilteredLoaiDuAns = filteredLoaiDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var loaiDuAns = from o in pagedAndFilteredLoaiDuAns
                         select new GetLoaiDuAnForViewDto() {
							LoaiDuAn = new LoaiDuAnDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLoaiDuAns.CountAsync();

            return new PagedResultDto<GetLoaiDuAnForViewDto>(
                totalCount,
                await loaiDuAns.ToListAsync()
            );
         }
		 
		 public async Task<GetLoaiDuAnForViewDto> GetLoaiDuAnForView(int id)
         {
            var loaiDuAn = await _loaiDuAnRepository.GetAsync(id);

            var output = new GetLoaiDuAnForViewDto { LoaiDuAn = ObjectMapper.Map<LoaiDuAnDto>(loaiDuAn) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LoaiDuAns_Edit)]
		 public async Task<GetLoaiDuAnForEditOutput> GetLoaiDuAnForEdit(EntityDto input)
         {
            var loaiDuAn = await _loaiDuAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLoaiDuAnForEditOutput {LoaiDuAn = ObjectMapper.Map<CreateOrEditLoaiDuAnDto>(loaiDuAn)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLoaiDuAnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LoaiDuAns_Create)]
		 protected virtual async Task Create(CreateOrEditLoaiDuAnDto input)
         {
            var loaiDuAn = ObjectMapper.Map<LoaiDuAn>(input);

			
			if (AbpSession.TenantId != null)
			{
				loaiDuAn.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _loaiDuAnRepository.InsertAsync(loaiDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_LoaiDuAns_Edit)]
		 protected virtual async Task Update(CreateOrEditLoaiDuAnDto input)
         {
            var loaiDuAn = await _loaiDuAnRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, loaiDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_LoaiDuAns_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _loaiDuAnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLoaiDuAnsToExcel(GetAllLoaiDuAnsForExcelInput input)
         {
			
			var filteredLoaiDuAns = _loaiDuAnRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter);

			var query = (from o in filteredLoaiDuAns
                         select new GetLoaiDuAnForViewDto() { 
							LoaiDuAn = new LoaiDuAnDto
							{
                                Name = o.Name,
                                Id = o.Id
							}
						 });


            var loaiDuAnListDtos = await query.ToListAsync();

            return _loaiDuAnsExcelExporter.ExportToFile(loaiDuAnListDtos);
         }


    }
}