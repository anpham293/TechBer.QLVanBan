using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
    public interface IChiTietDanhGiasAppService : IApplicationService 
    {
        Task<PagedResultDto<GetChiTietDanhGiaForViewDto>> GetAll(GetAllChiTietDanhGiasInput input);
	
    }
}