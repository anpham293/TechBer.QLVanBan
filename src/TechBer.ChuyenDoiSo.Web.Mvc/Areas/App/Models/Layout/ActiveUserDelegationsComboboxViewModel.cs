using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Delegation;
using TechBer.ChuyenDoiSo.Authorization.Users.Delegation.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Layout
{
    public class ActiveUserDelegationsComboboxViewModel
    {
        public IUserDelegationConfiguration UserDelegationConfiguration { get; set; }
        
        public List<UserDelegationDto> UserDelegations { get; set; }
    }
}
