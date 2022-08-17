using System.Threading.Tasks;
using Abp.Domain.Policies;

namespace TechBer.ChuyenDoiSo.Authorization.Users
{
    public interface IUserPolicy : IPolicy
    {
        Task CheckMaxUserCountAsync(int tenantId);
    }
}
