using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class QuyetDinhsExcelExporter : NpoiExcelExporterBase, IQuyetDinhsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public QuyetDinhsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetQuyetDinhForViewDto> quyetDinhs)
        {
            return CreateExcelPackage(
                "QuyetDinhs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("QuyetDinhs"));

                    AddHeader(
                        sheet,
                        L("So"),
                        L("Ten"),
                        L("NgayBanHanh"),
                        L("FileQuyetDinh"),
                        L("TrangThai")
                        );

                    AddObjects(
                        sheet, 2, quyetDinhs,
                        _ => _.QuyetDinh.So,
                        _ => _.QuyetDinh.Ten,
                        _ => _timeZoneConverter.Convert(_.QuyetDinh.NgayBanHanh, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.QuyetDinh.FileQuyetDinh,
                        _ => _.QuyetDinh.TrangThai
                        );

					
					for (var i = 1; i <= quyetDinhs.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}
