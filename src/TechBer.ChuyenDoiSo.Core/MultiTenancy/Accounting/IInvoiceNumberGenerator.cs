using System.Threading.Tasks;
using Abp.Dependency;

namespace TechBer.ChuyenDoiSo.MultiTenancy.Accounting
{
    public interface IInvoiceNumberGenerator : ITransientDependency
    {
        Task<string> GetNewInvoiceNumber();
    }
}