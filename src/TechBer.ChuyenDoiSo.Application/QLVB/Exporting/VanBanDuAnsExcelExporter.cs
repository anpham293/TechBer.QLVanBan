using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class VanBanDuAnsExcelExporter : NpoiExcelExporterBase, IVanBanDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public VanBanDuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetVanBanDuAnForViewDto> vanBanDuAns)
        {
            return CreateExcelPackage(
                "VanBanDuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("VanBanDuAns"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("KyHieuVanBan"),
                        L("NgayBanHanh"),
                        L("FileVanBan"),
                        (L("DuAn")) + L("Name"),
                        (L("QuyTrinhDuAn")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, vanBanDuAns,
                        _ => _.VanBanDuAn.Name,
                        _ => _.VanBanDuAn.KyHieuVanBan,
                        _ => _timeZoneConverter.Convert(_.VanBanDuAn.NgayBanHanh, _abpSession.TenantId, _abpSession.GetUserId()),
                        _ => _.VanBanDuAn.FileVanBan,
                        _ => _.DuAnName,
                        _ => _.QuyTrinhDuAnName
                        );

					
					for (var i = 1; i <= vanBanDuAns.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[3], "yyyy-mm-dd");
                    }
                    sheet.AutoSizeColumn(3);
                });
        }
    }
}
