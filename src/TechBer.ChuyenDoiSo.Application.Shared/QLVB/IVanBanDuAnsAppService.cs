using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using GetAllForLookupTableInput = TechBer.ChuyenDoiSo.QLVB.Dtos.GetAllForLookupTableInput;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IVanBanDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetVanBanDuAnForViewDto>> GetAll(GetAllVanBanDuAnsInput input);

        Task<GetVanBanDuAnForViewDto> GetVanBanDuAnForView(int id);

		Task<GetVanBanDuAnForEditOutput> GetVanBanDuAnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditVanBanDuAnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetVanBanDuAnsToExcel(GetAllVanBanDuAnsForExcelInput input);

		
		Task<PagedResultDto<VanBanDuAnDuAnLookupTableDto>> GetAllDuAnForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<VanBanDuAnQuyTrinhDuAnLookupTableDto>> GetAllQuyTrinhDuAnForLookupTable(GetAllForLookupTableInput input);

		Task<int> NhanHoSoGiay(NhanHoSoGiayDto input);

		Task XuLyHoSo(XuLyHoSoInputDto input);

		Task<PagedResultDto<GetVanBanDuAnForViewDto>> GetAllHoSoCanDuyet(
			GetAllHoSoCanDuyetInput input);
    }
}