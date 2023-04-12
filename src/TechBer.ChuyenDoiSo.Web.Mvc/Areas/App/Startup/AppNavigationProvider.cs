using Abp.Application.Navigation;
using Abp.Authorization;
using Abp.Localization;
using TechBer.ChuyenDoiSo.Authorization;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Startup
{
    public class AppNavigationProvider : NavigationProvider
    {
        public const string MenuName = "App";

        public override void SetNavigation(INavigationProviderContext context)
        {
            var menu = context.Manager.Menus[MenuName] =
                new MenuDefinition(MenuName, new FixedLocalizableString("Main Menu"));

            menu
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Host.Dashboard,
                        L("Dashboard"),
                        url: "App/HostDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions
                            .Pages_Administration_Host_Dashboard)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DuAns,
                        L("DuAns"),
                        url: "App/DuAns",
                        icon: "fas fa-layer-group",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DuAns)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DuyetHoSo,
                        L("DuyetHoSo"),
                        url: "App/DuyetHoSo",
                        icon: "fas fa-file-alt",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DuyetHoSo)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.PhongKhos,
                        L("PhongKhos"),
                        url: "App/PhongKhos",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_PhongKhos)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DayKes,
                        L("DayKes"),
                        url: "App/DayKes",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DayKes)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.ThungHoSos,
                        L("ThungHoSos"),
                        url: "App/ThungHoSos",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ThungHoSos)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.ChuyenHoSoGiaies,
                        L("ChuyenHoSoGiaies"),
                        url: "App/ChuyenHoSoGiaies",
                        icon: "fas fa-paper-plane",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_ChuyenHoSoGiaies)
                    )
                )
                .AddItem(new MenuItemDefinition(
                        "",
                        L("BaoCaoHoSo"),
                        icon: "fas fa-file-excel"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.LoaiDuAns,
                            L("BaoCaoHoSoNopTrongThang"),
                            url: "App/QuyTrinhDuAnAssigneds/BaoCaoNopHoSoTrongThangTheoDuAnView",
                            icon: "",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LoaiDuAns)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.LoaiDuAns,
                            L("BaoCaoHoSoTheoDuAn"),
                            url: "App/QuyTrinhDuAnAssigneds/BaoCaoHoSoTheoDuAnView",
                            icon: "",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LoaiDuAns)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        "",
                        L("DanhMucQuanLy"),
                        icon: "fas fa-align-justify"
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Common.LoaiDuAns,
                            L("LoaiDuAns"),
                            url: "App/LoaiDuAns",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LoaiDuAns)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.CapQuanLies,
                        L("CapQuanLies"),
                        url: "App/CapQuanLies",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_CapQuanLies)
                    )

                )
                    .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Chuongs,
                        L("Chuongs"),
                        url: "App/Chuongs",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Chuongs)
                    )
                )
                    .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.LoaiKhoans,
                        L("LoaiKhoans"),
                        url: "App/LoaiKhoans",
                        icon: "flaticon-more",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_LoaiKhoans)
                    )
                )

                )


                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.Tenants,
                //         L("Tenants"),
                //         url: "App/Tenants",
                //         icon: "flaticon-list-3",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenants)
                //     )
                // )
                // .AddItem(new MenuItemDefinition(
                //         AppPageNames.Host.Editions,
                //         L("Editions"),
                //         url: "App/Editions",
                //         icon: "flaticon-app",
                //         permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Editions)
                //     )
                // )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Tenant.Dashboard,
                        L("Dashboard"),
                        url: "App/TenantDashboard",
                        icon: "flaticon-line-graph",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_Tenant_Dashboard)
                    )
                ).AddItem(new MenuItemDefinition(
                        AppPageNames.Common.Administration,
                        L("Administration"),
                        icon: "flaticon-interface-8"
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.OrganizationUnits,
                            L("OrganizationUnits"),
                            url: "App/OrganizationUnits",
                            icon: "flaticon-map",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_OrganizationUnits)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Roles,
                            L("Roles"),
                            url: "App/Roles",
                            icon: "flaticon-suitcase",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Roles)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Users,
                            L("Users"),
                            url: "App/Users",
                            icon: "flaticon-users",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Users)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.Languages,
                            L("Languages"),
                            url: "App/Languages",
                            icon: "flaticon-tabs",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Languages)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.AuditLogs,
                            L("AuditLogs"),
                            url: "App/AuditLogs",
                            icon: "flaticon-folder-1",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_AuditLogs)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Maintenance,
                            L("Maintenance"),
                            url: "App/Maintenance",
                            icon: "flaticon-lock",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Maintenance)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.SubscriptionManagement,
                            L("Subscription"),
                            url: "App/SubscriptionManagement",
                            icon: "flaticon-refresh",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_SubscriptionManagement)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.UiCustomization,
                            L("VisualSettings"),
                            url: "App/UiCustomization",
                            icon: "flaticon-medical",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_UiCustomization)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.WebhookSubscriptions,
                            L("WebhookSubscriptions"),
                            url: "App/WebhookSubscription",
                            icon: "flaticon2-world",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_WebhookSubscription)
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Common.DynamicEntityParameters,
                            L("DynamicParameters"),
                            icon: "flaticon-interface-8"
                        ).AddItem(new MenuItemDefinition(
                                AppPageNames.Common.DynamicParameters,
                                L("Definitions"),
                                url: "App/DynamicParameter",
                                icon: "flaticon-map",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Administration_DynamicParameters)
                            )
                        )
                        .AddItem(new MenuItemDefinition(
                                AppPageNames.Common.EntityDynamicParameters,
                                L("EntityDynamicParameters"),
                                url: "App/EntityDynamicParameter",
                                icon: "flaticon-map",
                                permissionDependency: new SimplePermissionDependency(AppPermissions
                                    .Pages_Administration_EntityDynamicParameters)
                            )
                        )
                    ).AddItem(new MenuItemDefinition(
                            AppPageNames.Host.Settings,
                            L("Settings"),
                            url: "App/HostSettings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Host_Settings)
                        )
                    )
                    .AddItem(new MenuItemDefinition(
                            AppPageNames.Tenant.Settings,
                            L("Settings"),
                            url: "App/Settings",
                            icon: "flaticon-settings",
                            permissionDependency: new SimplePermissionDependency(AppPermissions
                                .Pages_Administration_Tenant_Settings)
                        )
                    )
                )
                .AddItem(new MenuItemDefinition(
                        AppPageNames.Common.DemoUiComponents,
                        L("DemoUiComponents"),
                        url: "App/DemoUiComponents",
                        icon: "flaticon-shapes",
                        permissionDependency: new SimplePermissionDependency(AppPermissions.Pages_DemoUiComponents)
                    )
                )
                ;
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ChuyenDoiSoConsts.LocalizationSourceName);
        }
    }
}