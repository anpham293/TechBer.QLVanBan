﻿using System;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.DanhMucThuHoies;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using Abp.Application.Services.Dto;
using Abp.Extensions;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DanhMucThuHoies)]
    public class DanhMucThuHoiesController : ChuyenDoiSoControllerBase
    {
        private readonly IDanhMucThuHoiesAppService _danhMucThuHoiesAppService;

        public DanhMucThuHoiesController(IDanhMucThuHoiesAppService danhMucThuHoiesAppService)
        {
            _danhMucThuHoiesAppService = danhMucThuHoiesAppService;
        }

        public ActionResult Index()
        {
            var model = new DanhMucThuHoiesViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public class DanhMucThuHoiInput
        {
            public long? id { get; set; }
            public long? duAnThuHoiId { get; set; }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DanhMucThuHoies_Create, AppPermissions.Pages_DanhMucThuHoies_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(DanhMucThuHoiInput input)
        {
            GetDanhMucThuHoiForEditOutput getDanhMucThuHoiForEditOutput;

            if (input.id.HasValue)
            {
                getDanhMucThuHoiForEditOutput =
                    await _danhMucThuHoiesAppService.GetDanhMucThuHoiForEdit(new EntityDto<long>
                        {Id = (long) input.id});
            }
            else
            {
                getDanhMucThuHoiForEditOutput = new GetDanhMucThuHoiForEditOutput
                {
                    DanhMucThuHoi = new CreateOrEditDanhMucThuHoiDto()
                };
                getDanhMucThuHoiForEditOutput.DanhMucThuHoi.DuAnThuHoiId = input.duAnThuHoiId;
            }

            var viewModel = new CreateOrEditDanhMucThuHoiModalViewModel()
            {
                DanhMucThuHoi = getDanhMucThuHoiForEditOutput.DanhMucThuHoi,
                DuAnThuHoiMaDATH = getDanhMucThuHoiForEditOutput.DuAnThuHoiMaDATH,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewDanhMucThuHoiModal(long id)
        {
            var getDanhMucThuHoiForViewDto = await _danhMucThuHoiesAppService.GetDanhMucThuHoiForView(id);

            var model = new DanhMucThuHoiViewModel()
            {
                DanhMucThuHoi = getDanhMucThuHoiForViewDto.DanhMucThuHoi,
                DuAnThuHoiMaDATH = getDanhMucThuHoiForViewDto.DuAnThuHoiMaDATH
            };

            return PartialView("_ViewDanhMucThuHoiModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DanhMucThuHoies_Create, AppPermissions.Pages_DanhMucThuHoies_Edit)]
        public PartialViewResult DuAnThuHoiLookupTableModal(long? id, string displayName)
        {
            var viewModel = new DanhMucThuHoiDuAnThuHoiLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DanhMucThuHoiDuAnThuHoiLookupTableModal", viewModel);
        }

        public async Task<PartialViewResult> ChiTietThuHoiModal(long id)
        {
            var getDanhMucThuHoiForViewDto = await _danhMucThuHoiesAppService.GetDanhMucThuHoiForView(id);

            var model = new DanhMucThuHoiViewModel()
            {
                DanhMucThuHoi = getDanhMucThuHoiForViewDto.DanhMucThuHoi,
                DuAnThuHoiMaDATH = getDanhMucThuHoiForViewDto.DuAnThuHoiMaDATH,
                DanhMucThuHoiId = id
            };

            return PartialView("_ChiTietThuHoiModal", model);
        }
    }
}