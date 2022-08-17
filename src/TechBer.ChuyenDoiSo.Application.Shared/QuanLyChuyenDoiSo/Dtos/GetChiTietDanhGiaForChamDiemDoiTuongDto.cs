using System;
using System.Collections.Generic;
using System.Text;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class GetChiTietDanhGiaForChamDiemDoiTuongDto
    {
        public GetChiTietDanhGiaForChamDiemDoiTuongDto()
        {
            DanhSach = new List<ChiTietDanhGiaDoiTuong>();
        }

        public List<ChiTietDanhGiaDoiTuong> DanhSach { get; set; }

        public string DoiTuongName { get; set; }
	}
}
