using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.DuAnThuHoies;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DuAnThuHoies)]
    public class DuAnThuHoiesController : ChuyenDoiSoControllerBase
    {
        private readonly IDuAnThuHoiesAppService _duAnThuHoiesAppService;

        public DuAnThuHoiesController(IDuAnThuHoiesAppService duAnThuHoiesAppService)
        {
            _duAnThuHoiesAppService = duAnThuHoiesAppService;
        }

        public ActionResult Index()
        {
            var model = new DuAnThuHoiesViewModel
			{
				FilterText = ""
			};

            return View(model);
        } 
       

			 [AbpMvcAuthorize(AppPermissions.Pages_DuAnThuHoies_Create, AppPermissions.Pages_DuAnThuHoies_Edit)]
			public async Task<PartialViewResult> CreateOrEditModal(long? id)
			{
				GetDuAnThuHoiForEditOutput getDuAnThuHoiForEditOutput;

				if (id.HasValue){
					getDuAnThuHoiForEditOutput = await _duAnThuHoiesAppService.GetDuAnThuHoiForEdit(new EntityDto<long> { Id = (long) id });
				}
				else {
					getDuAnThuHoiForEditOutput = new GetDuAnThuHoiForEditOutput{
						DuAnThuHoi = new CreateOrEditDuAnThuHoiDto()
					};
				// getDuAnThuHoiForEditOutput.DuAnThuHoi.ThoiHanBaoLanhHopDong = DateTime.Now;
				// getDuAnThuHoiForEditOutput.DuAnThuHoi.ThoiHanBaoLanhTamUng = DateTime.Now;// getDuAnThuHoiForEditOutput.DuAnThuHoi.ThoiHanBaoLanhHopDong = DateTime.Now;
				getDuAnThuHoiForEditOutput.DuAnThuHoi.NamQuanLy = DateTime.Now.Year;
				}

				var viewModel = new CreateOrEditDuAnThuHoiModalViewModel()
				{
					DuAnThuHoi = getDuAnThuHoiForEditOutput.DuAnThuHoi,                
				};

				return PartialView("_CreateOrEditModal", viewModel);
			}
			

        public async Task<PartialViewResult> ViewDuAnThuHoiModal(long id)
        {
			var getDuAnThuHoiForViewDto = await _duAnThuHoiesAppService.GetDuAnThuHoiForView(id);

            var model = new DuAnThuHoiViewModel()
            {
                DuAnThuHoi = getDuAnThuHoiForViewDto.DuAnThuHoi
            };

            return PartialView("_ViewDuAnThuHoiModal", model);
        }
        
        public async Task<PartialViewResult> DanhMucThuHoiModal(long id)
        {
			var getDuAnThuHoiForViewDto = await _duAnThuHoiesAppService.GetDuAnThuHoiForView(id);

            var model = new DuAnThuHoiViewModel()
            {
                DuAnThuHoi = getDuAnThuHoiForViewDto.DuAnThuHoi,
                DuAnThuHoiId = id
            };

            return PartialView("_DanhMucThuHoiModal", model);
        }


    }
}