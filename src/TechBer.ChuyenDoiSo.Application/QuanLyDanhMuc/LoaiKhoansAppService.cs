

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
	[AbpAuthorize(AppPermissions.Pages_LoaiKhoans)]
    public class LoaiKhoansAppService : ChuyenDoiSoAppServiceBase, ILoaiKhoansAppService
    {
		 private readonly IRepository<LoaiKhoan> _loaiKhoanRepository;
		 private readonly ILoaiKhoansExcelExporter _loaiKhoansExcelExporter;
		 

		  public LoaiKhoansAppService(IRepository<LoaiKhoan> loaiKhoanRepository, ILoaiKhoansExcelExporter loaiKhoansExcelExporter ) 
		  {
			_loaiKhoanRepository = loaiKhoanRepository;
			_loaiKhoansExcelExporter = loaiKhoansExcelExporter;
			
		  }

		 public async Task<PagedResultDto<GetLoaiKhoanForViewDto>> GetAll(GetAllLoaiKhoansInput input)
         {
			
			var filteredLoaiKhoans = _loaiKhoanRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.GhiChu.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter),  e => e.GhiChu == input.GhiChuFilter);

			var pagedAndFilteredLoaiKhoans = filteredLoaiKhoans
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var loaiKhoans = from o in pagedAndFilteredLoaiKhoans
                         select new GetLoaiKhoanForViewDto() {
							LoaiKhoan = new LoaiKhoanDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                GhiChu = o.GhiChu,
                                Id = o.Id
							}
						};

            var totalCount = await filteredLoaiKhoans.CountAsync();

            return new PagedResultDto<GetLoaiKhoanForViewDto>(
                totalCount,
                await loaiKhoans.ToListAsync()
            );
         }
		 
		 public async Task<GetLoaiKhoanForViewDto> GetLoaiKhoanForView(int id)
         {
            var loaiKhoan = await _loaiKhoanRepository.GetAsync(id);

            var output = new GetLoaiKhoanForViewDto { LoaiKhoan = ObjectMapper.Map<LoaiKhoanDto>(loaiKhoan) };
			
            return output;
         }
		 
		 [AbpAuthorize(AppPermissions.Pages_LoaiKhoans_Edit)]
		 public async Task<GetLoaiKhoanForEditOutput> GetLoaiKhoanForEdit(EntityDto input)
         {
            var loaiKhoan = await _loaiKhoanRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetLoaiKhoanForEditOutput {LoaiKhoan = ObjectMapper.Map<CreateOrEditLoaiKhoanDto>(loaiKhoan)};
			
            return output;
         }

		 public async Task CreateOrEdit(CreateOrEditLoaiKhoanDto input)
         {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
         }

		 [AbpAuthorize(AppPermissions.Pages_LoaiKhoans_Create)]
		 protected virtual async Task Create(CreateOrEditLoaiKhoanDto input)
         {
            var loaiKhoan = ObjectMapper.Map<LoaiKhoan>(input);

			
			if (AbpSession.TenantId != null)
			{
				loaiKhoan.TenantId = (int?) AbpSession.TenantId;
			}
		

            await _loaiKhoanRepository.InsertAsync(loaiKhoan);
         }

		 [AbpAuthorize(AppPermissions.Pages_LoaiKhoans_Edit)]
		 protected virtual async Task Update(CreateOrEditLoaiKhoanDto input)
         {
            var loaiKhoan = await _loaiKhoanRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, loaiKhoan);
         }

		 [AbpAuthorize(AppPermissions.Pages_LoaiKhoans_Delete)]
         public async Task Delete(EntityDto input)
         {
            await _loaiKhoanRepository.DeleteAsync(input.Id);
         } 

		public async Task<FileDto> GetLoaiKhoansToExcel(GetAllLoaiKhoansForExcelInput input)
         {
			
			var filteredLoaiKhoans = _loaiKhoanRepository.GetAll()
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.MaSo.Contains(input.Filter) || e.Ten.Contains(input.Filter) || e.GhiChu.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.MaSoFilter),  e => e.MaSo == input.MaSoFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter),  e => e.Ten == input.TenFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter),  e => e.GhiChu == input.GhiChuFilter);

			var query = (from o in filteredLoaiKhoans
                         select new GetLoaiKhoanForViewDto() { 
							LoaiKhoan = new LoaiKhoanDto
							{
                                MaSo = o.MaSo,
                                Ten = o.Ten,
                                GhiChu = o.GhiChu,
                                Id = o.Id
							}
						 });


            var loaiKhoanListDtos = await query.ToListAsync();

            return _loaiKhoansExcelExporter.ExportToFile(loaiKhoanListDtos);
         }


    }
}