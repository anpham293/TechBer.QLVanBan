using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DuAns
{
    public class DuAnChuongLookupTableViewModel
    {
        public int? Id { get; set; }

        public string DisplayName { get; set; }

        public string FilterText { get; set; }
        public List<CapQuanLyDto> ListCapQuanLy { get; set; }
    }
}