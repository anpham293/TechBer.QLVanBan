using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.QuyTrinhDuAnAssigneds;
using TechBer.ChuyenDoiSo.Web.Controllers;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using Abp.Application.Services.Dto;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.UI;
using System.Linq;
using System.Text.RegularExpressions;
using Abp.Linq.Extensions;
using Newtonsoft.Json;
using NPOI.POIFS.FileSystem;
using Org.BouncyCastle.Crypto.Agreement.JPake;
using TechBer.ChuyenDoiSo.Authorization.Users;
using TechBer.ChuyenDoiSo.Dto;
using TechBer.ChuyenDoiSo.QuanLyDanhMuc.Dtos;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds)]
    public class QuyTrinhDuAnAssignedsController : ChuyenDoiSoControllerBase
    {
        private readonly IQuyTrinhDuAnAssignedsAppService _quyTrinhDuAnAssignedsAppService;
        private readonly IRepository<QuyTrinhDuAnAssigned, long> _quyTrinhDuAnAssignedsRepository;
        private readonly IRepository<DuAn> _duAnRepository;
        private readonly IRepository<LoaiDuAn> _loaiDuAnRepository;
        private readonly IRepository<VanBanDuAn> _vanBanDuAnRepository;
        private readonly IRepository<User,long> _userRepository;

        public QuyTrinhDuAnAssignedsController(IQuyTrinhDuAnAssignedsAppService quyTrinhDuAnAssignedsAppService,
            IRepository<QuyTrinhDuAnAssigned, long> quyTrinhDuAnAssignedsRepository,
            IRepository<DuAn> duAnRepository,
            IRepository<LoaiDuAn> loaiDuAnRepository,
            IRepository<VanBanDuAn> vanBanDuRepository,
            IRepository<User,long> userRepository
        )
        {
            _quyTrinhDuAnAssignedsAppService = quyTrinhDuAnAssignedsAppService;
            _quyTrinhDuAnAssignedsRepository = quyTrinhDuAnAssignedsRepository;
            _duAnRepository = duAnRepository;
            _duAnRepository = duAnRepository;
            _loaiDuAnRepository = loaiDuAnRepository;
            _vanBanDuAnRepository = vanBanDuRepository;
            _userRepository = userRepository;
        }

        public ActionResult Index()
        {
            var model = new QuyTrinhDuAnAssignedsViewModel
            {
                FilterText = ""
            };

            return View(model);
        }

        public class CreateOrEditModalInput
        {
            public long? Id { get; set; }
            public int? ParentId { get; set; }

            public int LoaiDuAn { get; set; }
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create,
            AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public async Task<PartialViewResult> CreateOrEditModal(CreateOrEditModalInput input)
        {
            GetQuyTrinhDuAnAssignedForEditOutput getQuyTrinhDuAnAssignedForEditOutput;

            if (input.Id.HasValue)
            {
                getQuyTrinhDuAnAssignedForEditOutput =
                    await _quyTrinhDuAnAssignedsAppService.GetQuyTrinhDuAnAssignedForEdit(new EntityDto<long>
                        {Id = (long) input.Id});
            }
            else
            {
                getQuyTrinhDuAnAssignedForEditOutput = new GetQuyTrinhDuAnAssignedForEditOutput
                {
                    QuyTrinhDuAnAssigned = new CreateOrEditQuyTrinhDuAnAssignedDto()
                };
                getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssigned.ParentId = input.ParentId;
                getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssigned.DuAnId = input.LoaiDuAn;
                if (input.ParentId.HasValue)
                {
                    QuyTrinhDuAnAssigned quyTrinhDuAn =
                        await _quyTrinhDuAnAssignedsRepository.FirstOrDefaultAsync(input.ParentId.Value);
                    if (quyTrinhDuAn == null)
                    {
                        throw new UserFriendlyException("Không tìm thấy quy trình cha!");
                    }

                    getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssignedName = quyTrinhDuAn.Name;
                }
                else
                {
                    getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnName = "Quy trình gốc";
                }


                DuAn duAn = await _duAnRepository.FirstOrDefaultAsync(input.LoaiDuAn);
                if (duAn == null)
                {
                    throw new UserFriendlyException("Không tìm thấy dự án!");
                }
                getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssigned.LoaiDuAnId = duAn.LoaiDuAnId;
                getQuyTrinhDuAnAssignedForEditOutput.DuAnName = duAn.Name;
                
                LoaiDuAn loaiDuAn = await _loaiDuAnRepository.FirstOrDefaultAsync(duAn.LoaiDuAnId.Value);
                if (loaiDuAn == null)
                {
                    throw new UserFriendlyException("Không tìm thấy loại dự án!");
                }

                getQuyTrinhDuAnAssignedForEditOutput.LoaiDuAnName = loaiDuAn.Name;
            }

            var viewModel = new CreateOrEditQuyTrinhDuAnAssignedModalViewModel()
            {
                QuyTrinhDuAnAssigned = getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssigned,
                LoaiDuAnName = getQuyTrinhDuAnAssignedForEditOutput.LoaiDuAnName,
                QuyTrinhDuAnName = getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnName,
                QuyTrinhDuAnAssignedName = getQuyTrinhDuAnAssignedForEditOutput.QuyTrinhDuAnAssignedName,
                DuAnName = getQuyTrinhDuAnAssignedForEditOutput.DuAnName,
            };

            return PartialView("_CreateOrEditModal", viewModel);
        }


        public async Task<PartialViewResult> ViewQuyTrinhDuAnAssignedModal(long id)
        {
            var getQuyTrinhDuAnAssignedForViewDto =
                await _quyTrinhDuAnAssignedsAppService.GetQuyTrinhDuAnAssignedForView(id);

            var model = new QuyTrinhDuAnAssignedViewModel()
            {
                QuyTrinhDuAnAssigned = getQuyTrinhDuAnAssignedForViewDto.QuyTrinhDuAnAssigned,
                LoaiDuAnName = getQuyTrinhDuAnAssignedForViewDto.LoaiDuAnName,
                QuyTrinhDuAnName = getQuyTrinhDuAnAssignedForViewDto.QuyTrinhDuAnName,
                QuyTrinhDuAnAssignedName = getQuyTrinhDuAnAssignedForViewDto.QuyTrinhDuAnAssignedName,
                DuAnName = getQuyTrinhDuAnAssignedForViewDto.DuAnName
            };

            return PartialView("_ViewQuyTrinhDuAnAssignedModal", model);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create,
            AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult LoaiDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedLoaiDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedLoaiDuAnLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create,
            AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult QuyTrinhDuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedQuyTrinhDuAnLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create,
            AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult QuyTrinhDuAnAssignedLookupTableModal(long? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedQuyTrinhDuAnAssignedLookupTableModal", viewModel);
        }

        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Create,
            AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult DuAnLookupTableModal(int? id, string displayName)
        {
            var viewModel = new QuyTrinhDuAnAssignedDuAnLookupTableViewModel()
            {
                Id = id,
                DisplayName = displayName,
                FilterText = ""
            };

            return PartialView("_QuyTrinhDuAnAssignedDuAnLookupTableModal", viewModel);
        }
        
        [AbpMvcAuthorize(AppPermissions.Pages_QuyTrinhDuAnAssigneds_Edit)]
        public PartialViewResult ChuyenDuyetHoSoModal(ChuyenDuyetHoSoModalInput input)
        {
            var tenNguoiGiao = "";
            long nguoiGiaoId = -1;
            var ngayGuiPhieu = "";
            var vanBanDuAn = _vanBanDuAnRepository.FirstOrDefault(input.VanBanDuAnId);
            // var quyTrinhDuAnAssigned =
            //     _quyTrinhDuAnAssignedsRepository.FirstOrDefaultAsync((long)vanBanDuAn.QuyTrinhDuAnAssignedId);
            var duAn = _duAnRepository.FirstOrDefault((int)vanBanDuAn.DuAnId);

            List<CommonLookupTableDto> listKeToanPhuTrach = new List<CommonLookupTableDto>();
            foreach (var VARIABLE in _userRepository.GetAll())
            {
                listKeToanPhuTrach.Add(new CommonLookupTableDto()
                {
                    Id = (int)VARIABLE.Id,
                    DisplayName = VARIABLE.Surname + " " + VARIABLE.Name
                });
            }
            if (vanBanDuAn.NgayGui.HasValue)
            {
                ngayGuiPhieu = (vanBanDuAn.NgayGui).ToString();
            }
            else
            {
                ngayGuiPhieu = DateTime.Now.ToString("dd/MM/yyyy");
            }

            if (vanBanDuAn.NguoiGuiId.HasValue)
            {
                var nguoiGiao = _userRepository.FirstOrDefault((long)vanBanDuAn.NguoiGuiId);
                tenNguoiGiao = nguoiGiao.Surname + " " + nguoiGiao.Name;
                nguoiGiaoId = nguoiGiao.Id;
            
            }
            else
            {
                var nguoiGiao = _userRepository.FirstOrDefault((long)AbpSession.UserId);
                tenNguoiGiao = nguoiGiao.Surname + " " + nguoiGiao.Name;
                nguoiGiaoId = nguoiGiao.Id;
            }
            var keToanId = vanBanDuAn.KeToanTiepNhanId;
            var viewModel = new ChuyenDuyetHoSoModalViewModel()
            {
                VanBanDuAn = ObjectMapper.Map<VanBanDuAnDto>(vanBanDuAn),
                DuAn = ObjectMapper.Map<DuAnDto>(duAn),
                SoLuongVanBan = vanBanDuAn.SoLuongVanBanGiay,
                ListKeToanTiepNhan = listKeToanPhuTrach,
                TenNguoiGiao = tenNguoiGiao,
                NgayGuiPhieu = ngayGuiPhieu,
                KeToanTiepNhanId = keToanId,
                NguoiGuiId = nguoiGiaoId,
                TypeDuyetHoSo = input.TypeDuyetHoSo
            };
            return PartialView("_ChuyenDuyetHoSoModal", viewModel);
        }

        public ActionResult BaoCaoNopHoSoTrongThangTheoDuAnView()
        {
            return View("BaoCaoNopHoSoTrongThangTheoDuAnView");
        }

        [HttpPost]
        public async Task<PartialViewResult> BaoCaoNopHoSoTrongThangTheoDuAn(BaoCaoNopHoSoTrongThangTheoDuAnFilterDto input)
        {
            BaoCaoNopHoSoTrongThangTheoDuAnViewModel model = new BaoCaoNopHoSoTrongThangTheoDuAnViewModel();
            model.BaoCaoNopHoSoTrongThangTheoDuAn = new List<BaoCaoNopHoSoTrongThangTheoDuAnDto>();
            
            var vanBanDuAnFilter = _vanBanDuAnRepository.GetAll()
                .WhereIf(true, e => e.LastFileVanBanTime != null 
                                    && e.LastFileVanBanTime.Value.Month == input.Thang 
                                    && e.LastFileVanBanTime.Value.Year == input.Nam)
                .WhereIf(true, e => e.QuyTrinhDuAnAssignedFk.IsDeleted == false)
                .WhereIf(true, e => e.DuAnFk.IsDeleted == false)
                ;

            var hoSoNopTrongThangTheoDuAn = from o in vanBanDuAnFilter
                join o1 in _quyTrinhDuAnAssignedsRepository.GetAll() on o.QuyTrinhDuAnAssignedId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _duAnRepository.GetAll() on s1.DuAnId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _loaiDuAnRepository.GetAll() on s2.LoaiDuAnId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _userRepository.GetAll() on o.NguoiNopHoSoId equals o4.Id into j4
                from s4 in j4.DefaultIfEmpty() 
                orderby s3.Id descending
                select new BaoCaoNopHoSoTrongThangTheoDuAnDto()
                {
                    LoaiDuAn = new LoaiDuAnDto()
                    {
                        Id = s3.Id,
                        Name = s3.Name
                    },
                    DuAn = new DuAnDto()
                    {
                        Id = s2.Id,
                        Name = s2.Name
                    },
                    QuyTrinhDuAnAssigned = new QuyTrinhDuAnAssignedDto()
                    {
                        Id = s1.Id,
                        Name = s1.Name
                    },
                    VanBanDuAn = new VanBanDuAnDto()
                    {
                        Name = o.Name,
                        FileVanBan = (o.FileVanBan.IsNullOrEmpty()
                            ? o.FileVanBan
                            : JsonConvert.DeserializeObject<FileMauSerializeObj>(o.FileVanBan).FileName),
                        NgayBanHanh = o.NgayBanHanh,
                        LastFileVanBanTime = o.LastFileVanBanTime,
                        NguoiNopHoSoId = o.NguoiNopHoSoId
                    },
                    TenNguoiNop = o.NguoiNopHoSoId == null ? "" : s4.Surname + " " +s4.Name
                };
            model.BaoCaoNopHoSoTrongThangTheoDuAn.AddRange(hoSoNopTrongThangTheoDuAn.ToList());
            var a = hoSoNopTrongThangTheoDuAn.ToList();
            return PartialView("_BaoCaoNopHoSoTrongThangTheoDuAn", model);
        }
        
        public ActionResult BaoCaoHoSoTheoDuAnView()
        {
            return View("BaoCaoHoSoTheoDuAnView");
        }
        [HttpPost]
        public async Task<PartialViewResult> BaoCaoHoSoTheoDuAn(BaoCaoHoSoTheoDuAnFilterDto input)
        {
            BaoCaoHoSoTheoDuAnViewModel model = new BaoCaoHoSoTheoDuAnViewModel();
            model.BaoCaoHoSoTheoDuAn = new List<BaoCaoHoSoDuAnDto>();
            var pattern = @"\s+";
            string[] ngayQuyetDinh = Regex.Split(input.NgayQuyetDinh.Trim(), pattern);
            var quyetDinhTuNgay = DateTime.Parse(ngayQuyetDinh[0]);
            var quyetDinhDenNgay = DateTime.Parse(ngayQuyetDinh[2]);
            
            var vanBanDuAnFilter = _vanBanDuAnRepository.GetAll()
                .WhereIf(!input.MaDuAn.IsNullOrWhiteSpace(), e => e.DuAnFk.Descriptions.Equals(input.MaDuAn))
                .WhereIf(true, e => e.QuyTrinhDuAnAssignedFk.IsDeleted == false)
                .WhereIf(true, e => e.DuAnFk.IsDeleted == false)
                .WhereIf(!input.TenDuAn.IsNullOrWhiteSpace(), e => e.DuAnFk.Name.Contains(input.TenDuAn))
                .WhereIf(!input.TenHoSo.IsNullOrWhiteSpace(), e => e.Name.Contains(input.TenHoSo))
                .WhereIf(true, e => e.NgayBanHanh <= quyetDinhDenNgay && e.NgayBanHanh >= quyetDinhTuNgay)
                ;

            var hoSoNopTrongThangTheoDuAn = from o in vanBanDuAnFilter
                join o1 in _quyTrinhDuAnAssignedsRepository.GetAll() on o.QuyTrinhDuAnAssignedId equals o1.Id into j1
                from s1 in j1.DefaultIfEmpty()
                join o2 in _duAnRepository.GetAll() on s1.DuAnId equals o2.Id into j2
                from s2 in j2.DefaultIfEmpty()
                join o3 in _loaiDuAnRepository.GetAll() on s2.LoaiDuAnId equals o3.Id into j3
                from s3 in j3.DefaultIfEmpty()
                join o4 in _userRepository.GetAll() on o.NguoiNopHoSoId equals o4.Id into j4
                from s4 in j4.DefaultIfEmpty() 
                orderby s3.Id descending
                select new BaoCaoHoSoDuAnDto()
                {
                    LoaiDuAn = new LoaiDuAnDto()
                    {
                        Id = s3.Id,
                        Name = s3.Name
                    },
                    DuAn = new DuAnDto()
                    {
                        Id = s2.Id,
                        Name = s2.Name
                    },
                    QuyTrinhDuAnAssigned = new QuyTrinhDuAnAssignedDto()
                    {
                        Id = s1.Id,
                        Name = s1.Name
                    },
                    VanBanDuAn = new VanBanDuAnDto()
                    {
                        Name = o.Name,
                        FileVanBan = (o.FileVanBan.IsNullOrEmpty()
                            ? o.FileVanBan
                            : JsonConvert.DeserializeObject<FileMauSerializeObj>(o.FileVanBan).FileName),
                        NgayBanHanh = o.NgayBanHanh,
                        LastFileVanBanTime = o.LastFileVanBanTime,
                        NguoiNopHoSoId = o.NguoiNopHoSoId
                    },
                    TenNguoiNop = o.NguoiNopHoSoId == null ? "" : s4.Surname + " " +s4.Name
                };
            model.BaoCaoHoSoTheoDuAn.AddRange(hoSoNopTrongThangTheoDuAn.ToList());
            var a = hoSoNopTrongThangTheoDuAn.ToList();
            return PartialView("_BaoCaoHoSoTheoDuAn", model);
        }
    }
}