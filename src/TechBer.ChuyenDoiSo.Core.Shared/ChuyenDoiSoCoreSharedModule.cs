using Abp.Modules;
using Abp.Reflection.Extensions;

namespace TechBer.ChuyenDoiSo
{
    public class ChuyenDoiSoCoreSharedModule : AbpModule
    {
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(ChuyenDoiSoCoreSharedModule).GetAssembly());
        }
    }
}