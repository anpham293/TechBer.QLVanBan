using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.DynamicEntityParameters.Dto;
using TechBer.ChuyenDoiSo.EntityDynamicParameters;

namespace TechBer.ChuyenDoiSo.DynamicEntityParameters
{
    public interface IEntityDynamicParameterAppService
    {
        Task<EntityDynamicParameterDto> Get(int id);

        Task<ListResultDto<EntityDynamicParameterDto>> GetAllParametersOfAnEntity(EntityDynamicParameterGetAllInput input);

        Task<ListResultDto<EntityDynamicParameterDto>> GetAll();

        Task Add(EntityDynamicParameterDto dto);

        Task Update(EntityDynamicParameterDto dto);

        Task Delete(int id);
    }
}
