using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using Abp.Runtime.Session;
using Abp.Timing.Timezone;
using Core.Excel;
using Microsoft.Extensions.Caching.Memory;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using TechBer.ChuyenDoiSo.Common;
using TechBer.ChuyenDoiSo.DataExporting.Excel.NPOI;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting
{
    public class DuAnThuHoiesExcelExporter : NpoiExcelExporterBase, IDuAnThuHoiesExcelExporter
    {
        private readonly ITimeZoneConverter _timeZoneConverter;
        private readonly IAbpSession _abpSession;
        private readonly ExcelExporter _exporter;

        public DuAnThuHoiesExcelExporter(
            ITimeZoneConverter timeZoneConverter,
            IAbpSession abpSession,
            IMemoryCache cache,
            ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
            _timeZoneConverter = timeZoneConverter;
            _abpSession = abpSession;
            _exporter = new ExcelExporter(tempFileCacheManager);
        }

        public FileDto ExportToFile(List<GetDuAnThuHoiForViewDto> duAnThuHoies)
        {
            return CreateExcelPackage(
                "DuAnThuHoies.xlsx",
                excelPackage =>
                {
                    var sheet = excelPackage.CreateSheet(L("DuAnThuHoies"));

                    AddHeader(
                        sheet,
                        L("MaDATH"),
                        L("Ten"),
                        L("NamQuanLy"),
                        L("ThoiHanBaoLanhHopDong"),
                        L("ThoiHanBaoLanhTamUng"),
                        L("GhiChu"),
                        L("TrangThai")
                    );

                    AddObjects(
                        sheet, 2, duAnThuHoies,
                        _ => _.DuAnThuHoi.MaDATH,
                        _ => _.DuAnThuHoi.Ten,
                        _ => _.DuAnThuHoi.NamQuanLy,
                        _ => _timeZoneConverter.Convert(_.DuAnThuHoi.ThoiHanBaoLanhHopDong, _abpSession.TenantId,
                            _abpSession.GetUserId()),
                        _ => _timeZoneConverter.Convert(_.DuAnThuHoi.ThoiHanBaoLanhTamUng, _abpSession.TenantId,
                            _abpSession.GetUserId()),
                        _ => _.DuAnThuHoi.GhiChu,
                        _ => _.DuAnThuHoi.TrangThai
                    );


                    for (var i = 1; i <= duAnThuHoies.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[4], "yyyy-mm-dd");
                    }

                    sheet.AutoSizeColumn(4);
                    for (var i = 1; i <= duAnThuHoies.Count; i++)
                    {
                        SetCellDataFormat(sheet.GetRow(i).Cells[5], "yyyy-mm-dd");
                    }

                    sheet.AutoSizeColumn(5);
                });
        }

        // public FileDto BaoCaoDuAnThuHoi_ExportToFile(BaoCaoDuAnThuHoi_ExportToFileDto baoCao)
        // {
        //     try
        //     {
        //         return CreateExcelPackage(
        //             "baoCao_"+(int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + ".xlsx",
        //             excelPackage =>
        //             {
        //                 var sheet = excelPackage.CreateSheet("Báo cáo");
        //                 // AddHeader(
        //                 //     sheet,
        //                 //     "STT",
        //                 //     "TenDuAn",
        //                 //     "TongCong",
        //                 //     "Du1",
        //                 //     "Du2",
        //                 //     "Du3",
        //                 //     "Du4",
        //                 //     "Du5",
        //                 //     "Du6",
        //                 //     "Du7",
        //                 //     "Du8",
        //                 //     "Du9",
        //                 //     "Du10",
        //                 //     "Du11",
        //                 //     "Du12",
        //                 //     "TongCong",
        //                 //     "Thu1",
        //                 //     "Thu2",
        //                 //     "Thu3",
        //                 //     "Thu4",
        //                 //     "Thu5",
        //                 //     "Thu6",
        //                 //     "Thu7",
        //                 //     "Thu8",
        //                 //     "Thu9",
        //                 //     "Thu10",
        //                 //     "Thu11",
        //                 //     "Thu12",
        //                 //     "TongCong",
        //                 //     "ThuThucTe1",
        //                 //     "ThuThucTe2",
        //                 //     "ThuThucTe3",
        //                 //     "ThuThucTe4",
        //                 //     "ThuThucTe5",
        //                 //     "ThuThucTe6",
        //                 //     "ThuThucTe7",
        //                 //     "ThuThucTe8",
        //                 //     "ThuThucTe9",
        //                 //     "ThuThucTe10",
        //                 //     "ThuThucTe11",
        //                 //     "ThuThucTe12",
        //                 //     "KinhPhiTamUngChuyen2025",
        //                 //     "ThoiHanBaoLanhThucHienHĐ",
        //                 //     "ThoiHanBaoLanhTamUng",
        //                 //     "GhiChu"
        //                 // );
        //                 
        //                 var rowIndex = 0;
        //                 
        //                 //Style tiêu đề
        //                 //Style
        //                 var font = excelPackage.CreateFont();
        //                 font.FontHeightInPoints = 11;
        //                 font.IsBold = true; // In đậm
        //                 
        //                 var fontNoiDung = excelPackage.CreateFont();
        //                 fontNoiDung.FontHeightInPoints = 11;
        //                 fontNoiDung.IsItalic = true; // Nghiêng
        //             
        //                 ICellStyle styleTieuDe = excelPackage.CreateCellStyle();
        //                 styleTieuDe.SetFont(font);
        //                 styleTieuDe.BorderTop = BorderStyle.Thin;
        //                 styleTieuDe.BorderBottom = BorderStyle.Thin;
        //                 styleTieuDe.BorderLeft = BorderStyle.Thin;
        //                 styleTieuDe.BorderRight = BorderStyle.Thin;
        //                 styleTieuDe.Alignment = HorizontalAlignment.Center; 
        //                 styleTieuDe.VerticalAlignment = VerticalAlignment.Center;
        //                 
        //                 ICellStyle styleDuAn = excelPackage.CreateCellStyle();
        //                 styleDuAn.SetFont(font);
        //                 styleDuAn.BorderTop = BorderStyle.Thin;
        //                 styleDuAn.BorderBottom = BorderStyle.Thin;
        //                 styleDuAn.BorderLeft = BorderStyle.Thin;
        //                 styleDuAn.BorderRight = BorderStyle.Thin;
        //                 
        //                 ICellStyle styleDanhMuc = excelPackage.CreateCellStyle();
        //                 styleDanhMuc.SetFont(font);
        //                 styleDanhMuc.BorderTop = BorderStyle.Thin;
        //                 styleDanhMuc.BorderBottom = BorderStyle.Thin;
        //                 styleDanhMuc.BorderLeft = BorderStyle.Thin;
        //                 styleDanhMuc.BorderRight = BorderStyle.Thin;
        //
        //                 ICellStyle styleData = excelPackage.CreateCellStyle();
        //                 styleData.SetFont(fontNoiDung);
        //                 styleData.BorderTop = BorderStyle.Thin;
        //                 styleData.BorderBottom = BorderStyle.Thin;
        //                 styleData.BorderLeft = BorderStyle.Thin;
        //                 styleData.BorderRight = BorderStyle.Thin;
        //                 
        //                 //Tiêu đề
        //                 sheet.CreateRow(rowIndex);
        //                 IRow rowTieuDe1 = sheet.GetRow(rowIndex);
        //                 ICell cellTieuDe_1_0 = rowTieuDe1.CreateCell(0);
        //                 ICell cellTieuDe_1_1 = rowTieuDe1.CreateCell(1);
        //                 ICell cellTieuDe_1_2 = rowTieuDe1.CreateCell(2);
        //                 ICell cellTieuDe_1_3 = rowTieuDe1.CreateCell(3);
        //                 ICell cellTieuDe_1_4 = rowTieuDe1.CreateCell(4);
        //                 ICell cellTieuDe_1_5 = rowTieuDe1.CreateCell(5);
        //                 ICell cellTieuDe_1_6 = rowTieuDe1.CreateCell(6);
        //                 ICell cellTieuDe_1_7 = rowTieuDe1.CreateCell(7);
        //                 ICell cellTieuDe_1_8 = rowTieuDe1.CreateCell(8);
        //                 ICell cellTieuDe_1_9 = rowTieuDe1.CreateCell(9); 
        //                 ICell cellTieuDe_1_10 = rowTieuDe1.CreateCell(10);
        //                 ICell cellTieuDe_1_11 = rowTieuDe1.CreateCell(11);
        //                 ICell cellTieuDe_1_12 = rowTieuDe1.CreateCell(12);
        //                 ICell cellTieuDe_1_13 = rowTieuDe1.CreateCell(13);
        //                 ICell cellTieuDe_1_14 = rowTieuDe1.CreateCell(14);
        //                 ICell cellTieuDe_1_15 = rowTieuDe1.CreateCell(15);
        //                 ICell cellTieuDe_1_16 = rowTieuDe1.CreateCell(16);
        //                 ICell cellTieuDe_1_17 = rowTieuDe1.CreateCell(17);
        //                 ICell cellTieuDe_1_18 = rowTieuDe1.CreateCell(18);
        //                 ICell cellTieuDe_1_19 = rowTieuDe1.CreateCell(19);
        //                 ICell cellTieuDe_1_20 = rowTieuDe1.CreateCell(20);
        //                 ICell cellTieuDe_1_21 = rowTieuDe1.CreateCell(21);
        //                 ICell cellTieuDe_1_22 = rowTieuDe1.CreateCell(22);
        //                 ICell cellTieuDe_1_23 = rowTieuDe1.CreateCell(23);
        //                 ICell cellTieuDe_1_24 = rowTieuDe1.CreateCell(24);
        //                 ICell cellTieuDe_1_25 = rowTieuDe1.CreateCell(25);
        //                 ICell cellTieuDe_1_26 = rowTieuDe1.CreateCell(26);
        //                 ICell cellTieuDe_1_27 = rowTieuDe1.CreateCell(27);
        //                 ICell cellTieuDe_1_28 = rowTieuDe1.CreateCell(28);
        //                 ICell cellTieuDe_1_29 = rowTieuDe1.CreateCell(29);
        //                 ICell cellTieuDe_1_30 = rowTieuDe1.CreateCell(30);
        //                 ICell cellTieuDe_1_31 = rowTieuDe1.CreateCell(31);
        //                 ICell cellTieuDe_1_32 = rowTieuDe1.CreateCell(32);
        //                 ICell cellTieuDe_1_33 = rowTieuDe1.CreateCell(33);
        //                 ICell cellTieuDe_1_34 = rowTieuDe1.CreateCell(34);
        //                 ICell cellTieuDe_1_35 = rowTieuDe1.CreateCell(35);
        //                 ICell cellTieuDe_1_36 = rowTieuDe1.CreateCell(36);
        //                 ICell cellTieuDe_1_37 = rowTieuDe1.CreateCell(37);
        //                 ICell cellTieuDe_1_38 = rowTieuDe1.CreateCell(38);
        //                 ICell cellTieuDe_1_39 = rowTieuDe1.CreateCell(39);
        //                 ICell cellTieuDe_1_40 = rowTieuDe1.CreateCell(40);
        //                 ICell cellTieuDe_1_41 = rowTieuDe1.CreateCell(41);
        //                 ICell cellTieuDe_1_42 = rowTieuDe1.CreateCell(42);
        //                 ICell cellTieuDe_1_43 = rowTieuDe1.CreateCell(43);
        //                 ICell cellTieuDe_1_44 = rowTieuDe1.CreateCell(44);
        //                 rowIndex++;
        //                 
        //                 sheet.CreateRow(rowIndex);
        //                 IRow rowTieuDe2 = sheet.GetRow(rowIndex);
        //                 ICell cellTieuDe_2_0 = rowTieuDe2.CreateCell(0);
        //                 ICell cellTieuDe_2_1 = rowTieuDe2.CreateCell(1);
        //                 ICell cellTieuDe_2_2 = rowTieuDe2.CreateCell(2);
        //                 ICell cellTieuDe_2_3 = rowTieuDe2.CreateCell(3);
        //                 ICell cellTieuDe_2_4 = rowTieuDe2.CreateCell(4);
        //                 ICell cellTieuDe_2_5 = rowTieuDe2.CreateCell(5);
        //                 ICell cellTieuDe_2_6 = rowTieuDe2.CreateCell(6);
        //                 ICell cellTieuDe_2_7 = rowTieuDe2.CreateCell(7);
        //                 ICell cellTieuDe_2_8 = rowTieuDe2.CreateCell(8);
        //                 ICell cellTieuDe_2_9 = rowTieuDe2.CreateCell(9); 
        //                 ICell cellTieuDe_2_10 = rowTieuDe2.CreateCell(10);
        //                 ICell cellTieuDe_2_11 = rowTieuDe2.CreateCell(11);
        //                 ICell cellTieuDe_2_12 = rowTieuDe2.CreateCell(12);
        //                 ICell cellTieuDe_2_13 = rowTieuDe2.CreateCell(13);
        //                 ICell cellTieuDe_2_14 = rowTieuDe2.CreateCell(14);
        //                 ICell cellTieuDe_2_15 = rowTieuDe2.CreateCell(15);
        //                 ICell cellTieuDe_2_16 = rowTieuDe2.CreateCell(16);
        //                 ICell cellTieuDe_2_17 = rowTieuDe2.CreateCell(17);
        //                 ICell cellTieuDe_2_18 = rowTieuDe2.CreateCell(18);
        //                 ICell cellTieuDe_2_19 = rowTieuDe2.CreateCell(19);
        //                 ICell cellTieuDe_2_20 = rowTieuDe2.CreateCell(20);
        //                 ICell cellTieuDe_2_21 = rowTieuDe2.CreateCell(21);
        //                 ICell cellTieuDe_2_22 = rowTieuDe2.CreateCell(22);
        //                 ICell cellTieuDe_2_23 = rowTieuDe2.CreateCell(23);
        //                 ICell cellTieuDe_2_24 = rowTieuDe2.CreateCell(24);
        //                 ICell cellTieuDe_2_25 = rowTieuDe2.CreateCell(25);
        //                 ICell cellTieuDe_2_26 = rowTieuDe2.CreateCell(26);
        //                 ICell cellTieuDe_2_27 = rowTieuDe2.CreateCell(27);
        //                 ICell cellTieuDe_2_28 = rowTieuDe2.CreateCell(28);
        //                 ICell cellTieuDe_2_29 = rowTieuDe2.CreateCell(29);
        //                 ICell cellTieuDe_2_30 = rowTieuDe2.CreateCell(30);
        //                 ICell cellTieuDe_2_31 = rowTieuDe2.CreateCell(31);
        //                 ICell cellTieuDe_2_32 = rowTieuDe2.CreateCell(32);
        //                 ICell cellTieuDe_2_33 = rowTieuDe2.CreateCell(33);
        //                 ICell cellTieuDe_2_34 = rowTieuDe2.CreateCell(34);
        //                 ICell cellTieuDe_2_35 = rowTieuDe2.CreateCell(35);
        //                 ICell cellTieuDe_2_36 = rowTieuDe2.CreateCell(36);
        //                 ICell cellTieuDe_2_37 = rowTieuDe2.CreateCell(37);
        //                 ICell cellTieuDe_2_38 = rowTieuDe2.CreateCell(38);
        //                 ICell cellTieuDe_2_39 = rowTieuDe2.CreateCell(39);
        //                 ICell cellTieuDe_2_40 = rowTieuDe2.CreateCell(40);
        //                 ICell cellTieuDe_2_41 = rowTieuDe2.CreateCell(41);
        //                 ICell cellTieuDe_2_42 = rowTieuDe2.CreateCell(42);
        //                 ICell cellTieuDe_2_43 = rowTieuDe2.CreateCell(43);
        //                 ICell cellTieuDe_2_44 = rowTieuDe2.CreateCell(44);
        //                 rowIndex++;
        //
        //                 cellTieuDe_2_0.SetCellValue("STT");
        //                 cellTieuDe_2_1.SetCellValue("Tên dự án");
        //                 cellTieuDe_2_2.SetCellValue("Tổng cộng");
        //                 cellTieuDe_2_3.SetCellValue("Dư tháng 1");
        //                 cellTieuDe_2_4.SetCellValue("Dư tháng 2");
        //                 cellTieuDe_2_5.SetCellValue("Dư tháng 3");
        //                 cellTieuDe_2_6.SetCellValue("Dư tháng 4");
        //                 cellTieuDe_2_7.SetCellValue("Dư tháng 5");
        //                 cellTieuDe_2_8.SetCellValue("Dư tháng 6");
        //                 cellTieuDe_2_9.SetCellValue("Dư tháng 7");
        //                 cellTieuDe_2_10.SetCellValue("Dư tháng 8");
        //                 cellTieuDe_2_11.SetCellValue("Dư tháng 9");
        //                 cellTieuDe_2_12.SetCellValue("Dư tháng 10");
        //                 cellTieuDe_2_13.SetCellValue("Dư tháng 11");
        //                 cellTieuDe_2_14.SetCellValue("Dư tháng 12");
        //                 cellTieuDe_2_15.SetCellValue("Tổng cộng");
        //                 cellTieuDe_2_16.SetCellValue("Kế hoạch T.1");
        //                 cellTieuDe_2_17.SetCellValue("Kế hoạch T.2");
        //                 cellTieuDe_2_18.SetCellValue("Kế hoạch T.3");
        //                 cellTieuDe_2_19.SetCellValue("Kế hoạch T.4");
        //                 cellTieuDe_2_20.SetCellValue("Kế hoạch T.5");
        //                 cellTieuDe_2_21.SetCellValue("Kế hoạch T.6");
        //                 cellTieuDe_2_22.SetCellValue("Kế hoạch T.7");
        //                 cellTieuDe_2_23.SetCellValue("Kế hoạch T.8");
        //                 cellTieuDe_2_24.SetCellValue("Kế hoạch T.9");
        //                 cellTieuDe_2_25.SetCellValue("Kế hoạch T.10");
        //                 cellTieuDe_2_26.SetCellValue("Kế hoạch T.11");
        //                 cellTieuDe_2_27.SetCellValue("Kế hoạch T.12");
        //                 cellTieuDe_2_28.SetCellValue("Tổng cộng");
        //                 cellTieuDe_2_29.SetCellValue("Thực thu T.1");
        //                 cellTieuDe_2_30.SetCellValue("Thực thu T.2");
        //                 cellTieuDe_2_31.SetCellValue("Thực thu T.3");
        //                 cellTieuDe_2_32.SetCellValue("Thực thu T.4");
        //                 cellTieuDe_2_33.SetCellValue("Thực thu T.5");
        //                 cellTieuDe_2_34.SetCellValue("Thực thu T.6");
        //                 cellTieuDe_2_35.SetCellValue("Thực thu T.7");
        //                 cellTieuDe_2_36.SetCellValue("Thực thu T.8");
        //                 cellTieuDe_2_37.SetCellValue("Thực thu T.9");
        //                 cellTieuDe_2_38.SetCellValue("Thực thu T.10");
        //                 cellTieuDe_2_39.SetCellValue("Thực thu T.11");
        //                 cellTieuDe_2_40.SetCellValue("Thực thu T.12");
        //                 cellTieuDe_2_41.SetCellValue("Kinh phí tạm ứng chuyển năm 2025");
        //                 cellTieuDe_2_42.SetCellValue("Thời hạn bảo lãnh thực hiện HĐ");
        //                 cellTieuDe_2_43.SetCellValue("Thời hạn bảo lãnh tạm ứng");
        //                 cellTieuDe_2_44.SetCellValue("Ghi chú");
        //
        //                 cellTieuDe_2_0.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_1.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_2.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_3.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_4.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_5.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_6.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_7.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_8.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_9.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_10.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_11.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_12.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_13.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_14.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_15.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_16.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_17.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_18.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_19.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_20.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_21.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_22.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_23.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_24.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_25.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_26.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_27.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_28.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_29.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_30.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_31.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_32.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_33.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_34.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_35.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_36.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_37.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_38.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_39.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_40.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_41.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_42.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_43.CellStyle = styleTieuDe;
        //                 cellTieuDe_2_44.CellStyle = styleTieuDe;
        //                 
        //                 var stt = 1;
        //                 
        //                 sheet.CreateRow(rowIndex);
        //                 IRow rowDuAn = sheet.GetRow(rowIndex);
        //                 ICell cellDuAn_0 = rowDuAn.CreateCell(0);
        //                 ICell cellDuAn_1 = rowDuAn.CreateCell(1);
        //                 ICell cellDuAn_2 = rowDuAn.CreateCell(2);
        //                 ICell cellDuAn_3 = rowDuAn.CreateCell(3);
        //                 ICell cellDuAn_4 = rowDuAn.CreateCell(4);
        //                 ICell cellDuAn_5 = rowDuAn.CreateCell(5);
        //                 ICell cellDuAn_6 = rowDuAn.CreateCell(6);
        //                 ICell cellDuAn_7 = rowDuAn.CreateCell(7);
        //                 ICell cellDuAn_8 = rowDuAn.CreateCell(8);
        //                 ICell cellDuAn_9 = rowDuAn.CreateCell(9); 
        //                 ICell cellDuAn_10 = rowDuAn.CreateCell(10);
        //                 ICell cellDuAn_11 = rowDuAn.CreateCell(11);
        //                 ICell cellDuAn_12 = rowDuAn.CreateCell(12);
        //                 ICell cellDuAn_13 = rowDuAn.CreateCell(13);
        //                 ICell cellDuAn_14 = rowDuAn.CreateCell(14);
        //                 ICell cellDuAn_15 = rowDuAn.CreateCell(15);
        //                 ICell cellDuAn_16 = rowDuAn.CreateCell(16);
        //                 ICell cellDuAn_17 = rowDuAn.CreateCell(17);
        //                 ICell cellDuAn_18 = rowDuAn.CreateCell(18);
        //                 ICell cellDuAn_19 = rowDuAn.CreateCell(19);
        //                 ICell cellDuAn_20 = rowDuAn.CreateCell(20);
        //                 ICell cellDuAn_21 = rowDuAn.CreateCell(21);
        //                 ICell cellDuAn_22 = rowDuAn.CreateCell(22);
        //                 ICell cellDuAn_23 = rowDuAn.CreateCell(23);
        //                 ICell cellDuAn_24 = rowDuAn.CreateCell(24);
        //                 ICell cellDuAn_25 = rowDuAn.CreateCell(25);
        //                 ICell cellDuAn_26 = rowDuAn.CreateCell(26);
        //                 ICell cellDuAn_27 = rowDuAn.CreateCell(27);
        //                 ICell cellDuAn_28 = rowDuAn.CreateCell(28);
        //                 ICell cellDuAn_29 = rowDuAn.CreateCell(29);
        //                 ICell cellDuAn_30 = rowDuAn.CreateCell(30);
        //                 ICell cellDuAn_31 = rowDuAn.CreateCell(31);
        //                 ICell cellDuAn_32 = rowDuAn.CreateCell(32);
        //                 ICell cellDuAn_33 = rowDuAn.CreateCell(33);
        //                 ICell cellDuAn_34 = rowDuAn.CreateCell(34);
        //                 ICell cellDuAn_35 = rowDuAn.CreateCell(35);
        //                 ICell cellDuAn_36 = rowDuAn.CreateCell(36);
        //                 ICell cellDuAn_37 = rowDuAn.CreateCell(37);
        //                 ICell cellDuAn_38 = rowDuAn.CreateCell(38);
        //                 ICell cellDuAn_39 = rowDuAn.CreateCell(39);
        //                 ICell cellDuAn_40 = rowDuAn.CreateCell(40);
        //                 ICell cellDuAn_41 = rowDuAn.CreateCell(41);
        //                 ICell cellDuAn_42 = rowDuAn.CreateCell(42);
        //                 ICell cellDuAn_43 = rowDuAn.CreateCell(43);
        //                 ICell cellDuAn_44 = rowDuAn.CreateCell(44);
        //
        //                 cellDuAn_0.SetCellValue(stt);
        //                 cellDuAn_1.SetCellValue(baoCao.DuAnThuHoi.Ten);
        //
        //                 cellDuAn_0.CellStyle = styleDuAn;
        //                 cellDuAn_1.CellStyle = styleDuAn;
        //                 cellDuAn_2.CellStyle = styleDuAn;
        //                 cellDuAn_3.CellStyle = styleDuAn;
        //                 cellDuAn_4.CellStyle = styleDuAn;
        //                 cellDuAn_5.CellStyle = styleDuAn;
        //                 cellDuAn_6.CellStyle = styleDuAn;
        //                 cellDuAn_7.CellStyle = styleDuAn;
        //                 cellDuAn_8.CellStyle = styleDuAn;
        //                 cellDuAn_9.CellStyle = styleDuAn;
        //                 cellDuAn_10.CellStyle = styleDuAn;
        //                 cellDuAn_11.CellStyle = styleDuAn;
        //                 cellDuAn_12.CellStyle = styleDuAn;
        //                 cellDuAn_13.CellStyle = styleDuAn;
        //                 cellDuAn_14.CellStyle = styleDuAn;
        //                 cellDuAn_15.CellStyle = styleDuAn;
        //                 cellDuAn_16.CellStyle = styleDuAn;
        //                 cellDuAn_17.CellStyle = styleDuAn;
        //                 cellDuAn_18.CellStyle = styleDuAn;
        //                 cellDuAn_19.CellStyle = styleDuAn;
        //                 cellDuAn_20.CellStyle = styleDuAn;
        //                 cellDuAn_21.CellStyle = styleDuAn;
        //                 cellDuAn_22.CellStyle = styleDuAn;
        //                 cellDuAn_23.CellStyle = styleDuAn;
        //                 cellDuAn_24.CellStyle = styleDuAn;
        //                 cellDuAn_25.CellStyle = styleDuAn;
        //                 cellDuAn_26.CellStyle = styleDuAn;
        //                 cellDuAn_27.CellStyle = styleDuAn;
        //                 cellDuAn_28.CellStyle = styleDuAn;
        //                 cellDuAn_29.CellStyle = styleDuAn;
        //                 cellDuAn_30.CellStyle = styleDuAn;
        //                 cellDuAn_31.CellStyle = styleDuAn;
        //                 cellDuAn_32.CellStyle = styleDuAn;
        //                 cellDuAn_33.CellStyle = styleDuAn;
        //                 cellDuAn_34.CellStyle = styleDuAn;
        //                 cellDuAn_35.CellStyle = styleDuAn;
        //                 cellDuAn_36.CellStyle = styleDuAn;
        //                 cellDuAn_37.CellStyle = styleDuAn;
        //                 cellDuAn_38.CellStyle = styleDuAn;
        //                 cellDuAn_39.CellStyle = styleDuAn;
        //                 cellDuAn_40.CellStyle = styleDuAn;
        //                 cellDuAn_41.CellStyle = styleDuAn;
        //                 cellDuAn_42.CellStyle = styleDuAn;
        //                 cellDuAn_43.CellStyle = styleDuAn;
        //                 cellDuAn_44.CellStyle = styleDuAn;
        //                
        //                 
        //                 rowIndex++;
        //                 var tongDu_DuAn = 0;
        //                 var tongThu_DuAn = 0;
        //                 var tongThucTe_DuAn = 0;
        //                 foreach (var data in baoCao.ListData)
        //                 {
        //                     sheet.CreateRow(rowIndex);
        //                     IRow rowDanhMuc = sheet.GetRow(rowIndex);
        //                     ICell cellDanhMuc_0 = rowDanhMuc.CreateCell(0);
        //                     ICell cellDanhMuc_1 = rowDanhMuc.CreateCell(1);
        //                     ICell cellDanhMuc_2 = rowDanhMuc.CreateCell(2);
        //                     ICell cellDanhMuc_3 = rowDanhMuc.CreateCell(3);
        //                     ICell cellDanhMuc_4 = rowDanhMuc.CreateCell(4);
        //                     ICell cellDanhMuc_5 = rowDanhMuc.CreateCell(5);
        //                     ICell cellDanhMuc_6 = rowDanhMuc.CreateCell(6);
        //                     ICell cellDanhMuc_7 = rowDanhMuc.CreateCell(7);
        //                     ICell cellDanhMuc_8 = rowDanhMuc.CreateCell(8);
        //                     ICell cellDanhMuc_9 = rowDanhMuc.CreateCell(9);
        //                     ICell cellDanhMuc_10 = rowDanhMuc.CreateCell(10);
        //                     ICell cellDanhMuc_11 = rowDanhMuc.CreateCell(11);
        //                     ICell cellDanhMuc_12 = rowDanhMuc.CreateCell(12);
        //                     ICell cellDanhMuc_13 = rowDanhMuc.CreateCell(13);
        //                     ICell cellDanhMuc_14 = rowDanhMuc.CreateCell(14);
        //                     ICell cellDanhMuc_15 = rowDanhMuc.CreateCell(15);
        //                     ICell cellDanhMuc_16 = rowDanhMuc.CreateCell(16);
        //                     ICell cellDanhMuc_17 = rowDanhMuc.CreateCell(17);
        //                     ICell cellDanhMuc_18 = rowDanhMuc.CreateCell(18);
        //                     ICell cellDanhMuc_19 = rowDanhMuc.CreateCell(19);
        //                     ICell cellDanhMuc_20 = rowDanhMuc.CreateCell(20);
        //                     ICell cellDanhMuc_21 = rowDanhMuc.CreateCell(21);
        //                     ICell cellDanhMuc_22 = rowDanhMuc.CreateCell(22);
        //                     ICell cellDanhMuc_23 = rowDanhMuc.CreateCell(23);
        //                     ICell cellDanhMuc_24 = rowDanhMuc.CreateCell(24);
        //                     ICell cellDanhMuc_25 = rowDanhMuc.CreateCell(25);
        //                     ICell cellDanhMuc_26 = rowDanhMuc.CreateCell(26);
        //                     ICell cellDanhMuc_27 = rowDanhMuc.CreateCell(27);
        //                     ICell cellDanhMuc_28 = rowDanhMuc.CreateCell(28);
        //                     ICell cellDanhMuc_29 = rowDanhMuc.CreateCell(29);
        //                     ICell cellDanhMuc_30 = rowDanhMuc.CreateCell(30);
        //                     ICell cellDanhMuc_31 = rowDanhMuc.CreateCell(31);
        //                     ICell cellDanhMuc_32 = rowDanhMuc.CreateCell(32);
        //                     ICell cellDanhMuc_33 = rowDanhMuc.CreateCell(33);
        //                     ICell cellDanhMuc_34 = rowDanhMuc.CreateCell(34);
        //                     ICell cellDanhMuc_35 = rowDanhMuc.CreateCell(35);
        //                     ICell cellDanhMuc_36 = rowDanhMuc.CreateCell(36);
        //                     ICell cellDanhMuc_37 = rowDanhMuc.CreateCell(37);
        //                     ICell cellDanhMuc_38 = rowDanhMuc.CreateCell(38);
        //                     ICell cellDanhMuc_39 = rowDanhMuc.CreateCell(39);
        //                     ICell cellDanhMuc_40 = rowDanhMuc.CreateCell(40);
        //                     ICell cellDanhMuc_41 = rowDanhMuc.CreateCell(41);
        //                     ICell cellDanhMuc_42 = rowDanhMuc.CreateCell(42);
        //                     ICell cellDanhMuc_43 = rowDanhMuc.CreateCell(43);
        //                     ICell cellDanhMuc_44 = rowDanhMuc.CreateCell(44);
        //                     
        //                     cellDanhMuc_0.SetCellValue(stt+"."+data.DanhMucThuHoi.Stt);
        //                     cellDanhMuc_1.SetCellValue(data.DanhMucThuHoi.Ten);
        //                     
        //                     cellDanhMuc_0.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_1.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_2.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_3.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_4.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_5.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_6.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_7.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_8.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_9.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_10.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_11.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_12.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_13.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_14.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_15.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_16.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_17.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_18.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_19.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_20.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_21.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_22.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_23.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_24.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_25.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_26.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_27.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_28.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_29.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_30.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_31.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_32.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_33.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_34.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_35.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_36.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_37.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_38.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_39.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_40.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_41.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_42.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_43.CellStyle = styleDanhMuc;
        //                     cellDanhMuc_44.CellStyle = styleDanhMuc;
        //                     
        //                     rowIndex++;
        //                     
        //                     foreach (var dtr in data.ListChiTietThuHoi)
        //                     {
        //                         sheet.CreateRow(rowIndex);
        //                         IRow rowData = sheet.GetRow(rowIndex);
        //                         ICell cellData_0 = rowData.CreateCell(0);
        //                         ICell cellData_1 = rowData.CreateCell(1);
        //                         ICell cellData_2 = rowData.CreateCell(2);
        //                         ICell cellData_3 = rowData.CreateCell(3);
        //                         ICell cellData_4 = rowData.CreateCell(4);
        //                         ICell cellData_5 = rowData.CreateCell(5);
        //                         ICell cellData_6 = rowData.CreateCell(6);
        //                         ICell cellData_7 = rowData.CreateCell(7);
        //                         ICell cellData_8 = rowData.CreateCell(8);
        //                         ICell cellData_9 = rowData.CreateCell(9);
        //                         ICell cellData_10 = rowData.CreateCell(10);
        //                         ICell cellData_11 = rowData.CreateCell(11);
        //                         ICell cellData_12 = rowData.CreateCell(12);
        //                         ICell cellData_13 = rowData.CreateCell(13);
        //                         ICell cellData_14 = rowData.CreateCell(14);
        //                         ICell cellData_15 = rowData.CreateCell(15);
        //                         ICell cellData_16 = rowData.CreateCell(16);
        //                         ICell cellData_17 = rowData.CreateCell(17);
        //                         ICell cellData_18 = rowData.CreateCell(18);
        //                         ICell cellData_19 = rowData.CreateCell(19);
        //                         ICell cellData_20 = rowData.CreateCell(20);
        //                         ICell cellData_21 = rowData.CreateCell(21);
        //                         ICell cellData_22 = rowData.CreateCell(22);
        //                         ICell cellData_23 = rowData.CreateCell(23);
        //                         ICell cellData_24 = rowData.CreateCell(24);
        //                         ICell cellData_25 = rowData.CreateCell(25);
        //                         ICell cellData_26 = rowData.CreateCell(26);
        //                         ICell cellData_27 = rowData.CreateCell(27);
        //                         ICell cellData_28 = rowData.CreateCell(28);
        //                         ICell cellData_29 = rowData.CreateCell(29);
        //                         ICell cellData_30 = rowData.CreateCell(30);
        //                         ICell cellData_31 = rowData.CreateCell(31);
        //                         ICell cellData_32 = rowData.CreateCell(32);
        //                         ICell cellData_33 = rowData.CreateCell(33);
        //                         ICell cellData_34 = rowData.CreateCell(34);
        //                         ICell cellData_35 = rowData.CreateCell(35);
        //                         ICell cellData_36 = rowData.CreateCell(36);
        //                         ICell cellData_37 = rowData.CreateCell(37);
        //                         ICell cellData_38 = rowData.CreateCell(38);
        //                         ICell cellData_39 = rowData.CreateCell(39);
        //                         ICell cellData_40 = rowData.CreateCell(40);
        //                         ICell cellData_41 = rowData.CreateCell(41);
        //                         ICell cellData_42 = rowData.CreateCell(42);
        //                         ICell cellData_43 = rowData.CreateCell(43);
        //                         ICell cellData_44 = rowData.CreateCell(44);
        //                         
        //                         cellData_0.SetCellValue("");
        //                         cellData_1.SetCellValue(dtr.Ten);
        //                         cellData_2.SetCellValue(dtr.TongDu.toCurrency().Replace(".",","));
        //                         cellData_3.SetCellValue(dtr.Du1.toCurrency().Replace(".",","));
        //                         cellData_4.SetCellValue(dtr.Du2.toCurrency().Replace(".",","));
        //                         cellData_5.SetCellValue(dtr.Du3.toCurrency().Replace(".",","));
        //                         cellData_6.SetCellValue(dtr.Du4.toCurrency().Replace(".",","));
        //                         cellData_7.SetCellValue(dtr.Du5.toCurrency().Replace(".",","));
        //                         cellData_8.SetCellValue(dtr.Du6.toCurrency().Replace(".",","));
        //                         cellData_9.SetCellValue(dtr.Du7.toCurrency().Replace(".",","));
        //                         cellData_10.SetCellValue(dtr.Du8.toCurrency().Replace(".",","));
        //                         cellData_11.SetCellValue(dtr.Du9.toCurrency().Replace(".",","));
        //                         cellData_12.SetCellValue(dtr.Du10.toCurrency().Replace(".",","));
        //                         cellData_13.SetCellValue(dtr.Du11.toCurrency().Replace(".",","));
        //                         cellData_14.SetCellValue(dtr.Du12.toCurrency().Replace(".",","));
        //                         cellData_15.SetCellValue(dtr.TongThu.toCurrency().Replace(".",","));
        //                         cellData_16.SetCellValue(dtr.Thu1.toCurrency().Replace(".",","));
        //                         cellData_17.SetCellValue(dtr.Thu2.toCurrency().Replace(".",","));
        //                         cellData_18.SetCellValue(dtr.Thu3.toCurrency().Replace(".",","));
        //                         cellData_19.SetCellValue(dtr.Thu4.toCurrency().Replace(".",","));
        //                         cellData_20.SetCellValue(dtr.Thu5.toCurrency().Replace(".",","));
        //                         cellData_21.SetCellValue(dtr.Thu6.toCurrency().Replace(".",","));
        //                         cellData_22.SetCellValue(dtr.Thu7.toCurrency().Replace(".",","));
        //                         cellData_23.SetCellValue(dtr.Thu8.toCurrency().Replace(".",","));
        //                         cellData_24.SetCellValue(dtr.Thu9.toCurrency().Replace(".",","));
        //                         cellData_25.SetCellValue(dtr.Thu10.toCurrency().Replace(".",","));
        //                         cellData_26.SetCellValue(dtr.Thu11.toCurrency().Replace(".",","));
        //                         cellData_27.SetCellValue(dtr.Thu12.toCurrency().Replace(".",","));
        //                         cellData_28.SetCellValue(dtr.TongThucTe.toCurrency().Replace(".",","));
        //                         cellData_29.SetCellValue(dtr.ThucTe1.toCurrency().Replace(".",","));
        //                         cellData_30.SetCellValue(dtr.ThucTe2.toCurrency().Replace(".",","));
        //                         cellData_31.SetCellValue(dtr.ThucTe3.toCurrency().Replace(".",","));
        //                         cellData_32.SetCellValue(dtr.ThucTe4.toCurrency().Replace(".",","));
        //                         cellData_33.SetCellValue(dtr.ThucTe5.toCurrency().Replace(".",","));
        //                         cellData_34.SetCellValue(dtr.ThucTe6.toCurrency().Replace(".",","));
        //                         cellData_35.SetCellValue(dtr.ThucTe7.toCurrency().Replace(".",","));
        //                         cellData_36.SetCellValue(dtr.ThucTe8.toCurrency().Replace(".",","));
        //                         cellData_37.SetCellValue(dtr.ThucTe9.toCurrency().Replace(".",","));
        //                         cellData_38.SetCellValue(dtr.ThucTe10.toCurrency().Replace(".",","));
        //                         cellData_39.SetCellValue(dtr.ThucTe11.toCurrency().Replace(".",","));
        //                         cellData_40.SetCellValue(dtr.ThucTe12.toCurrency().Replace(".",","));
        //                         cellData_41.SetCellValue("");
        //                         cellData_42.SetCellValue("");
        //                         cellData_43.SetCellValue("");
        //                         cellData_44.SetCellValue("");
        //
        //                         cellData_0.CellStyle = styleData;
        //                         cellData_1.CellStyle = styleData;
        //                         cellData_2.CellStyle = styleData;
        //                         cellData_3.CellStyle = styleData;
        //                         cellData_4.CellStyle = styleData;
        //                         cellData_5.CellStyle = styleData;
        //                         cellData_6.CellStyle = styleData;
        //                         cellData_7.CellStyle = styleData;
        //                         cellData_8.CellStyle = styleData;
        //                         cellData_9.CellStyle = styleData;
        //                         cellData_10.CellStyle = styleData;
        //                         cellData_11.CellStyle = styleData;
        //                         cellData_12.CellStyle = styleData;
        //                         cellData_13.CellStyle = styleData;
        //                         cellData_14.CellStyle = styleData;
        //                         cellData_15.CellStyle = styleData;
        //                         cellData_16.CellStyle = styleData;
        //                         cellData_17.CellStyle = styleData;
        //                         cellData_18.CellStyle = styleData;
        //                         cellData_19.CellStyle = styleData;
        //                         cellData_20.CellStyle = styleData;
        //                         cellData_21.CellStyle = styleData;
        //                         cellData_22.CellStyle = styleData;
        //                         cellData_23.CellStyle = styleData;
        //                         cellData_24.CellStyle = styleData;
        //                         cellData_25.CellStyle = styleData;
        //                         cellData_26.CellStyle = styleData;
        //                         cellData_27.CellStyle = styleData;
        //                         cellData_28.CellStyle = styleData;
        //                         cellData_29.CellStyle = styleData;
        //                         cellData_30.CellStyle = styleData;
        //                         cellData_31.CellStyle = styleData;
        //                         cellData_32.CellStyle = styleData;
        //                         cellData_33.CellStyle = styleData;
        //                         cellData_34.CellStyle = styleData;
        //                         cellData_35.CellStyle = styleData;
        //                         cellData_36.CellStyle = styleData;
        //                         cellData_37.CellStyle = styleData;
        //                         cellData_38.CellStyle = styleData;
        //                         cellData_39.CellStyle = styleData;
        //                         cellData_40.CellStyle = styleData;
        //                         cellData_41.CellStyle = styleData;
        //                         cellData_42.CellStyle = styleData;
        //                         cellData_43.CellStyle = styleData;
        //                         cellData_44.CellStyle = styleData;
        //                         
        //                         rowIndex++;
        //                     }
        //                 }
        //                 sheet.AutoSizeColumn(0);
        //                 sheet.AutoSizeColumn(1);
        //                 sheet.AutoSizeColumn(2);
        //                 sheet.AutoSizeColumn(3);
        //                 sheet.AutoSizeColumn(4);
        //                 sheet.AutoSizeColumn(5);
        //                 sheet.AutoSizeColumn(6);
        //                 sheet.AutoSizeColumn(7);
        //                 sheet.AutoSizeColumn(8);
        //                 sheet.AutoSizeColumn(9);
        //                 sheet.AutoSizeColumn(10);
        //                 sheet.AutoSizeColumn(11);
        //                 sheet.AutoSizeColumn(12);
        //                 sheet.AutoSizeColumn(13);
        //                 sheet.AutoSizeColumn(14);
        //                 sheet.AutoSizeColumn(15);
        //                 sheet.AutoSizeColumn(16);
        //                 sheet.AutoSizeColumn(17);
        //                 sheet.AutoSizeColumn(18);
        //                 sheet.AutoSizeColumn(19);
        //                 sheet.AutoSizeColumn(20);
        //                 sheet.AutoSizeColumn(21);
        //                 sheet.AutoSizeColumn(22);
        //                 sheet.AutoSizeColumn(23);
        //                 sheet.AutoSizeColumn(24);
        //                 sheet.AutoSizeColumn(25);
        //                 sheet.AutoSizeColumn(26);
        //                 sheet.AutoSizeColumn(27);
        //                 sheet.AutoSizeColumn(28);
        //                 sheet.AutoSizeColumn(29);
        //                 sheet.AutoSizeColumn(30);
        //                 sheet.AutoSizeColumn(31);
        //                 sheet.AutoSizeColumn(32);
        //                 sheet.AutoSizeColumn(33);
        //                 sheet.AutoSizeColumn(34);
        //                 sheet.AutoSizeColumn(35);
        //                 sheet.AutoSizeColumn(36);
        //                 sheet.AutoSizeColumn(37);
        //                 sheet.AutoSizeColumn(38);
        //                 sheet.AutoSizeColumn(39);
        //                 sheet.AutoSizeColumn(40);
        //                 sheet.AutoSizeColumn(41);
        //                 sheet.AutoSizeColumn(42);
        //                 sheet.AutoSizeColumn(43);
        //                 sheet.AutoSizeColumn(44);
        //             });
        //     }
        //     catch (Exception e)
        //     {
        //         Console.WriteLine(e);
        //         throw;
        //     }
        //     
        // }

        public FileDto BaoCaoDuAnThuHoi_ExportToFile(BaoCaoDuAnThuHoi_ExportToFileDto baoCao)
        {
            try
            {
                return _exporter.CreateExcelPackage(
                    "BaoCaoThuHoi_" + baoCao.DuAnThuHoi.MaDATH + "_" +
                    (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds + ".xlsx",
                    excelPackage =>
                    {
                        using (FileStream stream =
                            new FileStream("wwwroot/TemplateFile/TempBaoCaoKeHoachThanhToan.xlsx", FileMode.Open))
                        {
                            using (ExcelPackage p = new ExcelPackage(stream))
                            {
                                foreach (ExcelWorksheet ws in p.Workbook.Worksheets)
                                {
                                    excelPackage.Workbook.Worksheets.Add(ws.Name, ws);
                                }
                            }
                        }

                        decimal TongDuDA = 0;
                        decimal TongDuDA_T1 = 0;
                        decimal TongDuDA_T2 = 0;
                        decimal TongDuDA_T3 = 0;
                        decimal TongDuDA_T4 = 0;
                        decimal TongDuDA_T5 = 0;
                        decimal TongDuDA_T6 = 0;
                        decimal TongDuDA_T7 = 0;
                        decimal TongDuDA_T8 = 0;
                        decimal TongDuDA_T9 = 0;
                        decimal TongDuDA_T10 = 0;
                        decimal TongDuDA_T11 = 0;
                        decimal TongDuDA_T12 = 0;

                        decimal TongKeHoachDA = 0;
                        decimal TongKeHoachDA_T1 = 0;
                        decimal TongKeHoachDA_T2 = 0;
                        decimal TongKeHoachDA_T3 = 0;
                        decimal TongKeHoachDA_T4 = 0;
                        decimal TongKeHoachDA_T5 = 0;
                        decimal TongKeHoachDA_T6 = 0;
                        decimal TongKeHoachDA_T7 = 0;
                        decimal TongKeHoachDA_T8 = 0;
                        decimal TongKeHoachDA_T9 = 0;
                        decimal TongKeHoachDA_T10 = 0;
                        decimal TongKeHoachDA_T11 = 0;
                        decimal TongKeHoachDA_T12 = 0;

                        decimal TongThucTeDA = 0;
                        decimal TongThucTeDA_T1 = 0;
                        decimal TongThucTeDA_T2 = 0;
                        decimal TongThucTeDA_T3 = 0;
                        decimal TongThucTeDA_T4 = 0;
                        decimal TongThucTeDA_T5 = 0;
                        decimal TongThucTeDA_T6 = 0;
                        decimal TongThucTeDA_T7 = 0;
                        decimal TongThucTeDA_T8 = 0;
                        decimal TongThucTeDA_T9 = 0;
                        decimal TongThucTeDA_T10 = 0;
                        decimal TongThucTeDA_T11 = 0;
                        decimal TongThucTeDA_T12 = 0;


                        //sheet1
                        var sheet = excelPackage.Workbook.Worksheets[0];
                        sheet.OutLineApplyStyle = true;
                        sheet.Cells["A" + 1].Value = "KẾ HOẠCH THU HỒI TẠM ỨNG";
                        int row = 5;
                        var stt = 1;

                        var rowOld_DA = row;

                        sheet.Cells["A" + row + ":AS" + row].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        sheet.Cells["A" + row + ":AS" + row].Style.Fill.BackgroundColor
                            .SetColor(Color.FromArgb(211, 211, 211));
                        sheet.Cells["A" + row + ":AS" + row].Style.Font.Bold = true;
                        sheet.Cells["A" + row].Style.HorizontalAlignment =
                            OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells["C" + row + ":AS" + row].Style.HorizontalAlignment =
                            OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                        sheet.Cells["A" + row].Value = stt;
                        sheet.Cells["B" + row].Value = baoCao.DuAnThuHoi.Ten;
                        // sheet.Cells["AP" + row].Value = baoCao.DuAnThuHoi.Ten;
                        sheet.Cells["AQ" + row].Value = baoCao.DuAnThuHoi.ThoiHanBaoLanhHopDong?.ToString("dd/MM/yyyy");
                        sheet.Cells["AR" + row].Value = baoCao.DuAnThuHoi.ThoiHanBaoLanhTamUng?.ToString("dd/MM/yyyy");

                        row++;

                        foreach (var data in baoCao.ListData)
                        {
                            decimal TongDuDM = 0;
                            decimal TongDuDM_T1 = 0;
                            decimal TongDuDM_T2 = 0;
                            decimal TongDuDM_T3 = 0;
                            decimal TongDuDM_T4 = 0;
                            decimal TongDuDM_T5 = 0;
                            decimal TongDuDM_T6 = 0;
                            decimal TongDuDM_T7 = 0;
                            decimal TongDuDM_T8 = 0;
                            decimal TongDuDM_T9 = 0;
                            decimal TongDuDM_T10 = 0;
                            decimal TongDuDM_T11 = 0;
                            decimal TongDuDM_T12 = 0;

                            decimal TongKeHoachDM = 0;
                            decimal TongKeHoachDM_T1 = 0;
                            decimal TongKeHoachDM_T2 = 0;
                            decimal TongKeHoachDM_T3 = 0;
                            decimal TongKeHoachDM_T4 = 0;
                            decimal TongKeHoachDM_T5 = 0;
                            decimal TongKeHoachDM_T6 = 0;
                            decimal TongKeHoachDM_T7 = 0;
                            decimal TongKeHoachDM_T8 = 0;
                            decimal TongKeHoachDM_T9 = 0;
                            decimal TongKeHoachDM_T10 = 0;
                            decimal TongKeHoachDM_T11 = 0;
                            decimal TongKeHoachDM_T12 = 0;

                            decimal TongThucTeDM = 0;
                            decimal TongThucTeDM_T1 = 0;
                            decimal TongThucTeDM_T2 = 0;
                            decimal TongThucTeDM_T3 = 0;
                            decimal TongThucTeDM_T4 = 0;
                            decimal TongThucTeDM_T5 = 0;
                            decimal TongThucTeDM_T6 = 0;
                            decimal TongThucTeDM_T7 = 0;
                            decimal TongThucTeDM_T8 = 0;
                            decimal TongThucTeDM_T9 = 0;
                            decimal TongThucTeDM_T10 = 0;
                            decimal TongThucTeDM_T11 = 0;
                            decimal TongThucTeDM_T12 = 0;

                            var rowOld_DM = row;

                            sheet.Cells["A" + row + ":AS" + row].Style.Font.Bold = true;
                            sheet.Cells["A" + row].Style.HorizontalAlignment =
                                OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheet.Cells["C" + row + ":AS" + row].Style.HorizontalAlignment =
                                OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                            sheet.Cells["A" + row].Value = stt + "." + data.DanhMucThuHoi.Stt;
                            sheet.Cells["B" + row].Value = data.DanhMucThuHoi.Ten;

                            row++;
                            foreach (var dtr in data.ListChiTietThuHoi)
                            {
                                sheet.Cells["A" + row + ":AS" + row].Style.Font.Italic = true;
                                sheet.Cells["A" + row].Style.HorizontalAlignment =
                                    OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                sheet.Cells["C" + row + ":AS" + row].Style.HorizontalAlignment =
                                    OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                sheet.Cells["A" + row].Value = "";
                                sheet.Cells["B" + row].Value = dtr.Ten;
                                sheet.Cells["C" + row].Value = dtr.TongDu.toCurrency().Replace(".", ",");
                                sheet.Cells["D" + row].Value = dtr.Du1.toCurrency().Replace(".", ",");
                                sheet.Cells["E" + row].Value = dtr.Du2.toCurrency().Replace(".", ",");
                                sheet.Cells["F" + row].Value = dtr.Du3.toCurrency().Replace(".", ",");
                                sheet.Cells["G" + row].Value = dtr.Du4.toCurrency().Replace(".", ",");
                                sheet.Cells["H" + row].Value = dtr.Du5.toCurrency().Replace(".", ",");
                                sheet.Cells["I" + row].Value = dtr.Du6.toCurrency().Replace(".", ",");
                                sheet.Cells["J" + row].Value = dtr.Du7.toCurrency().Replace(".", ",");
                                sheet.Cells["K" + row].Value = dtr.Du8.toCurrency().Replace(".", ",");
                                sheet.Cells["L" + row].Value = dtr.Du9.toCurrency().Replace(".", ",");
                                sheet.Cells["M" + row].Value = dtr.Du10.toCurrency().Replace(".", ",");
                                sheet.Cells["N" + row].Value = dtr.Du11.toCurrency().Replace(".", ",");
                                sheet.Cells["O" + row].Value = dtr.Du12.toCurrency().Replace(".", ",");
                                sheet.Cells["P" + row].Value = dtr.TongThu.toCurrency().Replace(".", ",");
                                sheet.Cells["Q" + row].Value = dtr.Thu1.toCurrency().Replace(".", ",");
                                sheet.Cells["R" + row].Value = dtr.Thu2.toCurrency().Replace(".", ",");
                                sheet.Cells["S" + row].Value = dtr.Thu3.toCurrency().Replace(".", ",");
                                sheet.Cells["T" + row].Value = dtr.Thu4.toCurrency().Replace(".", ",");
                                sheet.Cells["U" + row].Value = dtr.Thu5.toCurrency().Replace(".", ",");
                                sheet.Cells["V" + row].Value = dtr.Thu6.toCurrency().Replace(".", ",");
                                sheet.Cells["W" + row].Value = dtr.Thu7.toCurrency().Replace(".", ",");
                                sheet.Cells["X" + row].Value = dtr.Thu8.toCurrency().Replace(".", ",");
                                sheet.Cells["Y" + row].Value = dtr.Thu9.toCurrency().Replace(".", ",");
                                sheet.Cells["Z" + row].Value = dtr.Thu10.toCurrency().Replace(".", ",");
                                sheet.Cells["AA" + row].Value = dtr.Thu11.toCurrency().Replace(".", ",");
                                sheet.Cells["AB" + row].Value = dtr.Thu12.toCurrency().Replace(".", ",");
                                sheet.Cells["AC" + row].Value = dtr.TongThucTe.toCurrency().Replace(".", ",");
                                sheet.Cells["AD" + row].Value = dtr.ThucTe1.toCurrency().Replace(".", ",");
                                sheet.Cells["AE" + row].Value = dtr.ThucTe2.toCurrency().Replace(".", ",");
                                sheet.Cells["AF" + row].Value = dtr.ThucTe3.toCurrency().Replace(".", ",");
                                sheet.Cells["AG" + row].Value = dtr.ThucTe4.toCurrency().Replace(".", ",");
                                sheet.Cells["AH" + row].Value = dtr.ThucTe5.toCurrency().Replace(".", ",");
                                sheet.Cells["AI" + row].Value = dtr.ThucTe6.toCurrency().Replace(".", ",");
                                sheet.Cells["AJ" + row].Value = dtr.ThucTe7.toCurrency().Replace(".", ",");
                                sheet.Cells["AK" + row].Value = dtr.ThucTe8.toCurrency().Replace(".", ",");
                                sheet.Cells["AL" + row].Value = dtr.ThucTe9.toCurrency().Replace(".", ",");
                                sheet.Cells["AM" + row].Value = dtr.ThucTe10.toCurrency().Replace(".", ",");
                                sheet.Cells["AN" + row].Value = dtr.ThucTe11.toCurrency().Replace(".", ",");
                                sheet.Cells["AO" + row].Value = dtr.ThucTe12.toCurrency().Replace(".", ",");
                                sheet.Cells["AP" + row].Value = "";
                                sheet.Cells["AQ" + row].Value = "";
                                sheet.Cells["AR" + row].Value = "";
                                sheet.Cells["AS" + row].Value = "";

                                TongDuDM += dtr.TongDu;
                                TongDuDM_T1 += dtr.Du1;
                                TongDuDM_T2 += dtr.Du2;
                                TongDuDM_T3 += dtr.Du3;
                                TongDuDM_T4 += dtr.Du4;
                                TongDuDM_T5 += dtr.Du5;
                                TongDuDM_T6 += dtr.Du6;
                                TongDuDM_T7 += dtr.Du7;
                                TongDuDM_T8 += dtr.Du8;
                                TongDuDM_T9 += dtr.Du9;
                                TongDuDM_T10 += dtr.Du10;
                                TongDuDM_T11 += dtr.Du11;
                                TongDuDM_T12 += dtr.Du12;
                                TongKeHoachDM += dtr.TongThu;
                                TongKeHoachDM_T1 += dtr.Thu1;
                                TongKeHoachDM_T2 += dtr.Thu2;
                                TongKeHoachDM_T3 += dtr.Thu3;
                                TongKeHoachDM_T4 += dtr.Thu4;
                                TongKeHoachDM_T5 += dtr.Thu5;
                                TongKeHoachDM_T6 += dtr.Thu6;
                                TongKeHoachDM_T7 += dtr.Thu7;
                                TongKeHoachDM_T8 += dtr.Thu8;
                                TongKeHoachDM_T9 += dtr.Thu9;
                                TongKeHoachDM_T10 += dtr.Thu10;
                                TongKeHoachDM_T11 += dtr.Thu11;
                                TongKeHoachDM_T12 += dtr.Thu12;
                                TongThucTeDM += dtr.TongThucTe;
                                TongThucTeDM_T1 += dtr.ThucTe1;
                                TongThucTeDM_T2 += dtr.ThucTe2;
                                TongThucTeDM_T3 += dtr.ThucTe3;
                                TongThucTeDM_T4 += dtr.ThucTe4;
                                TongThucTeDM_T5 += dtr.ThucTe5;
                                TongThucTeDM_T6 += dtr.ThucTe6;
                                TongThucTeDM_T7 += dtr.ThucTe7;
                                TongThucTeDM_T8 += dtr.ThucTe8;
                                TongThucTeDM_T9 += dtr.ThucTe9;
                                TongThucTeDM_T10 += dtr.ThucTe10;
                                TongThucTeDM_T11 += dtr.ThucTe11;
                                TongThucTeDM_T12 += dtr.ThucTe12;

                                row++;
                            }

                            sheet.Cells["C" + rowOld_DM].Value = TongDuDM.toCurrency().Replace(".", ",");
                            sheet.Cells["D" + rowOld_DM].Value = TongDuDM_T1.toCurrency().Replace(".", ",");
                            sheet.Cells["E" + rowOld_DM].Value = TongDuDM_T2.toCurrency().Replace(".", ",");
                            sheet.Cells["F" + rowOld_DM].Value = TongDuDM_T3.toCurrency().Replace(".", ",");
                            sheet.Cells["G" + rowOld_DM].Value = TongDuDM_T4.toCurrency().Replace(".", ",");
                            sheet.Cells["H" + rowOld_DM].Value = TongDuDM_T5.toCurrency().Replace(".", ",");
                            sheet.Cells["I" + rowOld_DM].Value = TongDuDM_T6.toCurrency().Replace(".", ",");
                            sheet.Cells["J" + rowOld_DM].Value = TongDuDM_T7.toCurrency().Replace(".", ",");
                            sheet.Cells["K" + rowOld_DM].Value = TongDuDM_T8.toCurrency().Replace(".", ",");
                            sheet.Cells["L" + rowOld_DM].Value = TongDuDM_T9.toCurrency().Replace(".", ",");
                            sheet.Cells["M" + rowOld_DM].Value = TongDuDM_T10.toCurrency().Replace(".", ",");
                            sheet.Cells["N" + rowOld_DM].Value = TongDuDM_T11.toCurrency().Replace(".", ",");
                            sheet.Cells["O" + rowOld_DM].Value = TongDuDM_T12.toCurrency().Replace(".", ",");
                            sheet.Cells["P" + rowOld_DM].Value = TongKeHoachDM.toCurrency().Replace(".", ",");
                            sheet.Cells["Q" + rowOld_DM].Value = TongKeHoachDM_T1.toCurrency().Replace(".", ",");
                            sheet.Cells["R" + rowOld_DM].Value = TongKeHoachDM_T2.toCurrency().Replace(".", ",");
                            sheet.Cells["S" + rowOld_DM].Value = TongKeHoachDM_T3.toCurrency().Replace(".", ",");
                            sheet.Cells["T" + rowOld_DM].Value = TongKeHoachDM_T4.toCurrency().Replace(".", ",");
                            sheet.Cells["U" + rowOld_DM].Value = TongKeHoachDM_T5.toCurrency().Replace(".", ",");
                            sheet.Cells["V" + rowOld_DM].Value = TongKeHoachDM_T6.toCurrency().Replace(".", ",");
                            sheet.Cells["W" + rowOld_DM].Value = TongKeHoachDM_T7.toCurrency().Replace(".", ",");
                            sheet.Cells["X" + rowOld_DM].Value = TongKeHoachDM_T8.toCurrency().Replace(".", ",");
                            sheet.Cells["Y" + rowOld_DM].Value = TongKeHoachDM_T9.toCurrency().Replace(".", ",");
                            sheet.Cells["Z" + rowOld_DM].Value = TongKeHoachDM_T10.toCurrency().Replace(".", ",");
                            sheet.Cells["AA" + rowOld_DM].Value = TongKeHoachDM_T11.toCurrency().Replace(".", ",");
                            sheet.Cells["AB" + rowOld_DM].Value = TongKeHoachDM_T12.toCurrency().Replace(".", ",");
                            sheet.Cells["AC" + rowOld_DM].Value = TongThucTeDM.toCurrency().Replace(".", ",");
                            sheet.Cells["AD" + rowOld_DM].Value = TongThucTeDM_T1.toCurrency().Replace(".", ",");
                            sheet.Cells["AE" + rowOld_DM].Value = TongThucTeDM_T2.toCurrency().Replace(".", ",");
                            sheet.Cells["AF" + rowOld_DM].Value = TongThucTeDM_T3.toCurrency().Replace(".", ",");
                            sheet.Cells["AG" + rowOld_DM].Value = TongThucTeDM_T4.toCurrency().Replace(".", ",");
                            sheet.Cells["AH" + rowOld_DM].Value = TongThucTeDM_T5.toCurrency().Replace(".", ",");
                            sheet.Cells["AI" + rowOld_DM].Value = TongThucTeDM_T6.toCurrency().Replace(".", ",");
                            sheet.Cells["AJ" + rowOld_DM].Value = TongThucTeDM_T7.toCurrency().Replace(".", ",");
                            sheet.Cells["AK" + rowOld_DM].Value = TongThucTeDM_T8.toCurrency().Replace(".", ",");
                            sheet.Cells["AL" + rowOld_DM].Value = TongThucTeDM_T9.toCurrency().Replace(".", ",");
                            sheet.Cells["AM" + rowOld_DM].Value = TongThucTeDM_T10.toCurrency().Replace(".", ",");
                            sheet.Cells["AN" + rowOld_DM].Value = TongThucTeDM_T11.toCurrency().Replace(".", ",");
                            sheet.Cells["AO" + rowOld_DM].Value = TongThucTeDM_T12.toCurrency().Replace(".", ",");
                            sheet.Cells["AP" + rowOld_DM].Value = "";
                            sheet.Cells["AQ" + rowOld_DM].Value = "";
                            sheet.Cells["AR" + rowOld_DM].Value = "";
                            sheet.Cells["AS" + rowOld_DM].Value = "";

                            TongDuDA += TongDuDM;
                            TongDuDA_T1 += TongDuDM_T1;
                            TongDuDA_T2 += TongDuDM_T2;
                            TongDuDA_T3 += TongDuDM_T3;
                            TongDuDA_T4 += TongDuDM_T4;
                            TongDuDA_T5 += TongDuDM_T5;
                            TongDuDA_T6 += TongDuDM_T6;
                            TongDuDA_T7 += TongDuDM_T7;
                            TongDuDA_T8 += TongDuDM_T8;
                            TongDuDA_T9 += TongDuDM_T9;
                            TongDuDA_T10 += TongDuDM_T10;
                            TongDuDA_T11 += TongDuDM_T11;
                            TongDuDA_T12 += TongDuDM_T12;
                            TongKeHoachDA += TongKeHoachDM;
                            TongKeHoachDA_T1 += TongKeHoachDM_T1;
                            TongKeHoachDA_T2 += TongKeHoachDM_T2;
                            TongKeHoachDA_T3 += TongKeHoachDM_T3;
                            TongKeHoachDA_T4 += TongKeHoachDM_T4;
                            TongKeHoachDA_T5 += TongKeHoachDM_T5;
                            TongKeHoachDA_T6 += TongKeHoachDM_T6;
                            TongKeHoachDA_T7 += TongKeHoachDM_T7;
                            TongKeHoachDA_T8 += TongKeHoachDM_T8;
                            TongKeHoachDA_T9 += TongKeHoachDM_T9;
                            TongKeHoachDA_T10 += TongKeHoachDM_T10;
                            TongKeHoachDA_T11 += TongKeHoachDM_T11;
                            TongKeHoachDA_T12 += TongKeHoachDM_T12;
                            TongThucTeDA += TongThucTeDM;
                            TongThucTeDA_T1 += TongThucTeDM_T1;
                            TongThucTeDA_T2 += TongThucTeDM_T2;
                            TongThucTeDA_T3 += TongThucTeDM_T3;
                            TongThucTeDA_T4 += TongThucTeDM_T4;
                            TongThucTeDA_T5 += TongThucTeDM_T5;
                            TongThucTeDA_T6 += TongThucTeDM_T6;
                            TongThucTeDA_T7 += TongThucTeDM_T7;
                            TongThucTeDA_T8 += TongThucTeDM_T8;
                            TongThucTeDA_T9 += TongThucTeDM_T9;
                            TongThucTeDA_T10 += TongThucTeDM_T10;
                            TongThucTeDA_T11 += TongThucTeDM_T11;
                            TongThucTeDA_T12 += TongThucTeDM_T12;
                        }

                        sheet.Cells["C" + rowOld_DA].Value = TongDuDA.toCurrency().Replace(".", ",");
                        sheet.Cells["D" + rowOld_DA].Value = TongDuDA_T1.toCurrency().Replace(".", ",");
                        sheet.Cells["E" + rowOld_DA].Value = TongDuDA_T2.toCurrency().Replace(".", ",");
                        sheet.Cells["F" + rowOld_DA].Value = TongDuDA_T3.toCurrency().Replace(".", ",");
                        sheet.Cells["G" + rowOld_DA].Value = TongDuDA_T4.toCurrency().Replace(".", ",");
                        sheet.Cells["H" + rowOld_DA].Value = TongDuDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["I" + rowOld_DA].Value = TongDuDA_T6.toCurrency().Replace(".", ",");
                        sheet.Cells["J" + rowOld_DA].Value = TongDuDA_T7.toCurrency().Replace(".", ",");
                        sheet.Cells["K" + rowOld_DA].Value = TongDuDA_T8.toCurrency().Replace(".", ",");
                        sheet.Cells["L" + rowOld_DA].Value = TongDuDA_T9.toCurrency().Replace(".", ",");
                        sheet.Cells["M" + rowOld_DA].Value = TongDuDA_T10.toCurrency().Replace(".", ",");
                        sheet.Cells["N" + rowOld_DA].Value = TongDuDA_T11.toCurrency().Replace(".", ",");
                        sheet.Cells["O" + rowOld_DA].Value = TongDuDA_T12.toCurrency().Replace(".", ",");
                        sheet.Cells["P" + rowOld_DA].Value = TongKeHoachDA.toCurrency().Replace(".", ",");
                        sheet.Cells["Q" + rowOld_DA].Value = TongKeHoachDA_T1.toCurrency().Replace(".", ",");
                        sheet.Cells["R" + rowOld_DA].Value = TongKeHoachDA_T2.toCurrency().Replace(".", ",");
                        sheet.Cells["S" + rowOld_DA].Value = TongKeHoachDA_T3.toCurrency().Replace(".", ",");
                        sheet.Cells["T" + rowOld_DA].Value = TongKeHoachDA_T4.toCurrency().Replace(".", ",");
                        sheet.Cells["U" + rowOld_DA].Value = TongKeHoachDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["V" + rowOld_DA].Value = TongKeHoachDA_T6.toCurrency().Replace(".", ",");
                        sheet.Cells["W" + rowOld_DA].Value = TongKeHoachDA_T7.toCurrency().Replace(".", ",");
                        sheet.Cells["X" + rowOld_DA].Value = TongKeHoachDA_T8.toCurrency().Replace(".", ",");
                        sheet.Cells["Y" + rowOld_DA].Value = TongKeHoachDA_T9.toCurrency().Replace(".", ",");
                        sheet.Cells["Z" + rowOld_DA].Value = TongKeHoachDA_T10.toCurrency().Replace(".", ",");
                        sheet.Cells["AA" + rowOld_DA].Value = TongKeHoachDA_T11.toCurrency().Replace(".", ",");
                        sheet.Cells["AB" + rowOld_DA].Value = TongKeHoachDA_T12.toCurrency().Replace(".", ",");
                        sheet.Cells["AC" + rowOld_DA].Value = TongThucTeDA.toCurrency().Replace(".", ",");
                        sheet.Cells["AD" + rowOld_DA].Value = TongThucTeDA_T1.toCurrency().Replace(".", ",");
                        sheet.Cells["AE" + rowOld_DA].Value = TongThucTeDA_T2.toCurrency().Replace(".", ",");
                        sheet.Cells["AF" + rowOld_DA].Value = TongThucTeDA_T3.toCurrency().Replace(".", ",");
                        sheet.Cells["AG" + rowOld_DA].Value = TongThucTeDA_T4.toCurrency().Replace(".", ",");
                        sheet.Cells["AH" + rowOld_DA].Value = TongThucTeDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["AI" + rowOld_DA].Value = TongThucTeDA_T6.toCurrency().Replace(".", ",");
                        sheet.Cells["AJ" + rowOld_DA].Value = TongThucTeDA_T7.toCurrency().Replace(".", ",");
                        sheet.Cells["AK" + rowOld_DA].Value = TongThucTeDA_T8.toCurrency().Replace(".", ",");
                        sheet.Cells["AL" + rowOld_DA].Value = TongThucTeDA_T9.toCurrency().Replace(".", ",");
                        sheet.Cells["AM" + rowOld_DA].Value = TongThucTeDA_T10.toCurrency().Replace(".", ",");
                        sheet.Cells["AN" + rowOld_DA].Value = TongThucTeDA_T11.toCurrency().Replace(".", ",");
                        sheet.Cells["AO" + rowOld_DA].Value = TongThucTeDA_T12.toCurrency().Replace(".", ",");


                        string modelRange = "A5:AS" + row;
                        var modelTable = sheet.Cells[modelRange];
                        // Assign borders
                        modelTable.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.AutoFitColumns();
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public FileDto TongHop_BaoCaoDuAnThuHoi_ExportToFile(List<BaoCaoDuAnThuHoi_ExportToFileDto> listBaoCao)
        {
            try
            {
                return _exporter.CreateExcelPackage(
                    "TongHopBaoCaoThuHoi_" + (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds +
                    ".xlsx",
                    excelPackage =>
                    {
                        using (FileStream stream =
                            new FileStream("wwwroot/TemplateFile/TempTongHop_BaoCaoKeHoachThanhToan.xlsx",
                                FileMode.Open))
                        {
                            using (ExcelPackage p = new ExcelPackage(stream))
                            {
                                foreach (ExcelWorksheet ws in p.Workbook.Worksheets)
                                {
                                    excelPackage.Workbook.Worksheets.Add(ws.Name, ws);
                                }
                            }
                        }

                        var sheet = excelPackage.Workbook.Worksheets[0];
                        sheet.OutLineApplyStyle = true;
                        sheet.Cells["A" + 1].Value = "TỔNG HỢP KẾ HOẠCH THU HỒI TẠM ỨNG";
                        int row = 6;
                        var stt = 1;


                        decimal TongHop_TongDuDA = 0;
                        decimal TongHop_TongDuDA_T1 = 0;
                        decimal TongHop_TongDuDA_T2 = 0;
                        decimal TongHop_TongDuDA_T3 = 0;
                        decimal TongHop_TongDuDA_T4 = 0;
                        decimal TongHop_TongDuDA_T5 = 0;
                        decimal TongHop_TongDuDA_T6 = 0;
                        decimal TongHop_TongDuDA_T7 = 0;
                        decimal TongHop_TongDuDA_T8 = 0;
                        decimal TongHop_TongDuDA_T9 = 0;
                        decimal TongHop_TongDuDA_T10 = 0;
                        decimal TongHop_TongDuDA_T11 = 0;
                        decimal TongHop_TongDuDA_T12 = 0;

                        decimal TongHop_TongKeHoachDA = 0;
                        decimal TongHop_TongKeHoachDA_T1 = 0;
                        decimal TongHop_TongKeHoachDA_T2 = 0;
                        decimal TongHop_TongKeHoachDA_T3 = 0;
                        decimal TongHop_TongKeHoachDA_T4 = 0;
                        decimal TongHop_TongKeHoachDA_T5 = 0;
                        decimal TongHop_TongKeHoachDA_T6 = 0;
                        decimal TongHop_TongKeHoachDA_T7 = 0;
                        decimal TongHop_TongKeHoachDA_T8 = 0;
                        decimal TongHop_TongKeHoachDA_T9 = 0;
                        decimal TongHop_TongKeHoachDA_T10 = 0;
                        decimal TongHop_TongKeHoachDA_T11 = 0;
                        decimal TongHop_TongKeHoachDA_T12 = 0;

                        decimal TongHop_TongThucTeDA = 0;
                        decimal TongHop_TongThucTeDA_T1 = 0;
                        decimal TongHop_TongThucTeDA_T2 = 0;
                        decimal TongHop_TongThucTeDA_T3 = 0;
                        decimal TongHop_TongThucTeDA_T4 = 0;
                        decimal TongHop_TongThucTeDA_T5 = 0;
                        decimal TongHop_TongThucTeDA_T6 = 0;
                        decimal TongHop_TongThucTeDA_T7 = 0;
                        decimal TongHop_TongThucTeDA_T8 = 0;
                        decimal TongHop_TongThucTeDA_T9 = 0;
                        decimal TongHop_TongThucTeDA_T10 = 0;
                        decimal TongHop_TongThucTeDA_T11 = 0;
                        decimal TongHop_TongThucTeDA_T12 = 0;

                        foreach (var baoCao in listBaoCao)
                        {
                            decimal TongDuDA = 0;
                            decimal TongDuDA_T1 = 0;
                            decimal TongDuDA_T2 = 0;
                            decimal TongDuDA_T3 = 0;
                            decimal TongDuDA_T4 = 0;
                            decimal TongDuDA_T5 = 0;
                            decimal TongDuDA_T6 = 0;
                            decimal TongDuDA_T7 = 0;
                            decimal TongDuDA_T8 = 0;
                            decimal TongDuDA_T9 = 0;
                            decimal TongDuDA_T10 = 0;
                            decimal TongDuDA_T11 = 0;
                            decimal TongDuDA_T12 = 0;

                            decimal TongKeHoachDA = 0;
                            decimal TongKeHoachDA_T1 = 0;
                            decimal TongKeHoachDA_T2 = 0;
                            decimal TongKeHoachDA_T3 = 0;
                            decimal TongKeHoachDA_T4 = 0;
                            decimal TongKeHoachDA_T5 = 0;
                            decimal TongKeHoachDA_T6 = 0;
                            decimal TongKeHoachDA_T7 = 0;
                            decimal TongKeHoachDA_T8 = 0;
                            decimal TongKeHoachDA_T9 = 0;
                            decimal TongKeHoachDA_T10 = 0;
                            decimal TongKeHoachDA_T11 = 0;
                            decimal TongKeHoachDA_T12 = 0;

                            decimal TongThucTeDA = 0;
                            decimal TongThucTeDA_T1 = 0;
                            decimal TongThucTeDA_T2 = 0;
                            decimal TongThucTeDA_T3 = 0;
                            decimal TongThucTeDA_T4 = 0;
                            decimal TongThucTeDA_T5 = 0;
                            decimal TongThucTeDA_T6 = 0;
                            decimal TongThucTeDA_T7 = 0;
                            decimal TongThucTeDA_T8 = 0;
                            decimal TongThucTeDA_T9 = 0;
                            decimal TongThucTeDA_T10 = 0;
                            decimal TongThucTeDA_T11 = 0;
                            decimal TongThucTeDA_T12 = 0;


                            //sheet1
                            
                            var rowOld_DA = row;

                            sheet.Cells["A" + row + ":AS" + row].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            sheet.Cells["A" + row + ":AS" + row].Style.Fill.BackgroundColor
                                .SetColor(Color.FromArgb(211, 211, 211));
                            sheet.Cells["A" + row + ":AS" + row].Style.Font.Bold = true;
                            sheet.Cells["A" + row].Style.HorizontalAlignment =
                                OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            sheet.Cells["C" + row + ":AS" + row].Style.HorizontalAlignment =
                                OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;

                            sheet.Cells["A" + row].Value = stt;
                            sheet.Cells["B" + row].Value = baoCao.DuAnThuHoi.Ten;
                            // sheet.Cells["AP" + row].Value = baoCao.DuAnThuHoi.Ten;
                            sheet.Cells["AQ" + row].Value =
                                baoCao.DuAnThuHoi.ThoiHanBaoLanhHopDong?.ToString("dd/MM/yyyy");
                            sheet.Cells["AR" + row].Value =
                                baoCao.DuAnThuHoi.ThoiHanBaoLanhTamUng?.ToString("dd/MM/yyyy");

                            row++;

                            foreach (var data in baoCao.ListData)
                            {
                                decimal TongDuDM = 0;
                                decimal TongDuDM_T1 = 0;
                                decimal TongDuDM_T2 = 0;
                                decimal TongDuDM_T3 = 0;
                                decimal TongDuDM_T4 = 0;
                                decimal TongDuDM_T5 = 0;
                                decimal TongDuDM_T6 = 0;
                                decimal TongDuDM_T7 = 0;
                                decimal TongDuDM_T8 = 0;
                                decimal TongDuDM_T9 = 0;
                                decimal TongDuDM_T10 = 0;
                                decimal TongDuDM_T11 = 0;
                                decimal TongDuDM_T12 = 0;

                                decimal TongKeHoachDM = 0;
                                decimal TongKeHoachDM_T1 = 0;
                                decimal TongKeHoachDM_T2 = 0;
                                decimal TongKeHoachDM_T3 = 0;
                                decimal TongKeHoachDM_T4 = 0;
                                decimal TongKeHoachDM_T5 = 0;
                                decimal TongKeHoachDM_T6 = 0;
                                decimal TongKeHoachDM_T7 = 0;
                                decimal TongKeHoachDM_T8 = 0;
                                decimal TongKeHoachDM_T9 = 0;
                                decimal TongKeHoachDM_T10 = 0;
                                decimal TongKeHoachDM_T11 = 0;
                                decimal TongKeHoachDM_T12 = 0;

                                decimal TongThucTeDM = 0;
                                decimal TongThucTeDM_T1 = 0;
                                decimal TongThucTeDM_T2 = 0;
                                decimal TongThucTeDM_T3 = 0;
                                decimal TongThucTeDM_T4 = 0;
                                decimal TongThucTeDM_T5 = 0;
                                decimal TongThucTeDM_T6 = 0;
                                decimal TongThucTeDM_T7 = 0;
                                decimal TongThucTeDM_T8 = 0;
                                decimal TongThucTeDM_T9 = 0;
                                decimal TongThucTeDM_T10 = 0;
                                decimal TongThucTeDM_T11 = 0;
                                decimal TongThucTeDM_T12 = 0;

                                var rowOld_DM = row;

                                sheet.Cells["A" + row + ":AS" + row].Style.Font.Bold = true;
                                sheet.Cells["A" + row].Style.HorizontalAlignment =
                                    OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                sheet.Cells["C" + row + ":AS" + row].Style.HorizontalAlignment =
                                    OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                sheet.Cells["A" + row].Value = stt + "." + data.DanhMucThuHoi.Stt;
                                sheet.Cells["B" + row].Value = data.DanhMucThuHoi.Ten;

                                row++;
                                foreach (var dtr in data.ListChiTietThuHoi)
                                {
                                    sheet.Cells["A" + row + ":AS" + row].Style.Font.Italic = true;
                                    sheet.Cells["A" + row].Style.HorizontalAlignment =
                                        OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                    sheet.Cells["C" + row + ":AS" + row].Style.HorizontalAlignment =
                                        OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                                    sheet.Cells["A" + row].Value = "";
                                    sheet.Cells["B" + row].Value = dtr.Ten;
                                    sheet.Cells["C" + row].Value = dtr.TongDu.toCurrency().Replace(".", ",");
                                    sheet.Cells["D" + row].Value = dtr.Du1.toCurrency().Replace(".", ",");
                                    sheet.Cells["E" + row].Value = dtr.Du2.toCurrency().Replace(".", ",");
                                    sheet.Cells["F" + row].Value = dtr.Du3.toCurrency().Replace(".", ",");
                                    sheet.Cells["G" + row].Value = dtr.Du4.toCurrency().Replace(".", ",");
                                    sheet.Cells["H" + row].Value = dtr.Du5.toCurrency().Replace(".", ",");
                                    sheet.Cells["I" + row].Value = dtr.Du6.toCurrency().Replace(".", ",");
                                    sheet.Cells["J" + row].Value = dtr.Du7.toCurrency().Replace(".", ",");
                                    sheet.Cells["K" + row].Value = dtr.Du8.toCurrency().Replace(".", ",");
                                    sheet.Cells["L" + row].Value = dtr.Du9.toCurrency().Replace(".", ",");
                                    sheet.Cells["M" + row].Value = dtr.Du10.toCurrency().Replace(".", ",");
                                    sheet.Cells["N" + row].Value = dtr.Du11.toCurrency().Replace(".", ",");
                                    sheet.Cells["O" + row].Value = dtr.Du12.toCurrency().Replace(".", ",");
                                    sheet.Cells["P" + row].Value = dtr.TongThu.toCurrency().Replace(".", ",");
                                    sheet.Cells["Q" + row].Value = dtr.Thu1.toCurrency().Replace(".", ",");
                                    sheet.Cells["R" + row].Value = dtr.Thu2.toCurrency().Replace(".", ",");
                                    sheet.Cells["S" + row].Value = dtr.Thu3.toCurrency().Replace(".", ",");
                                    sheet.Cells["T" + row].Value = dtr.Thu4.toCurrency().Replace(".", ",");
                                    sheet.Cells["U" + row].Value = dtr.Thu5.toCurrency().Replace(".", ",");
                                    sheet.Cells["V" + row].Value = dtr.Thu6.toCurrency().Replace(".", ",");
                                    sheet.Cells["W" + row].Value = dtr.Thu7.toCurrency().Replace(".", ",");
                                    sheet.Cells["X" + row].Value = dtr.Thu8.toCurrency().Replace(".", ",");
                                    sheet.Cells["Y" + row].Value = dtr.Thu9.toCurrency().Replace(".", ",");
                                    sheet.Cells["Z" + row].Value = dtr.Thu10.toCurrency().Replace(".", ",");
                                    sheet.Cells["AA" + row].Value = dtr.Thu11.toCurrency().Replace(".", ",");
                                    sheet.Cells["AB" + row].Value = dtr.Thu12.toCurrency().Replace(".", ",");
                                    sheet.Cells["AC" + row].Value = dtr.TongThucTe.toCurrency().Replace(".", ",");
                                    sheet.Cells["AD" + row].Value = dtr.ThucTe1.toCurrency().Replace(".", ",");
                                    sheet.Cells["AE" + row].Value = dtr.ThucTe2.toCurrency().Replace(".", ",");
                                    sheet.Cells["AF" + row].Value = dtr.ThucTe3.toCurrency().Replace(".", ",");
                                    sheet.Cells["AG" + row].Value = dtr.ThucTe4.toCurrency().Replace(".", ",");
                                    sheet.Cells["AH" + row].Value = dtr.ThucTe5.toCurrency().Replace(".", ",");
                                    sheet.Cells["AI" + row].Value = dtr.ThucTe6.toCurrency().Replace(".", ",");
                                    sheet.Cells["AJ" + row].Value = dtr.ThucTe7.toCurrency().Replace(".", ",");
                                    sheet.Cells["AK" + row].Value = dtr.ThucTe8.toCurrency().Replace(".", ",");
                                    sheet.Cells["AL" + row].Value = dtr.ThucTe9.toCurrency().Replace(".", ",");
                                    sheet.Cells["AM" + row].Value = dtr.ThucTe10.toCurrency().Replace(".", ",");
                                    sheet.Cells["AN" + row].Value = dtr.ThucTe11.toCurrency().Replace(".", ",");
                                    sheet.Cells["AO" + row].Value = dtr.ThucTe12.toCurrency().Replace(".", ",");
                                    sheet.Cells["AP" + row].Value = "";
                                    sheet.Cells["AQ" + row].Value = "";
                                    sheet.Cells["AR" + row].Value = "";
                                    sheet.Cells["AS" + row].Value = "";

                                    TongDuDM += dtr.TongDu;
                                    TongDuDM_T1 += dtr.Du1;
                                    TongDuDM_T2 += dtr.Du2;
                                    TongDuDM_T3 += dtr.Du3;
                                    TongDuDM_T4 += dtr.Du4;
                                    TongDuDM_T5 += dtr.Du5;
                                    TongDuDM_T6 += dtr.Du6;
                                    TongDuDM_T7 += dtr.Du7;
                                    TongDuDM_T8 += dtr.Du8;
                                    TongDuDM_T9 += dtr.Du9;
                                    TongDuDM_T10 += dtr.Du10;
                                    TongDuDM_T11 += dtr.Du11;
                                    TongDuDM_T12 += dtr.Du12;
                                    TongKeHoachDM += dtr.TongThu;
                                    TongKeHoachDM_T1 += dtr.Thu1;
                                    TongKeHoachDM_T2 += dtr.Thu2;
                                    TongKeHoachDM_T3 += dtr.Thu3;
                                    TongKeHoachDM_T4 += dtr.Thu4;
                                    TongKeHoachDM_T5 += dtr.Thu5;
                                    TongKeHoachDM_T6 += dtr.Thu6;
                                    TongKeHoachDM_T7 += dtr.Thu7;
                                    TongKeHoachDM_T8 += dtr.Thu8;
                                    TongKeHoachDM_T9 += dtr.Thu9;
                                    TongKeHoachDM_T10 += dtr.Thu10;
                                    TongKeHoachDM_T11 += dtr.Thu11;
                                    TongKeHoachDM_T12 += dtr.Thu12;
                                    TongThucTeDM += dtr.TongThucTe;
                                    TongThucTeDM_T1 += dtr.ThucTe1;
                                    TongThucTeDM_T2 += dtr.ThucTe2;
                                    TongThucTeDM_T3 += dtr.ThucTe3;
                                    TongThucTeDM_T4 += dtr.ThucTe4;
                                    TongThucTeDM_T5 += dtr.ThucTe5;
                                    TongThucTeDM_T6 += dtr.ThucTe6;
                                    TongThucTeDM_T7 += dtr.ThucTe7;
                                    TongThucTeDM_T8 += dtr.ThucTe8;
                                    TongThucTeDM_T9 += dtr.ThucTe9;
                                    TongThucTeDM_T10 += dtr.ThucTe10;
                                    TongThucTeDM_T11 += dtr.ThucTe11;
                                    TongThucTeDM_T12 += dtr.ThucTe12;

                                    row++;
                                }

                                sheet.Cells["C" + rowOld_DM].Value = TongDuDM.toCurrency().Replace(".", ",");
                                sheet.Cells["D" + rowOld_DM].Value = TongDuDM_T1.toCurrency().Replace(".", ",");
                                sheet.Cells["E" + rowOld_DM].Value = TongDuDM_T2.toCurrency().Replace(".", ",");
                                sheet.Cells["F" + rowOld_DM].Value = TongDuDM_T3.toCurrency().Replace(".", ",");
                                sheet.Cells["G" + rowOld_DM].Value = TongDuDM_T4.toCurrency().Replace(".", ",");
                                sheet.Cells["H" + rowOld_DM].Value = TongDuDM_T5.toCurrency().Replace(".", ",");
                                sheet.Cells["I" + rowOld_DM].Value = TongDuDM_T6.toCurrency().Replace(".", ",");
                                sheet.Cells["J" + rowOld_DM].Value = TongDuDM_T7.toCurrency().Replace(".", ",");
                                sheet.Cells["K" + rowOld_DM].Value = TongDuDM_T8.toCurrency().Replace(".", ",");
                                sheet.Cells["L" + rowOld_DM].Value = TongDuDM_T9.toCurrency().Replace(".", ",");
                                sheet.Cells["M" + rowOld_DM].Value = TongDuDM_T10.toCurrency().Replace(".", ",");
                                sheet.Cells["N" + rowOld_DM].Value = TongDuDM_T11.toCurrency().Replace(".", ",");
                                sheet.Cells["O" + rowOld_DM].Value = TongDuDM_T12.toCurrency().Replace(".", ",");
                                sheet.Cells["P" + rowOld_DM].Value = TongKeHoachDM.toCurrency().Replace(".", ",");
                                sheet.Cells["Q" + rowOld_DM].Value = TongKeHoachDM_T1.toCurrency().Replace(".", ",");
                                sheet.Cells["R" + rowOld_DM].Value = TongKeHoachDM_T2.toCurrency().Replace(".", ",");
                                sheet.Cells["S" + rowOld_DM].Value = TongKeHoachDM_T3.toCurrency().Replace(".", ",");
                                sheet.Cells["T" + rowOld_DM].Value = TongKeHoachDM_T4.toCurrency().Replace(".", ",");
                                sheet.Cells["U" + rowOld_DM].Value = TongKeHoachDM_T5.toCurrency().Replace(".", ",");
                                sheet.Cells["V" + rowOld_DM].Value = TongKeHoachDM_T6.toCurrency().Replace(".", ",");
                                sheet.Cells["W" + rowOld_DM].Value = TongKeHoachDM_T7.toCurrency().Replace(".", ",");
                                sheet.Cells["X" + rowOld_DM].Value = TongKeHoachDM_T8.toCurrency().Replace(".", ",");
                                sheet.Cells["Y" + rowOld_DM].Value = TongKeHoachDM_T9.toCurrency().Replace(".", ",");
                                sheet.Cells["Z" + rowOld_DM].Value = TongKeHoachDM_T10.toCurrency().Replace(".", ",");
                                sheet.Cells["AA" + rowOld_DM].Value = TongKeHoachDM_T11.toCurrency().Replace(".", ",");
                                sheet.Cells["AB" + rowOld_DM].Value = TongKeHoachDM_T12.toCurrency().Replace(".", ",");
                                sheet.Cells["AC" + rowOld_DM].Value = TongThucTeDM.toCurrency().Replace(".", ",");
                                sheet.Cells["AD" + rowOld_DM].Value = TongThucTeDM_T1.toCurrency().Replace(".", ",");
                                sheet.Cells["AE" + rowOld_DM].Value = TongThucTeDM_T2.toCurrency().Replace(".", ",");
                                sheet.Cells["AF" + rowOld_DM].Value = TongThucTeDM_T3.toCurrency().Replace(".", ",");
                                sheet.Cells["AG" + rowOld_DM].Value = TongThucTeDM_T4.toCurrency().Replace(".", ",");
                                sheet.Cells["AH" + rowOld_DM].Value = TongThucTeDM_T5.toCurrency().Replace(".", ",");
                                sheet.Cells["AI" + rowOld_DM].Value = TongThucTeDM_T6.toCurrency().Replace(".", ",");
                                sheet.Cells["AJ" + rowOld_DM].Value = TongThucTeDM_T7.toCurrency().Replace(".", ",");
                                sheet.Cells["AK" + rowOld_DM].Value = TongThucTeDM_T8.toCurrency().Replace(".", ",");
                                sheet.Cells["AL" + rowOld_DM].Value = TongThucTeDM_T9.toCurrency().Replace(".", ",");
                                sheet.Cells["AM" + rowOld_DM].Value = TongThucTeDM_T10.toCurrency().Replace(".", ",");
                                sheet.Cells["AN" + rowOld_DM].Value = TongThucTeDM_T11.toCurrency().Replace(".", ",");
                                sheet.Cells["AO" + rowOld_DM].Value = TongThucTeDM_T12.toCurrency().Replace(".", ",");
                                sheet.Cells["AP" + rowOld_DM].Value = "";
                                sheet.Cells["AQ" + rowOld_DM].Value = "";
                                sheet.Cells["AR" + rowOld_DM].Value = "";
                                sheet.Cells["AS" + rowOld_DM].Value = "";

                                TongDuDA += TongDuDM;
                                TongDuDA_T1 += TongDuDM_T1;
                                TongDuDA_T2 += TongDuDM_T2;
                                TongDuDA_T3 += TongDuDM_T3;
                                TongDuDA_T4 += TongDuDM_T4;
                                TongDuDA_T5 += TongDuDM_T5;
                                TongDuDA_T6 += TongDuDM_T6;
                                TongDuDA_T7 += TongDuDM_T7;
                                TongDuDA_T8 += TongDuDM_T8;
                                TongDuDA_T9 += TongDuDM_T9;
                                TongDuDA_T10 += TongDuDM_T10;
                                TongDuDA_T11 += TongDuDM_T11;
                                TongDuDA_T12 += TongDuDM_T12;
                                TongKeHoachDA += TongKeHoachDM;
                                TongKeHoachDA_T1 += TongKeHoachDM_T1;
                                TongKeHoachDA_T2 += TongKeHoachDM_T2;
                                TongKeHoachDA_T3 += TongKeHoachDM_T3;
                                TongKeHoachDA_T4 += TongKeHoachDM_T4;
                                TongKeHoachDA_T5 += TongKeHoachDM_T5;
                                TongKeHoachDA_T6 += TongKeHoachDM_T6;
                                TongKeHoachDA_T7 += TongKeHoachDM_T7;
                                TongKeHoachDA_T8 += TongKeHoachDM_T8;
                                TongKeHoachDA_T9 += TongKeHoachDM_T9;
                                TongKeHoachDA_T10 += TongKeHoachDM_T10;
                                TongKeHoachDA_T11 += TongKeHoachDM_T11;
                                TongKeHoachDA_T12 += TongKeHoachDM_T12;
                                TongThucTeDA += TongThucTeDM;
                                TongThucTeDA_T1 += TongThucTeDM_T1;
                                TongThucTeDA_T2 += TongThucTeDM_T2;
                                TongThucTeDA_T3 += TongThucTeDM_T3;
                                TongThucTeDA_T4 += TongThucTeDM_T4;
                                TongThucTeDA_T5 += TongThucTeDM_T5;
                                TongThucTeDA_T6 += TongThucTeDM_T6;
                                TongThucTeDA_T7 += TongThucTeDM_T7;
                                TongThucTeDA_T8 += TongThucTeDM_T8;
                                TongThucTeDA_T9 += TongThucTeDM_T9;
                                TongThucTeDA_T10 += TongThucTeDM_T10;
                                TongThucTeDA_T11 += TongThucTeDM_T11;
                                TongThucTeDA_T12 += TongThucTeDM_T12;
                            }

                            sheet.Cells["C" + rowOld_DA].Value = TongDuDA.toCurrency().Replace(".", ",");
                            sheet.Cells["D" + rowOld_DA].Value = TongDuDA_T1.toCurrency().Replace(".", ",");
                            sheet.Cells["E" + rowOld_DA].Value = TongDuDA_T2.toCurrency().Replace(".", ",");
                            sheet.Cells["F" + rowOld_DA].Value = TongDuDA_T3.toCurrency().Replace(".", ",");
                            sheet.Cells["G" + rowOld_DA].Value = TongDuDA_T4.toCurrency().Replace(".", ",");
                            sheet.Cells["H" + rowOld_DA].Value = TongDuDA_T5.toCurrency().Replace(".", ",");
                            sheet.Cells["I" + rowOld_DA].Value = TongDuDA_T6.toCurrency().Replace(".", ",");
                            sheet.Cells["J" + rowOld_DA].Value = TongDuDA_T7.toCurrency().Replace(".", ",");
                            sheet.Cells["K" + rowOld_DA].Value = TongDuDA_T8.toCurrency().Replace(".", ",");
                            sheet.Cells["L" + rowOld_DA].Value = TongDuDA_T9.toCurrency().Replace(".", ",");
                            sheet.Cells["M" + rowOld_DA].Value = TongDuDA_T10.toCurrency().Replace(".", ",");
                            sheet.Cells["N" + rowOld_DA].Value = TongDuDA_T11.toCurrency().Replace(".", ",");
                            sheet.Cells["O" + rowOld_DA].Value = TongDuDA_T12.toCurrency().Replace(".", ",");
                            sheet.Cells["P" + rowOld_DA].Value = TongKeHoachDA.toCurrency().Replace(".", ",");
                            sheet.Cells["Q" + rowOld_DA].Value = TongKeHoachDA_T1.toCurrency().Replace(".", ",");
                            sheet.Cells["R" + rowOld_DA].Value = TongKeHoachDA_T2.toCurrency().Replace(".", ",");
                            sheet.Cells["S" + rowOld_DA].Value = TongKeHoachDA_T3.toCurrency().Replace(".", ",");
                            sheet.Cells["T" + rowOld_DA].Value = TongKeHoachDA_T4.toCurrency().Replace(".", ",");
                            sheet.Cells["U" + rowOld_DA].Value = TongKeHoachDA_T5.toCurrency().Replace(".", ",");
                            sheet.Cells["V" + rowOld_DA].Value = TongKeHoachDA_T6.toCurrency().Replace(".", ",");
                            sheet.Cells["W" + rowOld_DA].Value = TongKeHoachDA_T7.toCurrency().Replace(".", ",");
                            sheet.Cells["X" + rowOld_DA].Value = TongKeHoachDA_T8.toCurrency().Replace(".", ",");
                            sheet.Cells["Y" + rowOld_DA].Value = TongKeHoachDA_T9.toCurrency().Replace(".", ",");
                            sheet.Cells["Z" + rowOld_DA].Value = TongKeHoachDA_T10.toCurrency().Replace(".", ",");
                            sheet.Cells["AA" + rowOld_DA].Value = TongKeHoachDA_T11.toCurrency().Replace(".", ",");
                            sheet.Cells["AB" + rowOld_DA].Value = TongKeHoachDA_T12.toCurrency().Replace(".", ",");
                            sheet.Cells["AC" + rowOld_DA].Value = TongThucTeDA.toCurrency().Replace(".", ",");
                            sheet.Cells["AD" + rowOld_DA].Value = TongThucTeDA_T1.toCurrency().Replace(".", ",");
                            sheet.Cells["AE" + rowOld_DA].Value = TongThucTeDA_T2.toCurrency().Replace(".", ",");
                            sheet.Cells["AF" + rowOld_DA].Value = TongThucTeDA_T3.toCurrency().Replace(".", ",");
                            sheet.Cells["AG" + rowOld_DA].Value = TongThucTeDA_T4.toCurrency().Replace(".", ",");
                            sheet.Cells["AH" + rowOld_DA].Value = TongThucTeDA_T5.toCurrency().Replace(".", ",");
                            sheet.Cells["AI" + rowOld_DA].Value = TongThucTeDA_T6.toCurrency().Replace(".", ",");
                            sheet.Cells["AJ" + rowOld_DA].Value = TongThucTeDA_T7.toCurrency().Replace(".", ",");
                            sheet.Cells["AK" + rowOld_DA].Value = TongThucTeDA_T8.toCurrency().Replace(".", ",");
                            sheet.Cells["AL" + rowOld_DA].Value = TongThucTeDA_T9.toCurrency().Replace(".", ",");
                            sheet.Cells["AM" + rowOld_DA].Value = TongThucTeDA_T10.toCurrency().Replace(".", ",");
                            sheet.Cells["AN" + rowOld_DA].Value = TongThucTeDA_T11.toCurrency().Replace(".", ",");
                            sheet.Cells["AO" + rowOld_DA].Value = TongThucTeDA_T12.toCurrency().Replace(".", ",");

                            stt++;

                            TongHop_TongDuDA += TongDuDA;
                            TongHop_TongDuDA_T1 += TongDuDA_T1;
                            TongHop_TongDuDA_T2 += TongDuDA_T2;
                            TongHop_TongDuDA_T3 += TongDuDA_T3;
                            TongHop_TongDuDA_T4 += TongDuDA_T4;
                            TongHop_TongDuDA_T5 += TongDuDA_T5;
                            TongHop_TongDuDA_T6 += TongDuDA_T6;
                            TongHop_TongDuDA_T7 += TongDuDA_T7;
                            TongHop_TongDuDA_T8 += TongDuDA_T8;
                            TongHop_TongDuDA_T9 += TongDuDA_T9;
                            TongHop_TongDuDA_T10 += TongDuDA_T10;
                            TongHop_TongDuDA_T11 += TongDuDA_T11;
                            TongHop_TongDuDA_T12 += TongDuDA_T12;
                            TongHop_TongKeHoachDA += TongKeHoachDA;
                            TongHop_TongKeHoachDA_T1 += TongKeHoachDA_T1;
                            TongHop_TongKeHoachDA_T2 += TongKeHoachDA_T2;
                            TongHop_TongKeHoachDA_T3 += TongKeHoachDA_T3;
                            TongHop_TongKeHoachDA_T4 += TongKeHoachDA_T4;
                            TongHop_TongKeHoachDA_T5 += TongKeHoachDA_T5;
                            TongHop_TongKeHoachDA_T6 += TongKeHoachDA_T6;
                            TongHop_TongKeHoachDA_T7 += TongKeHoachDA_T7;
                            TongHop_TongKeHoachDA_T8 += TongKeHoachDA_T8;
                            TongHop_TongKeHoachDA_T9 += TongKeHoachDA_T9;
                            TongHop_TongKeHoachDA_T10 += TongKeHoachDA_T10;
                            TongHop_TongKeHoachDA_T11 += TongKeHoachDA_T11;
                            TongHop_TongKeHoachDA_T12 += TongKeHoachDA_T12;
                            TongHop_TongThucTeDA += TongThucTeDA;
                            TongHop_TongThucTeDA_T1 += TongThucTeDA_T1;
                            TongHop_TongThucTeDA_T2 += TongThucTeDA_T2;
                            TongHop_TongThucTeDA_T3 += TongThucTeDA_T3;
                            TongHop_TongThucTeDA_T4 += TongThucTeDA_T4;
                            TongHop_TongThucTeDA_T5 += TongThucTeDA_T5;
                            TongHop_TongThucTeDA_T6 += TongThucTeDA_T6;
                            TongHop_TongThucTeDA_T7 += TongThucTeDA_T7;
                            TongHop_TongThucTeDA_T8 += TongThucTeDA_T8;
                            TongHop_TongThucTeDA_T9 += TongThucTeDA_T9;
                            TongHop_TongThucTeDA_T10 += TongThucTeDA_T10;
                            TongHop_TongThucTeDA_T11 += TongThucTeDA_T11;
                            TongHop_TongThucTeDA_T12 += TongThucTeDA_T12;
                        }

                        sheet.Cells["A5:AS5"].Style.Font.Bold = true;
                        sheet.Cells["A5"].Style.HorizontalAlignment =
                            OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                        sheet.Cells["C5:AS5"].Style.HorizontalAlignment =
                            OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                        sheet.Cells["C" + 5].Value = TongHop_TongDuDA.toCurrency().Replace(".", ",");
                        sheet.Cells["D" + 5].Value = TongHop_TongDuDA_T1.toCurrency().Replace(".", ",");
                        sheet.Cells["E" + 5].Value = TongHop_TongDuDA_T2.toCurrency().Replace(".", ",");
                        sheet.Cells["F" + 5].Value = TongHop_TongDuDA_T3.toCurrency().Replace(".", ",");
                        sheet.Cells["G" + 5].Value = TongHop_TongDuDA_T4.toCurrency().Replace(".", ",");
                        sheet.Cells["H" + 5].Value = TongHop_TongDuDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["I" + 5].Value = TongHop_TongDuDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["J" + 5].Value = TongHop_TongDuDA_T7.toCurrency().Replace(".", ",");
                        sheet.Cells["K" + 5].Value = TongHop_TongDuDA_T8.toCurrency().Replace(".", ",");
                        sheet.Cells["L" + 5].Value = TongHop_TongDuDA_T9.toCurrency().Replace(".", ",");
                        sheet.Cells["M" + 5].Value = TongHop_TongDuDA_T10.toCurrency().Replace(".", ",");
                        sheet.Cells["N" + 5].Value = TongHop_TongDuDA_T11.toCurrency().Replace(".", ",");
                        sheet.Cells["O" + 5].Value = TongHop_TongDuDA_T12.toCurrency().Replace(".", ",");
                        sheet.Cells["P" + 5].Value = TongHop_TongKeHoachDA.toCurrency().Replace(".", ",");
                        sheet.Cells["Q" + 5].Value = TongHop_TongKeHoachDA_T1.toCurrency().Replace(".", ",");
                        sheet.Cells["R" + 5].Value = TongHop_TongKeHoachDA_T2.toCurrency().Replace(".", ",");
                        sheet.Cells["S" + 5].Value = TongHop_TongKeHoachDA_T3.toCurrency().Replace(".", ",");
                        sheet.Cells["T" + 5].Value = TongHop_TongKeHoachDA_T4.toCurrency().Replace(".", ",");
                        sheet.Cells["U" + 5].Value = TongHop_TongKeHoachDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["V" + 5].Value = TongHop_TongKeHoachDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["W" + 5].Value = TongHop_TongKeHoachDA_T7.toCurrency().Replace(".", ",");
                        sheet.Cells["X" + 5].Value = TongHop_TongKeHoachDA_T8.toCurrency().Replace(".", ",");
                        sheet.Cells["Y" + 5].Value = TongHop_TongKeHoachDA_T9.toCurrency().Replace(".", ",");
                        sheet.Cells["Z" + 5].Value = TongHop_TongKeHoachDA_T10.toCurrency().Replace(".", ",");
                        sheet.Cells["AA" + 5].Value = TongHop_TongKeHoachDA_T11.toCurrency().Replace(".", ",");
                        sheet.Cells["AB" + 5].Value = TongHop_TongKeHoachDA_T12.toCurrency().Replace(".", ",");
                        sheet.Cells["AC" + 5].Value = TongHop_TongThucTeDA.toCurrency().Replace(".", ",");
                        sheet.Cells["AD" + 5].Value = TongHop_TongThucTeDA_T1.toCurrency().Replace(".", ",");
                        sheet.Cells["AE" + 5].Value = TongHop_TongThucTeDA_T2.toCurrency().Replace(".", ",");
                        sheet.Cells["AF" + 5].Value = TongHop_TongThucTeDA_T3.toCurrency().Replace(".", ",");
                        sheet.Cells["AG" + 5].Value = TongHop_TongThucTeDA_T4.toCurrency().Replace(".", ",");
                        sheet.Cells["AH" + 5].Value = TongHop_TongThucTeDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["AI" + 5].Value = TongHop_TongThucTeDA_T5.toCurrency().Replace(".", ",");
                        sheet.Cells["AJ" + 5].Value = TongHop_TongThucTeDA_T7.toCurrency().Replace(".", ",");
                        sheet.Cells["AK" + 5].Value = TongHop_TongThucTeDA_T8.toCurrency().Replace(".", ",");
                        sheet.Cells["AL" + 5].Value = TongHop_TongThucTeDA_T9.toCurrency().Replace(".", ",");
                        sheet.Cells["AM" + 5].Value = TongHop_TongThucTeDA_T10.toCurrency().Replace(".", ",");
                        sheet.Cells["AN" + 5].Value = TongHop_TongThucTeDA_T11.toCurrency().Replace(".", ",");
                        sheet.Cells["AO" + 5].Value = TongHop_TongThucTeDA_T12.toCurrency().Replace(".", ",");


                        string modelRange = "A6:AS" + row;
                        var modelTable = sheet.Cells[modelRange];
                        // Assign borders
                        modelTable.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        modelTable.AutoFitColumns();
                    });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}