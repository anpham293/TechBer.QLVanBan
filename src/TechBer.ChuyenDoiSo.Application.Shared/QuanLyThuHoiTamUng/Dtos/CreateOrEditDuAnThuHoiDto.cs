
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class CreateOrEditDuAnThuHoiDto : EntityDto<long?>
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