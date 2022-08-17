using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public class TinhThanhsExcelExporter : NpoiExcelExporterBase, ITinhThanhsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public TinhThanhsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetTinhThanhForViewDto> tinhThanhs)
        {
            return CreateExcelPackage(
                "TinhThanhs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("TinhThanhs"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Ma")
                        );

                    AddObjects(
                        sheet, 2, tinhThanhs,
                        _ => _.TinhThanh.Name,
                        _ => _.TinhThanh.Ma
                        );

					
					
                });
        }
    }
}
