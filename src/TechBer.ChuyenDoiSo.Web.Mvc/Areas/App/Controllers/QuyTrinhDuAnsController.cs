using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyTrinhDuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
    public class QuyTrinhDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly IQuyTrinhDuAnsAppService _quyTrinhDuAnsAppService;

        public QuyTrinhDuAnsController(IQuyTrinhDuAnsAppService quyTrinhDuAnsAppService)
        {
            _quyTrinhDuAnsAppService = quyTrinhDuAnsAppService;
        }

        public ActionResult Index()
        {
            var model = new QuyTrinhDuAnsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Create, AppPermissions.Pages_QuyTrinhDuAns_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetQuyTrinhDuAnForEditOutput getQuyTrinhDuAnForEditOutput;

				if (id.HasValue){
					getQuyTrinhDuAnForEditOutput = await _quyTrinhDuAnsAppService.GetQuyTrinhDuAnForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getQuyTrinhDuAnForEditOutput = new GetQuyTrinhDuAnForEditOutput{
						QuyTrinhDuAn = new CreateOrEditQuyTrinhDuAnDto()
					};
				}

				var viewModel = new CreateOrEditQuyTrinhDuAnModalViewModel()
				{
					QuyTrinhDuAn = getQuyTrinhDuAnForEditOutput.QuyTrinhDuAn,
					LoaiDuAnName = getQuyTrinhDuAnForEditOutput.LoaiDuAnName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewQuyTrinhDuAnModal(int id)
        {
			var getQuyTrinhDuAnForViewDto = await _quyTrinhDuAnsAppService.GetQuyTrinhDuAnForView(id);

            var model = new QuyTrinhDuAnViewModel()
            {
                QuyTrinhDuAn = getQuyTrinhDuAnForViewDto.QuyTrinhDuAn
                , LoaiDuAnName = getQuyTrinhDuAnForViewDto.LoaiDuAnName 

            };

            return PartialView("_ViewQuyTrinhDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Create, AppPermissions.Pages_QuyTrinhDuAns_Edit)]
        public PartialViewResult LoaiDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnLoaiDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnLoaiDuAnLookupTableModal", viewModel);
        }

    }
}