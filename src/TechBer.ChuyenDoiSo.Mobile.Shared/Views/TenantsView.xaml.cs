using TechBer.ChuyenDoiSo.Models.Tenants;
using TechBer.ChuyenDoiSo.ViewModels;
using Xamarin.Forms;

namespace TechBer.ChuyenDoiSo.Views
{
    public partial class TenantsView : ContentPage, IXamarinView
    {
        public TenantsView()
        {
            InitializeComponent();
        }

        private async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((TenantsViewModel)BindingContext).LoadMoreTenantsIfNeedsAsync(e.Item as TenantListModel);
        }
    }
}