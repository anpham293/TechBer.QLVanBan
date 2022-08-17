using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class QuyTrinhDuAnsExcelExporter : NpoiExcelExporterBase, IQuyTrinhDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public QuyTrinhDuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetQuyTrinhDuAnForViewDto> quyTrinhDuAns)
        {
            return CreateExcelPackage(
                "QuyTrinhDuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("QuyTrinhDuAns"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Descriptions"),
                        L("STT"),
                        (L("LoaiDuAn")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, quyTrinhDuAns,
                        _ => _.QuyTrinhDuAn.Name,
                        _ => _.QuyTrinhDuAn.Descriptions,
                        _ => _.QuyTrinhDuAn.STT,
                        _ => _.LoaiDuAnName
                        );

					
					
                });
        }
    }
}
