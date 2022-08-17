using System;
using System.Collections.Generic;
using System.Text;

namespace TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto
{
    public class BaoCaoChamDiemOutput
    {
        public int DoiTuongId { get; set; }

        public string Name { get; set; }

        public double? DiemTuDanhGia { get; set; }

        public double? DiemHoiDongThamDinh { get; set; }

        public double? DiemDatDuoc { get; set; }

        public int Type { get; set; }
    }
}
