using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.ThungHoSos;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_ThungHoSos)]
    public class ThungHoSosController : ChuyenDoiSoControllerBase
    {
        private readonly IThungHoSosAppService _thungHoSosAppService;

        public ThungHoSosController(IThungHoSosAppService thungHoSosAppService)
        {
            _thungHoSosAppService = thungHoSosAppService;
        }

        public ActionResult Index()
        {
            var model = new ThungHoSosViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_ThungHoSos_Create, AppPermissions.Pages_ThungHoSos_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetThungHoSoForEditOutput getThungHoSoForEditOutput;

				if (id.HasValue){
					getThungHoSoForEditOutput = await _thungHoSosAppService.GetThungHoSoForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getThungHoSoForEditOutput = new GetThungHoSoForEditOutput{
						ThungHoSo = new CreateOrEditThungHoSoDto()
					};
				}

				var viewModel = new CreateOrEditThungHoSoModalViewModel()
				{
					ThungHoSo = getThungHoSoForEditOutput.ThungHoSo,
					DayKeMaSo = getThungHoSoForEditOutput.DayKeMaSo,
					DuAnName = getThungHoSoForEditOutput.DuAnName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewThungHoSoModal(int id)
        {
			var getThungHoSoForViewDto = await _thungHoSosAppService.GetThungHoSoForView(id);

            var model = new ThungHoSoViewModel()
            {
                ThungHoSo = getThungHoSoForViewDto.ThungHoSo
                , DayKeMaSo = getThungHoSoForViewDto.DayKeMaSo 

                , DuAnName = getThungHoSoForViewDto.DuAnName 

            };

            return PartialView("_ViewThungHoSoModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_ThungHoSos_Create, AppPermissions.Pages_ThungHoSos_Edit)]
        public PartialViewResult DayKeLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ThungHoSoDayKeLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ThungHoSoDayKeLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_ThungHoSos_Create, AppPermissions.Pages_ThungHoSos_Edit)]
        public PartialViewResult DuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new ThungHoSoDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_ThungHoSoDuAnLookupTableModal", viewModel);
        }

    }
}