using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IQuyetDinhsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetQuyetDinhForViewDto>> GetAll(GetAllQuyetDinhsInput input);

        Task<GetQuyetDinhForViewDto> GetQuyetDinhForView(int id);

		Task<GetQuyetDinhForEditOutput> GetQuyetDinhForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditQuyetDinhDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetQuyetDinhsToExcel(GetAllQuyetDinhsForExcelInput input);

		
    }
}