using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Public;
using TechBer.ChuyenDoiSo.Public.Dto;
using TechBer.ChuyenDoiSo.Web.Controllers;

namespace TechBer.ChuyenDoiSo.Web.Public.Controllers
{
    public class TraCuuController : ChuyenDoiSoControllerBase
    {
        private readonly IFrontendPublicAppService _frontendAppService;
        public TraCuuController(
            IFrontendPublicAppService frontendAppService
        )
        {
            _frontendAppService = frontendAppService;
        }
        
        
        public async Task<IActionResult> Index(string id)
        {
            GetDataFromQrCodeResultDto data = await _frontendAppService.GetDataFromQrCode(new GetDataFromQrCodeInputDto()
            {
                Qrstring = id
            });
            return View(data);
        }
    }
}