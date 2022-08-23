using System;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IQuyTrinhDuAnsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetQuyTrinhDuAnForViewDto>> GetAll(GetAllQuyTrinhDuAnsInput input);

        Task<GetQuyTrinhDuAnForViewDto> GetQuyTrinhDuAnForView(int id);

		Task<GetQuyTrinhDuAnForEditOutput> GetQuyTrinhDuAnForEdit(EntityDto input);

		Task<GetQuyTrinhDuAnForEditOutput> CreateOrEdit(CreateOrEditQuyTrinhDuAnDto input);

		Task Delete(EntityDto input);

		Task<FileDto> GetQuyTrinhDuAnsToExcel(GetAllQuyTrinhDuAnsForExcelInput input);

		
		Task<PagedResultDto<QuyTrinhDuAnLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<QuyTrinhDuAnQuyTrinhDuAnLookupTableDto>> GetAllQuyTrinhDuAnForLookupTable(GetAllForLookupTableInput input);
		Task<int> MoveTieuChi(MoveTreeDto input);

    }
}