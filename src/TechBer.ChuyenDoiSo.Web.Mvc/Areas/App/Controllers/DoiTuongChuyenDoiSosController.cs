using Abp.Application.Services.Dto;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.IO.Extensions;
using Abp.UI;
using Abp.Web.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Storage;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.DoiTuongChuyenDoiSos;
using TechBer.ChuyenDoiSo.Web.Controllers;
using UploadFileOutput = TechBer.ChuyenDoiSo.Dto.UploadFileOutput;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos)]
    public class DoiTuongChuyenDoiSosController : ChuyenDoiSoControllerBase
    {
        private readonly IDoiTuongChuyenDoiSosAppService _doiTuongChuyenDoiSosAppService;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private const int MaxFileSize = 5242880; //5MB

        public DoiTuongChuyenDoiSosController(
            IDoiTuongChuyenDoiSosAppService doiTuongChuyenDoiSosAppService, 
            IBinaryObjectManager binaryObjectManager,
            ITempFileCacheManager tempFileCacheManager)
        {
            _doiTuongChuyenDoiSosAppService = doiTuongChuyenDoiSosAppService;
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

        public ActionResult Index()
        {
            var model = new DoiTuongChuyenDoiSosViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       
		[AbpMvcAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_Create, AppPermissions.Pages_DoiTuongChuyenDoiSos_Edit)]
		public async Task<PartialViewResult> CreateOrEditModal(int? id)
		{
			GetDoiTuongChuyenDoiSoForEditOutput getDoiTuongChuyenDoiSoForEditOutput;

			if (id.HasValue){
				getDoiTuongChuyenDoiSoForEditOutput = await _doiTuongChuyenDoiSosAppService.GetDoiTuongChuyenDoiSoForEdit(new EntityDto { Id = (int) id });
			}
			else {
				getDoiTuongChuyenDoiSoForEditOutput = new GetDoiTuongChuyenDoiSoForEditOutput{
					DoiTuongChuyenDoiSo = new CreateOrEditDoiTuongChuyenDoiSoDto()
				};
			}

			var viewModel = new CreateOrEditDoiTuongChuyenDoiSoModalViewModel()
			{
				DoiTuongChuyenDoiSo = getDoiTuongChuyenDoiSoForEditOutput.DoiTuongChuyenDoiSo,
				UserName = getDoiTuongChuyenDoiSoForEditOutput.UserName,
				DoiTuongChuyenDoiSoUserList = await _doiTuongChuyenDoiSosAppService.GetAllUserForTableDropdown(),                
			};

			return PartialView("_CreateOrEditModal", viewModel);
		}
			

        public async Task<PartialViewResult> ViewDoiTuongChuyenDoiSoModal(int id)
        {
			var getDoiTuongChuyenDoiSoForViewDto = await _doiTuongChuyenDoiSosAppService.GetDoiTuongChuyenDoiSoForView(id);

            var model = new DoiTuongChuyenDoiSoViewModel()
            {
                DoiTuongChuyenDoiSo = getDoiTuongChuyenDoiSoForViewDto.DoiTuongChuyenDoiSo
                , UserName = getDoiTuongChuyenDoiSoForViewDto.UserName 

            };

            return PartialView("_ViewDoiTuongChuyenDoiSoModal", model);
        }

        public async Task<PartialViewResult> EditChiTietDanhGiaModal(int id)
        {
            var chiTietDanhGia = await _doiTuongChuyenDoiSosAppService.GetChiTietDanhGiaForEdit(id);

            var model = new EditChiTietDanhGiaModel();
            model.Description = chiTietDanhGia.Description;
            model.DiemDatDuoc = chiTietDanhGia.DiemDatDuoc;
            model.DiemHoiDongThamDinh = chiTietDanhGia.DiemHoiDongThamDinh;
            model.DiemTuDanhGia = chiTietDanhGia.DiemTuDanhGia;
            model.SoLieuKeKhai = chiTietDanhGia.SoLieuKeKhai;
            model.Id = chiTietDanhGia.Id;
            model.Name = chiTietDanhGia.Name;
            model.SoThuTu = chiTietDanhGia.SoThuTu;
            model.PhuongThucDanhGia = chiTietDanhGia.PhuongThucDanhGia;
            model.DiemToiDa = chiTietDanhGia.DiemToiDa;

            return PartialView("_EditChiTietDanhGiaModal", model);
        }

        [HttpGet]
        public async Task<ActionResult> Download(string guid, string contentType)
        {
            var binObj = await _binaryObjectManager.GetOrNullAsync(Guid.Parse(guid));
            var content = binObj.Bytes;
            return File(content, contentType);
        }

        [HttpGet]
        public async Task<ActionResult> TinhDiem(int id)
        {
            var doiTuong = await _doiTuongChuyenDoiSosAppService.GetDoiTuongChuyenDoiSoForView(id);

            var model = new TinhDiemModel()
            {
                IdDoiTuong = id,
                ChamDiemFlag = doiTuong.DoiTuongChuyenDoiSo.ChamDiemFlag
            };

            return View(model);
        }

        public UploadFileOutput UploadSoLieuThongKe(FileDto input)
        {
            try
            {
                var files = Request.Form.Files.First();

                if(files == null)
                {
                    throw new UserFriendlyException("Không có file nào");
                }

                if(files.Length > MaxFileSize)
                {
                    throw new UserFriendlyException(L("FileUpload_Warn_SizeLimit", AppConsts.MaxFileBytesUserFriendlyValue));
                }

                byte[] fileBytes;

                using(var stream = files.OpenReadStream())
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
            }catch(Exception ex)
            {
                return new UploadFileOutput(new ErrorInfo(ex.Message));
            }
        }
    }
}