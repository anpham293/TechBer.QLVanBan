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
using Microsoft.EntityFrameworkCore;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
	[AbpAuthorize(AppPermissions.Pages_ChiTietThuHoies)]
    public class ChiTietThuHoiesAppService : ChuyenDoiSoAppServiceBase, IChiTietThuHoiesAppService
    {
		 private readonly IRepository<ChiTietThuHoi, long> _chiTietThuHoiRepository;
		 private readonly IChiTietThuHoiesExcelExporter _chiTietThuHoiesExcelExporter;
		 private readonly IRepository<DanhMucThuHoi,long> _lookup_danhMucThuHoiRepository;
		 

		  public ChiTietThuHoiesAppService(IRepository<ChiTietThuHoi, long> chiTietThuHoiRepository, IChiTietThuHoiesExcelExporter chiTietThuHoiesExcelExporter , IRepository<DanhMucThuHoi, long> lookup_danhMucThuHoiRepository) 
		  {
			_chiTietThuHoiRepository = chiTietThuHoiRepository;
			_chiTietThuHoiesExcelExporter = chiTietThuHoiesExcelExporter;
			_lookup_danhMucThuHoiRepository = lookup_danhMucThuHoiRepository;
		
		  }

		 public async Task<PagedResultDto<GetChiTietThuHoiForViewDto>> GetAll(GetAllChiTietThuHoiesInput input)
		 {

			 var filteredChiTietThuHoies = _chiTietThuHoiRepository.GetAll()
					 .Include(e => e.DanhMucThuHoiFk)
					 .WhereIf(true, e => e.DanhMucThuHoiId == long.Parse(input.DanhMucThuHoiTenFilter))
				 ;

			var pagedAndFilteredChiTietThuHoies = filteredChiTietThuHoies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var chiTietThuHoies = from o in pagedAndFilteredChiTietThuHoies
                         join o1 in _lookup_danhMucThuHoiRepository.GetAll() on o.DanhMucThuHoiId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetChiTietThuHoiForViewDto() {
							ChiTietThuHoi = new ChiTietThuHoiDto
							{
                                Du1 = o.Du1,
                                Du2 = o.Du2,
                                Du3 = o.Du3,
                                Du4 = o.Du4,
                                Du5 = o.Du5,
                                Du6 = o.Du6,
                                Du7 = o.Du7,
                                Du8 = o.Du8,
                                Du9 = o.Du9,
                                Du10 = o.Du10,
                                Du11 = o.Du11,
                                Du12 = o.Du12,
                                Thu1 = o.Thu1,
                                Thu2 = o.Thu2,
                                Thu3 = o.Thu3,
                                Thu4 = o.Thu4,
                                Thu5 = o.Thu5,
                                Thu6 = o.Thu6,
                                Thu7 = o.Thu7,
                                Thu8 = o.Thu8,
                                Thu9 = o.Thu9,
                                Thu10 = o.Thu10,
                                Thu11 = o.Thu11,
                                Thu12 = o.Thu12,
                                GhiChu = o.GhiChu,
                                Ten = o.Ten,
                                Id = o.Id,
                                TongDu = o.TongDu,
                                TongThu = o.TongThu
							},
                         	DanhMucThuHoiTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
						};

            var totalCount = await filteredChiTietThuHoies.CountAsync();

            return new PagedResultDto<GetChiTietThuHoiForViewDto>(
                totalCount,
                await chiTietThuHoies.ToListAsync()
            );
         }
		 
		 public async Task<GetChiTietThuHoiForViewDto> GetChiTietThuHoiForView(long id)
         {
            var chiTietThuHoi = await _chiTietThuHoiRepository.GetAsync(id);

            var output = new GetChiTietThuHoiForViewDto { ChiTietThuHoi = ObjectMapper.Map<ChiTietThuHoiDto>(chiTietThuHoi) };

		    if (output.ChiTietThuHoi.DanhMucThuHoiId != null)
            {
                var _lookupDanhMucThuHoi = await _lookup_danhMucThuHoiRepository.FirstOrDefaultAsync((long)output.ChiTietThuHoi.DanhMucThuHoiId);
                output.DanhMucThuHoiTen = _lookupDanhMucThuHoi?.Ten?.ToString();
            }
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_ChiTietThuHoies_Edit)]
		 public async Task<GetChiTietThuHoiForEditOutput> GetChiTietThuHoiForEdit(EntityDto<long> input)
         {
            var chiTietThuHoi = await _chiTietThuHoiRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetChiTietThuHoiForEditOutput {ChiTietThuHoi = ObjectMapper.Map<CreateOrEditChiTietThuHoiDto>(chiTietThuHoi)};

		    if (output.ChiTietThuHoi.DanhMucThuHoiId != null)
            {
                var _lookupDanhMucThuHoi = await _lookup_danhMucThuHoiRepository.FirstOrDefaultAsync((long)output.ChiTietThuHoi.DanhMucThuHoiId);
                output.DanhMucThuHoiTen = _lookupDanhMucThuHoi?.Ten?.ToString();
            }
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditChiTietThuHoiDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_ChiTietThuHoies_Create)]
		 protected virtual async Task Create(CreateOrEditChiTietThuHoiDto input)
         {
            var chiTietThuHoi = ObjectMapper.Map<ChiTietThuHoi>(input);

			

            await _chiTietThuHoiRepository.InsertAsync(chiTietThuHoi);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChiTietThuHoies_Edit)]
		 protected virtual async Task Update(CreateOrEditChiTietThuHoiDto input)
         {
            var chiTietThuHoi = await _chiTietThuHoiRepository.FirstOrDefaultAsync((long)input.Id);
             ObjectMapper.Map(input, chiTietThuHoi);
         }

		 [AbpAuthorize(AppPermissions.Pages_ChiTietThuHoies_Delete)]
         public async Task Delete(EntityDto<long> input)
         {
            await _chiTietThuHoiRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetChiTietThuHoiesToExcel(GetAllChiTietThuHoiesForExcelInput input)
         {
			
			var filteredChiTietThuHoies = _chiTietThuHoiRepository.GetAll()
						.Include( e => e.DanhMucThuHoiFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.GhiChu.Contains(input.Filter) || e.Ten.Contains(input.Filter))
						.WhereIf(input.MinDu1Filter != null, e => e.Du1 >= input.MinDu1Filter)
						.WhereIf(input.MaxDu1Filter != null, e => e.Du1 <= input.MaxDu1Filter)
						.WhereIf(input.MinDu2Filter != null, e => e.Du2 >= input.MinDu2Filter)
						.WhereIf(input.MaxDu2Filter != null, e => e.Du2 <= input.MaxDu2Filter)
						.WhereIf(input.MinDu3Filter != null, e => e.Du3 >= input.MinDu3Filter)
						.WhereIf(input.MaxDu3Filter != null, e => e.Du3 <= input.MaxDu3Filter)
						.WhereIf(input.MinDu4Filter != null, e => e.Du4 >= input.MinDu4Filter)
						.WhereIf(input.MaxDu4Filter != null, e => e.Du4 <= input.MaxDu4Filter)
						.WhereIf(input.MinDu5Filter != null, e => e.Du5 >= input.MinDu5Filter)
						.WhereIf(input.MaxDu5Filter != null, e => e.Du5 <= input.MaxDu5Filter)
						.WhereIf(input.MinDu6Filter != null, e => e.Du6 >= input.MinDu6Filter)
						.WhereIf(input.MaxDu6Filter != null, e => e.Du6 <= input.MaxDu6Filter)
						.WhereIf(input.MinDu7Filter != null, e => e.Du7 >= input.MinDu7Filter)
						.WhereIf(input.MaxDu7Filter != null, e => e.Du7 <= input.MaxDu7Filter)
						.WhereIf(input.MinDu8Filter != null, e => e.Du8 >= input.MinDu8Filter)
						.WhereIf(input.MaxDu8Filter != null, e => e.Du8 <= input.MaxDu8Filter)
						.WhereIf(input.MinDu9Filter != null, e => e.Du9 >= input.MinDu9Filter)
						.WhereIf(input.MaxDu9Filter != null, e => e.Du9 <= input.MaxDu9Filter)
						.WhereIf(input.MinDu10Filter != null, e => e.Du10 >= input.MinDu10Filter)
						.WhereIf(input.MaxDu10Filter != null, e => e.Du10 <= input.MaxDu10Filter)
						.WhereIf(input.MinDu11Filter != null, e => e.Du11 >= input.MinDu11Filter)
						.WhereIf(input.MaxDu11Filter != null, e => e.Du11 <= input.MaxDu11Filter)
						.WhereIf(input.MinDu12Filter != null, e => e.Du12 >= input.MinDu12Filter)
						.WhereIf(input.MaxDu12Filter != null, e => e.Du12 <= input.MaxDu12Filter)
						.WhereIf(input.MinThu1Filter != null, e => e.Thu1 >= input.MinThu1Filter)
						.WhereIf(input.MaxThu1Filter != null, e => e.Thu1 <= input.MaxThu1Filter)
						.WhereIf(input.MinThu2Filter != null, e => e.Thu2 >= input.MinThu2Filter)
						.WhereIf(input.MaxThu2Filter != null, e => e.Thu2 <= input.MaxThu2Filter)
						.WhereIf(input.MinThu3Filter != null, e => e.Thu3 >= input.MinThu3Filter)
						.WhereIf(input.MaxThu3Filter != null, e => e.Thu3 <= input.MaxThu3Filter)
						.WhereIf(input.MinThu4Filter != null, e => e.Thu4 >= input.MinThu4Filter)
						.WhereIf(input.MaxThu4Filter != null, e => e.Thu4 <= input.MaxThu4Filter)
						.WhereIf(input.MinThu5Filter != null, e => e.Thu5 >= input.MinThu5Filter)
						.WhereIf(input.MaxThu5Filter != null, e => e.Thu5 <= input.MaxThu5Filter)
						.WhereIf(input.MinThu6Filter != null, e => e.Thu6 >= input.MinThu6Filter)
						.WhereIf(input.MaxThu6Filter != null, e => e.Thu6 <= input.MaxThu6Filter)
						.WhereIf(input.MinThu7Filter != null, e => e.Thu7 >= input.MinThu7Filter)
						.WhereIf(input.MaxThu7Filter != null, e => e.Thu7 <= input.MaxThu7Filter)
						.WhereIf(input.MinThu8Filter != null, e => e.Thu8 >= input.MinThu8Filter)
						.WhereIf(input.MaxThu8Filter != null, e => e.Thu8 <= input.MaxThu8Filter)
						.WhereIf(input.MinThu9Filter != null, e => e.Thu9 >= input.MinThu9Filter)
						.WhereIf(input.MaxThu9Filter != null, e => e.Thu9 <= input.MaxThu9Filter)
						.WhereIf(input.MinThu10Filter != null, e => e.Thu10 >= input.MinThu10Filter)
						.WhereIf(input.MaxThu10Filter != null, e => e.Thu10 <= input.MaxThu10Filter)
						.WhereIf(input.MinThu11Filter != null, e => e.Thu11 >= input.MinThu11Filter)
						.WhereIf(input.MaxThu11Filter != null, e => e.Thu11 <= input.MaxThu11Filter)
						.WhereIf(input.MinThu12Filter != null, e => e.Thu12 >= input.MinThu12Filter)
						.WhereIf(input.MaxThu12Filter != null, e => e.Thu12 <= input.MaxThu12Filter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter),  e => e.GhiChu == input.GhiChuFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.DanhMucThuHoiTenFilter), e => e.DanhMucThuHoiFk != null && e.DanhMucThuHoiFk.Ten == input.DanhMucThuHoiTenFilter);

			var query = (from o in filteredChiTietThuHoies
                         join o1 in _lookup_danhMucThuHoiRepository.GetAll() on o.DanhMucThuHoiId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetChiTietThuHoiForViewDto() { 
							ChiTietThuHoi = new ChiTietThuHoiDto
							{
                                Du1 = o.Du1,
                                Du2 = o.Du2,
                                Du3 = o.Du3,
                                Du4 = o.Du4,
                                Du5 = o.Du5,
                                Du6 = o.Du6,
                                Du7 = o.Du7,
                                Du8 = o.Du8,
                                Du9 = o.Du9,
                                Du10 = o.Du10,
                                Du11 = o.Du11,
                                Du12 = o.Du12,
                                Thu1 = o.Thu1,
                                Thu2 = o.Thu2,
                                Thu3 = o.Thu3,
                                Thu4 = o.Thu4,
                                Thu5 = o.Thu5,
                                Thu6 = o.Thu6,
                                Thu7 = o.Thu7,
                                Thu8 = o.Thu8,
                                Thu9 = o.Thu9,
                                Thu10 = o.Thu10,
                                Thu11 = o.Thu11,
                                Thu12 = o.Thu12,
                                GhiChu = o.GhiChu,
                                Ten = o.Ten,
                                Id = o.Id
							},
                         	DanhMucThuHoiTen = s1 == null || s1.Ten == null ? "" : s1.Ten.ToString()
						 });


            var chiTietThuHoiListDtos = await query.ToListAsync();

            return _chiTietThuHoiesExcelExporter.ExportToFile(chiTietThuHoiListDtos);
         }



		[AbpAuthorize(AppPermissions.Pages_ChiTietThuHoies)]
         public async Task<PagedResultDto<ChiTietThuHoiDanhMucThuHoiLookupTableDto>> GetAllDanhMucThuHoiForLookupTable(GetAllForLookupTableInput input)
         {
             var query = _lookup_danhMucThuHoiRepository.GetAll().WhereIf(
                    !string.IsNullOrWhiteSpace(input.Filter),
                   e=> e.Ten != null && e.Ten.Contains(input.Filter)
                );

            var totalCount = await query.CountAsync();

            var danhMucThuHoiList = await query
                .PageBy(input)
                .ToListAsync();

			var lookupTableDtoList = new List<ChiTietThuHoiDanhMucThuHoiLookupTableDto>();
			foreach(var danhMucThuHoi in danhMucThuHoiList){
				lookupTableDtoList.Add(new ChiTietThuHoiDanhMucThuHoiLookupTableDto
				{
					Id = danhMucThuHoi.Id,
					DisplayName = danhMucThuHoi.Ten?.ToString()
				});
			}

            return new PagedResultDto<ChiTietThuHoiDanhMucThuHoiLookupTableDto>(
                totalCount,
                lookupTableDtoList
            );
         }
    }
}