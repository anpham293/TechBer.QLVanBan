using Abp.Domain.Services;

namespace TechBer.ChuyenDoiSo
{
    public abstract class ChuyenDoiSoDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected ChuyenDoiSoDomainServiceBase()
        {
            LocalizationSourceName = ChuyenDoiSoConsts.LocalizationSourceName;
        }
    }
}
