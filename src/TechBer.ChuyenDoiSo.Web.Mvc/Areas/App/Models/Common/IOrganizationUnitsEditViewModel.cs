using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Organizations.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Common
{
    public interface IOrganizationUnitsEditViewModel
    {
        List<OrganizationUnitDto> AllOrganizationUnits { get; set; }

        List<string> MemberedOrganizationUnits { get; set; }
    }
}