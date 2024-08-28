using System;
using System.Collections.Generic;
using System.Data;
using Abp.AspNetZeroCore.Net;
// using Core.Collections;
// using Core.InputOutput;
// using Core.DataAccess;
using IdentityServer4.Extensions;
using OfficeOpenXml;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Text;
using OfficeOpenXml.Style;
using Microsoft.Extensions.Caching.Memory;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

namespace Core.Excel
{
    public class ExcelExporter
    {
        private readonly ITempFileCacheManager _tempFileCacheManager;

        public ExcelExporter(ITempFileCacheManager tempFileCacheManager)
        {
             _tempFileCacheManager = tempFileCacheManager;
            //_cache = new MemoryCache(new MemoryCacheOptions
            //{
            //    SizeLimit = 1024
            //});
        }

        public FileDto CreateExcelPackage(string fileName, Action<ExcelPackage> creator)
        {
            var file = new FileDto(fileName, MimeTypeNames.ApplicationVndOpenxmlformatsOfficedocumentSpreadsheetmlSheet);

            using (var excelPackage = new ExcelPackage())
            {
                creator(excelPackage);
                Save(excelPackage, file);
            }

            return file;
        }


        public void AddHeader(ExcelWorksheet sheet,  params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeader(sheet, i + 1, headerTexts[i]);
            }
        }

        protected void AddHeader(ExcelWorksheet sheet, int columnIndex, string headerText)
        {
            sheet.Cells[1, columnIndex].Value = headerText;
            sheet.Cells[1, columnIndex].Style.Font.Bold = true;
        }
        public void AddHeaderWithRowIndex(int rowIndex, ExcelWorksheet sheet, params string[] headerTexts)
        {
            if (headerTexts.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < headerTexts.Length; i++)
            {
                AddHeaderWithRowIndex(sheet, rowIndex, i + 1, headerTexts[i]);
            }
        }
        protected void AddHeaderWithRowIndex(ExcelWorksheet sheet, int rowIndex, int columnIndex, string headerText)
        {
            sheet.Cells[rowIndex, columnIndex].Value = headerText;
            sheet.Cells[rowIndex, columnIndex].Style.Font.Bold = true;
            sheet.Cells[rowIndex, columnIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
        }
        public void AddObjects<T>(ExcelWorksheet sheet, int startRowIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + 1].Value = propertySelectors[j](items[i]); // propertySelector( item )
                }
            }
        }
          
        public void AddDataTable(ExcelWorksheet sheet, int startRowIndex, DataTable dt, params Func<DataRow, object>[] columnSelectors)
          {
              if ( columnSelectors.IsNullOrEmpty())
              {
                  return;
              }

              for (var i = 0; i < dt.Rows.Count; i++)
              {
                  for (var j = 0; j < columnSelectors.Length; j++)
                  {
                      sheet.Cells[i + startRowIndex, j + 1].Value = columnSelectors[j](dt.Rows[i]);
                  }
              }
          }

        protected void AddObjectsFromColumnNumber<T>(ExcelWorksheet sheet, int startRowIndex, int startColIndex, IList<T> items, params Func<T, object>[] propertySelectors)
        {
            
            if (items.IsNullOrEmpty() || propertySelectors.IsNullOrEmpty())
            {
                return;
            }

            for (var i = 0; i < items.Count; i++)
            {
                for (var j = 0; j < propertySelectors.Length; j++)
                {
                    sheet.Cells[i + startRowIndex, j + startColIndex].Value = propertySelectors[j](items[i]);
                    sheet.Cells[i + startRowIndex, j + startColIndex].Style.Border.BorderAround(ExcelBorderStyle.Thin);
                }
            }
        }

        public void AddSum<T>(ExcelWorksheet sheet, int startRowIndex, int startColIndex, IList<T> items)
        {
            //sheet.Cells[items.Count + 3, 3].Value = "Total is :";
            sheet.Cells[items.Count + startRowIndex, startColIndex].Formula = 
                "SUM(" +  sheet.Cells[startRowIndex, startColIndex].Address + ":" + sheet.Cells[items.Count - 1 + startRowIndex , startColIndex].Address + ")";
            sheet.Cells[items.Count + startRowIndex, startColIndex].Style.Font.Bold = true;

        }
        
        public void AddOrderNumbersToFirstColumn(ExcelWorksheet sheet, int startRowIndex, int count)
        {
            for (var i = 0; i< count; i++)
            {
                sheet.Cells[i + startRowIndex, 1].Value = i + 1;
                sheet.Cells[i + startRowIndex, 1].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            }
        }

        protected void Save(ExcelPackage excelPackage, FileDto file)
        {
            // var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(30)).SetSize(1024);
            // _cache.Set(file.FileToken, excelPackage.GetAsByteArray(), cacheEntryOptions);
            //ghi file vao bo nho dem (cũ)
            _tempFileCacheManager.SetFile(file.FileToken,excelPackage.GetAsByteArray());
        }
    }
}