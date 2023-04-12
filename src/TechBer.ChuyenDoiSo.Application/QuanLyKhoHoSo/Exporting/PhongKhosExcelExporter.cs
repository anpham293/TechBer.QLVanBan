using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Exporting
{
    public class PhongKhosExcelExporter : NpoiExcelExporterBase, IPhongKhosExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public PhongKhosExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetPhongKhoForViewDto> phongKhos)
        {
            return CreateExcelPackage(
                "PhongKhos.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("PhongKhos"));

                    AddHeader(
                        sheet,
                        L("MaSo"),
                        L("Ten")
                        );

                    AddObjects(
                        sheet, 2, phongKhos,
                        _ => _.PhongKho.MaSo,
                        _ => _.PhongKho.Ten
                        );

					
					
                });
        }
    }
}
