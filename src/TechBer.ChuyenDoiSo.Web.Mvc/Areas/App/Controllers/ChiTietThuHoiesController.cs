using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.ChiTietThuHoies;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThuHoies)]
    public class ChiTietThuHoiesController : ChuyenDoiSoControllerBase
    {
        private readonly IChiTietThuHoiesAppService _chiTietThuHoiesAppService;

        public ChiTietThuHoiesController(IChiTietThuHoiesAppService chiTietThuHoiesAppService)
        {
            _chiTietThuHoiesAppService = chiTietThuHoiesAppService;
        }

        public ActionResult Index()
        {
            var model = new ChiTietThuHoiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public class ChiTietThuHoiInput
        {
            public long? id { get; set; }
            public long? danhMucThuHoiId { get; set; }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThuHoies_Create, AppPermissions.Pages_ChiTietThuHoies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(ChiTietThuHoiInput input)
        {
            GetChiTietThuHoiForEditOutput getChiTietThuHoiForEditOutput;

            if (input.id.HasValue)
            {
                getChiTietThuHoiForEditOutput =
                    await _chiTietThuHoiesAppService.GetChiTietThuHoiForEdit(new EntityDto<long> {Id = (long) input.id});
            }
            else
            {
                getChiTietThuHoiForEditOutput = new GetChiTietThuHoiForEditOutput
                {
                    ChiTietThuHoi = new CreateOrEditChiTietThuHoiDto()
                };
                getChiTietThuHoiForEditOutput.ChiTietThuHoi.DanhMucThuHoiId = input.danhMucThuHoiId;
            }

            var viewModel = new CreateOrEditChiTietThuHoiModalViewModel()
            {
                ChiTietThuHoi = getChiTietThuHoiForEditOutput.ChiTietThuHoi,
                DanhMucThuHoiTen = getChiTietThuHoiForEditOutput.DanhMucThuHoiTen,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewChiTietThuHoiModal(long id)
        {
            var getChiTietThuHoiForViewDto = await _chiTietThuHoiesAppService.GetChiTietThuHoiForView(id);

            var model = new ChiTietThuHoiViewModel()
            {
                ChiTietThuHoi = getChiTietThuHoiForViewDto.ChiTietThuHoi,
                DanhMucThuHoiTen = getChiTietThuHoiForViewDto.DanhMucThuHoiTen
            };

            return PartialView("_ViewChiTietThuHoiModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ChiTietThuHoies_Create, AppPermissions.Pages_ChiTietThuHoies_Edit)]
        public PartialViewResult DanhMucThuHoiLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ChiTietThuHoiDanhMucThuHoiLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ChiTietThuHoiDanhMucThuHoiLookupTableModal", viewModel);
        }
    }
}