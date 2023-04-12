using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Exporting
{
    public class DayKesExcelExporter : NpoiExcelExporterBase, IDayKesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DayKesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDayKeForViewDto> dayKes)
        {
            return CreateExcelPackage(
                "DayKes.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DayKes"));

                    AddHeader(
                        sheet,
                        L("MaSo"),
                        L("Ten"),
                        (L("PhongKho")) + L("MaSo")
                        );

                    AddObjects(
                        sheet, 2, dayKes,
                        _ => _.DayKe.MaSo,
                        _ => _.DayKe.Ten,
                        _ => _.PhongKhoMaSo
                        );

					
					
                });
        }
    }
}
