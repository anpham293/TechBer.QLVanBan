using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization.Permissions.Dto;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Common;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Users
{
    public class UsersViewModel : IPermissionsEditViewModel
    {
        public string FilterText { get; set; }

        public List<ComboboxItemDto> Roles { get; set; }

        public bool OnlyLockedUsers { get; set; }

        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }
    }
}