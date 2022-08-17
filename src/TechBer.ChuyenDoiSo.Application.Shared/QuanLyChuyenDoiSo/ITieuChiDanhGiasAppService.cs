using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Common.Dto;
using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
    public interface ITieuChiDanhGiasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTieuChiDanhGiaForViewDto>> GetAll(GetAllTieuChiDanhGiasInput input);

		/// <summary>
		/// Lay cac tieu chi cho cay
		/// </summary>
		/// <param name="id">Loai Phu Luc</param>
		/// <returns>Danh sach tieu chi</returns>
		Task<List<GetTieuChiDanhGiaForViewDto>> GetTieuChiForTree(int id);

		/// <summary>
		/// Api tao moi hoac cap nhat Tieu Chi cho phan tree
		/// </summary>
		/// <param name="input">Thong tin de tao hoac cap nhat tieu chi</param>
		/// <returns>Cap nhat hoac tao moi tieu chi</returns>
		Task<GetTieuChiDanhGiaForEditOutput> CreateOrEditTieuChi(CreateOrEditTieuChiDanhGiaDto input);

		/// <summary>
		/// Xoa tieu chi
		/// </summary>
		/// <param name="id">Id ban ghi</param>
		/// <returns>Ket qua xu ly xoa tieu chi</returns>
		Task<int> XoaTieuChi(int id);

		/// <summary>
		/// Di chuyển tiêu trí
		/// </summary>
		/// <param name="input">Thông tin di chuyển</param>
		/// <returns>Ket qua xu ly di chuyen</returns>
		Task<int> MoveTieuChi(MoveTieuChiDanhGiaInput input);

		/// <summary>
		/// Thiết lập chiều sâu chi tiêu chí
		/// </summary>
		/// <returns></returns>
		Task ThietLapChieuSauChoTieuChi();

	}
}