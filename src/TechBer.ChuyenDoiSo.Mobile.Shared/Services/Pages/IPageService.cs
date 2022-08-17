using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace TechBer.ChuyenDoiSo.Services.Pages
{
    public interface IPageService
    {
        Page MainPage { get; set; }

        Task<Page> CreatePage(Type viewType, object navigationParameter);
    }
}
