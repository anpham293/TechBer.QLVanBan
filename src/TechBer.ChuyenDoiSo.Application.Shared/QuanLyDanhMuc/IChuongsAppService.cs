using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
    public interface IChuongsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChuongForViewDto>> GetAll(GetAllChuongsInput input);

        Task<GetChuongForViewDto> GetChuongForView(int id);

		Task<GetChuongForEditOutput> GetChuongForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditChuongDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetChuongsToExcel(GetAllChuongsForExcelInput input);

		
		Task<PagedResultDto<ChuongCapQuanLyLookupTableDto>> GetAllCapQuanLyForLookupTable(GetAllForLookupTableInput input);
		
    }
}