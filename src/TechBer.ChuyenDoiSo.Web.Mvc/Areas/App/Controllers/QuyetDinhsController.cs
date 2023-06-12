using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyetDinhs;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuyetDinhs)]
    public class QuyetDinhsController : ChuyenDoiSoControllerBase
    {
        private readonly IQuyetDinhsAppService _quyetDinhsAppService;

        public QuyetDinhsController(IQuyetDinhsAppService quyetDinhsAppService)
        {
            _quyetDinhsAppService = quyetDinhsAppService;
        }

        public ActionResult Index()
        {
            var model = new QuyetDinhsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_QuyetDinhs_Create, AppPermissions.Pages_QuyetDinhs_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetQuyetDinhForEditOutput getQuyetDinhForEditOutput;

				if (id.HasValue){
					getQuyetDinhForEditOutput = await _quyetDinhsAppService.GetQuyetDinhForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getQuyetDinhForEditOutput = new GetQuyetDinhForEditOutput{
						QuyetDinh = new CreateOrEditQuyetDinhDto()
					};
				getQuyetDinhForEditOutput.QuyetDinh.NgayBanHanh = DateTime.Now;
				}

				var viewModel = new CreateOrEditQuyetDinhModalViewModel()
				{
					QuyetDinh = getQuyetDinhForEditOutput.QuyetDinh,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewQuyetDinhModal(int id)
        {
			var getQuyetDinhForViewDto = await _quyetDinhsAppService.GetQuyetDinhForView(id);

            var model = new QuyetDinhViewModel()
            {
                QuyetDinh = getQuyetDinhForViewDto.QuyetDinh
            };

            return PartialView("_ViewQuyetDinhModal", model);
        }


    }
}