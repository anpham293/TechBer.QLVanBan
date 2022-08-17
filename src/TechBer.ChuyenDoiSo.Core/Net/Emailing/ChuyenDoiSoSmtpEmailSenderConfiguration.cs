using Abp.Configuration;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Runtime.Security;

namespace TechBer.ChuyenDoiSo.Net.Emailing
{
    public class ChuyenDoiSoSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public ChuyenDoiSoSmtpEmailSenderConfiguration(ISettingManager settingManager) : base(settingManager)
        {

        }

        public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}