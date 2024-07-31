using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
    public interface IChiTietThuHoiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChiTietThuHoiForViewDto>> GetAll(GetAllChiTietThuHoiesInput input);

        Task<GetChiTietThuHoiForViewDto> GetChiTietThuHoiForView(long id);

		Task<GetChiTietThuHoiForEditOutput> GetChiTietThuHoiForEdit(EntityDto<long> input);

		Task CreateOrEdit(CreateOrEditChiTietThuHoiDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetChiTietThuHoiesToExcel(GetAllChiTietThuHoiesForExcelInput input);

		
		Task<PagedResultDto<ChiTietThuHoiDanhMucThuHoiLookupTableDto>> GetAllDanhMucThuHoiForLookupTable(GetAllForLookupTableInput input);
		
    }
}