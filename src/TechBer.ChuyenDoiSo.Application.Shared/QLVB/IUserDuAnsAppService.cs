using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IUserDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetUserDuAnForViewDto>> GetAll(GetAllUserDuAnsInput input);

        Task<GetUserDuAnForViewDto> GetUserDuAnForView(long id);

		Task<GetUserDuAnForEditOutput> GetUserDuAnForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditUserDuAnDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetUserDuAnsToExcel(GetAllUserDuAnsForExcelInput input);

		
		Task<PagedResultDto<UserDuAnUserLookupTableDto>> GetAllUserForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<UserDuAnDuAnLookupTableDto>> GetAllDuAnForLookupTable(GetAllForLookupTableInput input);
		
    }
}