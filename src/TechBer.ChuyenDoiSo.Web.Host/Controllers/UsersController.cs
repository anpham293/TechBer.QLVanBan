using Abp.AspNetCore.Mvc.Authorization;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.Storage;
using Abp.BackgroundJobs;

namespace TechBer.ChuyenDoiSo.Web.Controllers
{
    [AbpMvcAuthorize(AppPermissions.Pages_Administration_Users)]
    public class UsersController : UsersControllerBase
    {
        public UsersController(IBinaryObjectManager binaryObjectManager, IBackgroundJobManager backgroundJobManager)
            : base(binaryObjectManager, backgroundJobManager)
        {
        }
    }
}