using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.CapQuanLies;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CapQuanLies)]
    public class CapQuanLiesController : ChuyenDoiSoControllerBase
    {
        private readonly ICapQuanLiesAppService _capQuanLiesAppService;

        public CapQuanLiesController(ICapQuanLiesAppService capQuanLiesAppService)
        {
            _capQuanLiesAppService = capQuanLiesAppService;
        }

        public ActionResult Index()
        {
            var model = new CapQuanLiesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_CapQuanLies_Create, AppPermissions.Pages_CapQuanLies_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetCapQuanLyForEditOutput getCapQuanLyForEditOutput;

				if (id.HasValue){
					getCapQuanLyForEditOutput = await _capQuanLiesAppService.GetCapQuanLyForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getCapQuanLyForEditOutput = new GetCapQuanLyForEditOutput{
						CapQuanLy = new CreateOrEditCapQuanLyDto()
					};
				}

				var viewModel = new CreateOrEditCapQuanLyModalViewModel()
				{
					CapQuanLy = getCapQuanLyForEditOutput.CapQuanLy,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewCapQuanLyModal(int id)
        {
			var getCapQuanLyForViewDto = await _capQuanLiesAppService.GetCapQuanLyForView(id);

            var model = new CapQuanLyViewModel()
            {
                CapQuanLy = getCapQuanLyForViewDto.CapQuanLy
            };

            return PartialView("_ViewCapQuanLyModal", model);
        }


    }
}