using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting
{
    public class LoaiKhoansExcelExporter : NpoiExcelExporterBase, ILoaiKhoansExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public LoaiKhoansExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetLoaiKhoanForViewDto> loaiKhoans)
        {
            return CreateExcelPackage(
                "LoaiKhoans.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("LoaiKhoans"));

                    AddHeader(
                        sheet,
                        L("MaSo"),
                        L("Ten"),
                        L("GhiChu")
                        );

                    AddObjects(
                        sheet, 2, loaiKhoans,
                        _ => _.LoaiKhoan.MaSo,
                        _ => _.LoaiKhoan.Ten,
                        _ => _.LoaiKhoan.GhiChu
                        );

					
					
                });
        }
    }
}
