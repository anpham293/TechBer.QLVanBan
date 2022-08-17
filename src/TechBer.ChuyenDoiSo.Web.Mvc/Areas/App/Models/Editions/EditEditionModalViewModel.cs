using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using TechBer.ChuyenDoiSo.Editions.Dto;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Common;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Editions
{
    [AutoMapFrom(typeof(GetEditionEditOutput))]
    public class EditEditionModalViewModel : GetEditionEditOutput, IFeatureEditViewModel
    {
        public bool IsEditMode => Edition.Id.HasValue;

        public IReadOnlyList<ComboboxItemDto> EditionItems { get; set; }

        public IReadOnlyList<ComboboxItemDto> FreeEditionItems { get; set; }
    }
}