using System.Collections.Generic;
using MvvmHelpers;
using TechBer.ChuyenDoiSo.Models.NavigationMenu;

namespace TechBer.ChuyenDoiSo.Services.Navigation
{
    public interface IMenuProvider
    {
        ObservableRangeCollection<NavigationMenuItem> GetAuthorizedMenuItems(Dictionary<string, string> grantedPermissions);
    }
}