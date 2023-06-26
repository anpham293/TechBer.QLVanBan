using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using System;
using System.Linq;
using Abp.Organizations;
using TechBer.ChuyenDoiSo.Authorization.Roles;
using TechBer.ChuyenDoiSo.MultiTenancy;

namespace TechBer.ChuyenDoiSo.EntityHistory
{
    public static class EntityHistoryHelper
    {
        public const string EntityHistoryConfigurationName = "EntityHistory";

        public static readonly Type[] HostSideTrackedTypes =
        {
            typeof(BaoCaoVanBanDuAn),
            typeof(TraoDoiVanBanDuAn),
            typeof(QuyetDinh),
            typeof(ThungHoSo),
            typeof(DayKe),
            typeof(PhongKho),
            typeof(ChuyenHoSoGiay),
            typeof(LoaiKhoan),
            typeof(Chuong),
            typeof(CapQuanLy),
            typeof(QuyTrinhDuAnAssigned),
            typeof(QuyTrinhDuAn),
            typeof(VanBanDuAn),
            typeof(LoaiDuAn),
            typeof(QuanHuyen),
            typeof(TinhThanh),
            typeof(OrganizationUnit), typeof(Role), typeof(Tenant)
        };

        public static readonly Type[] TenantSideTrackedTypes =
        {
            typeof(BaoCaoVanBanDuAn),
            typeof(TraoDoiVanBanDuAn),
            typeof(QuyetDinh),
            typeof(ThungHoSo),
            typeof(DayKe),
            typeof(PhongKho),
            typeof(ChuyenHoSoGiay),
            typeof(LoaiKhoan),
            typeof(Chuong),
            typeof(CapQuanLy),
            typeof(QuyTrinhDuAnAssigned),
            typeof(QuyTrinhDuAn),
            typeof(VanBanDuAn),
            typeof(LoaiDuAn),
            typeof(ChiTietDanhGia),
            typeof(TieuChiDanhGia),
            typeof(DoiTuongChuyenDoiSo),
            typeof(OrganizationUnit), typeof(Role)
        };

        public static readonly Type[] TrackedTypes =
            HostSideTrackedTypes
                .Concat(TenantSideTrackedTypes)
                .GroupBy(type => type.FullName)
                .Select(types => types.First())
                .ToArray();
    }
}
