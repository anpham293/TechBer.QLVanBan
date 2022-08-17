using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;
using TechBer.ChuyenDoiSo.EntityDynamicParameterValues.Dto;

namespace TechBer.ChuyenDoiSo.DynamicEntityParameters
{
    public interface IEntityDynamicParameterValueAppService
    {
        Task<EntityDynamicParameterValueDto> Get(int id);

        Task<ListResultDto<EntityDynamicParameterValueDto>> GetAll(GetAllInput input);

        Task Add(EntityDynamicParameterValueDto input);

        Task Update(EntityDynamicParameterValueDto input);

        Task Delete(int id);

        Task<GetAllEntityDynamicParameterValuesOutput> GetAllEntityDynamicParameterValues(GetAllEntityDynamicParameterValuesInput input);
    }
}
