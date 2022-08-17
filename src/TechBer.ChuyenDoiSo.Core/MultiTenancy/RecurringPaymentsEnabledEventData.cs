using Abp.Events.Bus;

namespace TechBer.ChuyenDoiSo.MultiTenancy
{
    public class RecurringPaymentsEnabledEventData : EventData
    {
        public int TenantId { get; set; }
    }
}