using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.ChuyenHoSoGiaies;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies)]
    public class ChuyenHoSoGiaiesController : ChuyenDoiSoControllerBase
    {
        private readonly IChuyenHoSoGiaiesAppService _chuyenHoSoGiaiesAppService;

        public ChuyenHoSoGiaiesController(IChuyenHoSoGiaiesAppService chuyenHoSoGiaiesAppService)
        {
            _chuyenHoSoGiaiesAppService = chuyenHoSoGiaiesAppService;
        }

        public ActionResult Index()
        {
            var model = new ChuyenHoSoGiaiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Create, AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id, int vanBanDuAnId)
        {
            GetChuyenHoSoGiayForEditOutput getChuyenHoSoGiayForEditOutput;

            if (id.HasValue)
            {
                getChuyenHoSoGiayForEditOutput =
                    await _chuyenHoSoGiaiesAppService.GetChuyenHoSoGiayForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getChuyenHoSoGiayForEditOutput = new GetChuyenHoSoGiayForEditOutput
                {
                    ChuyenHoSoGiay = new CreateOrEditChuyenHoSoGiayDto()
                };
                getChuyenHoSoGiayForEditOutput.ChuyenHoSoGiay.ThoiGianChuyen = DateTime.Now;
                getChuyenHoSoGiayForEditOutput.ChuyenHoSoGiay.ThoiGianNhan = DateTime.Now;
            }

            var viewModel = new CreateOrEditChuyenHoSoGiayModalViewModel()
            {
                ChuyenHoSoGiay = getChuyenHoSoGiayForEditOutput.ChuyenHoSoGiay,
                VanBanDuAnName = getChuyenHoSoGiayForEditOutput.VanBanDuAnName,
                UserName = getChuyenHoSoGiayForEditOutput.UserName,
                VanBanDuAnId = vanBanDuAnId
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewChuyenHoSoGiayModal(int id)
        {
            var getChuyenHoSoGiayForViewDto = await _chuyenHoSoGiaiesAppService.GetChuyenHoSoGiayForView(id);

            var model = new ChuyenHoSoGiayViewModel()
            {
                ChuyenHoSoGiay = getChuyenHoSoGiayForViewDto.ChuyenHoSoGiay,
                VanBanDuAnName = getChuyenHoSoGiayForViewDto.VanBanDuAnName,
                UserName = getChuyenHoSoGiayForViewDto.UserName,
                TenNguoiChuyen = getChuyenHoSoGiayForViewDto.TenNguoiChuyen,
                TenNguoiNhan = getChuyenHoSoGiayForViewDto.TenNguoiNhan,
                QuyTrinhDuAnName = getChuyenHoSoGiayForViewDto.QuyTrinhDuAnName,
                DuAnName = getChuyenHoSoGiayForViewDto.DuAnName
            };

            return PartialView("_ViewChuyenHoSoGiayModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Create, AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
        public PartialViewResult VanBanDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ChuyenHoSoGiayVanBanDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ChuyenHoSoGiayVanBanDuAnLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ChuyenHoSoGiaies_Create, AppPermissions.Pages_ChuyenHoSoGiaies_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new ChuyenHoSoGiayUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ChuyenHoSoGiayUserLookupTableModal", viewModel);
        }

        public async Task<PartialViewResult> ChuyenHoSoGiayVanBanDuAn(int vanBanDuAnId)
        {
            var viewModel = new ChuyenHoSoGiaiesViewModel
            {
                FilterText = "",
                VanBanDuAnId = vanBanDuAnId
            };

            return PartialView("_ViewChuyenHoSoGiayVanBanDuAn", viewModel);
        }
    }
}