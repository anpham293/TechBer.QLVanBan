using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
    public interface IDanhMucThuHoiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetDanhMucThuHoiForViewDto>> GetAll(GetAllDanhMucThuHoiesInput input);

        Task<GetDanhMucThuHoiForViewDto> GetDanhMucThuHoiForView(long id);

		Task<GetDanhMucThuHoiForEditOutput> GetDanhMucThuHoiForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditDanhMucThuHoiDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetDanhMucThuHoiesToExcel(GetAllDanhMucThuHoiesForExcelInput input);

		
		Task<PagedResultDto<DanhMucThuHoiDuAnThuHoiLookupTableDto>> GetAllDuAnThuHoiForLookupTable(GetAllForLookupTableInput input);
		
    }
}