using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
	[AbpAuthorize(AppPermissions.Pages_ChiTietDanhGias)]
    public class ChiTietDanhGiasAppService : ChuyenDoiSoAppServiceBase, IChiTietDanhGiasAppService
    {
		 private readonly IRepository<ChiTietDanhGia> _chiTietDanhGiaRepository;
		 private readonly IRepository<TieuChiDanhGia,int> _lookup_tieuChiDanhGiaRepository;
		 private readonly IRepository<DoiTuongChuyenDoiSo,int> _lookup_doiTuongChuyenDoiSoRepository;
		 

		public ChiTietDanhGiasAppService(IRepository<ChiTietDanhGia> chiTietDanhGiaRepository , IRepository<TieuChiDanhGia, int> lookup_tieuChiDanhGiaRepository, IRepository<DoiTuongChuyenDoiSo, int> lookup_doiTuongChuyenDoiSoRepository) 
		{
		    _chiTietDanhGiaRepository = chiTietDanhGiaRepository;
		    _lookup_tieuChiDanhGiaRepository = lookup_tieuChiDanhGiaRepository;
		    _lookup_doiTuongChuyenDoiSoRepository = lookup_doiTuongChuyenDoiSoRepository;
		
		}

		 public async Task<PagedResultDto<GetChiTietDanhGiaForViewDto>> GetAll(GetAllChiTietDanhGiasInput input)
         {
			
			var filteredChiTietDanhGias = _chiTietDanhGiaRepository.GetAll()
						.Include( e => e.TieuChiDanhGiaFk)
						.Include( e => e.DoiTuongChuyenDoiSoFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Description.Contains(input.Filter) || e.SoLieuKeKhai.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionFilter),  e => e.Description == input.DescriptionFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.SoLieuKeKhaiFilter),  e => e.SoLieuKeKhai == input.SoLieuKeKhaiFilter)
						.WhereIf(input.MinDiemTuDanhGiaFilter != null, e => e.DiemTuDanhGia >= input.MinDiemTuDanhGiaFilter)
						.WhereIf(input.MaxDiemTuDanhGiaFilter != null, e => e.DiemTuDanhGia <= input.MaxDiemTuDanhGiaFilter)
						.WhereIf(input.MinDiemHoiDongThamDinhFilter != null, e => e.DiemHoiDongThamDinh >= input.MinDiemHoiDongThamDinhFilter)
						.WhereIf(input.MaxDiemHoiDongThamDinhFilter != null, e => e.DiemHoiDongThamDinh <= input.MaxDiemHoiDongThamDinhFilter)
						.WhereIf(input.MinDiemDatDuocFilter != null, e => e.DiemDatDuoc >= input.MinDiemDatDuocFilter)
						.WhereIf(input.MaxDiemDatDuocFilter != null, e => e.DiemDatDuoc <= input.MaxDiemDatDuocFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TieuChiDanhGiaNameFilter), e => e.TieuChiDanhGiaFk != null && e.TieuChiDanhGiaFk.Name == input.TieuChiDanhGiaNameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DoiTuongChuyenDoiSoNameFilter), e => e.DoiTuongChuyenDoiSoFk != null && e.DoiTuongChuyenDoiSoFk.Name == input.DoiTuongChuyenDoiSoNameFilter);

			var pagedAndFilteredChiTietDanhGias = filteredChiTietDanhGias
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chiTietDanhGias = from o in pagedAndFilteredChiTietDanhGias
                         join o1 in _lookup_tieuChiDanhGiaRepository.GetAll() on o.TieuChiDanhGiaId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         join o2 in _lookup_doiTuongChuyenDoiSoRepository.GetAll() on o.DoiTuongChuyenDoiSoId equals o2.Id into j2
                         from s2 in j2.DefaultIfEmpty()
                         
                         select new GetChiTietDanhGiaForViewDto() {
							ChiTietDanhGia = new ChiTietDanhGiaDto
							{
                                Description = o.Description,
                                SoLieuKeKhai = o.SoLieuKeKhai,
                                DiemTuDanhGia = o.DiemTuDanhGia,
                                DiemHoiDongThamDinh = o.DiemHoiDongThamDinh,
                                DiemDatDuoc = o.DiemDatDuoc,
                                Id = o.Id
							},
                         	TieuChiDanhGiaName = s1 == null || s1.Name == null ? "" : s1.Name.ToString(),
                         	DoiTuongChuyenDoiSoName = s2 == null || s2.Name == null ? "" : s2.Name.ToString()
						};

            var totalCount = await filteredChiTietDanhGias.CountAsync();

            return new PagedResultDto<GetChiTietDanhGiaForViewDto>(
                totalCount,
                await chiTietDanhGias.ToListAsync()
            );
         }
		 
    }
}