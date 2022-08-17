using System.Threading.Tasks;

namespace TechBer.ChuyenDoiSo.Security
{
    public interface IPasswordComplexitySettingStore
    {
        Task<PasswordComplexitySetting> GetSettingsAsync();
    }
}
