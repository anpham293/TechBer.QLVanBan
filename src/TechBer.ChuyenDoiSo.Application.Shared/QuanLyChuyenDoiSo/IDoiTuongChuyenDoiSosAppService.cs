using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using System.Collections.Generic;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
    public interface IDoiTuongChuyenDoiSosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDoiTuongChuyenDoiSoForViewDto>> GetAll(GetAllDoiTuongChuyenDoiSosInput input);

        Task<GetDoiTuongChuyenDoiSoForViewDto> GetDoiTuongChuyenDoiSoForView(int id);

		Task<GetDoiTuongChuyenDoiSoForEditOutput> GetDoiTuongChuyenDoiSoForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDoiTuongChuyenDoiSoDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDoiTuongChuyenDoiSosToExcel(GetAllDoiTuongChuyenDoiSosForExcelInput input);

		Task<List<DoiTuongChuyenDoiSoUserLookupTableDto>> GetAllUserForTableDropdown();

		/// <summary>
		/// Lay danh sach chi tiet danh gia cua doi tuong chuyen doi so
		/// </summary>
		/// <param name="idDoiTuongChuyenDoiSo">ID cua doi tuong chuyen doi so</param>
		/// <returns></returns>
		Task<GetChiTietDanhGiaForChamDiemDoiTuongDto> GetAllChiTietDanhGiaDoiTuong(int idDoiTuongChuyenDoiSo);

		/// <summary>
		/// Lay thong tin chi tiet danh gia
		/// </summary>
		/// <param name="id">id cua chi tiet danh gia</param>
		/// <returns>thong tin ban ghi chi tiet danh gia</returns>
		Task<ChiTietDanhGiaForEditTreeOutPut> GetChiTietDanhGiaForEdit(int id);

		/// <summary>
		/// Cap nhat thong tin chi tiet danh gia
		/// </summary>
		/// <param name="input">Du lieu chi tiet can chinh sua</param>
		/// <returns></returns>
		Task EditChiTietDanhGia(CreateOrEditChiTietDanhGiaDto input);

		/// <summary>
		/// Tong hop diem 
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		Task TongHopDiemDoiTuong(int id);

		/// <summary>
		/// Chuyen doi trang thai cham diem
		/// </summary>
		/// <param name="input">Thong tin trang thai cham diem cua doi tuong</param>
		/// <returns></returns>
		Task ChuyenTrangThaiChamDiem(TrangThaiChamDiemInput input);

		/// <summary>
		/// Xoa file dinh kem cua chi tiet danh gia
		/// </summary>
		/// <param name="input">Thong tin file can xoa</param>
		/// <returns></returns>
		Task XoaFileDinhKem(ThongTinFileXoa input);

		/// <summary>
		/// Lấy toàn bộ đối tượng chuyển đổi số
		/// </summary>
		/// <param name="searchTerm"></param>
		/// <returns>Danh sách đối tượng chuyển đổi số</returns>
		Task<List<DoiTuongChuyenDoiSoForLookupDto>> GetAllDoiTuongChuyenDoiSo(string searchTerm);

		/// <summary>
		/// Xử lý upload tài liệu cho chi tiết đánh giá
		/// </summary>
		/// <param name="input">Thông tin upload</param>
		/// <returns></returns>
		Task<UploadInfoOutputDto> UploadSoLieuThongKe(ChiTietThongTinUploadInput input);

		/// <summary>
		/// Xóa số liệu thống kê của một chi tiết đánh giá
		/// </summary>
		/// <param name="input">Thông tin số liệu cần xóa</param>
		/// <returns></returns>
		Task XoaSoLieuThongKe(ThongTinSoLieuCanXoaInput input);
	}
}