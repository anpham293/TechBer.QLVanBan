using System.Threading.Tasks;
using Abp.Application.Services;

namespace TechBer.ChuyenDoiSo.MultiTenancy
{
    public interface ISubscriptionAppService : IApplicationService
    {
        Task DisableRecurringPayments();

        Task EnableRecurringPayments();
    }
}
