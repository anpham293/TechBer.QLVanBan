using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.Web.Models.Account
{
    public class SendPasswordResetLinkViewModel
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}