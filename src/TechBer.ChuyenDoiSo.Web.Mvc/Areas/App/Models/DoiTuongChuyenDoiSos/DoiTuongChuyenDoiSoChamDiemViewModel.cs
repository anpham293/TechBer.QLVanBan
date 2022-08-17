using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DoiTuongChuyenDoiSos
{
    public class DoiTuongChuyenDoiSoChamDiemViewModel
    {
        public DoiTuongChuyenDoiSoChamDiemViewModel()
        {
            DanhSachDanhGiaChiTiet = new List<GetChiTietDanhGiaForEditOutput>();
        }

        public int Id { get; set; }

        public List<GetChiTietDanhGiaForEditOutput> DanhSachDanhGiaChiTiet { get; set; }

    }
}
