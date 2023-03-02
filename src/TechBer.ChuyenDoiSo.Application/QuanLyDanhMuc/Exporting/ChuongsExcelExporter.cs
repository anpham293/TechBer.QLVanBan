using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public class ChuongsExcelExporter : NpoiExcelExporterBase, IChuongsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public ChuongsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetChuongForViewDto> chuongs)
        {
            return CreateExcelPackage(
                "Chuongs.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("Chuongs"));

                    AddHeader(
                        sheet,
                        L("MaSo"),
                        L("Ten"),
                        (L("CapQuanLy")) + L("Ten")
                        );

                    AddObjects(
                        sheet, 2, chuongs,
                        _ => _.Chuong.MaSo,
                        _ => _.Chuong.Ten,
                        _ => _.CapQuanLyTen
                        );

					
					
                });
        }
    }
}
