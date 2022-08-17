using TechBer.ChuyenDoiSo.Models.Users;
using TechBer.ChuyenDoiSo.ViewModels;
using Xamarin.Forms;

namespace TechBer.ChuyenDoiSo.Views
{
    public partial class UsersView : ContentPage, IXamarinView
    {
        public UsersView()
        {
            InitializeComponent();
        }

        public async void ListView_OnItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            await ((UsersViewModel) BindingContext).LoadMoreUserIfNeedsAsync(e.Item as UserListModel);
        }
    }
}