using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.BaoCaoVanBanDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns)]
    public class BaoCaoVanBanDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly IBaoCaoVanBanDuAnsAppService _baoCaoVanBanDuAnsAppService;

        public BaoCaoVanBanDuAnsController(IBaoCaoVanBanDuAnsAppService baoCaoVanBanDuAnsAppService)
        {
            _baoCaoVanBanDuAnsAppService = baoCaoVanBanDuAnsAppService;
        }

        public ActionResult Index()
        {
            var model = new BaoCaoVanBanDuAnsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetBaoCaoVanBanDuAnForEditOutput getBaoCaoVanBanDuAnForEditOutput;

				if (id.HasValue){
					getBaoCaoVanBanDuAnForEditOutput = await _baoCaoVanBanDuAnsAppService.GetBaoCaoVanBanDuAnForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getBaoCaoVanBanDuAnForEditOutput = new GetBaoCaoVanBanDuAnForEditOutput{
						BaoCaoVanBanDuAn = new CreateOrEditBaoCaoVanBanDuAnDto()
					};
				}

				var viewModel = new CreateOrEditBaoCaoVanBanDuAnModalViewModel()
				{
					BaoCaoVanBanDuAn = getBaoCaoVanBanDuAnForEditOutput.BaoCaoVanBanDuAn,
					VanBanDuAnName = getBaoCaoVanBanDuAnForEditOutput.VanBanDuAnName,
					UserName = getBaoCaoVanBanDuAnForEditOutput.UserName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewBaoCaoVanBanDuAnModal(int id)
        {
			var getBaoCaoVanBanDuAnForViewDto = await _baoCaoVanBanDuAnsAppService.GetBaoCaoVanBanDuAnForView(id);

            var model = new BaoCaoVanBanDuAnViewModel()
            {
                BaoCaoVanBanDuAn = getBaoCaoVanBanDuAnForViewDto.BaoCaoVanBanDuAn
                , VanBanDuAnName = getBaoCaoVanBanDuAnForViewDto.VanBanDuAnName 

                , UserName = getBaoCaoVanBanDuAnForViewDto.UserName 

            };

            return PartialView("_ViewBaoCaoVanBanDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
        public PartialViewResult VanBanDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new BaoCaoVanBanDuAnVanBanDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BaoCaoVanBanDuAnVanBanDuAnLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_BaoCaoVanBanDuAns_Create, AppPermissions.Pages_BaoCaoVanBanDuAns_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new BaoCaoVanBanDuAnUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_BaoCaoVanBanDuAnUserLookupTableModal", viewModel);
        }

    }
}