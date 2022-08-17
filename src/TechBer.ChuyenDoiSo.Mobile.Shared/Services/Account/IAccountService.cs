using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.ApiClient.Models;

namespace TechBer.ChuyenDoiSo.Services.Account
{
    public interface IAccountService
    {
        AbpAuthenticateModel AbpAuthenticateModel { get; set; }
        
        AbpAuthenticateResultModel AuthenticateResultModel { get; set; }
        
        Task LoginUserAsync();

        Task LogoutAsync();
    }
}
