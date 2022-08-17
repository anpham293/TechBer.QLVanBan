using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Controllers;

namespace TechBer.ChuyenDoiSo.Web.Public.Controllers
{
    public class AboutController : ChuyenDoiSoControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}