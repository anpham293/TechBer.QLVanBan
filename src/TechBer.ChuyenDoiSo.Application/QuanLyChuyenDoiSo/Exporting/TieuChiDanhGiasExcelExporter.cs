using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Exporting
{
    public class TieuChiDanhGiasExcelExporter : NpoiExcelExporterBase, ITieuChiDanhGiasExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TieuChiDanhGiasExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTieuChiDanhGiaForViewDto> tieuChiDanhGias)
        {
            return CreateExcelPackage(
                "TieuChiDanhGias.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("TieuChiDanhGias"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("DiemToiDa"),
                        L("PhuongThucDanhGia"),
                        L("TaiLieuGiaiTrinh"),
                        L("GhiChu"),
                        L("LoaiPhuLuc"),
                        (L("TieuChiDanhGia")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, tieuChiDanhGias,
                        _ => _.TieuChiDanhGia.Name,
                        _ => _.TieuChiDanhGia.DiemToiDa,
                        _ => _.TieuChiDanhGia.PhuongThucDanhGia,
                        _ => _.TieuChiDanhGia.TaiLieuGiaiTrinh,
                        _ => _.TieuChiDanhGia.GhiChu,
                        _ => _.TieuChiDanhGia.LoaiPhuLuc,
                        _ => _.TieuChiDanhGiaName
                        );

					
					
                });
        }
    }
}
