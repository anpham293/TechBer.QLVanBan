using TechBer.ChuyenDoiSo.QLVB;


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
using Z.EntityFramework.Plus;

namespace TechBer.ChuyenDoiSo.QLVB
{
	[AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
    public class QuyTrinhDuAnsAppService : ChuyenDoiSoAppServiceBase, IQuyTrinhDuAnsAppService
    {
		 private readonly IRepository<QuyTrinhDuAn> _quyTrinhDuAnRepository;
		 private readonly IQuyTrinhDuAnsExcelExporter _quyTrinhDuAnsExcelExporter;
		 private readonly IRepository<LoaiDuAn,int> _lookup_loaiDuAnRepository;
		 

		  public QuyTrinhDuAnsAppService(IRepository<QuyTrinhDuAn> quyTrinhDuAnRepository, IQuyTrinhDuAnsExcelExporter quyTrinhDuAnsExcelExporter , IRepository<LoaiDuAn, int> lookup_loaiDuAnRepository) 
		  {
			_quyTrinhDuAnRepository = quyTrinhDuAnRepository;
			_quyTrinhDuAnsExcelExporter = quyTrinhDuAnsExcelExporter;
			_lookup_loaiDuAnRepository = lookup_loaiDuAnRepository;
		
		  }

		 public async Task<PagedResultDto<GetQuyTrinhDuAnForViewDto>> GetAll(GetAllQuyTrinhDuAnsInput input)
         {
			
			var filteredQuyTrinhDuAns = _quyTrinhDuAnRepository.GetAll()
						.Include( e => e.LoaiDuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),  e => e.Descriptions == input.DescriptionsFilter)
						.WhereIf(input.MinSTTFilter != null, e => e.STT >= input.MinSTTFilter)
						.WhereIf(input.MaxSTTFilter != null, e => e.STT <= input.MaxSTTFilter)
						.WhereIf(input.LoaiDuAnId.HasValue, e => e.LoaiDuAnId==input.LoaiDuAnId);

			var pagedAndFilteredQuyTrinhDuAns = filteredQuyTrinhDuAns
                .OrderBy("STT asc")
                .PageBy(input);

			var quyTrinhDuAns = from o in pagedAndFilteredQuyTrinhDuAns
                         join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetQuyTrinhDuAnForViewDto() {
							QuyTrinhDuAn = new QuyTrinhDuAnDto
							{
                                Name = o.Name,
                                Descriptions = o.Descriptions,
                                STT = o.STT,
                                Id = o.Id
							},
                         	LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredQuyTrinhDuAns.CountAsync();

            return new PagedResultDto<GetQuyTrinhDuAnForViewDto>(
                totalCount,
                await quyTrinhDuAns.ToListAsync()
            );
         }
		 
		 public async Task<GetQuyTrinhDuAnForViewDto> GetQuyTrinhDuAnForView(int id)
         {
            var quyTrinhDuAn = await _quyTrinhDuAnRepository.GetAsync(id);

            var output = new GetQuyTrinhDuAnForViewDto { QuyTrinhDuAn = ObjectMapper.Map<QuyTrinhDuAnDto>(quyTrinhDuAn) };

		    if (output.QuyTrinhDuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn = await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int)output.QuyTrinhDuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Edit)]
		 public async Task<GetQuyTrinhDuAnForEditOutput> GetQuyTrinhDuAnForEdit(EntityDto input)
         {
            var quyTrinhDuAn = await _quyTrinhDuAnRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetQuyTrinhDuAnForEditOutput {QuyTrinhDuAn = ObjectMapper.Map<CreateOrEditQuyTrinhDuAnDto>(quyTrinhDuAn)};

		    if (output.QuyTrinhDuAn.LoaiDuAnId != null)
            {
                var _lookupLoaiDuAn = await _lookup_loaiDuAnRepository.FirstOrDefaultAsync((int)output.QuyTrinhDuAn.LoaiDuAnId);
                output.LoaiDuAnName = _lookupLoaiDuAn?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditQuyTrinhDuAnDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Create)]
		 protected virtual async Task Create(CreateOrEditQuyTrinhDuAnDto input)
         {
	         
            var quyTrinhDuAn = ObjectMapper.Map<QuyTrinhDuAn>(input);
			
			
			if (AbpSession.TenantId != null)
			{
				quyTrinhDuAn.TenantId = (int?) AbpSession.TenantId;
			}

			await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
					p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId && p.STT >= quyTrinhDuAn.STT)
				.UpdateAsync(d => new QuyTrinhDuAn()
				{
					STT = d.STT + 1
				});
			
            await _quyTrinhDuAnRepository.InsertAsync(quyTrinhDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Edit)]
		 protected virtual async Task Update(CreateOrEditQuyTrinhDuAnDto input)
         {
            var quyTrinhDuAn = await _quyTrinhDuAnRepository.FirstOrDefaultAsync((int)input.Id);
            if (input.STT < quyTrinhDuAn.STT)
            {
	            await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
			            p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId && p.STT >= input.STT && p.STT<quyTrinhDuAn.STT)
		            .UpdateAsync(d => new QuyTrinhDuAn()
		            {
			            STT = d.STT + 1
		            });
            }
            else
            {
	            await _quyTrinhDuAnRepository.GetAll().WhereIf(true,
			            p => p.LoaiDuAnId == quyTrinhDuAn.LoaiDuAnId && p.STT <= input.STT && p.STT>quyTrinhDuAn.STT)
		            .UpdateAsync(d => new QuyTrinhDuAn()
		            {
			            STT = d.STT - 1
		            });
            }
             ObjectMapper.Map(input, quyTrinhDuAn);
         }

		 [AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _quyTrinhDuAnRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetQuyTrinhDuAnsToExcel(GetAllQuyTrinhDuAnsForExcelInput input)
         {
			
			var filteredQuyTrinhDuAns = _quyTrinhDuAnRepository.GetAll()
						.Include( e => e.LoaiDuAnFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Descriptions.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DescriptionsFilter),  e => e.Descriptions == input.DescriptionsFilter)
						.WhereIf(input.MinSTTFilter != null, e => e.STT >= input.MinSTTFilter)
						.WhereIf(input.MaxSTTFilter != null, e => e.STT <= input.MaxSTTFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.LoaiDuAnNameFilter), e => e.LoaiDuAnFk != null && e.LoaiDuAnFk.Name == input.LoaiDuAnNameFilter);

			var query = (from o in filteredQuyTrinhDuAns
                         join o1 in _lookup_loaiDuAnRepository.GetAll() on o.LoaiDuAnId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetQuyTrinhDuAnForViewDto() { 
							QuyTrinhDuAn = new QuyTrinhDuAnDto
							{
                                Name = o.Name,
                                Descriptions = o.Descriptions,
                                STT = o.STT,
                                Id = o.Id
							},
                         	LoaiDuAnName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						 });


            var quyTrinhDuAnListDtos = await query.ToListAsync();

            return _quyTrinhDuAnsExcelExporter.ExportToFile(quyTrinhDuAnListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_QuyTrinhDuAns)]
         public async Task<PagedResultDto<QuyTrinhDuAnLoaiDuAnLookupTableDto>> GetAllLoaiDuAnForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_loaiDuAnRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var loaiDuAnList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<QuyTrinhDuAnLoaiDuAnLookupTableDto>();
			foreach(var loaiDuAn in loaiDuAnList){
				lookupTableDtoList.Add(new QuyTrinhDuAnLoaiDuAnLookupTableDto
				{
					Id = loaiDuAn.Id,
					DisplayName = loaiDuAn.Name?.ToString()
				});
			}

            return new PagedResultDto<QuyTrinhDuAnLoaiDuAnLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}