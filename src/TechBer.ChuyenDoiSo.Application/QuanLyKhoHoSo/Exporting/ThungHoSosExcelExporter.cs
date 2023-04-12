using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Exporting
{
    public class ThungHoSosExcelExporter : NpoiExcelExporterBase, IThungHoSosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ThungHoSosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetThungHoSoForViewDto> thungHoSos)
        {
            return CreateExcelPackage(
                "ThungHoSos.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("ThungHoSos"));

                    AddHeader(
                        sheet,
                        L("MaSo"),
                        L("Ten"),
                        L("MoTa"),
                        L("TrangThai"),
                        (L("DayKe")) + L("MaSo"),
                        (L("DuAn")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, thungHoSos,
                        _ => _.ThungHoSo.MaSo,
                        _ => _.ThungHoSo.Ten,
                        _ => _.ThungHoSo.MoTa,
                        _ => _.ThungHoSo.TrangThai,
                        _ => _.DayKeMaSo,
                        _ => _.DuAnName
                        );

					
					
                });
        }
    }
}
