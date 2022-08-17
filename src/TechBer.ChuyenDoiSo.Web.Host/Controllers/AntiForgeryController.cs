using Microsoft.AspNetCore.Antiforgery;

namespace TechBer.ChuyenDoiSo.Web.Controllers
{
    public class AntiForgeryController : ChuyenDoiSoControllerBase
    {
        private readonly IAntiforgery _antiforgery;

        public AntiForgeryController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public void GetToken()
        {
            _antiforgery.SetCookieTokenAndHeader(HttpContext);
        }
    }
}
