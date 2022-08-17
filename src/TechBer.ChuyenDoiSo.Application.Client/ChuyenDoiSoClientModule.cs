using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo
{
    public class ChuyenDoiSoClientModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoClientModule).GetAssembly());
        }
    }
}
