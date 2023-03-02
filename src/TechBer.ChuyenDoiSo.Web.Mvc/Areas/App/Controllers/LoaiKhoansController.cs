using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.LoaiKhoans;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_LoaiKhoans)]
    public class LoaiKhoansController : ChuyenDoiSoControllerBase
    {
        private readonly ILoaiKhoansAppService _loaiKhoansAppService;

        public LoaiKhoansController(ILoaiKhoansAppService loaiKhoansAppService)
        {
            _loaiKhoansAppService = loaiKhoansAppService;
        }

        public ActionResult Index()
        {
            var model = new LoaiKhoansViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_LoaiKhoans_Create, AppPermissions.Pages_LoaiKhoans_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(int? id)
			{
				GetLoaiKhoanForEditOutput getLoaiKhoanForEditOutput;

				if (id.HasValue){
					getLoaiKhoanForEditOutput = await _loaiKhoansAppService.GetLoaiKhoanForEdit(new EntityDto { Id = (int) id });
				}
				else {
					getLoaiKhoanForEditOutput = new GetLoaiKhoanForEditOutput{
						LoaiKhoan = new CreateOrEditLoaiKhoanDto()
					};
				}

				var viewModel = new CreateOrEditLoaiKhoanModalViewModel()
				{
					LoaiKhoan = getLoaiKhoanForEditOutput.LoaiKhoan,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewLoaiKhoanModal(int id)
        {
			var getLoaiKhoanForViewDto = await _loaiKhoansAppService.GetLoaiKhoanForView(id);

            var model = new LoaiKhoanViewModel()
            {
                LoaiKhoan = getLoaiKhoanForViewDto.LoaiKhoan
            };

            return PartialView("_ViewLoaiKhoanModal", model);
        }


    }
}