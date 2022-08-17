using Abp.Dependency;
using TechBer.ChuyenDoiSo.Configuration;
using TechBer.ChuyenDoiSo.Url;

namespace TechBer.ChuyenDoiSo.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor configurationAccessor) :
            base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";
    }
}