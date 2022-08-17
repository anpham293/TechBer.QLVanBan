using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        [Required]
        public string EmailAddress { get; set; }
    }
}