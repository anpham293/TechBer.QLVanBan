using Microsoft.Extensions.Configuration;

namespace TechBer.ChuyenDoiSo.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
