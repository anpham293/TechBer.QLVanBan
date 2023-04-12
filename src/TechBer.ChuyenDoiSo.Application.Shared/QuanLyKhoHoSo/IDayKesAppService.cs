using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo
{
    public interface IDayKesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDayKeForViewDto>> GetAll(GetAllDayKesInput input);

        Task<GetDayKeForViewDto> GetDayKeForView(int id);

		Task<GetDayKeForEditOutput> GetDayKeForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDayKeDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDayKesToExcel(GetAllDayKesForExcelInput input);

		
		Task<PagedResultDto<DayKePhongKhoLookupTableDto>> GetAllPhongKhoForLookupTable(GetAllForLookupTableInput input);
		
    }
}