﻿using Abp;

namespace TechBer.ChuyenDoiSo
{
    /// <summary>
    /// This class can be used as a base class for services in this application.
    /// It has some useful objects property-injected and has some basic methods most of services may need to.
    /// It's suitable for non domain nor application service classes.
    /// For domain services inherit <see cref="ChuyenDoiSoDomainServiceBase"/>.
    /// For application services inherit ChuyenDoiSoAppServiceBase.
    /// </summary>
    public abstract class ChuyenDoiSoServiceBase : AbpServiceBase
    {
        protected ChuyenDoiSoServiceBase()
        {
            LocalizationSourceName = ChuyenDoiSoConsts.LocalizationSourceName;
        }
    }
}