using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
    public interface ILoaiKhoansAppService : IApplicationService 
    {
        Task<PagedResultDto<GetLoaiKhoanForViewDto>> GetAll(GetAllLoaiKhoansInput input);

        Task<GetLoaiKhoanForViewDto> GetLoaiKhoanForView(int id);

		Task<GetLoaiKhoanForEditOutput> GetLoaiKhoanForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditLoaiKhoanDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetLoaiKhoansToExcel(GetAllLoaiKhoansForExcelInput input);

		
    }
}