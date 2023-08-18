using System.Collections.Generic;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QLVB.Exporting
{
    public class UserDuAnsExcelExporter : NpoiExcelExporterBase, IUserDuAnsExcelExporter
    {

        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;

        public UserDuAnsExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
			ITempFileCacheManager tempFileCacheManager) :  
	base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
        }

        public FileDto ExportToFile(List<GetUserDuAnForViewDto> userDuAns)
        {
            return CreateExcelPackage(
                "UserDuAns.xlsx",
                excelPackage =>
                {
                    
                    var sheet = excelPackage.CreateSheet(L("UserDuAns"));

                    AddHeader(
                        sheet,
                        L("TrangThai"),
                        (L("User")) + L("Name"),
                        (L("DuAn")) + L("Name")
                        );

                    AddObjects(
                        sheet, 2, userDuAns,
                        _ => _.UserDuAn.TrangThai,
                        _ => _.UserName,
                        _ => _.DuAnName
                        );

					
					
                });
        }
    }
}
