using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Exporting
{
    public class DoiTuongChuyenDoiSosExcelExporter : NpoiExcelExporterBase, IDoiTuongChuyenDoiSosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public DoiTuongChuyenDoiSosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetDoiTuongChuyenDoiSoForViewDto> doiTuongChuyenDoiSos)
        {
            return CreateExcelPackage(
                "DoiTuongChuyenDoiSos.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("DoiTuongChuyenDoiSos"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Type"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, doiTuongChuyenDoiSos,
                        _ => _.DoiTuongChuyenDoiSo.Name,
                        _ => _.DoiTuongChuyenDoiSo.Type,
                        _ => _.UserName
                        );

					
					
                });
        }
    }
}
