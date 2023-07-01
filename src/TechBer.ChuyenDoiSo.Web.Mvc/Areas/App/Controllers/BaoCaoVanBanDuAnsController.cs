using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.BaoCaoVanBanDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyetDinhs;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns)]
    public class BaoCaoVanBanDuAnsController : ChuyenDoiSoControllerBase
    {
        private const int MaxFileSize = 524288000; //500MB
        private readonly IBaoCaoVanBanDuAnsAppService _baoCaoVanBanDuAnsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly IRepository<BaoCaoVanBanDuAn> _baoCaoVanBanDuAnRepository;
        private readonly IWebHostEnvironment _appEnvironment;

        public BaoCaoVanBanDuAnsController(IBaoCaoVanBanDuAnsAppService baoCaoVanBanDuAnsAppService,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager,
            IWebHostEnvironment appEnvironment,
            IRepository<BaoCaoVanBanDuAn> baoCaoVanBanDuAnRepository
        )
        {
            _baoCaoVanBanDuAnsAppService = baoCaoVanBanDuAnsAppService;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
            _appEnvironment = appEnvironment;
            _baoCaoVanBanDuAnRepository = baoCaoVanBanDuAnRepository;
        }

        public ActionResult Index()
        {
            var model = new BaoCaoVanBanDuAnsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id, int vanBanDuAnId)
        {
            GetBaoCaoVanBanDuAnForEditOutput getBaoCaoVanBanDuAnForEditOutput;

            if (id.HasValue)
            {
                getBaoCaoVanBanDuAnForEditOutput =
                    await _baoCaoVanBanDuAnsAppService.GetBaoCaoVanBanDuAnForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getBaoCaoVanBanDuAnForEditOutput = new GetBaoCaoVanBanDuAnForEditOutput
                {
                    BaoCaoVanBanDuAn = new CreateOrEditBaoCaoVanBanDuAnDto()
                };
            }

            var viewModel = new CreateOrEditBaoCaoVanBanDuAnModalViewModel()
            {
                BaoCaoVanBanDuAn = getBaoCaoVanBanDuAnForEditOutput.BaoCaoVanBanDuAn,
                VanBanDuAnName = getBaoCaoVanBanDuAnForEditOutput.VanBanDuAnName,
                UserName = getBaoCaoVanBanDuAnForEditOutput.UserName,
                VanBanDuAnId = vanBanDuAnId,
                NguoiGuiId = AbpSession.UserId
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewBaoCaoVanBanDuAnModal(int id)
        {
            var getBaoCaoVanBanDuAnForViewDto = await _baoCaoVanBanDuAnsAppService.GetBaoCaoVanBanDuAnForView(id);

            var model = new BaoCaoVanBanDuAnViewModel()
            {
                BaoCaoVanBanDuAn = getBaoCaoVanBanDuAnForViewDto.BaoCaoVanBanDuAn,
                VanBanDuAnName = getBaoCaoVanBanDuAnForViewDto.VanBanDuAnName,
                UserName = getBaoCaoVanBanDuAnForViewDto.UserName
            };

            return PartialView("_ViewBaoCaoVanBanDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
        public PartialViewResult VanBanDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BaoCaoVanBanDuAnVanBanDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BaoCaoVanBanDuAnVanBanDuAnLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BaoCaoVanBanDuAnUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BaoCaoVanBanDuAnUserLookupTableModal", viewModel);
        }

        [DisableRequestSizeLimit]
        public UploadFileOutput UploadFileBaoCaoVanBanDuAn(FileDto input)
        {
            try
            {
                var files = Request.Form.Files.First();

                if (files == null)
                {
                    throw new UserFriendlyException("Không có file nào");
                }

                if (files.Length > MaxFileSize)
                {
                    throw new UserFriendlyException(L("FileUpload_Warn_SizeLimit",
                        AppConsts.MaxFileBytesUserFriendlyValue));
                }

                byte[] fileBytes;

                using (var stream = files.OpenReadStream())
                {
                    fileBytes = stream.GetAllBytes();
                }

                _tempFileCacheManager.SetFile(input.FileToken, fileBytes);

                return new UploadFileOutput
                {
                    FileToken = input.FileToken,
                    FileName = input.FileName,
                    FileType = input.FileType,
                };
            }
            catch (Exception ex)
            {
                return new UploadFileOutput(new ErrorInfo(ex.Message));
            }
        }

        public class FileMauSerializeObj
        {
            public string Guid { get; set; }
            public string FileName { get; set; }
            public string ContentType { get; set; }
        }

        private string DeleteAllTemporaryFile()
        {
            try
            {
                string tempfolder = _appEnvironment.WebRootPath + "/Common/FileTemp";
                System.IO.DirectoryInfo di = new DirectoryInfo(tempfolder);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                return "1";
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
        public async Task<PartialViewResult> ViewBaoCaoVanBanDuAnFileExcel(int id)
        {
            DeleteAllTemporaryFile();
            BaoCaoVanBanDuAn baoCaoVanBanDuAn = await _baoCaoVanBanDuAnRepository.FirstOrDefaultAsync(id);

            FileMauSerializeObj fileMauSerializeObj =
                JsonConvert.DeserializeObject<FileMauSerializeObj>(baoCaoVanBanDuAn.FileBaoCao);

            BinaryObject file = await _binaryObjectManager.GetOrNullAsync(Guid.Parse(fileMauSerializeObj.Guid));

            byte[] fileByte = file.Bytes;
            byte[] downloadFileBytes;
            using (Stream stream = new MemoryStream())
            {
                stream.Write(fileByte, 0, (int)fileByte.Length);
                try
                {
                    using (SpreadsheetDocument doc = SpreadsheetDocument.Open(stream, false))
                    {
                        WorkbookPart workbookPart = doc.WorkbookPart;
                        SharedStringTablePart sstpart = workbookPart.GetPartsOfType<SharedStringTablePart>().First();
                        SharedStringTable sst = sstpart.SharedStringTable;

                        WorksheetPart worksheetPart = workbookPart.WorksheetParts.First();
                        Worksheet sheet = worksheetPart.Worksheet;

                        var cells = sheet.Descendants<Cell>();
                        var rows = sheet.Descendants<Row>();

                        Console.WriteLine("Row count = {0}", rows.LongCount());
                        Console.WriteLine("Cell count = {0}", cells.LongCount());

                        // One way: go through each cell in the sheet
                        foreach (Cell cell in cells)
                        {
                            if ((cell.DataType != null) && (cell.DataType == CellValues.SharedString))
                            {
                                int ssid = int.Parse(cell.CellValue.Text);
                                string str = sst.ChildElements[ssid].InnerText;
                                Console.WriteLine("Shared string {0}: {1}", ssid, str);
                            }
                            else if (cell.CellValue != null)
                            {
                                Console.WriteLine("Cell contents: {0}", cell.CellValue.Text);
                            }
                        }

                        // Or... via each row
                        foreach (Row row in rows)
                        {
                            foreach (Cell c in row.Elements<Cell>())
                            {
                                if ((c.DataType != null) && (c.DataType == CellValues.SharedString))
                                {
                                    int ssid = int.Parse(c.CellValue.Text);
                                    string str = sst.ChildElements[ssid].InnerText;
                                    Console.WriteLine("Shared string {0}: {1}", ssid, str);
                                }
                                else if (c.CellValue != null)
                                {
                                    Console.WriteLine("Cell contents: {0}", c.CellValue.Text);
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                stream.Position = 0;
                downloadFileBytes = stream.GetAllBytes();
            }

            byte[] bytes = downloadFileBytes;
            FileStream fs =
                new FileStream(_appEnvironment.WebRootPath + "/Common/FileTemp/" + AbpSession.UserId + "BaoCaoVanBanDuAn.xlsx",
                    FileMode.OpenOrCreate);
            fs.Write(bytes, 0, bytes.Length);
            fs.Close();
            var model = new BaoCaoVanBanDuAnFileExcelViewModel()
            {
                FileName = AbpSession.UserId + "BaoCaoVanBanDuAn.xlsx"
            };
            return PartialView("_ViewBaoCaoVanBanDuAnFileExcel", model);
        }
    }
}