using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting
{
    public class DuAnThuHoiesExcelExporter : NpoiExcelExporterBase, IDuAnThuHoiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DuAnThuHoiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDuAnThuHoiForViewDto> duAnThuHoies)
        {
            return CreateExcelPackage(
                "DuAnThuHoies.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DuAnThuHoies"));

                    AddHeader(
                        sheet,
                        L("MaDATH"),
                        L("Ten"),
                        L("NamQuanLy"),
                        L("ThoiHanBaoLanhHopDong"),
                        L("ThoiHanBaoLanhTamUng"),
                        L("GhiChu"),
                        L("TrangThai")
                        );

                    AddObjects(
                        sheet, 2, duAnThuHoies,
                        _ => _.DuAnThuHoi.MaDATH,
                        _ => _.DuAnThuHoi.Ten,
                        _ => _.DuAnThuHoi.NamQuanLy,
                        _ => _timeZoneConverter.Convert(_.DuAnThuHoi.ThoiHanBaoLanhHopDong, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.DuAnThuHoi.ThoiHanBaoLanhTamUng, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.DuAnThuHoi.GhiChu,
                        _ => _.DuAnThuHoi.TrangThai
                        );

					
					for (var i = 1; i <= duAnThuHoies.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(4);for (var i = 1; i <= duAnThuHoies.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5);
                });
        }
    }
}
