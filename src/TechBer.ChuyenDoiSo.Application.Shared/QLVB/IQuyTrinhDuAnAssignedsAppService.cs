using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;


namespace TechBer.ChuyenDoiSo.QLVB
{
    public interface IQuyTrinhDuAnAssignedsAppService : IApplicationService 
    {
        Task<PagedResultDto<GetQuyTrinhDuAnAssignedForViewDto>> GetAll(GetAllQuyTrinhDuAnAssignedsInput input);

        Task<GetQuyTrinhDuAnAssignedForViewDto> GetQuyTrinhDuAnAssignedForView(long id);

		Task<GetQuyTrinhDuAnAssignedForEditOutput> GetQuyTrinhDuAnAssignedForEdit(EntityDto<long> input);

		Task<GetQuyTrinhDuAnAssignedForEditOutput> CreateOrEdit(CreateOrEditQuyTrinhDuAnAssignedDto input);

		Task Delete(EntityDto<long> input);

		Task<FileDto> GetQuyTrinhDuAnAssignedsToExcel(GetAllQuyTrinhDuAnAssignedsForExcelInput input);

		
		Task<PagedResultDto<QuyTrinhDuAnAssignedLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableDto>> GetAllQuyTrinhDuAnForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableDto>> GetAllQuyTrinhDuAnAssignedForLookupTable(GetAllForLookupTableInput input);
		
		Task<PagedResultDto<QuyTrinhDuAnAssignedDuAnLookupTableDto>> GetAllDuAnForLookupTable(GetAllForLookupTableInput input);

		Task<List<GetQuyTrinhDuAnAssignedForView2Dto>> GetDataForTree(int duanid);
		Task<int> XoaTieuChi(long id);
		Task<int> MoveTieuChi(MoveTreeDto input);
    }
}