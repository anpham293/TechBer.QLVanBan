using TechBer.ChuyenDoiSo.Authorization.Users;
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
	[AbpAuthorize(AppPermissions.Pages_UserDuAns)]
    public class UserDuAnsAppService : ChuyenDoiSoAppServiceBase, IUserDuAnsAppService
    {
		 private readonly IRepository<UserDuAn, long> _userDuAnRepository;
		 private readonly IUserDuAnsExcelExporter _userDuAnsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<DuAn,int> _lookup_duAnRepository;
		 

		  public UserDuAnsAppService(IRepository<UserDuAn, long> userDuAnRepository, IUserDuAnsExcelExporter userDuAnsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<DuAn, int> lookup_duAnRepository) 
		  {
			_userDuAnRepository = userDuAnRepository;
			_userDuAnsExcelExporter = userDuAnsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_duAnRepository = lookup_duAnRepository;
		
		  }

		 public async Task<PagedResultDto<GetUserDuAnForViewDto>> GetAll(GetAllUserDuAnsInput input)
         {
			
			var filteredUserDuAns = _userDuAnRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter), e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter);

			var pagedAndFilteredUserDuAns = filteredUserDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var userDuAns = from o in pagedAndFilteredUserDuAns
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserDuAnForViewDto() {
							UserDuAn = new UserDuAnDto
							{
                                TrangThai = o.TrangThai,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	DuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredUserDuAns.CountAsync();

            return new PagedResultDto<GetUserDuAnForViewDto>(
                totalCount,
                await userDuAns.ToListAsync()
            );
         }
		 
		 public async Task<GetUserDuAnForViewDto> GetUserDuAnForView(long id)
         {
            var userDuAn = await _userDuAnRepository.GetAsync(id);

            var output = new GetUserDuAnForViewDto { UserDuAn = ObjectMapper.Map<UserDuAnDto>(userDuAn) };

		    if (output.UserDuAn.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDuAn.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.UserDuAn.DuAnId != null)
            {
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int)output.UserDuAn.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_UserDuAns_Edit)]
		 public async Task<GetUserDuAnForEditOutput> GetUserDuAnForEdit(EntityDto<long> input)
         {
            var userDuAn = await _userDuAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetUserDuAnForEditOutput {UserDuAn = ObjectMapper.Map<CreateOrEditUserDuAnDto>(userDuAn)};

		    if (output.UserDuAn.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.UserDuAn.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.UserDuAn.DuAnId != null)
            {
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int)output.UserDuAn.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditUserDuAnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDuAns_Create)]
		 protected virtual async Task Create(CreateOrEditUserDuAnDto input)
         {
            var userDuAn = ObjectMapper.Map<UserDuAn>(input);

			
			if (AbpSession.TenantId != null)
			{
				userDuAn.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _userDuAnRepository.InsertAsync(userDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDuAns_Edit)]
		 protected virtual async Task Update(CreateOrEditUserDuAnDto input)
         {
            var userDuAn = await _userDuAnRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, userDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_UserDuAns_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _userDuAnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetUserDuAnsToExcel(GetAllUserDuAnsForExcelInput input)
         {
			
			var filteredUserDuAns = _userDuAnRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.DuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DuAnNameFilter), e => e.DuAnFk != null && e.DuAnFk.Name == input.DuAnNameFilter);

			var query = (from o in filteredUserDuAns
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_duAnRepository.GetAll() on o.DuAnId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetUserDuAnForViewDto() { 
							UserDuAn = new UserDuAnDto
							{
                                TrangThai = o.TrangThai,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	DuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var userDuAnListDtos = await query.ToListAsync();

            return _userDuAnsExcelExporter.ExportToFile(userDuAnListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_UserDuAns)]
         public async Task<PagedResultDto<UserDuAnUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserDuAnUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new UserDuAnUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<UserDuAnUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_UserDuAns)]
         public async Task<PagedResultDto<UserDuAnDuAnLookupTableDto>> GetAllDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_duAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var duAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<UserDuAnDuAnLookupTableDto>();
			foreach(var duAn in duAnList){
				lookupTableDtoList.Add(new UserDuAnDuAnLookupTableDto
				{
					Id = duAn.Id,
					DisplayName = duAn.Name?.ToString()
				});
			}

            return new PagedResultDto<UserDuAnDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}