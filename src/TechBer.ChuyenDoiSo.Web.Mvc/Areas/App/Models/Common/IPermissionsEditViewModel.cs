using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Permissions.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Common
{
    public interface IPermissionsEditViewModel
    {
        List<FlatPermissionDto> Permissions { get; set; }

        List<string> GrantedPermissionNames { get; set; }
    }
}