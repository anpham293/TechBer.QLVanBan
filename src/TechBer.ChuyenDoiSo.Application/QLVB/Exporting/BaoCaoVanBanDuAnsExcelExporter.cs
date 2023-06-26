using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class BaoCaoVanBanDuAnsExcelExporter : NpoiExcelExporterBase, IBaoCaoVanBanDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public BaoCaoVanBanDuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetBaoCaoVanBanDuAnForViewDto> baoCaoVanBanDuAns)
        {
            return CreateExcelPackage(
                "BaoCaoVanBanDuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("BaoCaoVanBanDuAns"));

                    AddHeader(
                        sheet,
                        L("NoiDungCongViec"),
                        L("MoTaChiTiet"),
                        L("FileBaoCao"),
                        (L("VanBanDuAn")) + L("Name"),
                        (L("User")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, baoCaoVanBanDuAns,
                        _ => _.BaoCaoVanBanDuAn.NoiDungCongViec,
                        _ => _.BaoCaoVanBanDuAn.MoTaChiTiet,
                        _ => _.BaoCaoVanBanDuAn.FileBaoCao,
                        _ => _.VanBanDuAnName,
                        _ => _.UserName
                        );

					
					
                });
        }
    }
}
