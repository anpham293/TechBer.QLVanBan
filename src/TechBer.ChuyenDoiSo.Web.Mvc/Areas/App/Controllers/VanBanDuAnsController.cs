using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.VanBanDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_VanBanDuAns)]
    public class VanBanDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly IVanBanDuAnsAppService _vanBanDuAnsAppService;

        public VanBanDuAnsController(IVanBanDuAnsAppService vanBanDuAnsAppService)
        {
            _vanBanDuAnsAppService = vanBanDuAnsAppService;
        }

        public ActionResult Index()
        {
            var model = new VanBanDuAnsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_VanBanDuAns_Create, AppPermissions.Pages_VanBanDuAns_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetVanBanDuAnForEditOutput getVanBanDuAnForEditOutput;

				if (id.HasValue){
					getVanBanDuAnForEditOutput = await _vanBanDuAnsAppService.GetVanBanDuAnForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getVanBanDuAnForEditOutput = new GetVanBanDuAnForEditOutput{
						VanBanDuAn = new CreateOrEditVanBanDuAnDto()
					};
				getVanBanDuAnForEditOutput.VanBanDuAn.NgayBanHanh = DateTime.Now;
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
                VanBanDuAn = getVanBanDuAnForViewDto.VanBanDuAn
                , DuAnName = getVanBanDuAnForViewDto.DuAnName 

                , QuyTrinhDuAnName = getVanBanDuAnForViewDto.QuyTrinhDuAnName 

            };

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

    }
}