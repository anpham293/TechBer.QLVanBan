using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Exporting;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using System;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
	[AbpAuthorize(AppPermissions.Pages_CayTieuChi)]
    public class TieuChiDanhGiasAppService : ChuyenDoiSoAppServiceBase, ITieuChiDanhGiasAppService
    {
		 private readonly IRepository<TieuChiDanhGia> _tieuChiDanhGiaRepository;
		 private readonly ITieuChiDanhGiasExcelExporter _tieuChiDanhGiasExcelExporter;
		 private readonly IRepository<TieuChiDanhGia,int> _lookup_tieuChiDanhGiaRepository;
		 

		  public TieuChiDanhGiasAppService(IRepository<TieuChiDanhGia> tieuChiDanhGiaRepository, ITieuChiDanhGiasExcelExporter tieuChiDanhGiasExcelExporter , IRepository<TieuChiDanhGia, int> lookup_tieuChiDanhGiaRepository) 
		  {
			_tieuChiDanhGiaRepository = tieuChiDanhGiaRepository;
			_tieuChiDanhGiasExcelExporter = tieuChiDanhGiasExcelExporter;
			_lookup_tieuChiDanhGiaRepository = lookup_tieuChiDanhGiaRepository;
		
		  }

		 public async Task<PagedResultDto<GetTieuChiDanhGiaForViewDto>> GetAll(GetAllTieuChiDanhGiasInput input)
         {
			
			var filteredTieuChiDanhGias = _tieuChiDanhGiaRepository.GetAll()
						.Include( e => e.ParentFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter) || e.PhuongThucDanhGia.Contains(input.Filter) || e.TaiLieuGiaiTrinh.Contains(input.Filter) || e.GhiChu.Contains(input.Filter) || e.SoThuTu.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
						.WhereIf(input.MinDiemToiDaFilter != null, e => e.DiemToiDa >= input.MinDiemToiDaFilter)
						.WhereIf(input.MaxDiemToiDaFilter != null, e => e.DiemToiDa <= input.MaxDiemToiDaFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.PhuongThucDanhGiaFilter),  e => e.PhuongThucDanhGia == input.PhuongThucDanhGiaFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TaiLieuGiaiTrinhFilter),  e => e.TaiLieuGiaiTrinh == input.TaiLieuGiaiTrinhFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter),  e => e.GhiChu == input.GhiChuFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.SoThuTuFilter), e => e.SoThuTu == input.SoThuTuFilter)
						.WhereIf(input.MinLoaiPhuLucFilter != null, e => e.LoaiPhuLuc >= input.MinLoaiPhuLucFilter)
						.WhereIf(input.MaxLoaiPhuLucFilter != null, e => e.LoaiPhuLuc <= input.MaxLoaiPhuLucFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.TieuChiDanhGiaNameFilter), e => e.ParentFk != null && e.ParentFk.Name == input.TieuChiDanhGiaNameFilter);

			var pagedAndFilteredTieuChiDanhGias = filteredTieuChiDanhGias
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var tieuChiDanhGias = from o in pagedAndFilteredTieuChiDanhGias
                         join o1 in _lookup_tieuChiDanhGiaRepository.GetAll() on o.ParentId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetTieuChiDanhGiaForViewDto() {
							TieuChiDanhGia = new TieuChiDanhGiaDto
							{
                                Name = o.Name,
                                SoThuTu = o.SoThuTu,
                                DiemToiDa = o.DiemToiDa,
                                PhuongThucDanhGia = o.PhuongThucDanhGia,
                                TaiLieuGiaiTrinh = o.TaiLieuGiaiTrinh,
                                GhiChu = o.GhiChu,
                                LoaiPhuLuc = o.LoaiPhuLuc,
                                Id = o.Id
							},
                         	TieuChiDanhGiaName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredTieuChiDanhGias.CountAsync();

            return new PagedResultDto<GetTieuChiDanhGiaForViewDto>(
                totalCount,
                await tieuChiDanhGias.OrderBy(o => o.TieuChiDanhGia.SoThuTu).ToListAsync()
            );
         }
		 
        public async Task<List<GetTieuChiDanhGiaForViewDto>> GetTieuChiForTree(int type)
        {
            var filteredTieuChiDanhGias = _tieuChiDanhGiaRepository.GetAll()
                        .Where(p => p.LoaiPhuLuc == type);

            var query = from o in filteredTieuChiDanhGias
                        select new GetTieuChiDanhGiaForViewDto()
                        {
                            TieuChiDanhGia = new TieuChiDanhGiaDto
                            {
                                Name = o.Name,
                                DiemToiDa = o.DiemToiDa,
                                PhuongThucDanhGia = o.PhuongThucDanhGia,
                                TaiLieuGiaiTrinh = o.TaiLieuGiaiTrinh,
                                GhiChu = o.GhiChu,
                                LoaiPhuLuc = o.LoaiPhuLuc,
                                SoThuTu = o.SoThuTu,
                                Id = o.Id,
                                ParentId = o.ParentId,
                                SapXep = o.SapXep
                            },
                        };

            return await query.ToListAsync();
        }

        public async Task<GetTieuChiDanhGiaForEditOutput> CreateOrEditTieuChi(CreateOrEditTieuChiDanhGiaDto input)
        {
            if(input.Id == null)
            {
                return await CreateTieuChi(input);
            }
            else
            {
                return await EditTieuChi(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CayTieuChi_ManageTree)]
        protected virtual async Task<GetTieuChiDanhGiaForEditOutput> CreateTieuChi(CreateOrEditTieuChiDanhGiaDto input)
        {
            var tieuChiDanhGia = ObjectMapper.Map<TieuChiDanhGia>(input);

            if (AbpSession.TenantId != null)
            {
                tieuChiDanhGia.TenantId = (int)AbpSession.TenantId;
            }

            var id = await _tieuChiDanhGiaRepository.InsertAndGetIdAsync(tieuChiDanhGia);
            tieuChiDanhGia.Id = id;

            var output = new GetTieuChiDanhGiaForEditOutput { TieuChiDanhGia = ObjectMapper.Map<CreateOrEditTieuChiDanhGiaDto>(tieuChiDanhGia) };
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CayTieuChi_ManageTree)]
        protected virtual async Task<GetTieuChiDanhGiaForEditOutput> EditTieuChi(CreateOrEditTieuChiDanhGiaDto input)
        {
            var tieuChiDanhGia = await _tieuChiDanhGiaRepository.FirstOrDefaultAsync((int)input.Id);
            tieuChiDanhGia.Name = input.Name;
            tieuChiDanhGia.PhuongThucDanhGia = input.PhuongThucDanhGia;
            tieuChiDanhGia.SoThuTu = input.SoThuTu;
            tieuChiDanhGia.TaiLieuGiaiTrinh = input.TaiLieuGiaiTrinh;
            tieuChiDanhGia.GhiChu = input.GhiChu;
            tieuChiDanhGia.DiemToiDa = input.DiemToiDa;
            tieuChiDanhGia.SapXep = input.SapXep;
            tieuChiDanhGia.PhanNhomLevel1 = input.PhanNhomLevel1;
            var output = new GetTieuChiDanhGiaForEditOutput { TieuChiDanhGia = ObjectMapper.Map<CreateOrEditTieuChiDanhGiaDto>(tieuChiDanhGia) };
            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_CayTieuChi_Delete)]
        public async Task<int> XoaTieuChi(int id)
        {
            try
            {
                var query = _tieuChiDanhGiaRepository.GetAll().Where(p => p.ParentId == id);

                if(await query.CountAsync() > 0)
                {
                    return (int)XoaTieuChiState.CHUA_XOA_HET_CON;
                }

                await _tieuChiDanhGiaRepository.DeleteAsync(id);

                return (int)XoaTieuChiState.XOA_THANH_CONG;
            }
            catch (Exception ex)
            {
                return (int)XoaTieuChiState.LOI_KHAC;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CayTieuChi_ManageTree)]
        public async Task<int> MoveTieuChi(MoveTieuChiDanhGiaInput input)
        {
            try
            {
                var tieuChi = await _tieuChiDanhGiaRepository.GetAsync(input.Id);
                tieuChi.ParentId = input.NewParentId;
                return (int)DiChuyenTieuChiState.DI_CHUYEN_OK;

            }catch(Exception ex)
            {
                return (int)DiChuyenTieuChiState.DI_CHUYEN_THAT_BAI;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_CayTieuChi_ManageTree)]
        public async Task ThietLapChieuSauChoTieuChi()
        {
            var dsTieuChi = await _tieuChiDanhGiaRepository.GetAll().ToListAsync();

            // perform DFS
            // khởi tạo stack
            var tieuChiStack = new Stack<TieuChiDanhGia>();
            
            foreach (var root in dsTieuChi.Where(p => p.ParentId == null))
            {
                root.DoSau = 0;
                tieuChiStack.Push(root);
            }

            // duyệt đến khi stack rỗng
            while (tieuChiStack.Count > 0)
            {
                // lấy phần tử đầu tiên
                var top = tieuChiStack.Pop();
                var childs = dsTieuChi.Where(p => p.ParentId == top.Id);

                // cập nhật chiều sâu và đẩy vào stack
                foreach (var child in childs)
                {
                    child.DoSau = top.DoSau + 1;
                    tieuChiStack.Push(child);
                }
            }
            // end DFS
        }
    }
}