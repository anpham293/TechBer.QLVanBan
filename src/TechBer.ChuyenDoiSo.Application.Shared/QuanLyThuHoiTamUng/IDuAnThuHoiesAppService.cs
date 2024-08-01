using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
    public interface IDuAnThuHoiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDuAnThuHoiForViewDto>> GetAll(GetAllDuAnThuHoiesInput input);

        Task<GetDuAnThuHoiForViewDto> GetDuAnThuHoiForView(long id);

		Task<GetDuAnThuHoiForEditOutput> GetDuAnThuHoiForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditDuAnThuHoiDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetDuAnThuHoiesToExcel(GetAllDuAnThuHoiesForExcelInput input);

		Task<string> SendZalo();
    }
}