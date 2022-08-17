using System.Collections.Generic;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Editions.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Dto
{
    public class GetTenantFeaturesEditOutput
    {
        public List<NameValueDto> FeatureValues { get; set; }

        public List<FlatFeatureDto> Features { get; set; }
    }
}