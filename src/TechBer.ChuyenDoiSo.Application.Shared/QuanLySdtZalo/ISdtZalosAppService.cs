using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLySdtZalo.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLySdtZalo
{
    public interface ISdtZalosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetSdtZaloForViewDto>> GetAll(GetAllSdtZalosInput input);

        Task<GetSdtZaloForViewDto> GetSdtZaloForView(long id);

		Task<GetSdtZaloForEditOutput> GetSdtZaloForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditSdtZaloDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetSdtZalosToExcel(GetAllSdtZalosForExcelInput input);

		
    }
}