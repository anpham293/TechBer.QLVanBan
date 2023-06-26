using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.TraoDoiVanBanDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns)]
    public class TraoDoiVanBanDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly ITraoDoiVanBanDuAnsAppService _traoDoiVanBanDuAnsAppService;

        public TraoDoiVanBanDuAnsController(ITraoDoiVanBanDuAnsAppService traoDoiVanBanDuAnsAppService)
        {
            _traoDoiVanBanDuAnsAppService = traoDoiVanBanDuAnsAppService;
        }

        public ActionResult Index()
        {
            var model = new TraoDoiVanBanDuAnsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Create, AppPermissions.Pages_TraoDoiVanBanDuAns_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetTraoDoiVanBanDuAnForEditOutput getTraoDoiVanBanDuAnForEditOutput;

				if (id.HasValue){
					getTraoDoiVanBanDuAnForEditOutput = await _traoDoiVanBanDuAnsAppService.GetTraoDoiVanBanDuAnForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getTraoDoiVanBanDuAnForEditOutput = new GetTraoDoiVanBanDuAnForEditOutput{
						TraoDoiVanBanDuAn = new CreateOrEditTraoDoiVanBanDuAnDto()
					};
				getTraoDoiVanBanDuAnForEditOutput.TraoDoiVanBanDuAn.NgayGui = DateTime.Now;
				}

				var viewModel = new CreateOrEditTraoDoiVanBanDuAnModalViewModel()
				{
					TraoDoiVanBanDuAn = getTraoDoiVanBanDuAnForEditOutput.TraoDoiVanBanDuAn,
					UserName = getTraoDoiVanBanDuAnForEditOutput.UserName,
					VanBanDuAnName = getTraoDoiVanBanDuAnForEditOutput.VanBanDuAnName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewTraoDoiVanBanDuAnModal(int id)
        {
			var getTraoDoiVanBanDuAnForViewDto = await _traoDoiVanBanDuAnsAppService.GetTraoDoiVanBanDuAnForView(id);

            var model = new TraoDoiVanBanDuAnViewModel()
            {
                TraoDoiVanBanDuAn = getTraoDoiVanBanDuAnForViewDto.TraoDoiVanBanDuAn
                , UserName = getTraoDoiVanBanDuAnForViewDto.UserName 

                , VanBanDuAnName = getTraoDoiVanBanDuAnForViewDto.VanBanDuAnName 

            };

            return PartialView("_ViewTraoDoiVanBanDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Create, AppPermissions.Pages_TraoDoiVanBanDuAns_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new TraoDoiVanBanDuAnUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TraoDoiVanBanDuAnUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_TraoDoiVanBanDuAns_Create, AppPermissions.Pages_TraoDoiVanBanDuAns_Edit)]
        public PartialViewResult VanBanDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new TraoDoiVanBanDuAnVanBanDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_TraoDoiVanBanDuAnVanBanDuAnLookupTableModal", viewModel);
        }

    }
}