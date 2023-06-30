using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface ITraoDoiVanBanDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTraoDoiVanBanDuAnForViewDto>> GetAll(GetAllTraoDoiVanBanDuAnsInput input);

        Task<GetTraoDoiVanBanDuAnForViewDto> GetTraoDoiVanBanDuAnForView(int id);

		Task<GetTraoDoiVanBanDuAnForEditOutput> GetTraoDoiVanBanDuAnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTraoDoiVanBanDuAnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTraoDoiVanBanDuAnsToExcel(GetAllTraoDoiVanBanDuAnsForExcelInput input);

		
		Task<PagedResultDto<TraoDoiVanBanDuAnUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<TraoDoiVanBanDuAnVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(GetAllForLookupTableInput input);
		Task GuiTraoDoi(TraoDoiVanBanDuAnDto input);
		Task<GetHienThiTraoDoiDto> GetHienThiTraoDoi(int id);

    }
}