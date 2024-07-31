using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
    [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies)]
    public class DuAnThuHoiesAppService : ChuyenDoiSoAppServiceBase, IDuAnThuHoiesAppService
    {
        private readonly IRepository<DuAnThuHoi, long> _duAnThuHoiRepository;
        private readonly IDuAnThuHoiesExcelExporter _duAnThuHoiesExcelExporter;


        public DuAnThuHoiesAppService(IRepository<DuAnThuHoi, long> duAnThuHoiRepository,
            IDuAnThuHoiesExcelExporter duAnThuHoiesExcelExporter)
        {
            _duAnThuHoiRepository = duAnThuHoiRepository;
            _duAnThuHoiesExcelExporter = duAnThuHoiesExcelExporter;
        }

        public async Task<PagedResultDto<GetDuAnThuHoiForViewDto>> GetAll(GetAllDuAnThuHoiesInput input)
        {
            var filteredDuAnThuHoies = _duAnThuHoiRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.MaDATH.Contains(input.Filter) || e.Ten.Contains(input.Filter) ||
                         e.GhiChu.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaDATHFilter), e => e.MaDATH == input.MaDATHFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(input.MinNamQuanLyFilter != null, e => e.NamQuanLy >= input.MinNamQuanLyFilter)
                .WhereIf(input.MaxNamQuanLyFilter != null, e => e.NamQuanLy <= input.MaxNamQuanLyFilter)
                .WhereIf(input.MinThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong >= input.MinThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MaxThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong <= input.MaxThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MinThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng >= input.MinThoiHanBaoLanhTamUngFilter)
                .WhereIf(input.MaxThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng <= input.MaxThoiHanBaoLanhTamUngFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter), e => e.GhiChu == input.GhiChuFilter)
                .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

            var pagedAndFilteredDuAnThuHoies = filteredDuAnThuHoies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var duAnThuHoies = from o in pagedAndFilteredDuAnThuHoies
                select new GetDuAnThuHoiForViewDto()
                {
                    DuAnThuHoi = new DuAnThuHoiDto
                    {
                        MaDATH = o.MaDATH,
                        Ten = o.Ten,
                        NamQuanLy = o.NamQuanLy,
                        ThoiHanBaoLanhHopDong = o.ThoiHanBaoLanhHopDong,
                        ThoiHanBaoLanhTamUng = o.ThoiHanBaoLanhTamUng,
                        GhiChu = o.GhiChu,
                        TrangThai = o.TrangThai,
                        Id = o.Id
                    }
                };

            var totalCount = await filteredDuAnThuHoies.CountAsync();

            return new PagedResultDto<GetDuAnThuHoiForViewDto>(
                totalCount,
                await duAnThuHoies.ToListAsync()
            );
        }

        public async Task<GetDuAnThuHoiForViewDto> GetDuAnThuHoiForView(long id)
        {
            var duAnThuHoi = await _duAnThuHoiRepository.GetAsync(id);

            var output = new GetDuAnThuHoiForViewDto {DuAnThuHoi = ObjectMapper.Map<DuAnThuHoiDto>(duAnThuHoi)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Edit)]
        public async Task<GetDuAnThuHoiForEditOutput> GetDuAnThuHoiForEdit(EntityDto<long> input)
        {
            var duAnThuHoi = await _duAnThuHoiRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDuAnThuHoiForEditOutput
                {DuAnThuHoi = ObjectMapper.Map<CreateOrEditDuAnThuHoiDto>(duAnThuHoi)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDuAnThuHoiDto input)
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

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Create)]
        protected virtual async Task Create(CreateOrEditDuAnThuHoiDto input)
        {
            //var duAnThuHoi = ObjectMapper.Map<DuAnThuHoi>(input);
            var duAnThuHoi = new DuAnThuHoi()
            {
                CreationTime = DateTime.Now,
                IsDeleted = false,
                CreatorUserId = AbpSession.UserId,
                Ten = input.Ten,
                ThoiHanBaoLanhHopDong = input.ThoiHanBaoLanhHopDong,
                ThoiHanBaoLanhTamUng = input.ThoiHanBaoLanhTamUng,
                NamQuanLy = input.NamQuanLy,
                GhiChu = input.GhiChu,
                TrangThai = DuAnThuHoiConst.TrangThaiDangXuLi
            };
            
            var soDuAn = 0;
            string fmt = "000";

            var duAnThuHoiOld = await _duAnThuHoiRepository.GetAll()
                .WhereIf(true, p => p.NamQuanLy == input.NamQuanLy)
                .OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (duAnThuHoiOld.IsNullOrDeleted())
            {
                soDuAn = 1;
            }
            else
            {
                soDuAn = duAnThuHoiOld.SoDuAn + 1;
            }

            duAnThuHoi.SoDuAn = soDuAn;
            duAnThuHoi.MaDATH = "TH" +input.NamQuanLy + "-" + soDuAn.ToString(fmt);

            await _duAnThuHoiRepository.InsertAsync(duAnThuHoi);
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Edit)]
        protected virtual async Task Update(CreateOrEditDuAnThuHoiDto input)
        {
            var duAnThuHoi = await _duAnThuHoiRepository.FirstOrDefaultAsync((long) input.Id);
            ObjectMapper.Map(input, duAnThuHoi);
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _duAnThuHoiRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDuAnThuHoiesToExcel(GetAllDuAnThuHoiesForExcelInput input)
        {
            var filteredDuAnThuHoies = _duAnThuHoiRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.MaDATH.Contains(input.Filter) || e.Ten.Contains(input.Filter) ||
                         e.GhiChu.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaDATHFilter), e => e.MaDATH == input.MaDATHFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(input.MinNamQuanLyFilter != null, e => e.NamQuanLy >= input.MinNamQuanLyFilter)
                .WhereIf(input.MaxNamQuanLyFilter != null, e => e.NamQuanLy <= input.MaxNamQuanLyFilter)
                .WhereIf(input.MinThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong >= input.MinThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MaxThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong <= input.MaxThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MinThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng >= input.MinThoiHanBaoLanhTamUngFilter)
                .WhereIf(input.MaxThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng <= input.MaxThoiHanBaoLanhTamUngFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter), e => e.GhiChu == input.GhiChuFilter)
                .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

            var query = (from o in filteredDuAnThuHoies
                select new GetDuAnThuHoiForViewDto()
                {
                    DuAnThuHoi = new DuAnThuHoiDto
                    {
                        MaDATH = o.MaDATH,
                        Ten = o.Ten,
                        NamQuanLy = o.NamQuanLy,
                        ThoiHanBaoLanhHopDong = o.ThoiHanBaoLanhHopDong,
                        ThoiHanBaoLanhTamUng = o.ThoiHanBaoLanhTamUng,
                        GhiChu = o.GhiChu,
                        TrangThai = o.TrangThai,
                        Id = o.Id
                    }
                });


            var duAnThuHoiListDtos = await query.ToListAsync();

            return _duAnThuHoiesExcelExporter.ExportToFile(duAnThuHoiListDtos);
        }
    }
}