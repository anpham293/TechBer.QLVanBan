using Abp.Auditing;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using TechBer.ChuyenDoiSo.Tenants.Dashboard.Dto;

namespace TechBer.ChuyenDoiSo.Tenants.Dashboard
{
    [DisableAuditing]
    [AbpAuthorize(AppPermissions.Pages_Tenant_Dashboard)]
    public class TenantDashboardAppService : ChuyenDoiSoAppServiceBase, ITenantDashboardAppService
    {
        private readonly IRepository<DoiTuongChuyenDoiSo> _doiTuongChuyenDoiSoRepository;
        private readonly IRepository<TieuChiDanhGia> _tieuChiDanhGiaRepository;
        private readonly IRepository<ChiTietDanhGia> _chiTietDanhGiaRepository;

        public TenantDashboardAppService(
            IRepository<DoiTuongChuyenDoiSo> doiTuongChuyenDoiSoRepository,
            IRepository<TieuChiDanhGia> tieuChiDanhGiaRepository,
            IRepository<ChiTietDanhGia> chiTietDanhGiaRepository)
        {
            _doiTuongChuyenDoiSoRepository = doiTuongChuyenDoiSoRepository;
            _tieuChiDanhGiaRepository = tieuChiDanhGiaRepository;
            _chiTietDanhGiaRepository = chiTietDanhGiaRepository;
        }

        public GetMemberActivityOutput GetMemberActivity()
        {
            return new GetMemberActivityOutput
            (
                DashboardRandomDataGenerator.GenerateMemberActivities()
            );
        }

        public GetDashboardDataOutput GetDashboardData(GetDashboardDataInput input)
        {
            var output = new GetDashboardDataOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500),
                SalesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod),
                Expenses = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(5000, 10000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(1000, 9000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(10000, 90000),
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50),
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };

            return output;
        }

        public GetTopStatsOutput GetTopStats()
        {
            return new GetTopStatsOutput
            {
                TotalProfit = DashboardRandomDataGenerator.GetRandomInt(5000, 9000),
                NewFeedbacks = DashboardRandomDataGenerator.GetRandomInt(1000, 5000),
                NewOrders = DashboardRandomDataGenerator.GetRandomInt(100, 900),
                NewUsers = DashboardRandomDataGenerator.GetRandomInt(50, 500)
            };
        }

        public GetProfitShareOutput GetProfitShare()
        {
            return new GetProfitShareOutput
            {
                ProfitShares = DashboardRandomDataGenerator.GetRandomPercentageArray(3)
            };
        }

        public GetDailySalesOutput GetDailySales()
        {
            return new GetDailySalesOutput
            {
                DailySales = DashboardRandomDataGenerator.GetRandomArray(30, 10, 50)
            };
        }

        public GetSalesSummaryOutput GetSalesSummary(GetSalesSummaryInput input)
        {
            var salesSummary = DashboardRandomDataGenerator.GenerateSalesSummaryData(input.SalesSummaryDatePeriod);
            return new GetSalesSummaryOutput(salesSummary)
            {
                Expenses = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Growth = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                Revenue = DashboardRandomDataGenerator.GetRandomInt(0, 3000),
                TotalSales = DashboardRandomDataGenerator.GetRandomInt(0, 3000)
            };
        }

        public GetRegionalStatsOutput GetRegionalStats()
        {
            return new GetRegionalStatsOutput(
                DashboardRandomDataGenerator.GenerateRegionalStat()
            );
        }

        public GetGeneralStatsOutput GetGeneralStats()
        {
            return new GetGeneralStatsOutput
            {
                TransactionPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                NewVisitPercent = DashboardRandomDataGenerator.GetRandomInt(10, 100),
                BouncePercent = DashboardRandomDataGenerator.GetRandomInt(10, 100)
            };
        }

        public async Task<List<BaoCaoChamDiemOutput>> GetBaoCaoChamDiem()
        {
            var doiTuongQuery = from o in _doiTuongChuyenDoiSoRepository.GetAll()
                                orderby o.TongDiemTuDanhGia descending, o.TongDiemHoiDongThamDinh descending, o.TongDiemDatDuoc descending
                                select new BaoCaoChamDiemOutput
                                {
                                    DoiTuongId = o.Id,
                                    Name = o.Name,
                                    Type = o.Type,
                                    DiemDatDuoc = o.TongDiemDatDuoc ?? 0,
                                    DiemHoiDongThamDinh = o.TongDiemHoiDongThamDinh ?? 0,
                                    DiemTuDanhGia = o.TongDiemTuDanhGia ?? 0
                                };

            return await doiTuongQuery.ToListAsync();
        }

        public async Task<BaoCaoTongHopOutput> GetBaoCaoTongHop()
        {
            var dsChiTietDanhGia = await _chiTietDanhGiaRepository.GetAll()
                                        .Include(p => p.TieuChiDanhGiaFk)
                                        .Where(p => p.TieuChiDanhGiaFk.DoSau == 1)
                                        .ToListAsync();

            var dsPhanNhom = Enum.GetValues(typeof(PhanNhomLevel1State)).Cast<PhanNhomLevel1State>();
            var soDoiTuong = await _doiTuongChuyenDoiSoRepository.GetAll().CountAsync();
            var output = new BaoCaoTongHopOutput();

            foreach (var item in dsChiTietDanhGia)
            {
                switch (item.TieuChiDanhGiaFk.PhanNhomLevel1)
                {
                    case (int)PhanNhomLevel1State.THONG_TIN_CHUNG:
                        output.DiemDatDuoc.ThongTinChung += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.ThongTinChung += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.ThongTinChung += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.NHAN_THUC_SO:
                        output.DiemDatDuoc.NhanThucSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.NhanThucSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.NhanThucSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.THE_CHE_SO:
                        output.DiemDatDuoc.TheCheSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.TheCheSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.TheCheSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.HA_TANG_SO:
                        output.DiemDatDuoc.HaTangSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.HaTangSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.HaTangSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.NHAN_LUC_SO:
                        output.DiemDatDuoc.NhanLucSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.NhanLucSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.NhanLucSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.AN_TOAN_THONG_TIN_MANG:
                        output.DiemDatDuoc.AnToanThongTinMang += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.AnToanThongTinMang += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.AnToanThongTinMang += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.HOAT_DONG_CHINH_QUYEN_SO:
                        output.DiemDatDuoc.HoatDongChinhQuyenSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.HoatDongChinhQuyenSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.HoatDongChinhQuyenSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.HOAT_DONG_KINH_TE_SO:
                        output.DiemDatDuoc.HoatDongKinhTeSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.HoatDongKinhTeSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.HoatDongKinhTeSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.HOAT_DONG_XA_HOI_SO:
                        output.DiemDatDuoc.HoatDongXaHoiSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.HoatDongXaHoiSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.HoatDongXaHoiSo += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.DO_THI_THONG_MINH:
                        output.DiemDatDuoc.DoThiThongMinh += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.DoThiThongMinh += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.DoThiThongMinh += item.DiemTuDanhGia ?? 0;
                        break;
                    case (int)PhanNhomLevel1State.THONG_TIN_VA_DU_LIEU_SO:
                        output.DiemDatDuoc.ThongTinVaDuLieuSo += item.DiemDatDuoc ?? 0;
                        output.DiemHoiDongThamDinh.ThongTinVaDuLieuSo += item.DiemHoiDongThamDinh ?? 0;
                        output.DiemTuDanhGia.ThongTinVaDuLieuSo += item.DiemTuDanhGia ?? 0;
                        break;
                    default:
                        break;
                }
            }

            output.ChiaDeuTheoSoDoiTuong(soDoiTuong);

            return output;
        }

        public async Task<List<DiemCuaTieuChiOutput>> GetBaoCaoChamDiemDoiTuong(int idDoiTuong)
        {
            var dsChiTietDanhGia = await _chiTietDanhGiaRepository.GetAll()
                            .Include(p => p.DoiTuongChuyenDoiSoFk)
                            .Include(p => p.TieuChiDanhGiaFk)
                            .Where(p => p.TieuChiDanhGiaFk.DoSau < 2)
                            .Where(p => p.DoiTuongChuyenDoiSoFk.Id == idDoiTuong)
                            .ToListAsync();

            var output = new List<DiemCuaTieuChiOutput>();

            var rootChiTietDanhGia = dsChiTietDanhGia.Where(p => p.TieuChiDanhGiaFk.DoSau == 0);

            foreach (var item in rootChiTietDanhGia)
            {
                var childs = dsChiTietDanhGia.Where(p => p.TieuChiDanhGiaFk.ParentId == item.TieuChiDanhGiaId);

                output.Add(new DiemCuaTieuChiOutput
                {
                    DiemDatDuoc = item.DiemDatDuoc ?? 0,
                    DiemHoiDongThamDinh = item.DiemHoiDongThamDinh ?? 0,
                    DiemTuDanhGia = item.DiemTuDanhGia ?? 0,
                    DoSau = item.TieuChiDanhGiaFk.DoSau,
                    TenTieuChi = item.TieuChiDanhGiaFk.Name
                });

                foreach (var child in childs)
                {
                    output.Add(new DiemCuaTieuChiOutput {
                        DiemDatDuoc = child.DiemDatDuoc ?? 0,
                        DiemHoiDongThamDinh = child.DiemHoiDongThamDinh ?? 0,
                        DiemTuDanhGia = child.DiemTuDanhGia ?? 0,
                        DoSau = child.TieuChiDanhGiaFk.DoSau,
                        TenTieuChi = child.TieuChiDanhGiaFk.Name
                    });
                }

            }

            return output;
        }
    }
}