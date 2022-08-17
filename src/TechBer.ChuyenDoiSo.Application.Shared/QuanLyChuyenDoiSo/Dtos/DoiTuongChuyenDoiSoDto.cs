
using System;
using Abp.Application.Services.Dto;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class DoiTuongChuyenDoiSoDto : EntityDto
    {
		public string Name { get; set; }

		public int Type { get; set; }

        public double? TongDiemTuDanhGia { get; set; }

        public double? TongDiemHoiDongThamDinh { get; set; }

        public double? TongDiemDatDuoc { get; set; }

        public int ChamDiemFlag { get; set; }

		public long? UserId { get; set; }
    }
}