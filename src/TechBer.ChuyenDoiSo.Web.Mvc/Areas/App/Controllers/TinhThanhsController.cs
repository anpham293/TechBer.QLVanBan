using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.TinhThanhs;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TinhThanhs)]
    public class TinhThanhsController : ChuyenDoiSoControllerBase
    {
        private readonly ITinhThanhsAppService _tinhThanhsAppService;

        public TinhThanhsController(ITinhThanhsAppService tinhThanhsAppService)
        {
            _tinhThanhsAppService = tinhThanhsAppService;
        }

        public ActionResult Index()
        {
            var model = new TinhThanhsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_TinhThanhs_Create, AppPermissions.Pages_TinhThanhs_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetTinhThanhForEditOutput getTinhThanhForEditOutput;

				if (id.HasValue){
					getTinhThanhForEditOutput = await _tinhThanhsAppService.GetTinhThanhForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getTinhThanhForEditOutput = new GetTinhThanhForEditOutput{
						TinhThanh = new CreateOrEditTinhThanhDto()
					};
				}

				var viewModel = new CreateOrEditTinhThanhModalViewModel()
				{
					TinhThanh = getTinhThanhForEditOutput.TinhThanh,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewTinhThanhModal(int id)
        {
			var getTinhThanhForViewDto = await _tinhThanhsAppService.GetTinhThanhForView(id);

            var model = new TinhThanhViewModel()
            {
                TinhThanh = getTinhThanhForViewDto.TinhThanh
            };

            return PartialView("_ViewTinhThanhModal", model);
        }


    }
}