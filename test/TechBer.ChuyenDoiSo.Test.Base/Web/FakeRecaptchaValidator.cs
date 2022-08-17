using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Security.Recaptcha;

namespace TechBer.ChuyenDoiSo.Test.Base.Web
{
    public class FakeRecaptchaValidator : IRecaptchaValidator
    {
        public Task ValidateAsync(string captchaResponse)
        {
            return Task.CompletedTask;
        }
    }
}
