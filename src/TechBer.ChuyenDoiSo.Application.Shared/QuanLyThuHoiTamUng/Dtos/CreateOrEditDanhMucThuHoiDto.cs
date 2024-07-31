
using System;
using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos
{
    public class CreateOrEditDanhMucThuHoiDto : EntityDto<long?>
    {

		public string Stt { get; set; }
		
		
		public string Ten { get; set; }
		
		
		public string GhiChu { get; set; }
		
		
		public int Type { get; set; }
		
		
		 public long? DuAnThuHoiId { get; set; }
		 
		 
    }
}