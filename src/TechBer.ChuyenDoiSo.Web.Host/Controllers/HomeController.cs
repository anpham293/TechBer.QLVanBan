using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace TechBer.ChuyenDoiSo.Web.Controllers
{
    public class HomeController : ChuyenDoiSoControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return RedirectToAction("Index", "Ui");
        }
    }
}
