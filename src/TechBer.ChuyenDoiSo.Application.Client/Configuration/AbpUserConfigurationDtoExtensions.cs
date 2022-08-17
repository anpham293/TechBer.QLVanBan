﻿using Abp.Web.Models.AbpUserConfiguration;

namespace TechBer.ChuyenDoiSo.Configuration
{
    public static class AbpUserConfigurationDtoExtensions
    {
        public static bool HasSessionUserId(this AbpUserConfigurationDto userConfiguration)
        {
            return userConfiguration.Session?.UserId != null;
        }
    }
}