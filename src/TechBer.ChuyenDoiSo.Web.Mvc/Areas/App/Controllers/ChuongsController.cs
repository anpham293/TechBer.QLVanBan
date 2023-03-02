using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.Chuongs;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_Chuongs)]
    public class ChuongsController : ChuyenDoiSoControllerBase
    {
        private readonly IChuongsAppService _chuongsAppService;

        public ChuongsController(IChuongsAppService chuongsAppService)
        {
            _chuongsAppService = chuongsAppService;
        }

        public ActionResult Index()
        {
            var model = new ChuongsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_Chuongs_Create, AppPermissions.Pages_Chuongs_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetChuongForEditOutput getChuongForEditOutput;

				if (id.HasValue){
					getChuongForEditOutput = await _chuongsAppService.GetChuongForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getChuongForEditOutput = new GetChuongForEditOutput{
						Chuong = new CreateOrEditChuongDto()
					};
				}

				var viewModel = new CreateOrEditChuongModalViewModel()
				{
					Chuong = getChuongForEditOutput.Chuong,
					CapQuanLyTen = getChuongForEditOutput.CapQuanLyTen,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewChuongModal(int id)
        {
			var getChuongForViewDto = await _chuongsAppService.GetChuongForView(id);

            var model = new ChuongViewModel()
            {
                Chuong = getChuongForViewDto.Chuong
                , CapQuanLyTen = getChuongForViewDto.CapQuanLyTen 

            };

            return PartialView("_ViewChuongModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Chuongs_Create, AppPermissions.Pages_Chuongs_Edit)]
        public PartialViewResult CapQuanLyLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ChuongCapQuanLyLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ChuongCapQuanLyLookupTableModal", viewModel);
        }

    }
}