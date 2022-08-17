using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Editions.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy
{
    public interface ITenantRegistrationAppService: IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

        Task<EditionsSelectOutput> GetEditionsForSelect();

        Task<EditionSelectDto> GetEdition(int editionId);
    }
}