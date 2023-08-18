using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.UserDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_UserDuAns)]
    public class UserDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly IUserDuAnsAppService _userDuAnsAppService;

        public UserDuAnsController(IUserDuAnsAppService userDuAnsAppService)
        {
            _userDuAnsAppService = userDuAnsAppService;
        }

        public ActionResult Index()
        {
            var model = new UserDuAnsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_UserDuAns_Create, AppPermissions.Pages_UserDuAns_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetUserDuAnForEditOutput getUserDuAnForEditOutput;

				if (id.HasValue){
					getUserDuAnForEditOutput = await _userDuAnsAppService.GetUserDuAnForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getUserDuAnForEditOutput = new GetUserDuAnForEditOutput{
						UserDuAn = new CreateOrEditUserDuAnDto()
					};
				}

				var viewModel = new CreateOrEditUserDuAnModalViewModel()
				{
					UserDuAn = getUserDuAnForEditOutput.UserDuAn,
					UserName = getUserDuAnForEditOutput.UserName,
					DuAnName = getUserDuAnForEditOutput.DuAnName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewUserDuAnModal(long id)
        {
			var getUserDuAnForViewDto = await _userDuAnsAppService.GetUserDuAnForView(id);

            var model = new UserDuAnViewModel()
            {
                UserDuAn = getUserDuAnForViewDto.UserDuAn
                , UserName = getUserDuAnForViewDto.UserName 

                , DuAnName = getUserDuAnForViewDto.DuAnName 

            };

            return PartialView("_ViewUserDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_UserDuAns_Create, AppPermissions.Pages_UserDuAns_Edit)]
        public PartialViewResult UserLookupTableModal(long? id, string displayName)
        {
            var viewModel = new UserDuAnUserLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserDuAnUserLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_UserDuAns_Create, AppPermissions.Pages_UserDuAns_Edit)]
        public PartialViewResult DuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new UserDuAnDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_UserDuAnDuAnLookupTableModal", viewModel);
        }

    }
}