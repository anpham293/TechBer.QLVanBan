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
        private readonly IRepository<VanBanDuAn, int> _lookup_vanBanDuAnRepository;
        private readonly IRepository<QuyTrinhDuAnAssigned, long> _lookup_quyTrinhDuAnAssignedRepository;
        private readonly IRepository<DuAn, int> _lookup_duAnRepository;
        private readonly IRepository<User, long> _lookup_userRepository;


        public ChuyenHoSoGiaiesAppService(IRepository<ChuyenHoSoGiay> chuyenHoSoGiayRepository,
            IChuyenHoSoGiaiesExcelExporter chuyenHoSoGiaiesExcelExporter,
            IRepository<VanBanDuAn, int> lookup_vanBanDuAnRepository, 
            IRepository<User, long> lookup_userRepository,
            IRepository<QuyTrinhDuAnAssigned, long> lookup_quyTrinhDuAnAssignedRepository,
            IRepository<DuAn, int> lookup_duAnRepository
        )
        {
            _chuyenHoSoGiayRepository = chuyenHoSoGiayRepository;
            _chuyenHoSoGiaiesExcelExporter = chuyenHoSoGiaiesExcelExporter;
            _lookup_vanBanDuAnRepository = lookup_vanBanDuAnRepository;
            _lookup_userRepository = lookup_userRepository;
            _lookup_quyTrinhDuAnAssignedRepository = lookup_quyTrinhDuAnAssignedRepository;
            _lookup_duAnRepository = lookup_duAnRepository;
        }

        public async Task<PagedResultDto<GetChuyenHoSoGiayForViewDto>> GetAll(GetAllChuyenHoSoGiaiesInput input)
        {
            var filteredChuyenHoSoGiaies = _chuyenHoSoGiayRepository.GetAll()
                    .Include(e => e.VanBanDuAnFk)
                    .Include(e => e.NguoiNhanFk)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                    .WhereIf(input.MinNguoiChuyenIdFilter != null, e => e.NguoiChuyenId >= input.MinNguoiChuyenIdFilter)
                    .WhereIf(input.MaxNguoiChuyenIdFilter != null, e => e.NguoiChuyenId <= input.MaxNguoiChuyenIdFilter)
                    .WhereIf(input.MinThoiGianChuyenFilter != null,
                        e => e.ThoiGianChuyen >= input.MinThoiGianChuyenFilter)
                    .WhereIf(input.MaxThoiGianChuyenFilter != null,
                        e => e.ThoiGianChuyen <= input.MaxThoiGianChuyenFilter)
                    .WhereIf(input.MinSoLuongFilter != null, e => e.SoLuong >= input.MinSoLuongFilter)
                    .WhereIf(input.MaxSoLuongFilter != null, e => e.SoLuong <= input.MaxSoLuongFilter)
                    .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                    .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter)
                    .WhereIf(input.MinThoiGianNhanFilter != null, e => e.ThoiGianNhan >= input.MinThoiGianNhanFilter)
                    .WhereIf(input.MaxThoiGianNhanFilter != null, e => e.ThoiGianNhan <= input.MaxThoiGianNhanFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter),
                        e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                        e => e.NguoiNhanFk != null && e.NguoiNhanFk.Name == input.UserNameFilter)
                    .WhereIf(true, e => e.NguoiNhanId == AbpSession.UserId || e.NguoiChuyenId == AbpSession.UserId)
                ;

            var pagedAndFilteredChuyenHoSoGiaies = filteredChuyenHoSoGiaies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var chuyenHoSoGiaies = from o in pagedAndFilteredChuyenHoSoGiaies
                join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_userRepository.GetAll() on o.NguoiNhanId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookup_userRepository.GetAll() on o.NguoiChuyenId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                select new GetChuyenHoSoGiayForViewDto()
                {
                    ChuyenHoSoGiay = new ChuyenHoSoGiayDto
                    {
                        NguoiChuyenId = o.NguoiChuyenId,
                        ThoiGianChuyen = o.ThoiGianChuyen,
                        SoLuong = o.SoLuong,
                        TrangThai = o.TrangThai,
                        ThoiGianNhan = o.ThoiGianNhan,
                        Id = o.Id,
                        NguoiNhanId = (int) o.NguoiNhanId
                    },
                    VanBanDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                    TenNguoiNhan = s2 == null || s2.Name == null ? "" : (s2.Surname + " " + s2.Name).ToString(),
                    TenNguoiChuyen = s3 == null || s3.Name == null ? "" : (s3.Surname + " " + s3.Name).ToString(),
                    UserId = (int) AbpSession.UserId
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

            var output = new GetChuyenHoSoGiayForViewDto
                {ChuyenHoSoGiay = ObjectMapper.Map<ChuyenHoSoGiayDto>(chuyenHoSoGiay)};

            if (output.ChuyenHoSoGiay.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn =
                    await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int) output.ChuyenHoSoGiay.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
                var _lookupQuyTrinhDuAn =
                    await _lookup_quyTrinhDuAnAssignedRepository.FirstOrDefaultAsync((long) _lookupVanBanDuAn.QuyTrinhDuAnAssignedId);
                output.QuyTrinhDuAnName = _lookupQuyTrinhDuAn?.Name?.ToString();
                var _lookupDuAn = await _lookup_duAnRepository.FirstOrDefaultAsync((int) _lookupVanBanDuAn.DuAnId);
                output.DuAnName = _lookupDuAn?.Name?.ToString();
            }

            if (output.ChuyenHoSoGiay.NguoiNhanId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long) output.ChuyenHoSoGiay.NguoiNhanId);
                output.UserName = _lookupUser?.Name?.ToString();
                output.TenNguoiNhan = _lookupUser?.Surname + " " +  _lookupUser?.Name;
            }
            if (output.ChuyenHoSoGiay.NguoiChuyenId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync(output.ChuyenHoSoGiay.NguoiChuyenId);
                output.TenNguoiChuyen = _lookupUser?.Surname + " " +  _lookupUser?.Name;
            }

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
        public async Task<GetChuyenHoSoGiayForEditOutput> GetChuyenHoSoGiayForEdit(EntityDto input)
        {
            var chuyenHoSoGiay = await _chuyenHoSoGiayRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetChuyenHoSoGiayForEditOutput
                {ChuyenHoSoGiay = ObjectMapper.Map<CreateOrEditChuyenHoSoGiayDto>(chuyenHoSoGiay)};

            if (output.ChuyenHoSoGiay.VanBanDuAnId != null)
            {
                var _lookupVanBanDuAn =
                    await _lookup_vanBanDuAnRepository.FirstOrDefaultAsync((int) output.ChuyenHoSoGiay.VanBanDuAnId);
                output.VanBanDuAnName = _lookupVanBanDuAn?.Name?.ToString();
            }

            if (output.ChuyenHoSoGiay.NguoiNhanId != null)
            {
                var _lookupUser =
                    await _lookup_userRepository.FirstOrDefaultAsync((long) output.ChuyenHoSoGiay.NguoiNhanId);
                output.UserName = _lookupUser?.Name?.ToString();
            }

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditChuyenHoSoGiayDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Create)]
        protected virtual async Task Create(CreateOrEditChuyenHoSoGiayDto input)
        {
            //var chuyenHoSoGiay = ObjectMapper.Map<ChuyenHoSoGiay>(input);
            ChuyenHoSoGiay chuyenHoSoGiay = new ChuyenHoSoGiay();
            chuyenHoSoGiay.NguoiChuyenId = (int) AbpSession.UserId;
            chuyenHoSoGiay.ThoiGianChuyen = DateTime.Now;
            chuyenHoSoGiay.SoLuong = input.SoLuong;
            chuyenHoSoGiay.NguoiNhanId = input.NguoiNhanId;
            chuyenHoSoGiay.VanBanDuAnId = input.VanBanDuAnId;
            chuyenHoSoGiay.TrangThai = TrangThaiChuyenHoSoGiayConst.CHUA_GUI_HO_SO;

            if (AbpSession.TenantId != null)
            {
                chuyenHoSoGiay.TenantId = (int?) AbpSession.TenantId;
            }


            await _chuyenHoSoGiayRepository.InsertAsync(chuyenHoSoGiay);
        }

        [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
        protected virtual async Task Update(CreateOrEditChuyenHoSoGiayDto input)
        {
            var chuyenHoSoGiay = await _chuyenHoSoGiayRepository.FirstOrDefaultAsync((int) input.Id);
            chuyenHoSoGiay.NguoiNhanId = input.NguoiNhanId;
            chuyenHoSoGiay.SoLuong = input.SoLuong;
            _chuyenHoSoGiayRepository.UpdateAsync(chuyenHoSoGiay);
            //ObjectMapper.Map(input, chuyenHoSoGiay);
        }

        [AbpAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _chuyenHoSoGiayRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetChuyenHoSoGiaiesToExcel(GetAllChuyenHoSoGiaiesForExcelInput input)
        {
            var filteredChuyenHoSoGiaies = _chuyenHoSoGiayRepository.GetAll()
                .Include(e => e.VanBanDuAnFk)
                .Include(e => e.NguoiNhanFk)
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
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
                .WhereIf(!string.IsNullOrWhiteSpace(input.VanBanDuAnNameFilter),
                    e => e.VanBanDuAnFk != null && e.VanBanDuAnFk.Name == input.VanBanDuAnNameFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter),
                    e => e.NguoiNhanFk != null && e.NguoiNhanFk.Name == input.UserNameFilter);

            var query = (from o in filteredChuyenHoSoGiaies
                join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_userRepository.GetAll() on o.NguoiNhanId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                select new GetChuyenHoSoGiayForViewDto()
                {
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
        public async Task<PagedResultDto<ChuyenHoSoGiayVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_vanBanDuAnRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var vanBanDuAnList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ChuyenHoSoGiayVanBanDuAnLookupTableDto>();
            foreach (var vanBanDuAn in vanBanDuAnList)
            {
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
        public async Task<PagedResultDto<ChuyenHoSoGiayUserLookupTableDto>> GetAllUserForLookupTable(
            GetAllForLookupTableInput input)
        {
            var query = _lookup_userRepository.GetAll().WhereIf(
                !string.IsNullOrWhiteSpace(input.Filter),
                e => e.Name != null && e.Name.Contains(input.Filter)
            );

            var totalCount = await query.CountAsync();

            var userList = await query
                .PageBy(input)
                .ToListAsync();

            var lookupTableDtoList = new List<ChuyenHoSoGiayUserLookupTableDto>();
            foreach (var user in userList)
            {
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

        public async Task<PagedResultDto<GetChuyenHoSoGiayForViewDto>> ChuyenHoSoGiayVanBanDuAnGetAll(
            ChuyenHoSoGiayVanBanDuAnGetAllInput input)
        {
            var filteredChuyenHoSoGiaies = _chuyenHoSoGiayRepository.GetAll()
                    .Include(e => e.VanBanDuAnFk)
                    .Include(e => e.NguoiNhanFk)
                    .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false)
                    .WhereIf(input.VanBanDuAnId != null, e => e.VanBanDuAnId == input.VanBanDuAnId)
                ;

            var pagedAndFilteredChuyenHoSoGiaies = filteredChuyenHoSoGiaies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var chuyenHoSoGiaies = from o in pagedAndFilteredChuyenHoSoGiaies
                join o1 in _lookup_vanBanDuAnRepository.GetAll() on o.VanBanDuAnId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _lookup_userRepository.GetAll() on o.NguoiNhanId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _lookup_userRepository.GetAll() on o.NguoiChuyenId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                select new GetChuyenHoSoGiayForViewDto()
                {
                    ChuyenHoSoGiay = new ChuyenHoSoGiayDto
                    {
                        NguoiChuyenId = o.NguoiChuyenId,
                        ThoiGianChuyen = o.ThoiGianChuyen,
                        SoLuong = o.SoLuong,
                        TrangThai = o.TrangThai,
                        ThoiGianNhan = o.ThoiGianNhan,
                        Id = o.Id,
                        NguoiNhanId = (int) o.NguoiNhanId
                    },
                    VanBanDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                    UserName = s2 == null || s2.Name == null ? "" : s2.Name.ToString(),
                    TenNguoiNhan = s2 == null || s2.Name == null ? "" : (s2.Surname + " " + s2.Name).ToString(),
                    TenNguoiChuyen = s3 == null || s3.Name == null ? "" : (s3.Surname + " " + s3.Name).ToString(),
                    UserId = (int) AbpSession.UserId
                };

            var totalCount = await filteredChuyenHoSoGiaies.CountAsync();

            return new PagedResultDto<GetChuyenHoSoGiayForViewDto>(
                totalCount,
                await chuyenHoSoGiaies.ToListAsync()
            );
        }

        public async Task<string> CapNhatTrangThaiChuyenHoSoGiay(int id, int trangThai)
        {
            //TODO bắt cả người gửi và người nhận ở đây
            ChuyenHoSoGiay chuyenHoSoGiay = _chuyenHoSoGiayRepository.FirstOrDefault(id);
            chuyenHoSoGiay.TrangThai = trangThai;
            if (trangThai == TrangThaiChuyenHoSoGiayConst.DA_NHAN_HO_SO)
            {
                chuyenHoSoGiay.ThoiGianNhan = DateTime.Now;
            }

            _chuyenHoSoGiayRepository.Update(chuyenHoSoGiay);
            return "";
        }
    }
}