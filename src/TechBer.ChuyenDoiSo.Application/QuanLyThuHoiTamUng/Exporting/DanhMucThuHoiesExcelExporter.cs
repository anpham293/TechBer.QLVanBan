using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting
{
    public class DanhMucThuHoiesExcelExporter : NpoiExcelExporterBase, IDanhMucThuHoiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DanhMucThuHoiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDanhMucThuHoiForViewDto> danhMucThuHoies)
        {
            return CreateExcelPackage(
                "DanhMucThuHoies.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DanhMucThuHoies"));

                    AddHeader(
                        sheet,
                        L("Stt"),
                        L("Ten"),
                        L("GhiChu"),
                        L("Type"),
                        (L("DuAnThuHoi")) + L("MaDATH")
                        );

                    AddObjects(
                        sheet, 2, danhMucThuHoies,
                        _ => _.DanhMucThuHoi.Stt,
                        _ => _.DanhMucThuHoi.Ten,
                        _ => _.DanhMucThuHoi.GhiChu,
                        _ => _.DanhMucThuHoi.Type,
                        _ => _.DuAnThuHoiMaDATH
                        );

					
					
                });
        }
    }
}
