using TechBer.ChuyenDoiSo.QuanLyDanhMuc;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Exporting;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyDanhMuc
{
	[AbpAuthorize(AppPermissions.Pages_QuanHuyens)]
    public class QuanHuyensAppService : ChuyenDoiSoAppServiceBase, IQuanHuyensAppService
    {
		 private readonly IRepository<QuanHuyen> _quanHuyenRepository;
		 private readonly IQuanHuyensExcelExporter _quanHuyensExcelExporter;
		 private readonly IRepository<TinhThanh,int> _lookup_tinhThanhRepository;
		 

		  public QuanHuyensAppService(IRepository<QuanHuyen> quanHuyenRepository, IQuanHuyensExcelExporter quanHuyensExcelExporter , IRepository<TinhThanh, int> lookup_tinhThanhRepository) 
		  {
			_quanHuyenRepository = quanHuyenRepository;
			_quanHuyensExcelExporter = quanHuyensExcelExporter;
			_lookup_tinhThanhRepository = lookup_tinhThanhRepository;
		
		  }

		 public async Task<PagedResultDto<GetQuanHuyenForViewDto>> GetAll(GetAllQuanHuyensInput input)
         {
			
			var filteredQuanHuyens = _quanHuyenRepository.GetAll()
						.Include( e => e.TinhThanhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Ma.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaFilter),  e => e.Ma == input.MaFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TinhThanhNameFilter), e => e.TinhThanhFk != null && e.TinhThanhFk.Name == input.TinhThanhNameFilter);

			var pagedAndFilteredQuanHuyens = filteredQuanHuyens
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var quanHuyens = from o in pagedAndFilteredQuanHuyens
                         join o1 in _lookup_tinhThanhRepository.GetAll() on o.TinhThanhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetQuanHuyenForViewDto() {
							QuanHuyen = new QuanHuyenDto
							{
                                Name = o.Name,
                                Ma = o.Ma,
                                Id = o.Id
							},
                         	TinhThanhName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredQuanHuyens.CountAsync();

            return new PagedResultDto<GetQuanHuyenForViewDto>(
                totalCount,
                await quanHuyens.ToListAsync()
            );
         }
		 
		 public async Task<GetQuanHuyenForViewDto> GetQuanHuyenForView(int id)
         {
            var quanHuyen = await _quanHuyenRepository.GetAsync(id);

            var output = new GetQuanHuyenForViewDto { QuanHuyen = ObjectMapper.Map<QuanHuyenDto>(quanHuyen) };

		    if (output.QuanHuyen.TinhThanhId != null)
            {
                var _lookupTinhThanh = await _lookup_tinhThanhRepository.FirstOrDefaultAsync((int)output.QuanHuyen.TinhThanhId);
                output.TinhThanhName = _lookupTinhThanh?.Name?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_QuanHuyens_Edit)]
		 public async Task<GetQuanHuyenForEditOutput> GetQuanHuyenForEdit(EntityDto input)
         {
            var quanHuyen = await _quanHuyenRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetQuanHuyenForEditOutput {QuanHuyen = ObjectMapper.Map<CreateOrEditQuanHuyenDto>(quanHuyen)};

		    if (output.QuanHuyen.TinhThanhId != null)
            {
                var _lookupTinhThanh = await _lookup_tinhThanhRepository.FirstOrDefaultAsync((int)output.QuanHuyen.TinhThanhId);
                output.TinhThanhName = _lookupTinhThanh?.Name?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditQuanHuyenDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_QuanHuyens_Create)]
		 protected virtual async Task Create(CreateOrEditQuanHuyenDto input)
         {
            var quanHuyen = ObjectMapper.Map<QuanHuyen>(input);

			

            await _quanHuyenRepository.InsertAsync(quanHuyen);
         }

		 [AbpAuthorize(AppPermissions.Pages_QuanHuyens_Edit)]
		 protected virtual async Task Update(CreateOrEditQuanHuyenDto input)
         {
            var quanHuyen = await _quanHuyenRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, quanHuyen);
         }

		 [AbpAuthorize(AppPermissions.Pages_QuanHuyens_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _quanHuyenRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetQuanHuyensToExcel(GetAllQuanHuyensForExcelInput input)
         {
			
			var filteredQuanHuyens = _quanHuyenRepository.GetAll()
						.Include( e => e.TinhThanhFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.Ma.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaFilter),  e => e.Ma == input.MaFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TinhThanhNameFilter), e => e.TinhThanhFk != null && e.TinhThanhFk.Name == input.TinhThanhNameFilter);

			var query = (from o in filteredQuanHuyens
                         join o1 in _lookup_tinhThanhRepository.GetAll() on o.TinhThanhId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetQuanHuyenForViewDto() { 
							QuanHuyen = new QuanHuyenDto
							{
                                Name = o.Name,
                                Ma = o.Ma,
                                Id = o.Id
							},
                         	TinhThanhName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						 });


            var quanHuyenListDtos = await query.ToListAsync();

            return _quanHuyensExcelExporter.ExportToFile(quanHuyenListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_QuanHuyens)]
         public async Task<PagedResultDto<QuanHuyenTinhThanhLookupTableDto>> GetAllTinhThanhForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_tinhThanhRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Name != null && e.Name.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var tinhThanhList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<QuanHuyenTinhThanhLookupTableDto>();
			foreach(var tinhThanh in tinhThanhList){
				lookupTableDtoList.Add(new QuanHuyenTinhThanhLookupTableDto
				{
					Id = tinhThanh.Id,
					DisplayName = tinhThanh.Name?.ToString()
				});
			}

            return new PagedResultDto<QuanHuyenTinhThanhLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}