using Abp.AspNetCore.Mvc.Authorization;
using Abp.Domain.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TechBer.ChuyenDoiSo.Authorization;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo;
using TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos;
using TechBer.ChuyenDoiSo.Web.Areas.App.Models.CayTieuChi;
using TechBer.ChuyenDoiSo.Web.Controllers;

namespace TechBer.ChuyenDoiSo.Web.Areas.App.Controllers
{
    [Area("App")]
    [AbpMvcAuthorize(AppPermissions.Pages_CayTieuChi)]
    public class CayTieuChiController : ChuyenDoiSoControllerBase
    {
        //private readonly ITieuChiDanhGiasAppService _tieuChiDanhGiasAppService;
        private readonly IRepository<TieuChiDanhGia, int> _tieuChiDanhGiasRepository;

        public CayTieuChiController(IRepository<TieuChiDanhGia, int> tieuChiDanhGiasRepository)
        {
            _tieuChiDanhGiasRepository = tieuChiDanhGiasRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        public PartialViewResult CreateModal(CreateTieuChiInputModel input)
        {
            return PartialView("_CreateModal", new CreateTieuChiModalViewModel(input.LoaiPhuLuc, input.ParentId));
        }

        public PartialViewResult EditModal(EditTieuChiInputModel input)
        {
            TieuChiDanhGia tieuChi = _tieuChiDanhGiasRepository.Get(input.Id);
            EditTieuChiModalViewModel model = new EditTieuChiModalViewModel()
            {
                Id = tieuChi.Id,
                LoaiPhuLuc = tieuChi.LoaiPhuLuc,
                DiemToiDa = tieuChi.DiemToiDa,
                Name = tieuChi.Name,
                ParentId = tieuChi.ParentId,
                GhiChu = tieuChi.GhiChu,
                PhuongThucDanhGia = tieuChi.PhuongThucDanhGia,
                SoThuTu = tieuChi.SoThuTu,
                TaiLieuGiaiTrinh = tieuChi.TaiLieuGiaiTrinh,
                SapXep = tieuChi.SapXep,
                PhanNhomLevel1 = tieuChi.PhanNhomLevel1,
                DoSau = tieuChi.DoSau
            };

            return PartialView("_EditModal", model);
        }
    }
}
