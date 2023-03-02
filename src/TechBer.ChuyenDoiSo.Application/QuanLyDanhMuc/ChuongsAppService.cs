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
	[AbpAuthorize(AppPermissions.Pages_Chuongs)]
    public class ChuongsAppService : ChuyenDoiSoAppServiceBase, IChuongsAppService
    {
		 private readonly IRepository<Chuong> _chuongRepository;
		 private readonly IChuongsExcelExporter _chuongsExcelExporter;
		 private readonly IRepository<CapQuanLy,int> _lookup_capQuanLyRepository;
		 

		  public ChuongsAppService(IRepository<Chuong> chuongRepository, IChuongsExcelExporter chuongsExcelExporter , IRepository<CapQuanLy, int> lookup_capQuanLyRepository) 
		  {
			_chuongRepository = chuongRepository;
			_chuongsExcelExporter = chuongsExcelExporter;
			_lookup_capQuanLyRepository = lookup_capQuanLyRepository;
		
		  }

		 public async Task<PagedResultDto<GetChuongForViewDto>> GetAll(GetAllChuongsInput input)
         {
			
			var filteredChuongs = _chuongRepository.GetAll()
						.Include( e => e.CapQuanLyFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CapQuanLyTenFilter), e => e.CapQuanLyFk != null && e.CapQuanLyFk.Ten == input.CapQuanLyTenFilter);

			var pagedAndFilteredChuongs = filteredChuongs
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chuongs = from o in pagedAndFilteredChuongs
                         join o1 in _lookup_capQuanLyRepository.GetAll() on o.CapQuanLyId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetChuongForViewDto() {
							Chuong = new ChuongDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                Id = o.Id
							},
                         	CapQuanLyTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
						};

            var totalCount = await filteredChuongs.CountAsync();

            return new PagedResultDto<GetChuongForViewDto>(
                totalCount,
                await chuongs.ToListAsync()
            );
         }
		 
		 public async Task<GetChuongForViewDto> GetChuongForView(int id)
         {
            var chuong = await _chuongRepository.GetAsync(id);

            var output = new GetChuongForViewDto { Chuong = ObjectMapper.Map<ChuongDto>(chuong) };

		    if (output.Chuong.CapQuanLyId != null)
            {
                var _lookupCapQuanLy = await _lookup_capQuanLyRepository.FirstOrDefaultAsync((int)output.Chuong.CapQuanLyId);
                output.CapQuanLyTen = _lookupCapQuanLy?.Ten?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_Chuongs_Edit)]
		 public async Task<GetChuongForEditOutput> GetChuongForEdit(EntityDto input)
         {
            var chuong = await _chuongRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChuongForEditOutput {Chuong = ObjectMapper.Map<CreateOrEditChuongDto>(chuong)};

		    if (output.Chuong.CapQuanLyId != null)
            {
                var _lookupCapQuanLy = await _lookup_capQuanLyRepository.FirstOrDefaultAsync((int)output.Chuong.CapQuanLyId);
                output.CapQuanLyTen = _lookupCapQuanLy?.Ten?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChuongDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_Chuongs_Create)]
		 protected virtual async Task Create(CreateOrEditChuongDto input)
         {
            var chuong = ObjectMapper.Map<Chuong>(input);

			
			if (AbpSession.TenantId != null)
			{
				chuong.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _chuongRepository.InsertAsync(chuong);
         }

		 [AbpAuthorize(AppPermissions.Pages_Chuongs_Edit)]
		 protected virtual async Task Update(CreateOrEditChuongDto input)
         {
            var chuong = await _chuongRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, chuong);
         }

		 [AbpAuthorize(AppPermissions.Pages_Chuongs_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _chuongRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetChuongsToExcel(GetAllChuongsForExcelInput input)
         {
			
			var filteredChuongs = _chuongRepository.GetAll()
						.Include( e => e.CapQuanLyFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.CapQuanLyTenFilter), e => e.CapQuanLyFk != null && e.CapQuanLyFk.Ten == input.CapQuanLyTenFilter);

			var query = (from o in filteredChuongs
                         join o1 in _lookup_capQuanLyRepository.GetAll() on o.CapQuanLyId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetChuongForViewDto() { 
							Chuong = new ChuongDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                Id = o.Id
							},
                         	CapQuanLyTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
						 });


            var chuongListDtos = await query.ToListAsync();

            return _chuongsExcelExporter.ExportToFile(chuongListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_Chuongs)]
         public async Task<PagedResultDto<ChuongCapQuanLyLookupTableDto>> GetAllCapQuanLyForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_capQuanLyRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Ten != null && e.Ten.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var capQuanLyList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChuongCapQuanLyLookupTableDto>();
			foreach(var capQuanLy in capQuanLyList){
				lookupTableDtoList.Add(new ChuongCapQuanLyLookupTableDto
				{
					Id = capQuanLy.Id,
					DisplayName = capQuanLy.Ten?.ToString()
				});
			}

            return new PagedResultDto<ChuongCapQuanLyLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}