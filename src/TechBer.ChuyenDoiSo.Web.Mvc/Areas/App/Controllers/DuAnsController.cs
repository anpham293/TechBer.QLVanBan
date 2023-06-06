using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.DuAns;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_DuAns)]
    public class DuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly IDuAnsAppService _duAnsAppService;
        private readonly IRepository<CapQuanLy> _capQuanLyRepository;

        public DuAnsController(IDuAnsAppService duAnsAppService,
            IRepository<CapQuanLy> capQuanLyRepository
        )
        {
            _duAnsAppService = duAnsAppService;
            _capQuanLyRepository = capQuanLyRepository;
        }

        public ActionResult Index()
        {
            var model = new DuAnsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        [AbpMvcAuthorize(AppPermissions.Pages_DuAns_Create, AppPermissions.Pages_DuAns_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(int? id)
        {
            GetDuAnForEditOutput getDuAnForEditOutput;

            if (id.HasValue)
            {
                getDuAnForEditOutput = await _duAnsAppService.GetDuAnForEdit(new EntityDto {Id = (int) id});
            }
            else
            {
                getDuAnForEditOutput = new GetDuAnForEditOutput
                {
                    DuAn = new CreateOrEditDuAnDto()
                };
            }

            var viewModel = new CreateOrEditDuAnModalViewModel()
            {
                DuAn = getDuAnForEditOutput.DuAn,
                LoaiDuAnName = getDuAnForEditOutput.LoaiDuAnName,
                ChuongName = getDuAnForEditOutput.ChuongName,
                LoaiKhoanName = getDuAnForEditOutput.LoaiKhoanName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewDuAnModal(int id)
        {
            var getDuAnForViewDto = await _duAnsAppService.GetDuAnForView(id);

            var model = new DuAnViewModel()
            {
                DuAn = getDuAnForViewDto.DuAn,
                LoaiDuAnName = getDuAnForViewDto.LoaiDuAnName,
                ChuongName = getDuAnForViewDto.ChuongName,
                LoaiKhoanName = getDuAnForViewDto.LoaiKhoanName,
                TongSoTienThanhToan = getDuAnForViewDto.TongSoTienThanhToan
            };

            return PartialView("_ViewDuAnModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DuAns_Create, AppPermissions.Pages_DuAns_Edit)]
        public PartialViewResult LoaiDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new DuAnLoaiDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_DuAnLoaiDuAnLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DuAns_Create, AppPermissions.Pages_DuAns_Edit)]
        public PartialViewResult ChuongLookupTableModal(int? id, string displayName)
        {
            var viewModel = new DuAnChuongLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = "",
                ListCapQuanLy = ObjectMapper.Map<List<CapQuanLyDto>>(_capQuanLyRepository.GetAllList())
            };
            return PartialView("_DuAnChuongLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_DuAns_Create, AppPermissions.Pages_DuAns_Edit)]
        public PartialViewResult LoaiKhoanLookupTableModal(int? id, string displayName)
        {
            var viewModel = new DuAnLoaiKhoanLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };
            return PartialView("_DuAnLoaiKhoanLookupTableModal", viewModel);
        }
        [AbpMvcAuthorize(AppPermissions.Pages_DuAns_ThemUserVaoDuAn)]
        public PartialViewResult DanhSachUserDuAnModal()
        {
            var viewModel = new DuAnLoaiKhoanLookupTableViewModel()
            {
                FilterText = ""
            };
            return PartialView("_ViewDanhSachUserDuAnModal", viewModel);
        }
    }
}