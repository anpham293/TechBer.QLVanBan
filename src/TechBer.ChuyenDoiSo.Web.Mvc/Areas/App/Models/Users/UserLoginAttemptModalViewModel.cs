using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Authorization.Users.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Users
{
    public class UserLoginAttemptModalViewModel
    {
        public List<UserLoginAttemptDto> LoginAttempts { get; set; }
    }
}