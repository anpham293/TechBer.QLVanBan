using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting
{
    public class ChiTietThuHoiesExcelExporter : NpoiExcelExporterBase, IChiTietThuHoiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChiTietThuHoiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChiTietThuHoiForViewDto> chiTietThuHoies)
        {
            return CreateExcelPackage(
                "ChiTietThuHoies.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ChiTietThuHoies"));

                    AddHeader(
                        sheet,
                        L("Du1"),
                        L("Du2"),
                        L("Du3"),
                        L("Du4"),
                        L("Du5"),
                        L("Du6"),
                        L("Du7"),
                        L("Du8"),
                        L("Du9"),
                        L("Du10"),
                        L("Du11"),
                        L("Du12"),
                        L("Thu1"),
                        L("Thu2"),
                        L("Thu3"),
                        L("Thu4"),
                        L("Thu5"),
                        L("Thu6"),
                        L("Thu7"),
                        L("Thu8"),
                        L("Thu9"),
                        L("Thu10"),
                        L("Thu11"),
                        L("Thu12"),
                        L("GhiChu"),
                        L("Ten"),
                        (L("DanhMucThuHoi")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, chiTietThuHoies,
                        _ => _.ChiTietThuHoi.Du1,
                        _ => _.ChiTietThuHoi.Du2,
                        _ => _.ChiTietThuHoi.Du3,
                        _ => _.ChiTietThuHoi.Du4,
                        _ => _.ChiTietThuHoi.Du5,
                        _ => _.ChiTietThuHoi.Du6,
                        _ => _.ChiTietThuHoi.Du7,
                        _ => _.ChiTietThuHoi.Du8,
                        _ => _.ChiTietThuHoi.Du9,
                        _ => _.ChiTietThuHoi.Du10,
                        _ => _.ChiTietThuHoi.Du11,
                        _ => _.ChiTietThuHoi.Du12,
                        _ => _.ChiTietThuHoi.Thu1,
                        _ => _.ChiTietThuHoi.Thu2,
                        _ => _.ChiTietThuHoi.Thu3,
                        _ => _.ChiTietThuHoi.Thu4,
                        _ => _.ChiTietThuHoi.Thu5,
                        _ => _.ChiTietThuHoi.Thu6,
                        _ => _.ChiTietThuHoi.Thu7,
                        _ => _.ChiTietThuHoi.Thu8,
                        _ => _.ChiTietThuHoi.Thu9,
                        _ => _.ChiTietThuHoi.Thu10,
                        _ => _.ChiTietThuHoi.Thu11,
                        _ => _.ChiTietThuHoi.Thu12,
                        _ => _.ChiTietThuHoi.GhiChu,
                        _ => _.ChiTietThuHoi.Ten,
                        _ => _.DanhMucThuHoiTen
                        );

					
					
                });
        }
    }
}
