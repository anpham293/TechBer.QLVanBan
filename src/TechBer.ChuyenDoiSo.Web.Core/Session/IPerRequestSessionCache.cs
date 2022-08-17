using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Sessions.Dto;

namespace TechBer.ChuyenDoiSo.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
