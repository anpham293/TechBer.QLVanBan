using Abp.Organizations;


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
		 private readonly IRepository<OrganizationUnit,long> _lookup_organizationUnitRepository;
		 

		  public LoaiDuAnsAppService(IRepository<LoaiDuAn> loaiDuAnRepository, ILoaiDuAnsExcelExporter loaiDuAnsExcelExporter , IRepository<OrganizationUnit, long> lookup_organizationUnitRepository) 
		  {
			_loaiDuAnRepository = loaiDuAnRepository;
			_loaiDuAnsExcelExporter = loaiDuAnsExcelExporter;
			_lookup_organizationUnitRepository = lookup_organizationUnitRepository;
		
		  }

		 public async Task<PagedResultDto<GetLoaiDuAnForViewDto>> GetAll(GetAllLoaiDuAnsInput input)
         {
			
			var filteredLoaiDuAns = _loaiDuAnRepository.GetAll()
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var pagedAndFilteredLoaiDuAns = filteredLoaiDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var loaiDuAns = from o in pagedAndFilteredLoaiDuAns
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetLoaiDuAnForViewDto() {
							LoaiDuAn = new LoaiDuAnDto
							{
                                Name = o.Name,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null || s1.DisplayName == null ? "" : s1.DisplayName.ToString()
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

		    if (output.LoaiDuAn.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.LoaiDuAn.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LoaiDuAns_Edit)]
		 public async Task<GetLoaiDuAnForEditOutput> GetLoaiDuAnForEdit(EntityDto input)
         {
            var loaiDuAn = await _loaiDuAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLoaiDuAnForEditOutput {LoaiDuAn = ObjectMapper.Map<CreateOrEditLoaiDuAnDto>(loaiDuAn)};

		    if (output.LoaiDuAn.OrganizationUnitId != null)
            {
                var _lookupOrganizationUnit = await _lookup_organizationUnitRepository.FirstOrDefaultAsync((long)output.LoaiDuAn.OrganizationUnitId);
                output.OrganizationUnitDisplayName = _lookupOrganizationUnit?.DisplayName?.ToString();
            }
			
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
						.Include( e => e.OrganizationUnitFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.OrganizationUnitDisplayNameFilter), e => e.OrganizationUnitFk != null && e.OrganizationUnitFk.DisplayName == input.OrganizationUnitDisplayNameFilter);

			var query = (from o in filteredLoaiDuAns
                         join o1 in _lookup_organizationUnitRepository.GetAll() on o.OrganizationUnitId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetLoaiDuAnForViewDto() { 
							LoaiDuAn = new LoaiDuAnDto
							{
                                Name = o.Name,
                                Id = o.Id
							},
                         	OrganizationUnitDisplayName = s1 == null || s1.DisplayName == null ? "" : s1.DisplayName.ToString()
						 });


            var loaiDuAnListDtos = await query.ToListAsync();

            return _loaiDuAnsExcelExporter.ExportToFile(loaiDuAnListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_LoaiDuAns)]
         public async Task<PagedResultDto<LoaiDuAnOrganizationUnitLookupTableDto>> GetAllOrganizationUnitForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_organizationUnitRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.DisplayName != null && e.DisplayName.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var organizationUnitList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<LoaiDuAnOrganizationUnitLookupTableDto>();
			foreach(var organizationUnit in organizationUnitList){
				lookupTableDtoList.Add(new LoaiDuAnOrganizationUnitLookupTableDto
				{
					Id = organizationUnit.Id,
					DisplayName = organizationUnit.DisplayName?.ToString()
				});
			}

            return new PagedResultDto<LoaiDuAnOrganizationUnitLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}