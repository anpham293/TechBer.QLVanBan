using TechBer.ChuyenDoiSo.Authorization.Users;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Exporting;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Abp.UI;
using TechBer.ChuyenDoiSo.Storage;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Text.RegularExpressions;
using System.Text;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
	[AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos)]
    public class DoiTuongChuyenDoiSosAppService : ChuyenDoiSoAppServiceBase, IDoiTuongChuyenDoiSosAppService
    {
        private const int MaxFileBytes = 5242880; //5MB
        private readonly IRepository<DoiTuongChuyenDoiSo> _doiTuongChuyenDoiSoRepository;
		private readonly IDoiTuongChuyenDoiSosExcelExporter _doiTuongChuyenDoiSosExcelExporter;
		private readonly IRepository<User,long> _lookup_userRepository;
        private readonly IRepository<ChiTietDanhGia> _chiTietDanhGiaRepository;
        private readonly IRepository<TieuChiDanhGia> _tieuChiDanhGiaRepository;
        private readonly ITempFileCacheManager _tempFileCacheManager;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public DoiTuongChuyenDoiSosAppService(
            IRepository<DoiTuongChuyenDoiSo> doiTuongChuyenDoiSoRepository, 
            IDoiTuongChuyenDoiSosExcelExporter doiTuongChuyenDoiSosExcelExporter, 
            IRepository<User, long> lookup_userRepository, 
            IRepository<ChiTietDanhGia> chiTietDanhGiaRepository, 
            IRepository<TieuChiDanhGia> tieuChiDanhGiaRepository,
            ITempFileCacheManager tempFileCacheManager,
            IBinaryObjectManager binaryObjectManager) 
		{
			_doiTuongChuyenDoiSoRepository = doiTuongChuyenDoiSoRepository;
			_doiTuongChuyenDoiSosExcelExporter = doiTuongChuyenDoiSosExcelExporter;
			_lookup_userRepository = lookup_userRepository;
            _chiTietDanhGiaRepository = chiTietDanhGiaRepository;
            _tieuChiDanhGiaRepository = tieuChiDanhGiaRepository;
            _binaryObjectManager = binaryObjectManager;
            _tempFileCacheManager = tempFileCacheManager;
        }

		public async Task<PagedResultDto<GetDoiTuongChuyenDoiSoForViewDto>> GetAll(GetAllDoiTuongChuyenDoiSosInput input)
        {

			var filteredDoiTuongChuyenDoiSos = _doiTuongChuyenDoiSoRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
                        .WhereIf(input.PhuLucFilter.HasValue, e => e.Type == input.PhuLucFilter)
						.WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var currentUser = GetCurrentUser();

            if (!currentUser.UserName.Equals("admin"))
            {
				filteredDoiTuongChuyenDoiSos = filteredDoiTuongChuyenDoiSos.Where(p => p.UserId == currentUser.Id);
            }

			var pagedAndFilteredDoiTuongChuyenDoiSos = filteredDoiTuongChuyenDoiSos
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

			var doiTuongChuyenDoiSos = from o in pagedAndFilteredDoiTuongChuyenDoiSos
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDoiTuongChuyenDoiSoForViewDto() {
							DoiTuongChuyenDoiSo = new DoiTuongChuyenDoiSoDto
							{
                                Name = o.Name,
                                Type = o.Type,
                                Id = o.Id,
                                TongDiemDatDuoc = o.TongDiemDatDuoc,
                                TongDiemHoiDongThamDinh = o.TongDiemHoiDongThamDinh,
                                TongDiemTuDanhGia = o.TongDiemTuDanhGia,
                                ChamDiemFlag = o.ChamDiemFlag
							},
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						};

            var totalCount = await filteredDoiTuongChuyenDoiSos.CountAsync();

            return new PagedResultDto<GetDoiTuongChuyenDoiSoForViewDto>(
                totalCount,
                await doiTuongChuyenDoiSos.ToListAsync()
            );
        }
		 
		public async Task<GetDoiTuongChuyenDoiSoForViewDto> GetDoiTuongChuyenDoiSoForView(int id)
        {
            var doiTuongChuyenDoiSo = await _doiTuongChuyenDoiSoRepository.GetAsync(id);

            var output = new GetDoiTuongChuyenDoiSoForViewDto { DoiTuongChuyenDoiSo = ObjectMapper.Map<DoiTuongChuyenDoiSoDto>(doiTuongChuyenDoiSo) };

		    if (output.DoiTuongChuyenDoiSo.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.DoiTuongChuyenDoiSo.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
        }
		 
		public async Task<GetDoiTuongChuyenDoiSoForEditOutput> GetDoiTuongChuyenDoiSoForEdit(EntityDto input)
        {
            var doiTuongChuyenDoiSo = await _doiTuongChuyenDoiSoRepository.FirstOrDefaultAsync(input.Id);
           
		    var output = new GetDoiTuongChuyenDoiSoForEditOutput {DoiTuongChuyenDoiSo = ObjectMapper.Map<CreateOrEditDoiTuongChuyenDoiSoDto>(doiTuongChuyenDoiSo)};

		    if (output.DoiTuongChuyenDoiSo.UserId != null)
            {
                var _lookupUser = await _lookup_userRepository.FirstOrDefaultAsync((long)output.DoiTuongChuyenDoiSo.UserId);
                output.UserName = _lookupUser?.Name?.ToString();
            }
			
            return output;
        }

		public async Task CreateOrEdit(CreateOrEditDoiTuongChuyenDoiSoDto input)
        {
            if(input.Id == null){
				await Create(input);
			}
			else{
				await Update(input);
			}
        }

		[AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_Create)]
		protected virtual async Task Create(CreateOrEditDoiTuongChuyenDoiSoDto input)
        {
            var doiTuongChuyenDoiSo = ObjectMapper.Map<DoiTuongChuyenDoiSo>(input);

			
			if (AbpSession.TenantId != null)
			{
				doiTuongChuyenDoiSo.TenantId = (int) AbpSession.TenantId;
			}
		

            await _doiTuongChuyenDoiSoRepository.InsertAsync(doiTuongChuyenDoiSo);
        }

		[AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_Edit)]
		protected virtual async Task Update(CreateOrEditDoiTuongChuyenDoiSoDto input)
        {
            var doiTuongChuyenDoiSo = await _doiTuongChuyenDoiSoRepository.FirstOrDefaultAsync((int)input.Id);
             ObjectMapper.Map(input, doiTuongChuyenDoiSo);
        }

		[AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_Delete)]
        public async Task Delete(EntityDto input)
        {
            await _doiTuongChuyenDoiSoRepository.DeleteAsync(input.Id);
        } 

		public async Task<FileDto> GetDoiTuongChuyenDoiSosToExcel(GetAllDoiTuongChuyenDoiSosForExcelInput input)
		{
			

			var filteredDoiTuongChuyenDoiSos = _doiTuongChuyenDoiSoRepository.GetAll()
						.Include( e => e.UserFk)
						.WhereIf(!string.IsNullOrWhiteSpace(input.Filter), e => false  || e.Name.Contains(input.Filter))
						.WhereIf(!string.IsNullOrWhiteSpace(input.NameFilter),  e => e.Name == input.NameFilter)
                        .WhereIf(input.PhuLucFilter.HasValue, e => e.Type == input.PhuLucFilter)
                        .WhereIf(!string.IsNullOrWhiteSpace(input.UserNameFilter), e => e.UserFk != null && e.UserFk.Name == input.UserNameFilter);

			var currentUser = GetCurrentUser();

			if (!currentUser.UserName.Equals("admin"))
			{
				filteredDoiTuongChuyenDoiSos = filteredDoiTuongChuyenDoiSos.Where(p => p.UserId == currentUser.Id);
			}

			var query = (from o in filteredDoiTuongChuyenDoiSos
                         join o1 in _lookup_userRepository.GetAll() on o.UserId equals o1.Id into j1
                         from s1 in j1.DefaultIfEmpty()
                         
                         select new GetDoiTuongChuyenDoiSoForViewDto() { 
							DoiTuongChuyenDoiSo = new DoiTuongChuyenDoiSoDto
							{
                                Name = o.Name,
                                Type = o.Type,
                                Id = o.Id,
                                TongDiemDatDuoc = o.TongDiemDatDuoc,
                                TongDiemHoiDongThamDinh = o.TongDiemHoiDongThamDinh,
                                TongDiemTuDanhGia = o.TongDiemTuDanhGia
                            },
                         	UserName = s1 == null || s1.Name == null ? "" : s1.Name.ToString()
						 });


            var doiTuongChuyenDoiSoListDtos = await query.ToListAsync();

            return _doiTuongChuyenDoiSosExcelExporter.ExportToFile(doiTuongChuyenDoiSoListDtos);
        }


		[AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos)]
		public async Task<List<DoiTuongChuyenDoiSoUserLookupTableDto>> GetAllUserForTableDropdown()
		{
			return await _lookup_userRepository.GetAll()
				.Select(user => new DoiTuongChuyenDoiSoUserLookupTableDto
				{
					Id = user.Id,
					DisplayName = user == null || user.Name == null ? "" : user.Name.ToString()
				}).ToListAsync();
		}

        public async Task<GetChiTietDanhGiaForChamDiemDoiTuongDto> GetAllChiTietDanhGiaDoiTuong(int idDoiTuongChuyenDoiSo)
        {
            var doiTuong = await _doiTuongChuyenDoiSoRepository.GetAsync(idDoiTuongChuyenDoiSo);

            var query = _chiTietDanhGiaRepository.GetAll()
                        .Include(e => e.TieuChiDanhGiaFk)
                        .Where(p => p.DoiTuongChuyenDoiSoId == idDoiTuongChuyenDoiSo)
                        .Where(p => p.TieuChiDanhGiaFk.LoaiPhuLuc == doiTuong.Type);

            var result = from o in query
                         select new ChiTietDanhGiaDoiTuong()
                         {
                             Id = o.Id,
                             Description = o.Description,
                             SoLieuKeKhai = o.SoLieuKeKhai,
                             DiemTuDanhGia = o.DiemTuDanhGia,
                             DiemHoiDongThamDinh = o.DiemHoiDongThamDinh,
                             DiemDatDuoc = o.DiemDatDuoc,
                             ParentId = o.TieuChiDanhGiaFk.ParentId,
                             TieuChiId = o.TieuChiDanhGiaFk.Id,
                             TenTieuChi = o.TieuChiDanhGiaFk.Name,
                             SoThuTu = o.TieuChiDanhGiaFk.SoThuTu,
                             SapXep = o.TieuChiDanhGiaFk.SapXep,
                             DiemToiDa = o.TieuChiDanhGiaFk.DiemToiDa,
                             IsHoiDongThamDinh = o.IsHoiDongThamDinh,
                             IsTuDanhGia = o.IsTuDanhGia
                         };

            // thuc hien danh dau cha con
            var ds = await result.ToListAsync();

            // init DFS
            var stack = new Stack<ChiTietDanhGiaDoiTuong>();
            foreach(var item in ds)
            {
                if(item.ParentId == null)
                {
                    item.ParentChiTietId = null;
                    stack.Push(item);
                }
            }

            // duyet DFS
            while (stack.Count > 0)
            {
                var top = stack.Pop();

                var listChild = ds.Where(p => p.ParentId == top.TieuChiId);
                if(listChild.Count() > 0) { 
                    foreach (var child in listChild) {
                        child.ParentChiTietId = top.Id;
                        stack.Push(child);
                    }
                }
                else
                {
                    top.IsLeaf = true;
                }
            }

            return new GetChiTietDanhGiaForChamDiemDoiTuongDto()
            {
                DanhSach = ds,
                DoiTuongName = doiTuong.Name
            };
        }

        [AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_DongBoChiTiet)]
        public async Task CapNhatChiTietTieuChiChoDoiTuong(int idDoiTuongChuyenDoiSo)
        {
            // lay thong tin doi tuong
            var doiTuong = await _doiTuongChuyenDoiSoRepository.GetAsync(idDoiTuongChuyenDoiSo);

            // lay toan bo tieu chi cua phu luc
            var danhSachTieuChiQuery = _tieuChiDanhGiaRepository.GetAll()
                    .Where(p => p.LoaiPhuLuc == doiTuong.Type);

            // lay cac chi tiet dang co cua doi tuong
            var chiTietQuery = _chiTietDanhGiaRepository.GetAll()
                    .Where(p => p.DoiTuongChuyenDoiSoId == idDoiTuongChuyenDoiSo);

            // truy van cac ban ghi tieu chi ma doi tuong chua co
            var query = from o in danhSachTieuChiQuery
                        where !(from o1 in chiTietQuery
                                select o1.TieuChiDanhGiaId)
                                .Contains(o.Id)
                        select o;
            var listTieuChiCanUpdate = await query.ToListAsync();

            // truy van cac ban ghi tieu chi cua doi tuong ma khong con trong phu luc
            var query2 = from o in chiTietQuery
                         where !(from o1 in danhSachTieuChiQuery
                                 select o1.Id)
                                 .Contains(o.TieuChiDanhGiaId)
                         select o;
            var listChiTietCanXoa = await query2.ToListAsync();

            // them cac tieu chi con thieu
            foreach (var item in listTieuChiCanUpdate)
            {
                var chiTiet = new ChiTietDanhGia();
                chiTiet.DoiTuongChuyenDoiSoId = idDoiTuongChuyenDoiSo;
                chiTiet.TieuChiDanhGiaId = item.Id;
                _chiTietDanhGiaRepository.Insert(chiTiet);
            }

            // loai bo cac tieu chi thua
            foreach (var item in listChiTietCanXoa)
            {
                var dsFile = JsonConvert.DeserializeObject<List<SoLieuKeKhaiSerializeObj>>(item.SoLieuKeKhai);
                if(dsFile != null)
                {
                    dsFile.ForEach(p =>
                    {
                        _binaryObjectManager.DeleteAsync(Guid.Parse(p.Guid));
                    });
                }
                _chiTietDanhGiaRepository.Delete(item);
            }

        }

        public async Task<ChiTietDanhGiaForEditTreeOutPut> GetChiTietDanhGiaForEdit(int id)
        {
            var chiTietDanhGia = await _chiTietDanhGiaRepository.GetAsync(id);

            if(chiTietDanhGia == null)
            {
                return null;
            }

            var tieuChi = await _tieuChiDanhGiaRepository.GetAsync(chiTietDanhGia.TieuChiDanhGiaId);
            if(tieuChi == null)
            {
                throw new UserFriendlyException("Có lỗi xảy ra");
            }

            var output = new ChiTietDanhGiaForEditTreeOutPut();

            output.Id = chiTietDanhGia.Id;
            output.SoLieuKeKhai = chiTietDanhGia.SoLieuKeKhai;
            output.DiemDatDuoc = chiTietDanhGia.DiemDatDuoc;
            output.DiemHoiDongThamDinh = chiTietDanhGia.DiemHoiDongThamDinh;
            output.DiemTuDanhGia = chiTietDanhGia.DiemTuDanhGia;
            output.Name = tieuChi.Name;
            output.PhuongThucDanhGia = tieuChi.PhuongThucDanhGia;
            output.SoThuTu = tieuChi.SoThuTu;
            output.DiemToiDa = tieuChi.DiemToiDa;
            output.Description = chiTietDanhGia.Description;

            var countChild = await _tieuChiDanhGiaRepository.GetAll().Where(p => p.ParentId == tieuChi.Id).CountAsync();

            output.IsLeaf = (countChild == 0);

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_ChamDiem, AppPermissions.Pages_DoiTuongChuyenDoiSos_HoiDongThamDinh)]
        public async Task EditChiTietDanhGia(CreateOrEditChiTietDanhGiaDto input)
        {
            if (!input.Id.HasValue)
            {
                throw new UserFriendlyException("Không có Id của chi tiết đánh giá");
            }
            var chiTiet = await _chiTietDanhGiaRepository.GetAsync((int)input.Id);

            if(chiTiet == null)
            {
                throw new UserFriendlyException("Tiêu chí này không tồn tại");
            }

            var doiTuong = await _doiTuongChuyenDoiSoRepository.GetAsync(chiTiet.DoiTuongChuyenDoiSoId);
            var tieuChi = await _tieuChiDanhGiaRepository.GetAsync(chiTiet.TieuChiDanhGiaId);

            if(doiTuong == null || tieuChi == null)
            {
                throw new UserFriendlyException("Có lỗi xảy ra");
            }

            var childCount = await _tieuChiDanhGiaRepository.GetAll().Where(p => p.ParentId == tieuChi.Id).CountAsync();

            // Chỉ xử lý các tiêu chí lá
            if(childCount == 0)
            {
                if (doiTuong.ChamDiemFlag == DoiTuongChuyenDoiSoConsts.HOI_DONG_THAM_DINH && await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_DoiTuongChuyenDoiSos_HoiDongThamDinh))
                {
                    chiTiet.DiemDatDuoc = input.DiemDatDuoc;
                    chiTiet.DiemHoiDongThamDinh = input.DiemHoiDongThamDinh;
                    chiTiet.IsHoiDongThamDinh = true;
                }

                if (doiTuong.ChamDiemFlag == DoiTuongChuyenDoiSoConsts.TU_DANH_GIA && await PermissionChecker.IsGrantedAsync(AppPermissions.Pages_DoiTuongChuyenDoiSos_ChamDiem))
                {
                    chiTiet.DiemTuDanhGia = input.DiemTuDanhGia;
                    chiTiet.Description = input.Description;
                    chiTiet.IsTuDanhGia = true;

                    //if (!string.IsNullOrEmpty(input.UploadedFileToken))
                    //{
                    //    byte[] byteArray;
                    //    var fileBytes = _tempFileCacheManager.GetFile(input.UploadedFileToken);

                    //    if(fileBytes == null)
                    //    {
                    //        return;
                    //    }

                    //    using (var stream = new MemoryStream(fileBytes))
                    //    {
                    //        byteArray = stream.ToArray();
                    //    }

                    //    if(byteArray.Length > MaxFileBytes)
                    //    {
                    //        return;
                    //    }

                    //    var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
                    //    var solieu = new SoLieuKeKhaiSerializeObj
                    //    {
                    //        FileName = input.SoLieuKeKhai,
                    //        Guid = storedFile.Id.ToString(),
                    //        ContentType = input.ContentType
                    //    };

                    //    var dsSoLieuKeKhai = new List<SoLieuKeKhaiSerializeObj>();
                    //    if (!string.IsNullOrWhiteSpace(chiTiet.SoLieuKeKhai))
                    //    {
                    //        dsSoLieuKeKhai = JsonConvert.DeserializeObject<List<SoLieuKeKhaiSerializeObj>>(chiTiet.SoLieuKeKhai);
                    //    }
                        
                    //    dsSoLieuKeKhai.Add(solieu);

                    //    await _binaryObjectManager.SaveAsync(storedFile);
                    //    chiTiet.SoLieuKeKhai = JsonConvert.SerializeObject(dsSoLieuKeKhai);
                    //}
                }
            }

        }

        [AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_TongHopDiem)]
        public async Task TongHopDiemDoiTuong(int id)
        {
            var doiTuong = await _doiTuongChuyenDoiSoRepository.GetAsync(id);

            var query = _chiTietDanhGiaRepository.GetAll()
                        .Include(e => e.TieuChiDanhGiaFk)
                        .Where(p => p.DoiTuongChuyenDoiSoId == id)
                        .Where(p => p.TieuChiDanhGiaFk.LoaiPhuLuc == doiTuong.Type);

            var result = from o in query
                         select new ChiTietDanhGiaDoiTuong()
                         {
                             Id = o.Id,
                             Description = o.Description,
                             SoLieuKeKhai = o.SoLieuKeKhai,
                             DiemTuDanhGia = o.DiemTuDanhGia,
                             DiemHoiDongThamDinh = o.DiemHoiDongThamDinh,
                             DiemDatDuoc = o.DiemDatDuoc,
                             ParentId = o.TieuChiDanhGiaFk.ParentId,
                             TieuChiId = o.TieuChiDanhGiaFk.Id,
                             TenTieuChi = o.TieuChiDanhGiaFk.Name
                         };

            // thuc hien danh dau cha con
            var ds = await result.ToListAsync();

            // init DFS
            var stack = new Stack<ChiTietDanhGiaDoiTuong>();
            foreach (var item in ds)
            {
                if (item.ParentId == null)
                {
                    item.ParentChiTietId = null;
                    stack.Push(item);
                }
            }

            // duyet DFS
            while (stack.Count > 0)
            {
                var top = stack.Pop();

                var listChild = ds.Where(p => p.ParentId == top.TieuChiId);

                if(listChild.Count() > 0)
                {
                    top.DiemDatDuoc = 0;
                    top.DiemTuDanhGia = 0;
                    top.DiemHoiDongThamDinh = 0;
                }

                foreach (var child in listChild)
                {
                    child.ParentChiTietId = top.Id;
                    stack.Push(child);
                }
            }
            // end duyet DFS
            // end danh dau cha con

            // mapping
            var listMainItem = await query.ToListAsync();
            var mapItemFromBackTracking = new Dictionary<int, ChiTietDanhGia>();
            foreach(var item in listMainItem)
            {
                mapItemFromBackTracking.Add(item.Id, item);
            }
            // end mapping

            // backtracking tinh tong
            // backtracking tung cay
            foreach(var item in ds.Where(p => p.ParentChiTietId == null))
            {
                BackTracking(item, ds);
            }
            // end backtracking

            // cap nhat tinh tong
            foreach(var item in ds)
            {
                var temp = mapItemFromBackTracking[item.Id];
                temp.DiemDatDuoc = item.DiemDatDuoc;
                temp.DiemHoiDongThamDinh = item.DiemHoiDongThamDinh;
                temp.DiemTuDanhGia = item.DiemTuDanhGia;
            }
            // end cap nhat

            // cap nhat tong diem cho doi tuong
            var dsRoot = ds.Where(p => p.ParentChiTietId == null);
            doiTuong.TongDiemTuDanhGia = dsRoot.Sum(p => p.DiemTuDanhGia);
            doiTuong.TongDiemHoiDongThamDinh = dsRoot.Sum(p => p.DiemHoiDongThamDinh);
            doiTuong.TongDiemDatDuoc = dsRoot.Sum(p => p.DiemDatDuoc);
            // end cap nhat tong diem cho doi tuong
        }

        private void BackTracking(ChiTietDanhGiaDoiTuong item, List<ChiTietDanhGiaDoiTuong> danhSach)
        {
            var dsCon = danhSach.Where(p => p.ParentChiTietId == item.Id);

            foreach(var con in dsCon)
            {
                BackTracking(con, danhSach);
                con.DiemDatDuoc = con.DiemDatDuoc ?? 0;
                con.DiemHoiDongThamDinh = con.DiemHoiDongThamDinh ?? 0;
                con.DiemTuDanhGia = con.DiemTuDanhGia ?? 0;

                item.DiemDatDuoc += con.DiemDatDuoc;
                item.DiemHoiDongThamDinh += con.DiemHoiDongThamDinh;
                item.DiemTuDanhGia += con.DiemTuDanhGia;
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_HoiDongThamDinh)]
        public async Task ChuyenTrangThaiChamDiem(TrangThaiChamDiemInput input)
        {
            var doiTuong = await _doiTuongChuyenDoiSoRepository.GetAsync(input.DoiTuongId);
            doiTuong.ChamDiemFlag = input.ChamDiemFlag;
            await _doiTuongChuyenDoiSoRepository.UpdateAsync(doiTuong);
        }

        [AbpAuthorize(AppPermissions.Pages_DoiTuongChuyenDoiSos_ChamDiem, AppPermissions.Pages_DoiTuongChuyenDoiSos_HoiDongThamDinh)]
        public async Task XoaFileDinhKem(ThongTinFileXoa input)
        {
            if (!string.IsNullOrWhiteSpace(input.Guid))
            {
                var chiTiet = await _chiTietDanhGiaRepository.GetAsync(input.ChiTietId);
                var doiTuong = await _doiTuongChuyenDoiSoRepository.GetAsync(chiTiet.DoiTuongChuyenDoiSoId);

                if(doiTuong == null)
                {
                    return;
                }
                else
                {
                    if(doiTuong.ChamDiemFlag == DoiTuongChuyenDoiSoConsts.HOI_DONG_THAM_DINH)
                    {
                        return;
                    }
                }

                if(chiTiet != null)
                {
                    try
                    {
                        var dsSoLieuKeKhai = JsonConvert.DeserializeObject<List<SoLieuKeKhaiSerializeObj>>(chiTiet.SoLieuKeKhai);
                        var itemToDelete = dsSoLieuKeKhai.Single(p => p.Guid == input.Guid);
                        dsSoLieuKeKhai.Remove(itemToDelete);

                        await _binaryObjectManager.DeleteAsync(Guid.Parse(input.Guid));
                        chiTiet.SoLieuKeKhai = JsonConvert.SerializeObject(dsSoLieuKeKhai);
                    }
                    catch(Exception ex)
                    {
                        throw new UserFriendlyException(ex.Message);
                    }

                }
                else
                {
                    throw new UserFriendlyException("Chi tiết đánh giá này không còn tồn tại");
                }
            }
            else
            {
                throw new UserFriendlyException("File không tồn tại!");
            }

        }

        public async Task<List<DoiTuongChuyenDoiSoForLookupDto>> GetAllDoiTuongChuyenDoiSo(string searchTerm)
        {
            var query = from o in _doiTuongChuyenDoiSoRepository.GetAll()
                        .WhereIf(!PermissionChecker.IsGranted(AppPermissions.Pages_Administration), p => p.UserId == AbpSession.UserId)
                        .WhereIf(!string.IsNullOrWhiteSpace(searchTerm), p => p.Name.Contains(searchTerm))
                        select new DoiTuongChuyenDoiSoForLookupDto
                        {
                            Id = o.Id,
                            DisplayName = o.Name
                        };

            return await query.OrderBy(p => p.DisplayName).ToListAsync();
        }

        public async Task<UploadInfoOutputDto> UploadSoLieuThongKe(ChiTietThongTinUploadInput input)
        {
            var output = new UploadInfoOutputDto();

            var chiTiet = await _chiTietDanhGiaRepository.GetAsync(input.Id);
            if (chiTiet != null)
            {
                byte[] byteArray;
                var dsSoLieuKeKhai = new List<SoLieuKeKhaiSerializeObj>();
                // thêm mới fileupload
                if (!string.IsNullOrWhiteSpace(input.FileToken))
                {
                    var fileBytes = _tempFileCacheManager.GetFile(input.FileToken);

                    if (fileBytes == null)
                    {
                        throw new UserFriendlyException("Có lỗi trong quá trình tải lên!");
                    }

                    using (var stream = new MemoryStream(fileBytes))
                    {
                        byteArray = stream.ToArray();
                    }

                    if (byteArray.Length > MaxFileBytes)
                    {
                        throw new UserFriendlyException("Dung lượng tệp tin phải nhỏ hơn 5MB!");
                    }

                    var storedFile = new BinaryObject(AbpSession.TenantId, byteArray);
                    var solieu = new SoLieuKeKhaiSerializeObj
                    {
                        FileName = input.FileName,
                        Guid = storedFile.Id.ToString(),
                        ContentType = input.FileType,
                        NoiDung = input.NoiDung
                    };
                    
                    // trường hợp số liệu đang có sẵn
                    if (!string.IsNullOrWhiteSpace(chiTiet.SoLieuKeKhai))
                    {
                        dsSoLieuKeKhai = JsonConvert.DeserializeObject<List<SoLieuKeKhaiSerializeObj>>(chiTiet.SoLieuKeKhai);

                        // Trường hợp tồn tại file cũ -> cần xóa file cũ và thêm file mới
                        if (!string.IsNullOrWhiteSpace(input.OldFile))
                        {
                            var oldItem = dsSoLieuKeKhai.FirstOrDefault(p => p.Guid == input.FileToken);
                            if (oldItem != null)
                            {
                                await _binaryObjectManager.DeleteAsync(Guid.Parse(oldItem.Guid));
                                dsSoLieuKeKhai.Remove(oldItem);
                            }
                        }

                    }
                    dsSoLieuKeKhai.Add(solieu);
                    await _binaryObjectManager.SaveAsync(storedFile);
                    output.OldFile = solieu.Guid;
                    output.OldFileName = solieu.FileName;
                    output.OldContentType = solieu.ContentType;
                }
                else // không có file tải lên => thay đổi nội dung
                {
                    if (!string.IsNullOrWhiteSpace(chiTiet.SoLieuKeKhai))
                    {
                        dsSoLieuKeKhai = JsonConvert.DeserializeObject<List<SoLieuKeKhaiSerializeObj>>(chiTiet.SoLieuKeKhai);
                        if (!string.IsNullOrWhiteSpace(input.OldFile))
                        {
                            var oldItem = dsSoLieuKeKhai.FirstOrDefault(p => p.Guid == input.OldFile);
                            if(oldItem != null)
                            {
                                oldItem.NoiDung = input.NoiDung;
                                output.OldFile = input.OldFile;
                                output.OldFileName = oldItem.FileName;
                                output.OldContentType = oldItem.ContentType;
                            }
                            else
                            {
                                throw new UserFriendlyException("Tệp số liệu thống kê không còn hãy làm mới trình duyệt!");
                            }
                        }
                        else
                        {
                            throw new UserFriendlyException("Hãy chọn tệp tin cho nội dung báo cáo!");
                        }
                    }
                    else
                    {
                        throw new UserFriendlyException("Thao tác không hợp lệ!");
                    }
                }


                //dsSoLieuKeKhai.Add(solieu);
                //output.OldFile = solieu.Guid;
                //await _binaryObjectManager.SaveAsync(storedFile);

                chiTiet.SoLieuKeKhai = JsonConvert.SerializeObject(dsSoLieuKeKhai);
                
            }

            return output;
        }

        public async Task XoaSoLieuThongKe(ThongTinSoLieuCanXoaInput input)
        {
            var chiTiet = await _chiTietDanhGiaRepository.GetAsync(input.Id);

            if(chiTiet != null && !string.IsNullOrWhiteSpace(chiTiet.SoLieuKeKhai))
            {
                var dsSoLieuKeKhai = JsonConvert.DeserializeObject<List<SoLieuKeKhaiSerializeObj>>(chiTiet.SoLieuKeKhai);

                var deleteItem = dsSoLieuKeKhai.FirstOrDefault(p => p.Guid == input.Guid);
                await _binaryObjectManager.DeleteAsync(Guid.Parse(input.Guid));
                dsSoLieuKeKhai.Remove(deleteItem);

                chiTiet.SoLieuKeKhai = JsonConvert.SerializeObject(dsSoLieuKeKhai);
            }
        }
    }
}