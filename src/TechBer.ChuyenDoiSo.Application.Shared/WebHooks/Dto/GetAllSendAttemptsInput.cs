using TechBer.ChuyenDoiSo.Dto;

namespace TechBer.ChuyenDoiSo.WebHooks.Dto
{
    public class GetAllSendAttemptsInput : PagedInputDto
    {
        public string SubscriptionId { get; set; }
    }
}
