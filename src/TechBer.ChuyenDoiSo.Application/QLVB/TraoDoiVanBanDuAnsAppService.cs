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
	[AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns)]
    public class TraoDoiVanBanDuAnsAppService : ChuyenDoiSoAppServiceBase, ITraoDoiVanBanDuAnsAppService
    {
		 private readonly IRepository<TraoDoiVanBanDuAn> _traoDoiVanBanDuAnRepository;
		 private readonly ITraoDoiVanBanDuAnsExcelExporter _traoDoiVanBanDuAnsExcelExporter;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 private readonly IRepository<VanBanDuAn,int> _lookup_vanBanDuAnRepository;


		 public TraoDoiVanBanDuAnsAppService(IRepository<TraoDoiVanBanDuAn> traoDoiVanBanDuAnRepository, ITraoDoiVanBanDuAnsExcelExporter traoDoiVanBanDuAnsExcelExporter , IRepository<User, long> lookup_userRepository, IRepository<VanBanDuAn, int> lookup_vanBanDuAnRepository) 
		  {
			_traoDoiVanBanDuAnRepository = traoDoiVanBanDuAnRepository;
			_traoDoiVanBanDuAnsExcelExporter = traoDoiVanBanDuAnsExcelExporter;
			_lookup_userRepository = lookup_userRepository;
		_lookup_vanBanDuAnRepository = lookup_vanBanDuAnRepository;
		
		  }

		 public async Task<PagedResultDto<GetTraoDoiVanBanDuAnForViewDto>> GetAll(GetAllTraoDoiVanBanDuAnsInput input)
         {
			
			var filteredTraoDoiVanBanDuAns = _traoDoiVanBanDuAnRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.VanBanDuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NoiDung.Contains(input.Filter))
						.WhereIf(input.MinNgayGuiFilter != null, e => e.NgayGui >= input.MinNgayGuiFilter)
						.WhereIf(input.MaxNgayGuiFilter != null, e => e.NgayGui <= input.MaxNgayGuiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoiDungFilter),  e => e.NoiDung == input.NoiDungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter), e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter);

			var pagedAndFilteredTraoDoiVanBanDuAns = filteredTraoDoiVanBanDuAns
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var traoDoiVanBanDuAns = from o in pagedAndFilteredTraoDoiVanBanDuAns
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTraoDoiVanBanDuAnForViewDto() {
							TraoDoiVanBanDuAn = new TraoDoiVanBanDuAnDto
							{
                                NgayGui = o.NgayGui,
                                NoiDung = o.NoiDung,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	VanBanDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredTraoDoiVanBanDuAns.CountAsync();

            return new PagedResultDto<GetTraoDoiVanBanDuAnForViewDto>(
                totalCount,
                await traoDoiVanBanDuAns.ToListAsync()
            );
         }
		 
		 public async Task<GetTraoDoiVanBanDuAnForViewDto> GetTraoDoiVanBanDuAnForView(int id)
         {
            var traoDoiVanBanDuAn = await _traoDoiVanBanDuAnRepository.GetAsync(id);

            var output = new GetTraoDoiVanBanDuAnForViewDto { TraoDoiVanBanDuAn = ObjectMapper.Map<TraoDoiVanBanDuAnDto>(traoDoiVanBanDuAn) };

		    if (output.TraoDoiVanBanDuAn.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TraoDoiVanBanDuAn.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.TraoDoiVanBanDuAn.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn = await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int)output.TraoDoiVanBanDuAn.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Edit)]
		 public async Task<GetTraoDoiVanBanDuAnForEditOutput> GetTraoDoiVanBanDuAnForEdit(EntityDto input)
         {
            var traoDoiVanBanDuAn = await _traoDoiVanBanDuAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetTraoDoiVanBanDuAnForEditOutput {TraoDoiVanBanDuAn = ObjectMapper.Map<CreateOrEditTraoDoiVanBanDuAnDto>(traoDoiVanBanDuAn)};

		    if (output.TraoDoiVanBanDuAn.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.TraoDoiVanBanDuAn.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

		    if (output.TraoDoiVanBanDuAn.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn = await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int)output.TraoDoiVanBanDuAn.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditTraoDoiVanBanDuAnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Create)]
		 protected virtual async Task Create(CreateOrEditTraoDoiVanBanDuAnDto input)
         {
            var traoDoiVanBanDuAn = ObjectMapper.Map<TraoDoiVanBanDuAn>(input);

			
			if (AbpSession.TenantId != null)
			{
				traoDoiVanBanDuAn.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _traoDoiVanBanDuAnRepository.InsertAsync(traoDoiVanBanDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Edit)]
		 protected virtual async Task Update(CreateOrEditTraoDoiVanBanDuAnDto input)
         {
            var traoDoiVanBanDuAn = await _traoDoiVanBanDuAnRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, traoDoiVanBanDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _traoDoiVanBanDuAnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetTraoDoiVanBanDuAnsToExcel(GetAllTraoDoiVanBanDuAnsForExcelInput input)
         {
			
			var filteredTraoDoiVanBanDuAns = _traoDoiVanBanDuAnRepository.GetAll()
						.Include( e => e.UserFk)
						.Include( e => e.VanBanDuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.NoiDung.Contains(input.Filter))
						.WhereIf(input.MinNgayGuiFilter != null, e => e.NgayGui >= input.MinNgayGuiFilter)
						.WhereIf(input.MaxNgayGuiFilter != null, e => e.NgayGui <= input.MaxNgayGuiFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.NoiDungFilter),  e => e.NoiDung == input.NoiDungFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter), e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter);

			var query = (from o in filteredTraoDoiVanBanDuAns
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetTraoDoiVanBanDuAnForViewDto() { 
							TraoDoiVanBanDuAn = new TraoDoiVanBanDuAnDto
							{
                                NgayGui = o.NgayGui,
                                NoiDung = o.NoiDung,
                                Id = o.Id
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	VanBanDuAnName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var traoDoiVanBanDuAnListDtos = await query.ToListAsync();

            return _traoDoiVanBanDuAnsExcelExporter.ExportToFile(traoDoiVanBanDuAnListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns)]
         public async Task<PagedResultDto<TraoDoiVanBanDuAnUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TraoDoiVanBanDuAnUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new TraoDoiVanBanDuAnUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<TraoDoiVanBanDuAnUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns)]
         public async Task<PagedResultDto<TraoDoiVanBanDuAnVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_vanBanDuAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var vanBanDuAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<TraoDoiVanBanDuAnVanBanDuAnLookupTableDto>();
			foreach(var vanBanDuAn in vanBanDuAnList){
				lookupTableDtoList.Add(new TraoDoiVanBanDuAnVanBanDuAnLookupTableDto
				{
					Id = vanBanDuAn.Id,
					DisplayName = vanBanDuAn.Name?.ToString()
				});
			}

            return new PagedResultDto<TraoDoiVanBanDuAnVanBanDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

         public async Task<GetHienThiTraoDoiDto> GetHienThiTraoDoi(int id)
         {
	         var traoDoi = _traoDoiVanBanDuAnRepository.GetAll().WhereIf(true, p => p.VanBanDuAnId == id).ToList();

	         var traoDoiUsername = ObjectMapper.Map<List<TraoDoiVanBanDuAnDto>>(traoDoi);
	         foreach (var VARIABLE in traoDoiUsername)
	         {
		         var user = _lookup_userRepository.FirstOrDefault((long) VARIABLE.UserId);
		         VARIABLE.UserName = user.Surname + " " + user.Name;
	         }
	         return new GetHienThiTraoDoiDto()
	         {
		         ListTraoDoiVanBanDuAn = traoDoiUsername
	         };
         }
         public async Task GuiTraoDoi(TraoDoiVanBanDuAnDto input)
         {
	         var traoDoiVanBanDuAn = ObjectMapper.Map<TraoDoiVanBanDuAn>(input);
	         
	         if (AbpSession.TenantId != null)
	         {
		         traoDoiVanBanDuAn.TenantId = (int?) AbpSession.TenantId;
	         }
	         traoDoiVanBanDuAn.NgayGui = DateTime.Now;
	         traoDoiVanBanDuAn.UserId = AbpSession.UserId;
		

	         await _traoDoiVanBanDuAnRepository.InsertAsync(traoDoiVanBanDuAn);
         }
    }
}