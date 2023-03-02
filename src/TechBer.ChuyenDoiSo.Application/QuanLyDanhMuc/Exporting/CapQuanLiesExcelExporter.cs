using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public class CapQuanLiesExcelExporter : NpoiExcelExporterBase, ICapQuanLiesExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public CapQuanLiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetCapQuanLyForViewDto> capQuanLies)
        {
            return CreateExcelPackage(
                "CapQuanLies.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("CapQuanLies"));

                    AddHeader(
                        sheet,
                        L("Ten")
                        );

                    AddObjects(
                        sheet, 2, capQuanLies,
                        _ => _.CapQuanLy.Ten
                        );

					
					
                });
        }
    }
}
