using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo
{
    public interface IThungHoSosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetThungHoSoForViewDto>> GetAll(GetAllThungHoSosInput input);

        Task<GetThungHoSoForViewDto> GetThungHoSoForView(int id);

		Task<GetThungHoSoForEditOutput> GetThungHoSoForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditThungHoSoDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetThungHoSosToExcel(GetAllThungHoSosForExcelInput input);

		
		Task<PagedResultDto<ThungHoSoDayKeLookupTableDto>> GetAllDayKeForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<ThungHoSoDuAnLookupTableDto>> GetAllDuAnForLookupTable(GetAllForLookupTableInput input);
		
    }
}