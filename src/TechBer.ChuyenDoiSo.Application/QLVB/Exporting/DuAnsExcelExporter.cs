using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class DuAnsExcelExporter : NpoiExcelExporterBase, IDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDuAnForViewDto> duAns)
        {
            return CreateExcelPackage(
                "DuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DuAns"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Descriptions")
                        );

                    AddObjects(
                        sheet, 2, duAns,
                        _ => _.DuAn.Name,
                        _ => _.DuAn.Descriptions
                        );

					
					
                });
        }
    }
}
