using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.Authorization.Users;


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
	[AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns)]
    public class BaoCaoVanBanDuAnsAppService : ChuyenDoiSoAppServiceBase, IBaoCaoVanBanDuAnsAppService
    {
		 private readonly IRepository<BaoCaoVanBanDuAn> _baoCaoVanBanDuAnRepository;
		 private readonly IBaoCaoVanBanDuAnsExcelExporter _baoCaoVanBanDuAnsExcelExporter;
		 private readonly IRepository<VanBanDuAn,int> _lookup_vanBanDuAnRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public BaoCaoVanBanDuAnsAppService(IRepository<BaoCaoVanBanDuAn> baoCaoVanBanDuAnRepository, IBaoCaoVanBanDuAnsExcelExporter baoCaoVanBanDuAnsExcelExporter , IRepository<VanBanDuAn, int> lookup_vanBanDuAnRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_baoCaoVanBanDuAnRepository = baoCaoVanBanDuAnRepository;
			_baoCaoVanBanDuAnsExcelExporter = baoCaoVanBanDuAnsExcelExporter;
			_lookup_vanBanDuAnRepository = lookup_vanBanDuAnRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetBaoCaoVanBanDuAnForViewDto>> GetAll(GetAllBaoCaoVanBanDuAnsInput input)
         {
			
			var filteredBaoCaoVanBanDuAns = _baoCaoVanBanDuAnRepository.GetAll()
						.Include( e => e.VanBanDuAnFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NoiDungCongViec.Contains(input.Filter) || e.MoTaChiTiet.Contains(input.Filter) || e.FileBaoCao.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoiDungCongViecFilter),  e => e.NoiDungCongViec == input.NoiDungCongViecFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaChiTietFilter),  e => e.MoTaChiTiet == input.MoTaChiTietFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FileBaoCaoFilter),  e => e.FileBaoCao == input.FileBaoCaoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter), e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var pagedAndFilteredBaoCaoVanBanDuAns = filteredBaoCaoVanBanDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var baoCaoVanBanDuAns = from o in pagedAndFilteredBaoCaoVanBanDuAns
                         join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBaoCaoVanBanDuAnForViewDto() {
							BaoCaoVanBanDuAn = new BaoCaoVanBanDuAnDto
							{
                                NoiDungCongViec = o.NoiDungCongViec,
                                MoTaChiTiet = o.MoTaChiTiet,
                                FileBaoCao = o.FileBaoCao,
                                Id = o.Id
							},
                         	VanBanDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredBaoCaoVanBanDuAns.CountAsync();

            return new PagedResultDto<GetBaoCaoVanBanDuAnForViewDto>(
                totalCount,
                await baoCaoVanBanDuAns.ToListAsync()
            );
         }
		 
		 public async Task<GetBaoCaoVanBanDuAnForViewDto> GetBaoCaoVanBanDuAnForView(int id)
         {
            var baoCaoVanBanDuAn = await _baoCaoVanBanDuAnRepository.GetAsync(id);

            var output = new GetBaoCaoVanBanDuAnForViewDto { BaoCaoVanBanDuAn = ObjectMapper.Map<BaoCaoVanBanDuAnDto>(baoCaoVanBanDuAn) };

		    if (output.BaoCaoVanBanDuAn.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn = await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int)output.BaoCaoVanBanDuAn.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }

		    if (output.BaoCaoVanBanDuAn.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BaoCaoVanBanDuAn.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
		 public async Task<GetBaoCaoVanBanDuAnForEditOutput> GetBaoCaoVanBanDuAnForEdit(EntityDto input)
         {
            var baoCaoVanBanDuAn = await _baoCaoVanBanDuAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetBaoCaoVanBanDuAnForEditOutput {BaoCaoVanBanDuAn = ObjectMapper.Map<CreateOrEditBaoCaoVanBanDuAnDto>(baoCaoVanBanDuAn)};

		    if (output.BaoCaoVanBanDuAn.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn = await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int)output.BaoCaoVanBanDuAn.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }

		    if (output.BaoCaoVanBanDuAn.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.BaoCaoVanBanDuAn.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditBaoCaoVanBanDuAnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create)]
		 protected virtual async Task Create(CreateOrEditBaoCaoVanBanDuAnDto input)
         {
            var baoCaoVanBanDuAn = ObjectMapper.Map<BaoCaoVanBanDuAn>(input);

			
			if (AbpSession.TenantId != null)
			{
				baoCaoVanBanDuAn.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _baoCaoVanBanDuAnRepository.InsertAsync(baoCaoVanBanDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
		 protected virtual async Task Update(CreateOrEditBaoCaoVanBanDuAnDto input)
         {
            var baoCaoVanBanDuAn = await _baoCaoVanBanDuAnRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, baoCaoVanBanDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _baoCaoVanBanDuAnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetBaoCaoVanBanDuAnsToExcel(GetAllBaoCaoVanBanDuAnsForExcelInput input)
         {
			
			var filteredBaoCaoVanBanDuAns = _baoCaoVanBanDuAnRepository.GetAll()
						.Include( e => e.VanBanDuAnFk)
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NoiDungCongViec.Contains(input.Filter) || e.MoTaChiTiet.Contains(input.Filter) || e.FileBaoCao.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoiDungCongViecFilter),  e => e.NoiDungCongViec == input.NoiDungCongViecFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MoTaChiTietFilter),  e => e.MoTaChiTiet == input.MoTaChiTietFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FileBaoCaoFilter),  e => e.FileBaoCao == input.FileBaoCaoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter), e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var query = (from o in filteredBaoCaoVanBanDuAns
                         join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.UserId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetBaoCaoVanBanDuAnForViewDto() { 
							BaoCaoVanBanDuAn = new BaoCaoVanBanDuAnDto
							{
                                NoiDungCongViec = o.NoiDungCongViec,
                                MoTaChiTiet = o.MoTaChiTiet,
                                FileBaoCao = o.FileBaoCao,
                                Id = o.Id
							},
                         	VanBanDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var baoCaoVanBanDuAnListDtos = await query.ToListAsync();

            return _baoCaoVanBanDuAnsExcelExporter.ExportToFile(baoCaoVanBanDuAnListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns)]
         public async Task<PagedResultDto<BaoCaoVanBanDuAnVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_vanBanDuAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var vanBanDuAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BaoCaoVanBanDuAnVanBanDuAnLookupTableDto>();
			foreach(var vanBanDuAn in vanBanDuAnList){
				lookupTableDtoList.Add(new BaoCaoVanBanDuAnVanBanDuAnLookupTableDto
				{
					Id = vanBanDuAn.Id,
					DisplayName = vanBanDuAn.Name?.ToString()
				});
			}

            return new PagedResultDto<BaoCaoVanBanDuAnVanBanDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns)]
         public async Task<PagedResultDto<BaoCaoVanBanDuAnUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<BaoCaoVanBanDuAnUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new BaoCaoVanBanDuAnUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<BaoCaoVanBanDuAnUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}