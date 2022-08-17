using System.ComponentModel.DataAnnotations;

namespace TechBer.ChuyenDoiSo.Localization.Dto
{
    public class CreateOrUpdateLanguageInput
    {
        [Required]
        public ApplicationLanguageEditDto Language { get; set; }
    }
}