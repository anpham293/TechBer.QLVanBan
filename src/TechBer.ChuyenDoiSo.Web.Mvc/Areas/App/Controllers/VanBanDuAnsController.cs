using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.VanBanDuAns;
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
using Newtonsoft.Json;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.Storage;
using UploadFileOutput = TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos.UploadFileOutput;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_VanBanDuAns)]
    public class VanBanDuAnsController : ChuyenDoiSoControllerBase
    {
        private const int MaxFileSize = 52428800; //5MB
        private readonly IVanBanDuAnsAppService _vanBanDuAnsAppService;
        private readonly IRepository<DuAn> _duAnRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public VanBanDuAnsController(IVanBanDuAnsAppService vanBanDuAnsAppService,
            IRepository<DuAn> duAnRepository,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager
        )
        {
            _vanBanDuAnsAppService = vanBanDuAnsAppService;
            _duAnRepository = duAnRepository;
            _tempFileCacheManager = tempFileCacheManager;
            _binaryObjectManager = binaryObjectManager;
        }

        public ActionResult Index(int Duanid)
        {
            DuAn t = _duAnRepository.FirstOrDefault(Duanid);
            if (t == null)
            {
                return RedirectToAction("Index", "DuAns");
            }

            var model = new VanBanDuAnsViewModel
            {
                FilterText = "",
                DuAnId = t,
                ListDuAn = ObjectMapper.Map<List<DuAnDto>>(_duAnRepository.GetAllList())
            };

            return View(model);
        }

        public class CreateOrEditModalInput
        {
            public int? id { get; set; }
            public int duanid { get; set; }
            public int quytrinhid { get; set; }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VanBanDuAns_Create, AppPermissions.Pages_VanBanDuAns_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(CreateOrEditModalInput input)
        {
            GetVanBanDuAnForEditOutput getVanBanDuAnForEditOutput;

            if (input.id.HasValue)
            {
                getVanBanDuAnForEditOutput =
                    await _vanBanDuAnsAppService.GetVanBanDuAnForEdit(new EntityDto {Id = (int) input.id});
            }
            else
            {
                getVanBanDuAnForEditOutput = new GetVanBanDuAnForEditOutput
                {
                    VanBanDuAn = new CreateOrEditVanBanDuAnDto()
                };
                getVanBanDuAnForEditOutput.VanBanDuAn.NgayBanHanh = DateTime.Now;
                getVanBanDuAnForEditOutput.VanBanDuAn.QuyTrinhDuAnId = input.quytrinhid;
                getVanBanDuAnForEditOutput.VanBanDuAn.DuAnId = input.duanid;
            }

            var viewModel = new CreateOrEditVanBanDuAnModalViewModel()
            {
                VanBanDuAn = getVanBanDuAnForEditOutput.VanBanDuAn,
                DuAnName = getVanBanDuAnForEditOutput.DuAnName,
                QuyTrinhDuAnName = getVanBanDuAnForEditOutput.QuyTrinhDuAnName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewVanBanDuAnModal(int id)
        {
            var getVanBanDuAnForViewDto = await _vanBanDuAnsAppService.GetVanBanDuAnForView(id);

            var model = new VanBanDuAnViewModel()
            {
                VanBanDuAn = getVanBanDuAnForViewDto.VanBanDuAn, DuAnName = getVanBanDuAnForViewDto.DuAnName,
                QuyTrinhDuAnName = getVanBanDuAnForViewDto.QuyTrinhDuAnName
            };
            var file = JsonConvert.DeserializeObject<FileMauSerializeObj>(getVanBanDuAnForViewDto.VanBanDuAn.FileVanBan);
            
            model.guid = file.Guid;
            model.contentType = file.ContentType;
            model.fileName = file.FileName;
            return PartialView("_ViewVanBanDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VanBanDuAns_Create, AppPermissions.Pages_VanBanDuAns_Edit)]
        public PartialViewResult DuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new VanBanDuAnDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_VanBanDuAnDuAnLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_VanBanDuAns_Create, AppPermissions.Pages_VanBanDuAns_Edit)]
        public PartialViewResult QuyTrinhDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new VanBanDuAnQuyTrinhDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_VanBanDuAnQuyTrinhDuAnLookupTableModal", viewModel);
        }

        public UploadFileOutput UploadFileHopDong(FileDto input)
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

        [HttpGet]
        public async Task<ActionResult> Download(string guid, string contentType)
        {
            var binObj = await _binaryObjectManager.GetOrNullAsync(Guid.Parse(guid));
            var content = binObj.Bytes;
            return File(content, contentType);
        }
    }
}