using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class TraoDoiVanBanDuAnsExcelExporter : NpoiExcelExporterBase, ITraoDoiVanBanDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TraoDoiVanBanDuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTraoDoiVanBanDuAnForViewDto> traoDoiVanBanDuAns)
        {
            return CreateExcelPackage(
                "TraoDoiVanBanDuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("TraoDoiVanBanDuAns"));

                    AddHeader(
                        sheet,
                        L("NgayGui"),
                        L("NoiDung"),
                        (L("User")) + L("Name"),
                        (L("VanBanDuAn")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, traoDoiVanBanDuAns,
                        _ => _timeZoneConverter.Convert(_.TraoDoiVanBanDuAn.NgayGui, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.TraoDoiVanBanDuAn.NoiDung,
                        _ => _.UserName,
                        _ => _.VanBanDuAnName
                        );

					
					for (var i = 1; i <= traoDoiVanBanDuAns.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[1], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(1);
                });
        }
    }
}
