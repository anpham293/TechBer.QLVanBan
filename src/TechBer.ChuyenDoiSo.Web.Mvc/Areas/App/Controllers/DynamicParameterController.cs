using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.DynamicEntityParameters;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.DynamicEntityParameters;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.DynamicParameters;
using TechBer.ChuyenDoiSo.Web.Controllers;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    public class DynamicParameterController : ChuyenDoiSoControllerBase
    {
        private readonly IDynamicParameterAppService _dynamicParameterAppService;
        private readonly IDynamicParameterValueAppService _dynamicParameterValueAppService;
        private readonly IDynamicEntityParameterDefinitionManager _dynamicEntityParameterDefinitionManager;

        public DynamicParameterController(
            IDynamicParameterAppService dynamicParameterAppService,
            IDynamicParameterValueAppService dynamicParameterValueAppService,
            IDynamicEntityParameterDefinitionManager dynamicEntityParameterDefinitionManager)
        {
            _dynamicParameterAppService = dynamicParameterAppService;
            _dynamicParameterValueAppService = dynamicParameterValueAppService;
            _dynamicEntityParameterDefinitionManager = dynamicEntityParameterDefinitionManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DynamicParameters_Edit, AppPermissions.Pages_Administration_DynamicParameters_Create)]
        public async Task<IActionResult> CreateOrEditModal(int? id)
        {
            var model = new CreateOrEditDynamicParameterViewModel
            {
                AllowedInputTypes = _dynamicEntityParameterDefinitionManager.GetAllAllowedInputTypeNames()
            };

            if (id.HasValue)
            {
                model.DynamicParameterDto = await _dynamicParameterAppService.Get(id.Value);
            }
            else
            {
                model.DynamicParameterDto = new DynamicParameterDto();
            }

            return PartialView("_CreateOrEditModal", model);
        }

        public async Task<IActionResult> Detail(int id)
        {
            var dynamicParameter = await _dynamicParameterAppService.Get(id);
            return View(dynamicParameter);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_Administration_DynamicParameterValue_Edit, AppPermissions.Pages_Administration_DynamicParameterValue_Create)]
        public async Task<IActionResult> CreateOrEditValueModal(int? id, int? dynamicParameterId)
        {
            var dynamicParameterDto = new DynamicParameterValueDto()
            {
                DynamicParameterId = dynamicParameterId ?? 0
            };

            if (id.HasValue)
            {
                dynamicParameterDto = await _dynamicParameterValueAppService.Get(id.Value);
            }

            return PartialView("_CreateOrEditValueModal", dynamicParameterDto);
        }
    }
}