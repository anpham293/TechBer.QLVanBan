using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng;


using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
	[AbpAuthorize(AppPermissions.Pages_DanhMucThuHoies)]
    public class DanhMucThuHoiesAppService : ChuyenDoiSoAppServiceBase, IDanhMucThuHoiesAppService
    {
		 private readonly IRepository<DanhMucThuHoi, long> _danhMucThuHoiRepository;
		 private readonly IDanhMucThuHoiesExcelExporter _danhMucThuHoiesExcelExporter;
		 private readonly IRepository<DuAnThuHoi,long> _lookup_duAnThuHoiRepository;
		 private readonly IRepository<ChiTietThuHoi,long> _chiTietThuHoiRepository;
		 

		  public DanhMucThuHoiesAppService(IRepository<DanhMucThuHoi, long> danhMucThuHoiRepository, 
										   IDanhMucThuHoiesExcelExporter danhMucThuHoiesExcelExporter,
										   IRepository<DuAnThuHoi, long> lookup_duAnThuHoiRepository,
										   IRepository<ChiTietThuHoi,long> chiTietThuHoiRepository
										   ) 
		  {
			_danhMucThuHoiRepository = danhMucThuHoiRepository;
			_danhMucThuHoiesExcelExporter = danhMucThuHoiesExcelExporter;
			_lookup_duAnThuHoiRepository = lookup_duAnThuHoiRepository;
			_chiTietThuHoiRepository = chiTietThuHoiRepository;
		  }

		 public async Task<PagedResultDto<GetDanhMucThuHoiForViewDto>> GetAll(GetAllDanhMucThuHoiesInput input)
         {
			
			var filteredDanhMucThuHoies = _danhMucThuHoiRepository.GetAll()
						.Include( e => e.DuAnThuHoiFk)
						// .WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Stt.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.GhiChu.Contains(input.Filter))
						// .WhereIf(!string.IsNullOrWhiteSpace(input.SttFilter),  e => e.Stt == input.SttFilter)
						// .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						// .WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter),  e => e.GhiChu == input.GhiChuFilter)
						// .WhereIf(input.MinTypeFilter != null, e => e.Type >= input.MinTypeFilter)
						// .WhereIf(input.MaxTypeFilter != null, e => e.Type <= input.MaxTypeFilter)
						//.WhereIf(!string.IsNullOrWhiteSpace(input.DuAnThuHoiMaDATHFilter), e => e.DuAnThuHoiFk != null && e.DuAnThuHoiFk.MaDATH == input.DuAnThuHoiMaDATHFilter)
						.WhereIf(true, e => e.DuAnThuHoiId == long.Parse(input.DuAnThuHoiMaDATHFilter))
				;

			var pagedAndFilteredDanhMucThuHoies = filteredDanhMucThuHoies
                .OrderBy(input.Sorting ?? "stt asc")
                .PageBy(input);

			var danhMucThuHoies = await (from o in pagedAndFilteredDanhMucThuHoies
                         join o1 in _lookup_duAnThuHoiRepository.GetAll() on o.DuAnThuHoiId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDanhMucThuHoiForViewDto() {
							DanhMucThuHoi = new DanhMucThuHoiDto
							{
                                Stt = o.Stt,
                                Ten = o.Ten,
                                GhiChu = o.GhiChu,
                                Type = o.Type,
                                Id = o.Id
							},
                         	DuAnThuHoiMaDATH = s1 == null || s1.MaDATH == null ? "" : s1.MaDATH.ToString()
						}).ToListAsync();

				foreach (var dm in danhMucThuHoies)
				{
					var listChiTiet = _chiTietThuHoiRepository.GetAll()
						.WhereIf(true, p => p.DanhMucThuHoiId == dm.DanhMucThuHoi.Id).ToList();
					decimal tongDu = 0;
					decimal tongThu = 0;
					if (!listChiTiet.IsNullOrEmpty())
					{
						tongDu = listChiTiet.Sum(p => p.TongDu);
						tongThu = listChiTiet.Sum(p => p.TongThu);
					}
					
					dm.TongDuDanhMuc = tongDu;
					dm.TongThuDanhMuc = tongThu;
				}
			
	         
            var totalCount = await filteredDanhMucThuHoies.CountAsync();

            return new PagedResultDto<GetDanhMucThuHoiForViewDto>(
                totalCount,
	            danhMucThuHoies
            );
         }
		 
		 public async Task<GetDanhMucThuHoiForViewDto> GetDanhMucThuHoiForView(long id)
         {
            var danhMucThuHoi = await _danhMucThuHoiRepository.GetAsync(id);

            var output = new GetDanhMucThuHoiForViewDto { DanhMucThuHoi = ObjectMapper.Map<DanhMucThuHoiDto>(danhMucThuHoi) };

		    if (output.DanhMucThuHoi.DuAnThuHoiId != null)
            {
                var _lookupDuAnThuHoi = await _lookup_duAnThuHoiRepository.FirstOrDefaultAsync((long)output.DanhMucThuHoi.DuAnThuHoiId);
                output.DuAnThuHoiMaDATH = _lookupDuAnThuHoi?.MaDATH?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_DanhMucThuHoies_Edit)]
		 public async Task<GetDanhMucThuHoiForEditOutput> GetDanhMucThuHoiForEdit(EntityDto<long> input)
         {
            var danhMucThuHoi = await _danhMucThuHoiRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDanhMucThuHoiForEditOutput {DanhMucThuHoi = ObjectMapper.Map<CreateOrEditDanhMucThuHoiDto>(danhMucThuHoi)};

		    if (output.DanhMucThuHoi.DuAnThuHoiId != null)
            {
                var _lookupDuAnThuHoi = await _lookup_duAnThuHoiRepository.FirstOrDefaultAsync((long)output.DanhMucThuHoi.DuAnThuHoiId);
                output.DuAnThuHoiMaDATH = _lookupDuAnThuHoi?.MaDATH?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditDanhMucThuHoiDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_DanhMucThuHoies_Create)]
		 protected virtual async Task Create(CreateOrEditDanhMucThuHoiDto input)
         {
            var danhMucThuHoi = ObjectMapper.Map<DanhMucThuHoi>(input);

			

            await _danhMucThuHoiRepository.InsertAsync(danhMucThuHoi);
         }

		 [AbpAuthorize(AppPermissions.Pages_DanhMucThuHoies_Edit)]
		 protected virtual async Task Update(CreateOrEditDanhMucThuHoiDto input)
         {
            var danhMucThuHoi = await _danhMucThuHoiRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, danhMucThuHoi);
         }

		 [AbpAuthorize(AppPermissions.Pages_DanhMucThuHoies_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _danhMucThuHoiRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetDanhMucThuHoiesToExcel(GetAllDanhMucThuHoiesForExcelInput input)
         {
			
			var filteredDanhMucThuHoies = _danhMucThuHoiRepository.GetAll()
						.Include( e => e.DuAnThuHoiFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Stt.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.GhiChu.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.SttFilter),  e => e.Stt == input.SttFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter),  e => e.GhiChu == input.GhiChuFilter)
						.WhereIf(input.MinTypeFilter != null, e => e.Type >= input.MinTypeFilter)
						.WhereIf(input.MaxTypeFilter != null, e => e.Type <= input.MaxTypeFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DuAnThuHoiMaDATHFilter), e => e.DuAnThuHoiFk != null && e.DuAnThuHoiFk.MaDATH == input.DuAnThuHoiMaDATHFilter);

			var query = (from o in filteredDanhMucThuHoies
                         join o1 in _lookup_duAnThuHoiRepository.GetAll() on o.DuAnThuHoiId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDanhMucThuHoiForViewDto() { 
							DanhMucThuHoi = new DanhMucThuHoiDto
							{
                                Stt = o.Stt,
                                Ten = o.Ten,
                                GhiChu = o.GhiChu,
                                Type = o.Type,
                                Id = o.Id
							},
                         	DuAnThuHoiMaDATH = s1 == null || s1.MaDATH == null ? "" : s1.MaDATH.ToString()
						 });


            var danhMucThuHoiListDtos = await query.ToListAsync();

            return _danhMucThuHoiesExcelExporter.ExportToFile(danhMucThuHoiListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_DanhMucThuHoies)]
         public async Task<PagedResultDto<DanhMucThuHoiDuAnThuHoiLookupTableDto>> GetAllDuAnThuHoiForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_duAnThuHoiRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.MaDATH != null && e.MaDATH.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var duAnThuHoiList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<DanhMucThuHoiDuAnThuHoiLookupTableDto>();
			foreach(var duAnThuHoi in duAnThuHoiList){
				lookupTableDtoList.Add(new DanhMucThuHoiDuAnThuHoiLookupTableDto
				{
					Id = duAnThuHoi.Id,
					DisplayName = duAnThuHoi.MaDATH?.ToString()
				});
			}

            return new PagedResultDto<DanhMucThuHoiDuAnThuHoiLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}