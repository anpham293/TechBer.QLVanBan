using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}