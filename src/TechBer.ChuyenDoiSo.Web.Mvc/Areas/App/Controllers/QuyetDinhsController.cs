using System;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyetDinhs;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;
using UploadFileOutput = TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos.UploadFileOutput;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuyetDinhs)]
    public class QuyetDinhsController : ChuyenDoiSoControllerBase
    {
        private const int MaxFileSize = 524288000; //500MB
        private readonly IQuyetDinhsAppService _quyetDinhsAppService;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public QuyetDinhsController(IQuyetDinhsAppService quyetDinhsAppService,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager)
        {
            _quyetDinhsAppService = quyetDinhsAppService;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
        }

        public ActionResult Index()
        {
            var model = new QuyetDinhsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_QuyetDinhs_Create, AppPermissions.Pages_QuyetDinhs_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetQuyetDinhForEditOutput getQuyetDinhForEditOutput;

            if (id.HasValue)
            {
                getQuyetDinhForEditOutput =
                    await _quyetDinhsAppService.GetQuyetDinhForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getQuyetDinhForEditOutput = new GetQuyetDinhForEditOutput
                {
                    QuyetDinh = new CreateOrEditQuyetDinhDto()
                };
                getQuyetDinhForEditOutput.QuyetDinh.NgayBanHanh = DateTime.Now;
            }

            var viewModel = new CreateOrEditQuyetDinhModalViewModel()
            {
                QuyetDinh = getQuyetDinhForEditOutput.QuyetDinh,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewQuyetDinhModal(int id)
        {
            var getQuyetDinhForViewDto = await _quyetDinhsAppService.GetQuyetDinhForView(id);

            var model = new QuyetDinhViewModel()
            {
                QuyetDinh = getQuyetDinhForViewDto.QuyetDinh
            };

            return PartialView("_ViewQuyetDinhModal", model);
        }
        
        [DisableRequestSizeLimit]
        public UploadFileOutput UploadFileQuyetDinh(FileDto input)
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
    }
}