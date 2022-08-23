using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class LoaiDuAnsExcelExporter : NpoiExcelExporterBase, ILoaiDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LoaiDuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLoaiDuAnForViewDto> loaiDuAns)
        {
            return CreateExcelPackage(
                "LoaiDuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("LoaiDuAns"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        (L("OrganizationUnit")) + L("DisplayName")
                        );

                    AddObjects(
                        sheet, 2, loaiDuAns,
                        _ => _.LoaiDuAn.Name,
                        _ => _.OrganizationUnitDisplayName
                        );

					
					
                });
        }
    }
}
