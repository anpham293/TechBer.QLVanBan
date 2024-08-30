using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLySdtZalo.Exporting
{
    public class SdtZalosExcelExporter : NpoiExcelExporterBase, ISdtZalosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public SdtZalosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetSdtZaloForViewDto> sdtZalos)
        {
            return CreateExcelPackage(
                "SdtZalos.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("SdtZalos"));

                    AddHeader(
                        sheet,
                        L("Ten"),
                        L("Sdt")
                        );

                    AddObjects(
                        sheet, 2, sdtZalos,
                        _ => _.SdtZalo.Ten,
                        _ => _.SdtZalo.Sdt
                        );

					
					
                });
        }
    }
}
