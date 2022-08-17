using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
    public interface IQuanHuyensAppService : IApplicationService 
    {
        Task<PagedResultDto<GetQuanHuyenForViewDto>> GetAll(GetAllQuanHuyensInput input);

        Task<GetQuanHuyenForViewDto> GetQuanHuyenForView(int id);

		Task<GetQuanHuyenForEditOutput> GetQuanHuyenForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditQuanHuyenDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetQuanHuyensToExcel(GetAllQuanHuyensForExcelInput input);

		
		Task<PagedResultDto<QuanHuyenTinhThanhLookupTableDto>> GetAllTinhThanhForLookupTable(GetAllForLookupTableInput input);
		
    }
}