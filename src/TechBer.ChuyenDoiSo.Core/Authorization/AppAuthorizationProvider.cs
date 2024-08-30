using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Abp.MultiTenancy;

namespace TechBer.ChuyenDoiSo.Authorization
{
    /// <summary>
    /// Application's authorization provider.
    /// Defines permissions for the application.
    /// See <see cref="AppPermissions"/> for all permission names.
    /// </summary>
    public class AppAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public AppAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public AppAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            //COMMON PERMISSIONS (FOR BOTH OF TENANTS AND HOST)

            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var sdtZalos = pages.CreateChildPermission(AppPermissions.Pages_SdtZalos, L("SdtZalos"), multiTenancySides: MultiTenancySides.Host);
            sdtZalos.CreateChildPermission(AppPermissions.Pages_SdtZalos_Create, L("CreateNewSdtZalo"), multiTenancySides: MultiTenancySides.Host);
            sdtZalos.CreateChildPermission(AppPermissions.Pages_SdtZalos_Edit, L("EditSdtZalo"), multiTenancySides: MultiTenancySides.Host);
            sdtZalos.CreateChildPermission(AppPermissions.Pages_SdtZalos_Delete, L("DeleteSdtZalo"), multiTenancySides: MultiTenancySides.Host);



            var chiTietThuHoies = pages.CreateChildPermission(AppPermissions.Pages_ChiTietThuHoies, L("ChiTietThuHoies"), multiTenancySides: MultiTenancySides.Host);
            chiTietThuHoies.CreateChildPermission(AppPermissions.Pages_ChiTietThuHoies_Create, L("CreateNewChiTietThuHoi"), multiTenancySides: MultiTenancySides.Host);
            chiTietThuHoies.CreateChildPermission(AppPermissions.Pages_ChiTietThuHoies_Edit, L("EditChiTietThuHoi"), multiTenancySides: MultiTenancySides.Host);
            chiTietThuHoies.CreateChildPermission(AppPermissions.Pages_ChiTietThuHoies_Delete, L("DeleteChiTietThuHoi"), multiTenancySides: MultiTenancySides.Host);



            var danhMucThuHoies = pages.CreateChildPermission(AppPermissions.Pages_DanhMucThuHoies, L("DanhMucThuHoies"), multiTenancySides: MultiTenancySides.Host);
            danhMucThuHoies.CreateChildPermission(AppPermissions.Pages_DanhMucThuHoies_Create, L("CreateNewDanhMucThuHoi"), multiTenancySides: MultiTenancySides.Host);
            danhMucThuHoies.CreateChildPermission(AppPermissions.Pages_DanhMucThuHoies_Edit, L("EditDanhMucThuHoi"), multiTenancySides: MultiTenancySides.Host);
            danhMucThuHoies.CreateChildPermission(AppPermissions.Pages_DanhMucThuHoies_Delete, L("DeleteDanhMucThuHoi"), multiTenancySides: MultiTenancySides.Host);



            var duAnThuHoies = pages.CreateChildPermission(AppPermissions.Pages_DuAnThuHoies, L("DuAnThuHoies"), multiTenancySides: MultiTenancySides.Host);
            duAnThuHoies.CreateChildPermission(AppPermissions.Pages_DuAnThuHoies_Create, L("CreateNewDuAnThuHoi"), multiTenancySides: MultiTenancySides.Host);
            duAnThuHoies.CreateChildPermission(AppPermissions.Pages_DuAnThuHoies_Edit, L("EditDuAnThuHoi"), multiTenancySides: MultiTenancySides.Host);
            duAnThuHoies.CreateChildPermission(AppPermissions.Pages_DuAnThuHoies_Delete, L("DeleteDuAnThuHoi"), multiTenancySides: MultiTenancySides.Host);



            var userDuAns = pages.CreateChildPermission(AppPermissions.Pages_UserDuAns, L("UserDuAns"));
            userDuAns.CreateChildPermission(AppPermissions.Pages_UserDuAns_Create, L("CreateNewUserDuAn"));
            userDuAns.CreateChildPermission(AppPermissions.Pages_UserDuAns_Edit, L("EditUserDuAn"));
            userDuAns.CreateChildPermission(AppPermissions.Pages_UserDuAns_Delete, L("DeleteUserDuAn"));



            var baoCaoVanBanDuAns = pages.CreateChildPermission(AppPermissions.Pages_BaoCaoVanBanDuAns, L("BaoCaoVanBanDuAns"));
            baoCaoVanBanDuAns.CreateChildPermission(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, L("CreateNewBaoCaoVanBanDuAn"));
            baoCaoVanBanDuAns.CreateChildPermission(AppPermissions.Pages_BaoCaoVanBanDuAns_Edit, L("EditBaoCaoVanBanDuAn"));
            baoCaoVanBanDuAns.CreateChildPermission(AppPermissions.Pages_BaoCaoVanBanDuAns_Delete, L("DeleteBaoCaoVanBanDuAn"));



            var traoDoiVanBanDuAns = pages.CreateChildPermission(AppPermissions.Pages_TraoDoiVanBanDuAns, L("TraoDoiVanBanDuAns"));
            traoDoiVanBanDuAns.CreateChildPermission(AppPermissions.Pages_TraoDoiVanBanDuAns_Create, L("CreateNewTraoDoiVanBanDuAn"));
            traoDoiVanBanDuAns.CreateChildPermission(AppPermissions.Pages_TraoDoiVanBanDuAns_Edit, L("EditTraoDoiVanBanDuAn"));
            traoDoiVanBanDuAns.CreateChildPermission(AppPermissions.Pages_TraoDoiVanBanDuAns_Delete, L("DeleteTraoDoiVanBanDuAn"));



            var quyetDinhs = pages.CreateChildPermission(AppPermissions.Pages_QuyetDinhs, L("QuyetDinhs"));
            quyetDinhs.CreateChildPermission(AppPermissions.Pages_QuyetDinhs_Create, L("CreateNewQuyetDinh"));
            quyetDinhs.CreateChildPermission(AppPermissions.Pages_QuyetDinhs_Edit, L("EditQuyetDinh"));
            quyetDinhs.CreateChildPermission(AppPermissions.Pages_QuyetDinhs_Delete, L("DeleteQuyetDinh"));



            var thungHoSos = pages.CreateChildPermission(AppPermissions.Pages_ThungHoSos, L("ThungHoSos"));
            thungHoSos.CreateChildPermission(AppPermissions.Pages_ThungHoSos_Create, L("CreateNewThungHoSo"));
            thungHoSos.CreateChildPermission(AppPermissions.Pages_ThungHoSos_Edit, L("EditThungHoSo"));
            thungHoSos.CreateChildPermission(AppPermissions.Pages_ThungHoSos_Delete, L("DeleteThungHoSo"));



            var dayKes = pages.CreateChildPermission(AppPermissions.Pages_DayKes, L("DayKes"));
            dayKes.CreateChildPermission(AppPermissions.Pages_DayKes_Create, L("CreateNewDayKe"));
            dayKes.CreateChildPermission(AppPermissions.Pages_DayKes_Edit, L("EditDayKe"));
            dayKes.CreateChildPermission(AppPermissions.Pages_DayKes_Delete, L("DeleteDayKe"));



            var phongKhos = pages.CreateChildPermission(AppPermissions.Pages_PhongKhos, L("PhongKhos"));
            phongKhos.CreateChildPermission(AppPermissions.Pages_PhongKhos_Create, L("CreateNewPhongKho"));
            phongKhos.CreateChildPermission(AppPermissions.Pages_PhongKhos_Edit, L("EditPhongKho"));
            phongKhos.CreateChildPermission(AppPermissions.Pages_PhongKhos_Delete, L("DeletePhongKho"));



            var chuyenHoSoGiaies = pages.CreateChildPermission(AppPermissions.Pages_ChuyenHoSoGiaies, L("ChuyenHoSoGiaies"));
            chuyenHoSoGiaies.CreateChildPermission(AppPermissions.Pages_ChuyenHoSoGiaies_Create, L("CreateNewChuyenHoSoGiay"));
            chuyenHoSoGiaies.CreateChildPermission(AppPermissions.Pages_ChuyenHoSoGiaies_Edit, L("EditChuyenHoSoGiay"));
            chuyenHoSoGiaies.CreateChildPermission(AppPermissions.Pages_ChuyenHoSoGiaies_Delete, L("DeleteChuyenHoSoGiay"));



            var loaiKhoans = pages.CreateChildPermission(AppPermissions.Pages_LoaiKhoans, L("LoaiKhoans"));
            loaiKhoans.CreateChildPermission(AppPermissions.Pages_LoaiKhoans_Create, L("CreateNewLoaiKhoan"));
            loaiKhoans.CreateChildPermission(AppPermissions.Pages_LoaiKhoans_Edit, L("EditLoaiKhoan"));
            loaiKhoans.CreateChildPermission(AppPermissions.Pages_LoaiKhoans_Delete, L("DeleteLoaiKhoan"));



            var chuongs = pages.CreateChildPermission(AppPermissions.Pages_Chuongs, L("Chuongs"));
            chuongs.CreateChildPermission(AppPermissions.Pages_Chuongs_Create, L("CreateNewChuong"));
            chuongs.CreateChildPermission(AppPermissions.Pages_Chuongs_Edit, L("EditChuong"));
            chuongs.CreateChildPermission(AppPermissions.Pages_Chuongs_Delete, L("DeleteChuong"));



            var capQuanLies = pages.CreateChildPermission(AppPermissions.Pages_CapQuanLies, L("CapQuanLies"));
            capQuanLies.CreateChildPermission(AppPermissions.Pages_CapQuanLies_Create, L("CreateNewCapQuanLy"));
            capQuanLies.CreateChildPermission(AppPermissions.Pages_CapQuanLies_Edit, L("EditCapQuanLy"));
            capQuanLies.CreateChildPermission(AppPermissions.Pages_CapQuanLies_Delete, L("DeleteCapQuanLy"));



            var quyTrinhDuAnAssigneds = pages.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAnAssigneds, L("QuyTrinhDuAnAssigneds"));
            quyTrinhDuAnAssigneds.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create, L("CreateNewQuyTrinhDuAnAssigned"));
            quyTrinhDuAnAssigneds.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit, L("EditQuyTrinhDuAnAssigned"));
            quyTrinhDuAnAssigneds.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Delete, L("DeleteQuyTrinhDuAnAssigned"));



            var vanBanDuAns = pages.CreateChildPermission(AppPermissions.Pages_VanBanDuAns, L("VanBanDuAns"));
            vanBanDuAns.CreateChildPermission(AppPermissions.Pages_VanBanDuAns_Create, L("CreateNewVanBanDuAn"));
            vanBanDuAns.CreateChildPermission(AppPermissions.Pages_VanBanDuAns_Edit, L("EditVanBanDuAn"));
            vanBanDuAns.CreateChildPermission(AppPermissions.Pages_VanBanDuAns_Delete, L("DeleteVanBanDuAn"));



            var duAns = pages.CreateChildPermission(AppPermissions.Pages_DuAns, L("DuAns"));
            duAns.CreateChildPermission(AppPermissions.Pages_DuAns_Create, L("CreateNewDuAn"));
            duAns.CreateChildPermission(AppPermissions.Pages_DuAns_Edit, L("EditDuAn"));
            duAns.CreateChildPermission(AppPermissions.Pages_DuAns_Delete, L("DeleteDuAn"));
            duAns.CreateChildPermission(AppPermissions.Pages_DuAns_XemToanBoDuAn, L("XemToanBoDuAn"));
            duAns.CreateChildPermission(AppPermissions.Pages_DuAns_ThemUserVaoDuAn, L("ThemUserVaoDuAn"));

            var duyethoso = pages.CreateChildPermission(AppPermissions.Pages_DuyetHoSo, L("DuyetHoSo"));

            var quyTrinhDuAns = pages.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAns, L("QuyTrinhDuAns"));
            quyTrinhDuAns.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAns_Create, L("CreateNewQuyTrinhDuAn"));
            quyTrinhDuAns.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAns_Edit, L("EditQuyTrinhDuAn"));
            quyTrinhDuAns.CreateChildPermission(AppPermissions.Pages_QuyTrinhDuAns_Delete, L("DeleteQuyTrinhDuAn"));



            var loaiDuAns = pages.CreateChildPermission(AppPermissions.Pages_LoaiDuAns, L("LoaiDuAns"));
            loaiDuAns.CreateChildPermission(AppPermissions.Pages_LoaiDuAns_Create, L("CreateNewLoaiDuAn"));
            loaiDuAns.CreateChildPermission(AppPermissions.Pages_LoaiDuAns_Edit, L("EditLoaiDuAn"));
            loaiDuAns.CreateChildPermission(AppPermissions.Pages_LoaiDuAns_Delete, L("DeleteLoaiDuAn"));



            var chuyenDoiSo = pages.CreateChildPermission(AppPermissions.Pages_ChuyenDoiSo, L("ChuyenDoiSo"), multiTenancySides: MultiTenancySides.Tenant);

            var chiTietDanhGias = pages.CreateChildPermission(AppPermissions.Pages_ChiTietDanhGias, L("ChiTietDanhGias"), multiTenancySides: MultiTenancySides.Tenant);
            chiTietDanhGias.CreateChildPermission(AppPermissions.Pages_ChiTietDanhGias_Create, L("CreateNewChiTietDanhGia"), multiTenancySides: MultiTenancySides.Tenant);
            chiTietDanhGias.CreateChildPermission(AppPermissions.Pages_ChiTietDanhGias_Edit, L("EditChiTietDanhGia"), multiTenancySides: MultiTenancySides.Tenant);
            chiTietDanhGias.CreateChildPermission(AppPermissions.Pages_ChiTietDanhGias_Delete, L("DeleteChiTietDanhGia"), multiTenancySides: MultiTenancySides.Tenant);



            //var tieuChiDanhGias = pages.CreateChildPermission(AppPermissions.Pages_TieuChiDanhGias, L("TieuChiDanhGias"), multiTenancySides: MultiTenancySides.Tenant);
            //tieuChiDanhGias.CreateChildPermission(AppPermissions.Pages_TieuChiDanhGias_Create, L("CreateNewTieuChiDanhGia"), multiTenancySides: MultiTenancySides.Tenant);
            //tieuChiDanhGias.CreateChildPermission(AppPermissions.Pages_TieuChiDanhGias_Edit, L("EditTieuChiDanhGia"), multiTenancySides: MultiTenancySides.Tenant);
            //tieuChiDanhGias.CreateChildPermission(AppPermissions.Pages_TieuChiDanhGias_Delete, L("DeleteTieuChiDanhGia"), multiTenancySides: MultiTenancySides.Tenant);

            var cayTieuChi = chuyenDoiSo.CreateChildPermission(AppPermissions.Pages_CayTieuChi, L("CayTieuChi"), multiTenancySides: MultiTenancySides.Tenant);
            cayTieuChi.CreateChildPermission(AppPermissions.Pages_CayTieuChi_ManageTree, L("CayTieuChiManage"), multiTenancySides: MultiTenancySides.Tenant);
            cayTieuChi.CreateChildPermission(AppPermissions.Pages_CayTieuChi_Delete, L("CayTieuChiDelete"), multiTenancySides: MultiTenancySides.Tenant);

            var quanHuyens = pages.CreateChildPermission(AppPermissions.Pages_QuanHuyens, L("QuanHuyens"), multiTenancySides: MultiTenancySides.Host);
            quanHuyens.CreateChildPermission(AppPermissions.Pages_QuanHuyens_Create, L("CreateNewQuanHuyen"), multiTenancySides: MultiTenancySides.Host);
            quanHuyens.CreateChildPermission(AppPermissions.Pages_QuanHuyens_Edit, L("EditQuanHuyen"), multiTenancySides: MultiTenancySides.Host);
            quanHuyens.CreateChildPermission(AppPermissions.Pages_QuanHuyens_Delete, L("DeleteQuanHuyen"), multiTenancySides: MultiTenancySides.Host);



            var tinhThanhs = pages.CreateChildPermission(AppPermissions.Pages_TinhThanhs, L("TinhThanhs"), multiTenancySides: MultiTenancySides.Host);
            tinhThanhs.CreateChildPermission(AppPermissions.Pages_TinhThanhs_Create, L("CreateNewTinhThanh"), multiTenancySides: MultiTenancySides.Host);
            tinhThanhs.CreateChildPermission(AppPermissions.Pages_TinhThanhs_Edit, L("EditTinhThanh"), multiTenancySides: MultiTenancySides.Host);
            tinhThanhs.CreateChildPermission(AppPermissions.Pages_TinhThanhs_Delete, L("DeleteTinhThanh"), multiTenancySides: MultiTenancySides.Host);



            var doiTuongChuyenDoiSos = chuyenDoiSo.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos, L("DoiTuongChuyenDoiSos"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_Create, L("CreateNewDoiTuongChuyenDoiSo"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_Edit, L("EditDoiTuongChuyenDoiSo"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_Delete, L("DeleteDoiTuongChuyenDoiSo"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_ChamDiem, L("ChamDiemDoiTuongChuyenDoiSo"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_DongBoChiTiet, L("DongBoChiTiet"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_TongHopDiem, L("TongHopDiem"), multiTenancySides: MultiTenancySides.Tenant);
            doiTuongChuyenDoiSos.CreateChildPermission(AppPermissions.Pages_DoiTuongChuyenDoiSos_HoiDongThamDinh, L("HoiDongThamDinh"), multiTenancySides: MultiTenancySides.Tenant);


            pages.CreateChildPermission(AppPermissions.Pages_DemoUiComponents, L("DemoUiComponents"));

            var administration = pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var roles = administration.CreateChildPermission(AppPermissions.Pages_Administration_Roles, L("Roles"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Create, L("CreatingNewRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Edit, L("EditingRole"));
            roles.CreateChildPermission(AppPermissions.Pages_Administration_Roles_Delete, L("DeletingRole"));

            var users = administration.CreateChildPermission(AppPermissions.Pages_Administration_Users, L("Users"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Create, L("CreatingNewUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Edit, L("EditingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Delete, L("DeletingUser"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_ChangePermissions, L("ChangingPermissions"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Impersonation, L("LoginForUsers"));
            users.CreateChildPermission(AppPermissions.Pages_Administration_Users_Unlock, L("Unlock"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_AuditLogs, L("AuditLogs"));

            var organizationUnits = administration.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits, L("OrganizationUnits"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageOrganizationTree, L("ManagingOrganizationTree"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageMembers, L("ManagingMembers"));
            organizationUnits.CreateChildPermission(AppPermissions.Pages_Administration_OrganizationUnits_ManageRoles, L("ManagingRoles"));

            administration.CreateChildPermission(AppPermissions.Pages_Administration_UiCustomization, L("VisualSettings"));

            var webhooks = administration.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription, L("Webhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Create, L("CreatingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Edit, L("EditingWebhooks"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_ChangeActivity, L("ChangingWebhookActivity"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_WebhookSubscription_Detail, L("DetailingSubscription"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ListSendAttempts, L("ListingSendAttempts"));
            webhooks.CreateChildPermission(AppPermissions.Pages_Administration_Webhook_ResendWebhook, L("ResendingWebhook"));

            var dynamicParameters = administration.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters, L("DynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Create, L("CreatingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Edit, L("EditingDynamicParameters"));
            dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameters_Delete, L("DeletingDynamicParameters"));

            var dynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue, L("DynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Create, L("CreatingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, L("EditingDynamicParameterValue"));
            dynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_DynamicParameterValue_Delete, L("DeletingDynamicParameterValue"));

            var entityDynamicParameters = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters, L("EntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Create, L("CreatingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Edit, L("EditingEntityDynamicParameters"));
            entityDynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameters_Delete, L("DeletingEntityDynamicParameters"));

            var entityDynamicParameterValues = dynamicParameters.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue, L("EntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create, L("CreatingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit, L("EditingEntityDynamicParameterValue"));
            entityDynamicParameterValues.CreateChildPermission(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete, L("DeletingEntityDynamicParameterValue"));

            //TENANT-SPECIFIC PERMISSIONS

            pages.CreateChildPermission(AppPermissions.Pages_Tenant_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Tenant);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Tenant_SubscriptionManagement, L("Subscription"), multiTenancySides: MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Admin_Tenant_Widgets, L("DashboardBaoCaoAdmin"), multiTenancySides: MultiTenancySides.Tenant);

            //HOST-SPECIFIC PERMISSIONS

            var editions = pages.CreateChildPermission(AppPermissions.Pages_Editions, L("Editions"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Create, L("CreatingNewEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Edit, L("EditingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_Delete, L("DeletingEdition"), multiTenancySides: MultiTenancySides.Host);
            editions.CreateChildPermission(AppPermissions.Pages_Editions_MoveTenantsToAnotherEdition, L("MoveTenantsToAnotherEdition"), multiTenancySides: MultiTenancySides.Host);

            var tenants = pages.CreateChildPermission(AppPermissions.Pages_Tenants, L("Tenants"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Create, L("CreatingNewTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Edit, L("EditingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_ChangeFeatures, L("ChangingFeatures"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Delete, L("DeletingTenant"), multiTenancySides: MultiTenancySides.Host);
            tenants.CreateChildPermission(AppPermissions.Pages_Tenants_Impersonation, L("LoginForTenants"), multiTenancySides: MultiTenancySides.Host);

            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Settings, L("Settings"), multiTenancySides: MultiTenancySides.Host);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Maintenance, L("Maintenance"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_HangfireDashboard, L("HangfireDashboard"), multiTenancySides: _isMultiTenancyEnabled ? MultiTenancySides.Host : MultiTenancySides.Tenant);
            administration.CreateChildPermission(AppPermissions.Pages_Administration_Host_Dashboard, L("Dashboard"), multiTenancySides: MultiTenancySides.Host);

            var languages = administration.CreateChildPermission(AppPermissions.Pages_Administration_Languages, L("Languages"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Create, L("CreatingNewLanguage"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Edit, L("EditingLanguage"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_Delete, L("DeletingLanguages"), multiTenancySides: MultiTenancySides.Host);
            languages.CreateChildPermission(AppPermissions.Pages_Administration_Languages_ChangeTexts, L("ChangingTexts"), multiTenancySides: MultiTenancySides.Host);
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, ChuyenDoiSoConsts.LocalizationSourceName);
        }
    }
}
