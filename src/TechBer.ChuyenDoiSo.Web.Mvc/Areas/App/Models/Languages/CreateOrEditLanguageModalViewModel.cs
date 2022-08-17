using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.Localization.Dto;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Languages
{
    [AutoMapFrom(typeof(GetLanguageForEditOutput))]
    public class CreateOrEditLanguageModalViewModel : GetLanguageForEditOutput
    {
        public bool IsEditMode => Language.Id.HasValue;
    }
}