using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.Extensions.Configuration;
using TechBer.ChuyenDoiSo.Configuration;

namespace TechBer.ChuyenDoiSo.Test.Base.Configuration
{
    public class TestAppConfigurationAccessor : IAppConfigurationAccessor, ISingletonDependency
    {
        public IConfigurationRoot Configuration { get; }

        public TestAppConfigurationAccessor()
        {
            Configuration = AppConfigurations.Get(
                typeof(ChuyenDoiSoTestBaseModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }
    }
}
