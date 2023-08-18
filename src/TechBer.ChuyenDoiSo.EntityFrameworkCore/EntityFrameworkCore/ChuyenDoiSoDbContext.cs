using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using Abp.IdentityServer4;
using Abp.Organizations;
using Abp.Zero.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TechBer.ChuyenDoiSo.Authorization.Delegation;
using TechBer.ChuyenDoiSo.Authorization.Roles;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.Chat;
using TechBer.ChuyenDoiSo.Editions;
using TechBer.ChuyenDoiSo.Friendships;
using TechBer.ChuyenDoiSo.MultiTenancy;
using TechBer.ChuyenDoiSo.MultiTenancy.Accounting;
using TechBer.ChuyenDoiSo.MultiTenancy.Payments;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.EntityFrameworkCore
{
    public class ChuyenDoiSoDbContext : AbpZeroDbContext<Tenant, Role, User, ChuyenDoiSoDbContext>, IAbpPersistedGrantDbContext
    {
        public virtual DbSet<UserDuAn> UserDuAns { get; set; }

        public virtual DbSet<BaoCaoVanBanDuAn> BaoCaoVanBanDuAns { get; set; }

        public virtual DbSet<TraoDoiVanBanDuAn> TraoDoiVanBanDuAns { get; set; }

        public virtual DbSet<QuyetDinh> QuyetDinhs { get; set; }

        public virtual DbSet<ThungHoSo> ThungHoSos { get; set; }

        public virtual DbSet<DayKe> DayKes { get; set; }

        public virtual DbSet<PhongKho> PhongKhos { get; set; }

        public virtual DbSet<ChuyenHoSoGiay> ChuyenHoSoGiaies { get; set; }

        public virtual DbSet<LoaiKhoan> LoaiKhoans { get; set; }

        public virtual DbSet<Chuong> Chuongs { get; set; }

        public virtual DbSet<CapQuanLy> CapQuanLies { get; set; }

        public virtual DbSet<QuyTrinhDuAnAssigned> QuyTrinhDuAnAssigneds { get; set; }

        public virtual DbSet<VanBanDuAn> VanBanDuAns { get; set; }

        public virtual DbSet<DuAn> DuAns { get; set; }

        public virtual DbSet<QuyTrinhDuAn> QuyTrinhDuAns { get; set; }

        public virtual DbSet<LoaiDuAn> LoaiDuAns { get; set; }

        public virtual DbSet<ChiTietDanhGia> ChiTietDanhGias { get; set; }

        public virtual DbSet<TieuChiDanhGia> TieuChiDanhGias { get; set; }

        public virtual DbSet<QuanHuyen> QuanHuyens { get; set; }

        public virtual DbSet<TinhThanh> TinhThanhs { get; set; }

        public virtual DbSet<DoiTuongChuyenDoiSo> DoiTuongChuyenDoiSos { get; set; }

        /* Define an IDbSet for each entity of the application */

        public virtual DbSet<BinaryObject> BinaryObjects { get; set; }

        public virtual DbSet<Friendship> Friendships { get; set; }

        public virtual DbSet<ChatMessage> ChatMessages { get; set; }

        public virtual DbSet<SubscribableEdition> SubscribableEditions { get; set; }

        public virtual DbSet<SubscriptionPayment> SubscriptionPayments { get; set; }

        public virtual DbSet<Invoice> Invoices { get; set; }

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        public virtual DbSet<SubscriptionPaymentExtensionData> SubscriptionPaymentExtensionDatas { get; set; }

        public virtual DbSet<UserDelegation> UserDelegations { get; set; }

        public ChuyenDoiSoDbContext(DbContextOptions<ChuyenDoiSoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
           
            modelBuilder.Entity<UserDuAn>(u =>
            {
                u.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BaoCaoVanBanDuAn>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<TraoDoiVanBanDuAn>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<QuyetDinh>(q =>
            {
                q.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ThungHoSo>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DayKe>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<PhongKho>(p =>
            {
                p.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ChuyenHoSoGiay>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LoaiKhoan>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<Chuong>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<CapQuanLy>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<QuyTrinhDuAnAssigned>(q =>
            {
                q.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<VanBanDuAn>(v =>
            {
                v.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DuAn>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<QuyTrinhDuAn>(q =>
            {
                q.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<LoaiDuAn>(l =>
            {
                l.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<ChiTietDanhGia>(c =>
            {
                c.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<TieuChiDanhGia>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<DoiTuongChuyenDoiSo>(d =>
            {
                d.HasIndex(e => new { e.TenantId });
            });
 modelBuilder.Entity<BinaryObject>(b =>
            {
                b.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<ChatMessage>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId, e.ReadState });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.TargetUserId, e.ReadState });
                b.HasIndex(e => new { e.TargetTenantId, e.UserId, e.ReadState });
            });

            modelBuilder.Entity<Friendship>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.UserId });
                b.HasIndex(e => new { e.TenantId, e.FriendUserId });
                b.HasIndex(e => new { e.FriendTenantId, e.UserId });
                b.HasIndex(e => new { e.FriendTenantId, e.FriendUserId });
            });

            modelBuilder.Entity<Tenant>(b =>
            {
                b.HasIndex(e => new { e.SubscriptionEndDateUtc });
                b.HasIndex(e => new { e.CreationTime });
            });

            modelBuilder.Entity<SubscriptionPayment>(b =>
            {
                b.HasIndex(e => new { e.Status, e.CreationTime });
                b.HasIndex(e => new { PaymentId = e.ExternalPaymentId, e.Gateway });
            });

            modelBuilder.Entity<SubscriptionPaymentExtensionData>(b =>
            {
                b.HasQueryFilter(m => !m.IsDeleted)
                    .HasIndex(e => new { e.SubscriptionPaymentId, e.Key, e.IsDeleted })
                    .IsUnique();
            });

            modelBuilder.Entity<UserDelegation>(b =>
            {
                b.HasIndex(e => new { e.TenantId, e.SourceUserId });
                b.HasIndex(e => new { e.TenantId, e.TargetUserId });
            });

            modelBuilder.ConfigurePersistedGrantEntity();
        }
    }
}
