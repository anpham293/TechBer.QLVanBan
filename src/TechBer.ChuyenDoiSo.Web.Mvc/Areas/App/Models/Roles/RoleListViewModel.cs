using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization.Permissions.Dto;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Common;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Roles
{
    public class RoleListViewModel : IPermissionsEditViewModel
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}