using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.DynamicEntityParameters;
using Abp.UI.Inputs;
using Microsoft.AspNetCore.Authorization;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;

namespace TechBer.ChuyenDoiSo.DynamicEntityParameters
{
    [Authorize(AppPermissions.Pages_Administration_DynamicParameters)]
    public class DynamicParameterAppService : ChuyenDoiSoAppServiceBase, IDynamicParameterAppService
    {
        private readonly IDynamicParameterManager _dynamicParameterManager;
        private readonly IDynamicParameterStore _dynamicParameterStore;
        private readonly IDynamicEntityParameterDefinitionManager _dynamicEntityParameterDefinitionManager;

        public DynamicParameterAppService(
            IDynamicParameterManager dynamicParameterManager,
            IDynamicParameterStore dynamicParameterStore,
            IDynamicEntityParameterDefinitionManager dynamicEntityParameterDefinitionManager)
        {
            _dynamicParameterManager = dynamicParameterManager;
            _dynamicParameterStore = dynamicParameterStore;
            _dynamicEntityParameterDefinitionManager = dynamicEntityParameterDefinitionManager;
        }

        public async Task<DynamicParameterDto> Get(int id)
        {
            var entity = await _dynamicParameterManager.GetAsync(id);
            return ObjectMapper.Map<DynamicParameterDto>(entity);
        }

        public async Task<ListResultDto<DynamicParameterDto>> GetAll()
        {
            var entities = await _dynamicParameterStore.GetAllAsync();

            return new ListResultDto<DynamicParameterDto>(
                ObjectMapper.Map<List<DynamicParameterDto>>(entities)
            );
        }

        [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue_Create)]
        public async Task Add(DynamicParameterDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicParameterManager.AddAsync(ObjectMapper.Map<DynamicParameter>(dto));
        }

        [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue_Edit)]
        public async Task Update(DynamicParameterDto dto)
        {
            dto.TenantId = AbpSession.TenantId;
            await _dynamicParameterManager.UpdateAsync(ObjectMapper.Map<DynamicParameter>(dto));
        }

        [Authorize(AppPermissions.Pages_Administration_DynamicParameterValue_Delete)]
        public async Task Delete(int id)
        {
            await _dynamicParameterManager.DeleteAsync(id);
        }

        public IInputType FindAllowedInputType(string name)
        {
            return _dynamicEntityParameterDefinitionManager.GetOrNullAllowedInputType(name);
        }
    }
}
