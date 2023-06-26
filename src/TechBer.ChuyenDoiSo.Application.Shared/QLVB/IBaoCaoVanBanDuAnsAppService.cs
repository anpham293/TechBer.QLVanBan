using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IBaoCaoVanBanDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetBaoCaoVanBanDuAnForViewDto>> GetAll(GetAllBaoCaoVanBanDuAnsInput input);

        Task<GetBaoCaoVanBanDuAnForViewDto> GetBaoCaoVanBanDuAnForView(int id);

		Task<GetBaoCaoVanBanDuAnForEditOutput> GetBaoCaoVanBanDuAnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditBaoCaoVanBanDuAnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetBaoCaoVanBanDuAnsToExcel(GetAllBaoCaoVanBanDuAnsForExcelInput input);

		
		Task<PagedResultDto<BaoCaoVanBanDuAnVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<BaoCaoVanBanDuAnUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
    }
}