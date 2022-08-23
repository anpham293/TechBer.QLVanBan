using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.LoaiDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LoaiDuAns)]
    public class LoaiDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly ILoaiDuAnsAppService _loaiDuAnsAppService;

        public LoaiDuAnsController(ILoaiDuAnsAppService loaiDuAnsAppService)
        {
            _loaiDuAnsAppService = loaiDuAnsAppService;
        }

        public ActionResult Index()
        {
            var model = new LoaiDuAnsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_LoaiDuAns_Create, AppPermissions.Pages_LoaiDuAns_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetLoaiDuAnForEditOutput getLoaiDuAnForEditOutput;

				if (id.HasValue){
					getLoaiDuAnForEditOutput = await _loaiDuAnsAppService.GetLoaiDuAnForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getLoaiDuAnForEditOutput = new GetLoaiDuAnForEditOutput{
						LoaiDuAn = new CreateOrEditLoaiDuAnDto()
					};
				}

				var viewModel = new CreateOrEditLoaiDuAnModalViewModel()
				{
					LoaiDuAn = getLoaiDuAnForEditOutput.LoaiDuAn,
					OrganizationUnitDisplayName = getLoaiDuAnForEditOutput.OrganizationUnitDisplayName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewLoaiDuAnModal(int id)
        {
			var getLoaiDuAnForViewDto = await _loaiDuAnsAppService.GetLoaiDuAnForView(id);

            var model = new LoaiDuAnViewModel()
            {
                LoaiDuAn = getLoaiDuAnForViewDto.LoaiDuAn
                , OrganizationUnitDisplayName = getLoaiDuAnForViewDto.OrganizationUnitDisplayName 

            };

            return PartialView("_ViewLoaiDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_LoaiDuAns_Create, AppPermissions.Pages_LoaiDuAns_Edit)]
        public PartialViewResult OrganizationUnitLookupTableModal(long? id, string displayName)
        {
            var viewModel = new LoaiDuAnOrganizationUnitLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_LoaiDuAnOrganizationUnitLookupTableModal", viewModel);
        }

    }
}