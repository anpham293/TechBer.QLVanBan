using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Sessions.Dto;

namespace TechBer.ChuyenDoiSo.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();

        Task<UpdateUserSignInTokenOutput> UpdateUserSignInToken();
    }
}
