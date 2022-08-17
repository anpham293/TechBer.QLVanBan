using System;
using TechBer.ChuyenDoiSo.Core;
using TechBer.ChuyenDoiSo.Core.Dependency;
using TechBer.ChuyenDoiSo.Services.Permission;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TechBer.ChuyenDoiSo.Extensions.MarkupExtensions
{
    [ContentProperty("Text")]
    public class HasPermissionExtension : IMarkupExtension
    {
        public string Text { get; set; }
        
        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ApplicationBootstrapper.AbpBootstrapper == null || Text == null)
            {
                return false;
            }

            var permissionService = DependencyResolver.Resolve<IPermissionService>();
            return permissionService.HasPermission(Text);
        }
    }
}