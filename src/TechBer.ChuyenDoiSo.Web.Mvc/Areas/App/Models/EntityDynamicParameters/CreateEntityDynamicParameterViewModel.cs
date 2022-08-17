using System.Collections.Generic;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.EntityDynamicParameters
{
    public class CreateEntityDynamicParameterViewModel
    {
        public string EntityFullName { get; set; }

        public List<string> AllEntities { get; set; }

        public List<DynamicParameterDto> DynamicParameters { get; set; }
    }
}
