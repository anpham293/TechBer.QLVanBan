using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyTrinhDuAnAssigneds;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
    public class QuyTrinhDuAnAssignedsController : ChuyenDoiSoControllerBase
    {
        private readonly IQuyTrinhDuAnAssignedsAppService _quyTrinhDuAnAssignedsAppService;

        public QuyTrinhDuAnAssignedsController(IQuyTrinhDuAnAssignedsAppService quyTrinhDuAnAssignedsAppService)
        {
            _quyTrinhDuAnAssignedsAppService = quyTrinhDuAnAssignedsAppService;
        }

        public ActionResult Index()
        {
            var model = new QuyTrinhDuAnAssignedsViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create, AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetQuyTrinhDuAnAssignedForEditOutput getQuyTrinhDuAnAssignedForEditOutput;

				if (id.HasValue){
					getQuyTrinhDuAnAssignedForEditOutput = await _quyTrinhDuAnAssignedsAppService.GetQuyTrinhDuAnAssignedForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getQuyTrinhDuAnAssignedForEditOutput = new GetQuyTrinhDuAnAssignedForEditOutput{
						QuyTrinhDuAnAssigned = new CreateOrEditQuyTrinhDuAnAssignedDto()
					};
				}

				var viewModel = new CreateOrEditQuyTrinhDuAnAssignedModalViewModel()
				{
					QuyTrinhDuAnAssigned = getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssigned,
					LoaiDuAnName = getQuyTrinhDuAnAssignedForEditOutput.LoaiDuAnName,
					QuyTrinhDuAnName = getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnName,
					QuyTrinhDuAnAssignedName = getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssignedName,
					DuAnName = getQuyTrinhDuAnAssignedForEditOutput.DuAnName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewQuyTrinhDuAnAssignedModal(long id)
        {
			var getQuyTrinhDuAnAssignedForViewDto = await _quyTrinhDuAnAssignedsAppService.GetQuyTrinhDuAnAssignedForView(id);

            var model = new QuyTrinhDuAnAssignedViewModel()
            {
                QuyTrinhDuAnAssigned = getQuyTrinhDuAnAssignedForViewDto.QuyTrinhDuAnAssigned
                , LoaiDuAnName = getQuyTrinhDuAnAssignedForViewDto.LoaiDuAnName 

                , QuyTrinhDuAnName = getQuyTrinhDuAnAssignedForViewDto.QuyTrinhDuAnName 

                , QuyTrinhDuAnAssignedName = getQuyTrinhDuAnAssignedForViewDto.QuyTrinhDuAnAssignedName 

                , DuAnName = getQuyTrinhDuAnAssignedForViewDto.DuAnName 

            };

            return PartialView("_ViewQuyTrinhDuAnAssignedModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create, AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult LoaiDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedLoaiDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedLoaiDuAnLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create, AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult QuyTrinhDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create, AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult QuyTrinhDuAnAssignedLookupTableModal(long? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create, AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult DuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedDuAnLookupTableModal", viewModel);
        }

    }
}