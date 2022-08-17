using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
    public interface ITinhThanhsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetTinhThanhForViewDto>> GetAll(GetAllTinhThanhsInput input);

        Task<GetTinhThanhForViewDto> GetTinhThanhForView(int id);

		Task<GetTinhThanhForEditOutput> GetTinhThanhForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditTinhThanhDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetTinhThanhsToExcel(GetAllTinhThanhsForExcelInput input);

		
    }
}