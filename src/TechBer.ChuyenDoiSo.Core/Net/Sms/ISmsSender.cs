using System.Threading.Tasks;

namespace TechBer.ChuyenDoiSo.Net.Sms
{
    public interface ISmsSender
    {
        Task SendAsync(string number, string message);
    }
}