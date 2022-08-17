using System.Collections.Generic;
using TechBer.ChuyenDoiSo.Editions.Dto;
using TechBer.ChuyenDoiSo.Security;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Models.Tenants
{
    public class CreateTenantViewModel
    {
        public IReadOnlyList<SubscribableEditionComboboxItemDto> EditionItems { get; set; }

        public PasswordComplexitySetting PasswordComplexitySetting { get; set; }

        public CreateTenantViewModel(IReadOnlyList<SubscribableEditionComboboxItemDto> editionItems)
        {
            EditionItems = editionItems;
        }
    }
}