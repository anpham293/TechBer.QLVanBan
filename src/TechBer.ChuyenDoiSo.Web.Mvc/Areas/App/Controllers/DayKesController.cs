using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.DayKes;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DayKes)]
    public class DayKesController : ChuyenDoiSoControllerBase
    {
        private readonly IDayKesAppService _dayKesAppService;

        public DayKesController(IDayKesAppService dayKesAppService)
        {
            _dayKesAppService = dayKesAppService;
        }

        public ActionResult Index()
        {
            var model = new DayKesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_DayKes_Create, AppPermissions.Pages_DayKes_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetDayKeForEditOutput getDayKeForEditOutput;

				if (id.HasValue){
					getDayKeForEditOutput = await _dayKesAppService.GetDayKeForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getDayKeForEditOutput = new GetDayKeForEditOutput{
						DayKe = new CreateOrEditDayKeDto()
					};
				}

				var viewModel = new CreateOrEditDayKeModalViewModel()
				{
					DayKe = getDayKeForEditOutput.DayKe,
					PhongKhoMaSo = getDayKeForEditOutput.PhongKhoMaSo,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewDayKeModal(int id)
        {
			var getDayKeForViewDto = await _dayKesAppService.GetDayKeForView(id);

            var model = new DayKeViewModel()
            {
                DayKe = getDayKeForViewDto.DayKe
                , PhongKhoMaSo = getDayKeForViewDto.PhongKhoMaSo 

            };

            return PartialView("_ViewDayKeModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DayKes_Create, AppPermissions.Pages_DayKes_Edit)]
        public PartialViewResult PhongKhoLookupTableModal(int? id, string displayName)
        {
            var viewModel = new DayKePhongKhoLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DayKePhongKhoLookupTableModal", viewModel);
        }

    }
}