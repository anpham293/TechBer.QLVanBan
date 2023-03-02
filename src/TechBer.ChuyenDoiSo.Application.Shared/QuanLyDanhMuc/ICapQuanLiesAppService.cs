using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
    public interface ICapQuanLiesAppService : IApplicationService 
    {
        Task<PagedResultDto<GetCapQuanLyForViewDto>> GetAll(GetAllCapQuanLiesInput input);

        Task<GetCapQuanLyForViewDto> GetCapQuanLyForView(int id);

		Task<GetCapQuanLyForEditOutput> GetCapQuanLyForEdit(EntityDto input);

		Task CreateOrEdit(CreateOrEditCapQuanLyDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetCapQuanLiesToExcel(GetAllCapQuanLiesForExcelInput input);

		
    }
}