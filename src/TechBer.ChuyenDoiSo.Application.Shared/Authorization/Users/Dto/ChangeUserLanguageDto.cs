using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}
