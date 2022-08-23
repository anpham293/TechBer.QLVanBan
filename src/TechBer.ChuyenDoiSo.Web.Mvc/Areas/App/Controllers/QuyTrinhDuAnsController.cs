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
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
    public class QuyTrinhDuAnsController : ChuyenDoiSoControllerBase
    {
        private readonly IQuyTrinhDuAnsAppService _quyTrinhDuAnsAppService;
        private readonly IRepository<QuyTrinhDuAn> _quyTrinhDuAnsRepository;
        private readonly IRepository<LoaiDuAn> _loaiDuAnsRepository;

        public QuyTrinhDuAnsController(IQuyTrinhDuAnsAppService quyTrinhDuAnsAppService,
            IRepository<QuyTrinhDuAn> quyTrinhDuAnsRepository,
            IRepository<LoaiDuAn> loaiDuAnsRepository
        )
        {
            _quyTrinhDuAnsAppService = quyTrinhDuAnsAppService;
            _quyTrinhDuAnsRepository = quyTrinhDuAnsRepository;
            _loaiDuAnsRepository = loaiDuAnsRepository;
        }

        public ActionResult Index()
        {
            var model = new QuyTrinhDuAnsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }


        public class CreateOrEditModalInput
        {
            public int? Id { get; set; }
            public int? ParentId { get; set; }

            public int LoaiDuAn { get; set; }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Create, AppPermissions.Pages_QuyTrinhDuAns_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(CreateOrEditModalInput input)
        {
            GetQuyTrinhDuAnForEditOutput getQuyTrinhDuAnForEditOutput;

            if (input.Id.HasValue)
            {
                getQuyTrinhDuAnForEditOutput =
                    await _quyTrinhDuAnsAppService.GetQuyTrinhDuAnForEdit(new EntityDto {Id = (int) input.Id});
            }
            else
            {
                getQuyTrinhDuAnForEditOutput = new GetQuyTrinhDuAnForEditOutput
                {
                    QuyTrinhDuAn = new CreateOrEditQuyTrinhDuAnDto()
                };
                getQuyTrinhDuAnForEditOutput.QuyTrinhDuAn.ParentId = input.ParentId;
                getQuyTrinhDuAnForEditOutput.QuyTrinhDuAn.LoaiDuAnId = input.LoaiDuAn;

                if (input.ParentId.HasValue)
                {
                    QuyTrinhDuAn quyTrinhDuAn = await _quyTrinhDuAnsRepository.FirstOrDefaultAsync(input.ParentId.Value);
                    if (quyTrinhDuAn == null)
                    {
                        throw new UserFriendlyException("Không tìm thấy quy trình cha!");
                    }

                    getQuyTrinhDuAnForEditOutput.QuyTrinhDuAnName = quyTrinhDuAn.Name;
                }
                else
                {
                    getQuyTrinhDuAnForEditOutput.QuyTrinhDuAnName = "Quy trình gốc";
                }
            


                LoaiDuAn loaiDuAn = await _loaiDuAnsRepository.FirstOrDefaultAsync(input.LoaiDuAn);
                if (loaiDuAn == null)
                {
                    throw new UserFriendlyException("Không tìm thấy loại dự án!");
                }

                getQuyTrinhDuAnForEditOutput.LoaiDuAnName = loaiDuAn.Name;

            }

           

            var viewModel = new CreateOrEditQuyTrinhDuAnModalViewModel()
            {
                QuyTrinhDuAn = getQuyTrinhDuAnForEditOutput.QuyTrinhDuAn,
                LoaiDuAnName = getQuyTrinhDuAnForEditOutput.LoaiDuAnName,
                QuyTrinhDuAnName = getQuyTrinhDuAnForEditOutput.QuyTrinhDuAnName
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewQuyTrinhDuAnModal(int id)
        {
            var getQuyTrinhDuAnForViewDto = await _quyTrinhDuAnsAppService.GetQuyTrinhDuAnForView(id);

            var model = new QuyTrinhDuAnViewModel()
            {
                QuyTrinhDuAn = getQuyTrinhDuAnForViewDto.QuyTrinhDuAn,
                LoaiDuAnName = getQuyTrinhDuAnForViewDto.LoaiDuAnName,
                QuyTrinhDuAnName = getQuyTrinhDuAnForViewDto.QuyTrinhDuAnName
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

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Create, AppPermissions.Pages_QuyTrinhDuAns_Edit)]
        public PartialViewResult QuyTrinhDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnQuyTrinhDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnQuyTrinhDuAnLookupTableModal", viewModel);
        }
    }
}