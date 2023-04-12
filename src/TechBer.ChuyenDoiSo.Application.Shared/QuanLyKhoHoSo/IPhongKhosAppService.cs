using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyKhoHoSo
{
    public interface IPhongKhosAppService : IApplicationService 
    {
        Task<PagedResultDto<GetPhongKhoForViewDto>> GetAll(GetAllPhongKhosInput input);

        Task<GetPhongKhoForViewDto> GetPhongKhoForView(int id);

		Task<GetPhongKhoForEditOutput> GetPhongKhoForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditPhongKhoDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetPhongKhosToExcel(GetAllPhongKhosForExcelInput input);

		
    }
}