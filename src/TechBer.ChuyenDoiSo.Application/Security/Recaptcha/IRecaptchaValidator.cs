using System.Threading.Tasks;

namespace TechBer.ChuyenDoiSo.Security.Recaptcha
{
    public interface IRecaptchaValidator
    {
        Task ValidateAsync(string captchaResponse);
    }
}