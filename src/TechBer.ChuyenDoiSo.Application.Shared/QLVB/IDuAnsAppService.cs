using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDuAnForViewDto>> GetAll(GetAllDuAnsInput input);

        Task<GetDuAnForViewDto> GetDuAnForView(int id);

		Task<GetDuAnForEditOutput> GetDuAnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditDuAnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetDuAnsToExcel(GetAllDuAnsForExcelInput input);

		
		Task<PagedResultDto<DuAnLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(GetAllForLookupTableInput input);

		Task<PagedResultDto<DuAnChuongLookupTableDto>> GetAllChuongForLookupTable(
			ChuongLookupTableInput input);

    }
}