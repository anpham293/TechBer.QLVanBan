using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Views;
using Xamarin.Forms;

namespace TechBer.ChuyenDoiSo.Services.Modal
{
    public interface IModalService
    {
        Task ShowModalAsync(Page page);

        Task ShowModalAsync<TView>(object navigationParameter) where TView : IXamarinView;

        Task<Page> CloseModalAsync();
    }
}
