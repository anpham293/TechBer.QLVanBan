using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public class QuanHuyensExcelExporter : NpoiExcelExporterBase, IQuanHuyensExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public QuanHuyensExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetQuanHuyenForViewDto> quanHuyens)
        {
            return CreateExcelPackage(
                "QuanHuyens.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("QuanHuyens"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Ma"),
                        (L("TinhThanh")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, quanHuyens,
                        _ => _.QuanHuyen.Name,
                        _ => _.QuanHuyen.Ma,
                        _ => _.TinhThanhName
                        );

					
					
                });
        }
    }
}
