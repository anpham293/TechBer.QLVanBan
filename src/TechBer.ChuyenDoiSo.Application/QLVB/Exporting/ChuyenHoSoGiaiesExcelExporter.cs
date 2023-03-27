using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class ChuyenHoSoGiaiesExcelExporter : NpoiExcelExporterBase, IChuyenHoSoGiaiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChuyenHoSoGiaiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChuyenHoSoGiayForViewDto> chuyenHoSoGiaies)
        {
            return CreateExcelPackage(
                "ChuyenHoSoGiaies.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ChuyenHoSoGiaies"));

                    AddHeader(
                        sheet,
                        L("NguoiChuyenId"),
                        L("ThoiGianChuyen"),
                        L("SoLuong"),
                        L("TrangThai"),
                        L("ThoiGianNhan"),
                        (L("VanBanDuAn")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, chuyenHoSoGiaies,
                        _ => _.ChuyenHoSoGiay.NguoiChuyenId,
                        _ => _timeZoneConverter.Convert(_.ChuyenHoSoGiay.ThoiGianChuyen, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.ChuyenHoSoGiay.SoLuong,
                        _ => _.ChuyenHoSoGiay.TrangThai,
                        _ => _timeZoneConverter.Convert(_.ChuyenHoSoGiay.ThoiGianNhan, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.VanBanDuAnName,
                        _ => _.UserName
                        );

					
					for (var i = 1; i <= chuyenHoSoGiaies.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[2], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(2);for (var i = 1; i <= chuyenHoSoGiaies.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(5);
                });
        }
    }
}
