using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.SdtZalos;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLySdtZalo;
using TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_SdtZalos)]
    public class SdtZalosController : ChuyenDoiSoControllerBase
    {
        private readonly ISdtZalosAppService _sdtZalosAppService;

        public SdtZalosController(ISdtZalosAppService sdtZalosAppService)
        {
            _sdtZalosAppService = sdtZalosAppService;
        }

        public ActionResult Index()
        {
            var model = new SdtZalosViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_SdtZalos_Create, AppPermissions.Pages_SdtZalos_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetSdtZaloForEditOutput getSdtZaloForEditOutput;

				if (id.HasValue){
					getSdtZaloForEditOutput = await _sdtZalosAppService.GetSdtZaloForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getSdtZaloForEditOutput = new GetSdtZaloForEditOutput{
						SdtZalo = new CreateOrEditSdtZaloDto()
					};
				}

				var viewModel = new CreateOrEditSdtZaloModalViewModel()
				{
					SdtZalo = getSdtZaloForEditOutput.SdtZalo,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewSdtZaloModal(long id)
        {
			var getSdtZaloForViewDto = await _sdtZalosAppService.GetSdtZaloForView(id);

            var model = new SdtZaloViewModel()
            {
                SdtZalo = getSdtZaloForViewDto.SdtZalo
            };

            return PartialView("_ViewSdtZaloModal", model);
        }


    }
}