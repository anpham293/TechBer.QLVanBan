using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.MultiTenancy.Accounting.Dto;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Accounting
{
    public interface IInvoiceAppService
    {
        Task<InvoiceDto> GetInvoiceInfo(EntityDto<long> input);

        Task CreateInvoice(CreateInvoiceDto input);
    }
}
