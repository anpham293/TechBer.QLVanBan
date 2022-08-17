using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface ILoaiDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLoaiDuAnForViewDto>> GetAll(GetAllLoaiDuAnsInput input);

        Task<GetLoaiDuAnForViewDto> GetLoaiDuAnForView(int id);

		Task<GetLoaiDuAnForEditOutput> GetLoaiDuAnForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLoaiDuAnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLoaiDuAnsToExcel(GetAllLoaiDuAnsForExcelInput input);

		
    }
}