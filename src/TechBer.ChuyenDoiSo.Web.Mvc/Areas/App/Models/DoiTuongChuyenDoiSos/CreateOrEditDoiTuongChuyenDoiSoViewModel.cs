using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using System.Collections.Generic;

using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DoiTuongChuyenDoiSos
{
    public class CreateOrEditDoiTuongChuyenDoiSoModalViewModel
    {
        public CreateOrEditDoiTuongChuyenDoiSoDto DoiTuongChuyenDoiSo { get; set; }

	    public string UserName { get; set;}

        public List<DoiTuongChuyenDoiSoUserLookupTableDto> DoiTuongChuyenDoiSoUserList { get; set;}

	    public bool IsEditMode => DoiTuongChuyenDoiSo.Id.HasValue;
    }
}