using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.DynamicEntityParameters;
using Microsoft.AspNetCore.Authorization;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;
using TechBer.ChuyenDoiSo.EntityDynamicParameterValues.Dto;

namespace TechBer.ChuyenDoiSo.DynamicEntityParameters
{
    [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue)]
    public class EntityDynamicParameterValueAppService : ChuyenDoiSoAppServiceBase, IEntityDynamicParameterValueAppService
    {
        private readonly IEntityDynamicParameterValueManager _entityDynamicParameterValueManager;
        private readonly IDynamicParameterValueManager _dynamicParameterValueManager;
        private readonly IEntityDynamicParameterManager _entityDynamicParameterManager;
        private readonly IDynamicEntityParameterDefinitionManager _dynamicEntityParameterDefinitionManager;

        public EntityDynamicParameterValueAppService(
            IEntityDynamicParameterValueManager entityDynamicParameterValueManager,
            IDynamicParameterValueManager dynamicParameterValueManager,
            IEntityDynamicParameterManager entityDynamicParameterManager,
            IDynamicEntityParameterDefinitionManager dynamicEntityParameterDefinitionManager)
        {
            _entityDynamicParameterValueManager = entityDynamicParameterValueManager;
            _dynamicParameterValueManager = dynamicParameterValueManager;
            _entityDynamicParameterManager = entityDynamicParameterManager;
            _dynamicEntityParameterDefinitionManager = dynamicEntityParameterDefinitionManager;
        }

        public async Task<EntityDynamicParameterValueDto> Get(int id)
        {
            var entity = await _entityDynamicParameterValueManager.GetAsync(id);
            return ObjectMapper.Map<EntityDynamicParameterValueDto>(entity);
        }

        public async Task<ListResultDto<EntityDynamicParameterValueDto>> GetAll(GetAllInput input)
        {
            var entities = await _entityDynamicParameterValueManager.GetValuesAsync(input.ParameterId, input.EntityId);
            return new ListResultDto<EntityDynamicParameterValueDto>(
                ObjectMapper.Map<List<EntityDynamicParameterValueDto>>(entities)
            );
        }

        [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create)]
        public async Task Add(EntityDynamicParameterValueDto input)
        {
            var entity = ObjectMapper.Map<EntityDynamicParameterValue>(input);
            entity.TenantId = AbpSession.TenantId;
            await _entityDynamicParameterValueManager.AddAsync(entity);
        }

        [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit)]
        public async Task Update(EntityDynamicParameterValueDto input)
        {
            var entity = await _entityDynamicParameterValueManager.GetAsync(input.Id);
            if (entity == null || entity.TenantId != AbpSession.TenantId)
            {
                throw new EntityNotFoundException(typeof(EntityDynamicParameterValue), input.Id);
            }

            entity.Value = input.Value;
            entity.EntityDynamicParameterId = input.EntityDynamicParameterId;
            entity.EntityId = input.EntityId;

            await _entityDynamicParameterValueManager.UpdateAsync(entity);
        }

        [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete)]
        public async Task Delete(int id)
        {
            await _entityDynamicParameterValueManager.DeleteAsync(id);
        }

        public async Task<GetAllEntityDynamicParameterValuesOutput> GetAllEntityDynamicParameterValues(GetAllEntityDynamicParameterValuesInput input)
        {
            var localCacheOfDynamicParameterValues = new Dictionary<int, List<string>>();

            async Task<List<string>> LocalGetAllValuesOfDynamicParameter(int dynamicParameterId)
            {
                if (!localCacheOfDynamicParameterValues.ContainsKey(dynamicParameterId))
                {
                    localCacheOfDynamicParameterValues[dynamicParameterId] = (await _dynamicParameterValueManager
                        .GetAllValuesOfDynamicParameterAsync(dynamicParameterId))
                        .Select(x => x.Value).ToList();
                }

                return localCacheOfDynamicParameterValues[dynamicParameterId];
            }

            var output = new GetAllEntityDynamicParameterValuesOutput();
            var entityDynamicParameters = await _entityDynamicParameterManager.GetAllAsync(input.EntityFullName);

            var entityDynamicParameterSelectedValues = (await _entityDynamicParameterValueManager.GetValuesAsync(input.EntityFullName, input.EntityId))
                .GroupBy(value => value.EntityDynamicParameterId)
                .ToDictionary(group => group.Key, items => items.ToList().Select(value => value.Value).ToList());

            foreach (var entityDynamicParameter in entityDynamicParameters)
            {
                var outputItem = new GetAllEntityDynamicParameterValuesOutputItem
                {
                    EntityDynamicParameterId = entityDynamicParameter.Id,
                    InputType = _dynamicEntityParameterDefinitionManager.GetOrNullAllowedInputType(entityDynamicParameter.DynamicParameter.InputType),
                    ParameterName = entityDynamicParameter.DynamicParameter.ParameterName,
                    AllValuesInputTypeHas = await LocalGetAllValuesOfDynamicParameter(entityDynamicParameter.DynamicParameter.Id),
                    SelectedValues = entityDynamicParameterSelectedValues.ContainsKey(entityDynamicParameter.Id)
                        ? entityDynamicParameterSelectedValues[entityDynamicParameter.Id]
                        : new List<string>()
                };

                output.Items.Add(outputItem);
            }

            return output;
        }

        [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Create)]
        [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Edit)]
        public async Task InsertOrUpdateAllValues(InsertOrUpdateAllValuesInput input)
        {
            if (input.Items.IsNullOrEmpty())
            {
                return;
            }

            foreach (var item in input.Items)
            {
                await _entityDynamicParameterValueManager.CleanValuesAsync(item.EntityDynamicParameterId, item.EntityId);

                foreach (var newValue in item.Values)
                {
                    await _entityDynamicParameterValueManager.AddAsync(new EntityDynamicParameterValue
                    {
                        EntityDynamicParameterId = item.EntityDynamicParameterId,
                        EntityId = item.EntityId,
                        Value = newValue,
                        TenantId = AbpSession.TenantId
                    });
                }
            }
        }

        [Authorize(AppPermissions.Pages_Administration_EntityDynamicParameterValue_Delete)]
        public async Task CleanValues(CleanValuesInput input)
        {
            await _entityDynamicParameterValueManager.CleanValuesAsync(input.EntityDynamicParameterId, input.EntityId);
        }
    }
}
