using System;
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
using Abp.Extensions;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;

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

        public BaoCaoVanBanDuAnsController(IBaoCaoVanBanDuAnsAppService baoCaoVanBanDuAnsAppService,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager
        )
        {
            _baoCaoVanBanDuAnsAppService = baoCaoVanBanDuAnsAppService;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
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
    }
}