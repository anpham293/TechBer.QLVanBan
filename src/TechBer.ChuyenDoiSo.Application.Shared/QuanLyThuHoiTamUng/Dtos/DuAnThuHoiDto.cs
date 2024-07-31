
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class DuAnThuHoiDto : EntityDto<long>
    {
		public string MaDATH { get; set; }

		public string Ten { get; set; }

		public int NamQuanLy { get; set; }

		public DateTime? ThoiHanBaoLanhHopDong { get; set; }

		public DateTime? ThoiHanBaoLanhTamUng { get; set; }

		public string GhiChu { get; set; }

		public int TrangThai { get; set; }



    }
}