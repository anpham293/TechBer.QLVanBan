using Abp.Dependency;
using TechBer.ChuyenDoiSo.Configuration;
using TechBer.ChuyenDoiSo.Url;
using TechBer.ChuyenDoiSo.Web.Url;

namespace TechBer.ChuyenDoiSo.Web.Public.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
            IAppConfigurationAccessor appConfigurationAccessor) :
            base(appConfigurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:WebSiteRootAddress";

        public override string ServerRootAddressFormatKey => "App:AdminWebSiteRootAddress";
    }
}