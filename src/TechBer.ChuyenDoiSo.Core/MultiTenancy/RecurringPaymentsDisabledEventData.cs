﻿using Abp.Events.Bus;

namespace TechBer.ChuyenDoiSo.MultiTenancy
{
    public class RecurringPaymentsDisabledEventData : EventData
    {
        public int TenantId { get; set; }

        public int EditionId { get; set; }
    }
}
