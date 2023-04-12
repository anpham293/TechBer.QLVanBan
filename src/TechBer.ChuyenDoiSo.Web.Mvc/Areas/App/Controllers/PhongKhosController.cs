using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.PhongKhos;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_PhongKhos)]
    public class PhongKhosController : ChuyenDoiSoControllerBase
    {
        private readonly IPhongKhosAppService _phongKhosAppService;

        public PhongKhosController(IPhongKhosAppService phongKhosAppService)
        {
            _phongKhosAppService = phongKhosAppService;
        }

        public ActionResult Index()
        {
            var model = new PhongKhosViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_PhongKhos_Create, AppPermissions.Pages_PhongKhos_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetPhongKhoForEditOutput getPhongKhoForEditOutput;

				if (id.HasValue){
					getPhongKhoForEditOutput = await _phongKhosAppService.GetPhongKhoForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getPhongKhoForEditOutput = new GetPhongKhoForEditOutput{
						PhongKho = new CreateOrEditPhongKhoDto()
					};
				}

				var viewModel = new CreateOrEditPhongKhoModalViewModel()
				{
					PhongKho = getPhongKhoForEditOutput.PhongKho,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewPhongKhoModal(int id)
        {
			var getPhongKhoForViewDto = await _phongKhosAppService.GetPhongKhoForView(id);

            var model = new PhongKhoViewModel()
            {
                PhongKho = getPhongKhoForViewDto.PhongKho
            };

            return PartialView("_ViewPhongKhoModal", model);
        }


    }
}