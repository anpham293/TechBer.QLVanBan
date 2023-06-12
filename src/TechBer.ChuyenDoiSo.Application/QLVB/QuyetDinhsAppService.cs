

using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QLVB.Exporting;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[AbpAuthorize(AppPermissions.Pages_QuyetDinhs)]
    public class QuyetDinhsAppService : ChuyenDoiSoAppServiceBase, IQuyetDinhsAppService
    {
		 private readonly IRepository<QuyetDinh> _quyetDinhRepository;
		 private readonly IQuyetDinhsExcelExporter _quyetDinhsExcelExporter;
		 

		  public QuyetDinhsAppService(IRepository<QuyetDinh> quyetDinhRepository, IQuyetDinhsExcelExporter quyetDinhsExcelExporter ) 
		  {
			_quyetDinhRepository = quyetDinhRepository;
			_quyetDinhsExcelExporter = quyetDinhsExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetQuyetDinhForViewDto>> GetAll(GetAllQuyetDinhsInput input)
         {
			
			var filteredQuyetDinhs = _quyetDinhRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.So.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.FileQuyetDinh.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.SoFilter),  e => e.So == input.SoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(input.MinNgayBanHanhFilter != null, e => e.NgayBanHanh >= input.MinNgayBanHanhFilter)
						.WhereIf(input.MaxNgayBanHanhFilter != null, e => e.NgayBanHanh <= input.MaxNgayBanHanhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FileQuyetDinhFilter),  e => e.FileQuyetDinh == input.FileQuyetDinhFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

			var pagedAndFilteredQuyetDinhs = filteredQuyetDinhs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var quyetDinhs = from o in pagedAndFilteredQuyetDinhs
                         select new GetQuyetDinhForViewDto() {
							QuyetDinh = new QuyetDinhDto
							{
                                So = o.So,
                                Ten = o.Ten,
                                NgayBanHanh = o.NgayBanHanh,
                                FileQuyetDinh = o.FileQuyetDinh,
                                TrangThai = o.TrangThai,
                                Id = o.Id
							}
						};

            var totalCount = await filteredQuyetDinhs.CountAsync();

            return new PagedResultDto<GetQuyetDinhForViewDto>(
                totalCount,
                await quyetDinhs.ToListAsync()
            );
         }
		 
		 public async Task<GetQuyetDinhForViewDto> GetQuyetDinhForView(int id)
         {
            var quyetDinh = await _quyetDinhRepository.GetAsync(id);

            var output = new GetQuyetDinhForViewDto { QuyetDinh = ObjectMapper.Map<QuyetDinhDto>(quyetDinh) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Edit)]
		 public async Task<GetQuyetDinhForEditOutput> GetQuyetDinhForEdit(EntityDto input)
         {
            var quyetDinh = await _quyetDinhRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetQuyetDinhForEditOutput {QuyetDinh = ObjectMapper.Map<CreateOrEditQuyetDinhDto>(quyetDinh)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditQuyetDinhDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Create)]
		 protected virtual async Task Create(CreateOrEditQuyetDinhDto input)
         {
            var quyetDinh = ObjectMapper.Map<QuyetDinh>(input);

			
			if (AbpSession.TenantId != null)
			{
				quyetDinh.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _quyetDinhRepository.InsertAsync(quyetDinh);
         }

		 [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Edit)]
		 protected virtual async Task Update(CreateOrEditQuyetDinhDto input)
         {
            var quyetDinh = await _quyetDinhRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, quyetDinh);
         }

		 [AbpAuthorize(AppPermissions.Pages_QuyetDinhs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _quyetDinhRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetQuyetDinhsToExcel(GetAllQuyetDinhsForExcelInput input)
         {
			
			var filteredQuyetDinhs = _quyetDinhRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.So.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.FileQuyetDinh.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.SoFilter),  e => e.So == input.SoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(input.MinNgayBanHanhFilter != null, e => e.NgayBanHanh >= input.MinNgayBanHanhFilter)
						.WhereIf(input.MaxNgayBanHanhFilter != null, e => e.NgayBanHanh <= input.MaxNgayBanHanhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.FileQuyetDinhFilter),  e => e.FileQuyetDinh == input.FileQuyetDinhFilter)
						.WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
						.WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

			var query = (from o in filteredQuyetDinhs
                         select new GetQuyetDinhForViewDto() { 
							QuyetDinh = new QuyetDinhDto
							{
                                So = o.So,
                                Ten = o.Ten,
                                NgayBanHanh = o.NgayBanHanh,
                                FileQuyetDinh = o.FileQuyetDinh,
                                TrangThai = o.TrangThai,
                                Id = o.Id
							}
						 });


            var quyetDinhListDtos = await query.ToListAsync();

            return _quyetDinhsExcelExporter.ExportToFile(quyetDinhListDtos);
         }


    }
}