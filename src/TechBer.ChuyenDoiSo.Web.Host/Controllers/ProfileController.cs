using Abp.AspNetCore.Mvc.Authorization;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.Web.Controllers
{
    [AbpMvcAuthorize]
    public class ProfileController : ProfileControllerBase
    {
        public ProfileController(ITempFileCacheManager tempFileCacheManager) :
            base(tempFileCacheManager)
        {
        }
    }
}