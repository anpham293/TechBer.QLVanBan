using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuanHuyens;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuanHuyens)]
    public class QuanHuyensController : ChuyenDoiSoControllerBase
    {
        private readonly IQuanHuyensAppService _quanHuyensAppService;

        public QuanHuyensController(IQuanHuyensAppService quanHuyensAppService)
        {
            _quanHuyensAppService = quanHuyensAppService;
        }

        public ActionResult Index()
        {
            var model = new QuanHuyensViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_QuanHuyens_Create, AppPermissions.Pages_QuanHuyens_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetQuanHuyenForEditOutput getQuanHuyenForEditOutput;

				if (id.HasValue){
					getQuanHuyenForEditOutput = await _quanHuyensAppService.GetQuanHuyenForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getQuanHuyenForEditOutput = new GetQuanHuyenForEditOutput{
						QuanHuyen = new CreateOrEditQuanHuyenDto()
					};
				}

				var viewModel = new CreateOrEditQuanHuyenModalViewModel()
				{
					QuanHuyen = getQuanHuyenForEditOutput.QuanHuyen,
					TinhThanhName = getQuanHuyenForEditOutput.TinhThanhName,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewQuanHuyenModal(int id)
        {
			var getQuanHuyenForViewDto = await _quanHuyensAppService.GetQuanHuyenForView(id);

            var model = new QuanHuyenViewModel()
            {
                QuanHuyen = getQuanHuyenForViewDto.QuanHuyen
                , TinhThanhName = getQuanHuyenForViewDto.TinhThanhName 

            };

            return PartialView("_ViewQuanHuyenModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuanHuyens_Create, AppPermissions.Pages_QuanHuyens_Edit)]
        public PartialViewResult TinhThanhLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuanHuyenTinhThanhLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuanHuyenTinhThanhLookupTableModal", viewModel);
        }

    }
}