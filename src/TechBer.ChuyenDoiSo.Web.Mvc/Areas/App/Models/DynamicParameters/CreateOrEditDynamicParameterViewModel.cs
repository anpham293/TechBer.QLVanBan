using System.Collections.Generic;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.DynamicParameters
{
    public class CreateOrEditDynamicParameterViewModel
    {
        public DynamicParameterDto DynamicParameterDto { get; set; }

        public List<string> AllowedInputTypes { get; set; }
    }
}
