using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IChuyenHoSoGiaiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChuyenHoSoGiayForViewDto>> GetAll(GetAllChuyenHoSoGiaiesInput input);

        Task<GetChuyenHoSoGiayForViewDto> GetChuyenHoSoGiayForView(int id);

		Task<GetChuyenHoSoGiayForEditOutput> GetChuyenHoSoGiayForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditChuyenHoSoGiayDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetChuyenHoSoGiaiesToExcel(GetAllChuyenHoSoGiaiesForExcelInput input);

		
		Task<PagedResultDto<ChuyenHoSoGiayVanBanDuAnLookupTableDto>> GetAllVanBanDuAnForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ChuyenHoSoGiayUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<GetChuyenHoSoGiayForViewDto>> ChuyenHoSoGiayVanBanDuAnGetAll(ChuyenHoSoGiayVanBanDuAnGetAllInput input);

		Task<string> CapNhatTrangThaiChuyenHoSoGiay(int id, int trangThai);
    }
}