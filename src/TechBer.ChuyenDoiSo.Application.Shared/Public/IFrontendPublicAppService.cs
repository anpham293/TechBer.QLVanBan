using System.Threading.Tasks;
using Abp.Application.Services;
using TechBer.ChuyenDoiSo.Public.Dto;

namespace TechBer.ChuyenDoiSo.Public
{
    public interface IFrontendPublicAppService : IApplicationService
    {
        Task<GetDataFromQrCodeResultDto> GetDataFromQrCode(GetDataFromQrCodeInputDto input);
    }
}