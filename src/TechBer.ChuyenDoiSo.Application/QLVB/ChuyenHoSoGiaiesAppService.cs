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
	[AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies)]
    public class ChuyenHoSoGiaiesAppService : ChuyenDoiSoAppServiceBase, IChuyenHoSoGiaiesAppService
    {
		 private readonly IRepository<ChuyenHoSoGiay> _chuyenHoSoGiayRepository;
		 private readonly IChuyenHoSoGiaiesExcelExporter _chuyenHoSoGiaiesExcelExporter;
		 private readonly IRepository<VanBanDuAn,int> _lookup_vanBanDuAnRepository;
		 private readonly IRepository<User,long> _lookup_userRepository;
		 

		  public ChuyenHoSoGiaiesAppService(IRepository<ChuyenHoSoGiay> chuyenHoSoGiayRepository, IChuyenHoSoGiaiesExcelExporter chuyenHoSoGiaiesExcelExporter , IRepository<VanBanDuAn, int> lookup_vanBanDuAnRepository, IRepository<User, long> lookup_userRepository) 
		  {
			_chuyenHoSoGiayRepository = chuyenHoSoGiayRepository;
			_chuyenHoSoGiaiesExcelExporter = chuyenHoSoGiaiesExcelExporter;
			_lookup_vanBanDuAnRepository = lookup_vanBanDuAnRepository;
		_lookup_userRepository = lookup_userRepository;
		
		  }

		 public async Task<PagedResultDto<GetChuyenHoSoGiayForViewDto>> GetAll(GetAllChuyenHoSoGiaiesInput input)
         {
			
			var filteredChuyenHoSoGiaies = _chuyenHoSoGiayRepository.GetAll()
						.Include( e => e.VanBanDuAnFk)
						.Include( e => e.NguoiNhanFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinNguoiChuyenIdFilter != null, e => e.NguoiChuyenId >= input.MinNguoiChuyenIdFilter)
						.WhereIf(input.MaxNguoiChuyenIdFilter != null, e => e.NguoiChuyenId <= input.MaxNguoiChuyenIdFilter)
						.WhereIf(input.MinThoiGianChuyenFilter != null, e => e.ThoiGianChuyen >= input.MinThoiGianChuyenFilter)
						.WhereIf(input.MaxThoiGianChuyenFilter != null, e => e.ThoiGianChuyen <= input.MaxThoiGianChuyenFilter)
						.WhereIf(input.MinSoLuongFilter != null, e => e.SoLuong >= input.MinSoLuongFilter)
						.WhereIf(input.MaxSoLuongFilter != null, e => e.SoLuong <= input.MaxSoLuongFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(input.MinThoiGianNhanFilter != null, e => e.ThoiGianNhan >= input.MinThoiGianNhanFilter)
						.WhereIf(input.MaxThoiGianNhanFilter != null, e => e.ThoiGianNhan <= input.MaxThoiGianNhanFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter), e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.NguoiNhanFk != null && e.NguoiNhanFk.Name == input.UserNameFilter);

			var pagedAndFilteredChuyenHoSoGiaies = filteredChuyenHoSoGiaies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chuyenHoSoGiaies = from o in pagedAndFilteredChuyenHoSoGiaies
                         join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.NguoiNhanId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetChuyenHoSoGiayForViewDto() {
							ChuyenHoSoGiay = new ChuyenHoSoGiayDto
							{
                                NguoiChuyenId = o.NguoiChuyenId,
                                ThoiGianChuyen = o.ThoiGianChuyen,
                                SoLuong = o.SoLuong,
                                TrangThai = o.TrangThai,
                                ThoiGianNhan = o.ThoiGianNhan,
                                Id = o.Id
							},
                         	VanBanDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredChuyenHoSoGiaies.CountAsync();

            return new PagedResultDto<GetChuyenHoSoGiayForViewDto>(
                totalCount,
                await chuyenHoSoGiaies.ToListAsync()
            );
         }
		 
		 public async Task<GetChuyenHoSoGiayForViewDto> GetChuyenHoSoGiayForView(int id)
         {
            var chuyenHoSoGiay = await _chuyenHoSoGiayRepository.GetAsync(id);

            var output = new GetChuyenHoSoGiayForViewDto { ChuyenHoSoGiay = ObjectMapper.Map<ChuyenHoSoGiayDto>(chuyenHoSoGiay) };

		    if (output.ChuyenHoSoGiay.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn = await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int)output.ChuyenHoSoGiay.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }

		    if (output.ChuyenHoSoGiay.NguoiNhanId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ChuyenHoSoGiay.NguoiNhanId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
		 public async Task<GetChuyenHoSoGiayForEditOutput> GetChuyenHoSoGiayForEdit(EntityDto input)
         {
            var chuyenHoSoGiay = await _chuyenHoSoGiayRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChuyenHoSoGiayForEditOutput {ChuyenHoSoGiay = ObjectMapper.Map<CreateOrEditChuyenHoSoGiayDto>(chuyenHoSoGiay)};

		    if (output.ChuyenHoSoGiay.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn = await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int)output.ChuyenHoSoGiay.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }

		    if (output.ChuyenHoSoGiay.NguoiNhanId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.ChuyenHoSoGiay.NguoiNhanId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChuyenHoSoGiayDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Create)]
		 protected virtual async Task Create(CreateOrEditChuyenHoSoGiayDto input)
         {
            var chuyenHoSoGiay = ObjectMapper.Map<ChuyenHoSoGiay>(input);

			
			if (AbpSession.TenantId != null)
			{
				chuyenHoSoGiay.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _chuyenHoSoGiayRepository.InsertAsync(chuyenHoSoGiay);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
		 protected virtual async Task Update(CreateOrEditChuyenHoSoGiayDto input)
         {
            var chuyenHoSoGiay = await _chuyenHoSoGiayRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, chuyenHoSoGiay);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _chuyenHoSoGiayRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetChuyenHoSoGiaiesToExcel(GetAllChuyenHoSoGiaiesForExcelInput input)
         {
			
			var filteredChuyenHoSoGiaies = _chuyenHoSoGiayRepository.GetAll()
						.Include( e => e.VanBanDuAnFk)
						.Include( e => e.NguoiNhanFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false )
						.WhereIf(input.MinNguoiChuyenIdFilter != null, e => e.NguoiChuyenId >= input.MinNguoiChuyenIdFilter)
						.WhereIf(input.MaxNguoiChuyenIdFilter != null, e => e.NguoiChuyenId <= input.MaxNguoiChuyenIdFilter)
						.WhereIf(input.MinThoiGianChuyenFilter != null, e => e.ThoiGianChuyen >= input.MinThoiGianChuyenFilter)
						.WhereIf(input.MaxThoiGianChuyenFilter != null, e => e.ThoiGianChuyen <= input.MaxThoiGianChuyenFilter)
						.WhereIf(input.MinSoLuongFilter != null, e => e.SoLuong >= input.MinSoLuongFilter)
						.WhereIf(input.MaxSoLuongFilter != null, e => e.SoLuong <= input.MaxSoLuongFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
						.WhereIf(input.MinThoiGianNhanFilter != null, e => e.ThoiGianNhan >= input.MinThoiGianNhanFilter)
						.WhereIf(input.MaxThoiGianNhanFilter != null, e => e.ThoiGianNhan <= input.MaxThoiGianNhanFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter), e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.NguoiNhanFk != null && e.NguoiNhanFk.Name == input.UserNameFilter);

			var query = (from o in filteredChuyenHoSoGiaies
                         join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_userRepository.GetAll() on o.NguoiNhanId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetChuyenHoSoGiayForViewDto() { 
							ChuyenHoSoGiay = new ChuyenHoSoGiayDto
							{
                                NguoiChuyenId = o.NguoiChuyenId,
                                ThoiGianChuyen = o.ThoiGianChuyen,
                                SoLuong = o.SoLuong,
                                TrangThai = o.TrangThai,
                                ThoiGianNhan = o.ThoiGianNhan,
                                Id = o.Id
							},
                         	VanBanDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						 });


            var chuyenHoSoGiayListDtos = await query.ToListAsync();

            return _chuyenHoSoGiaiesExcelExporter.ExportToFile(chuyenHoSoGiayListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies)]
         public async Task<PagedResultDto<ChuyenHoSoGiayVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_vanBanDuAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var vanBanDuAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChuyenHoSoGiayVanBanDuAnLookupTableDto>();
			foreach(var vanBanDuAn in vanBanDuAnList){
				lookupTableDtoList.Add(new ChuyenHoSoGiayVanBanDuAnLookupTableDto
				{
					Id = vanBanDuAn.Id,
					DisplayName = vanBanDuAn.Name?.ToString()
				});
			}

            return new PagedResultDto<ChuyenHoSoGiayVanBanDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }

		[AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies)]
         public async Task<PagedResultDto<ChuyenHoSoGiayUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_userRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChuyenHoSoGiayUserLookupTableDto>();
			foreach(var user in userList){
				lookupTableDtoList.Add(new ChuyenHoSoGiayUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user.Name?.ToString()
				});
			}

            return new PagedResultDto<ChuyenHoSoGiayUserLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}