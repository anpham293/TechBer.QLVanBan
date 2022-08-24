using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class QuyTrinhDuAnAssignedsExcelExporter : NpoiExcelExporterBase, IQuyTrinhDuAnAssignedsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public QuyTrinhDuAnAssignedsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetQuyTrinhDuAnAssignedForViewDto> quyTrinhDuAnAssigneds)
        {
            return CreateExcelPackage(
                "QuyTrinhDuAnAssigneds.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("QuyTrinhDuAnAssigneds"));

                    AddHeader(
                        sheet,
                        L("Name"),
                        L("Descriptions"),
                        L("STT"),
                        L("SoVanBanQuyDinh"),
                        L("MaQuyTrinh"),
                        (L("LoaiDuAn")) + L("Name"),
                        (L("QuyTrinhDuAn")) + L("Name"),
                        (L("QuyTrinhDuAnAssigned")) + L("Name"),
                        (L("DuAn")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, quyTrinhDuAnAssigneds,
                        _ => _.QuyTrinhDuAnAssigned.Name,
                        _ => _.QuyTrinhDuAnAssigned.Descriptions,
                        _ => _.QuyTrinhDuAnAssigned.STT,
                        _ => _.QuyTrinhDuAnAssigned.SoVanBanQuyDinh,
                        _ => _.QuyTrinhDuAnAssigned.MaQuyTrinh,
                        _ => _.LoaiDuAnName,
                        _ => _.QuyTrinhDuAnName,
                        _ => _.QuyTrinhDuAnAssignedName,
                        _ => _.DuAnName
                        );

					
					
                });
        }
    }
}
