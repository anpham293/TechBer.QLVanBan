using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Abp.Domain.Repositories;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Exporting;
using TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng.Dtos;
using TechBer.ChuyenDoiSo.Dto;
using Abp.Application.Services.Dto;
using TechBer.ChuyenDoiSo.Authorization;
using Abp.Extensions;
using Abp.Authorization;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using iText.Html2pdf;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using Karion.BusinessSolution.EinvoiceExtension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechBer.ChuyenDoiSo.Common;
using TechBer.ChuyenDoiSo.QuanLySdtZalo;
using TechBer.ChuyenDoiSo.Storage;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
    [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies)]
    public class DuAnThuHoiesAppService : ChuyenDoiSoAppServiceBase, IDuAnThuHoiesAppService
    {
        private readonly IRepository<DuAnThuHoi, long> _duAnThuHoiRepository;
        private readonly IDuAnThuHoiesExcelExporter _duAnThuHoiesExcelExporter;
        private readonly IRepository<ChiTietThuHoi, long> _chiTietThuHoiRepository;
        private readonly IRepository<DanhMucThuHoi, long> _danhMucThuHoiRepository;
        private readonly IRepository<SdtZalo, long> _sdtZaloRepository;
        private readonly IBinaryObjectManager _binaryObjectManager;

        public DuAnThuHoiesAppService(IRepository<DuAnThuHoi, long> duAnThuHoiRepository,
            IDuAnThuHoiesExcelExporter duAnThuHoiesExcelExporter,
            IRepository<ChiTietThuHoi, long> chiTietThuHoiRepository,
            IRepository<DanhMucThuHoi, long> danhMucThuHoiRepository,
            IRepository<SdtZalo, long> sdtZaloRepository,
            IBinaryObjectManager binaryObjectManager)
        {
            _duAnThuHoiRepository = duAnThuHoiRepository;
            _duAnThuHoiesExcelExporter = duAnThuHoiesExcelExporter;
            _chiTietThuHoiRepository = chiTietThuHoiRepository;
            _danhMucThuHoiRepository = danhMucThuHoiRepository;
            _binaryObjectManager = binaryObjectManager;
            _sdtZaloRepository = sdtZaloRepository;
        }

        public async Task<PagedResultDto<GetDuAnThuHoiForViewDto>> GetAll(GetAllDuAnThuHoiesInput input)
        {
            var filteredDuAnThuHoies = _duAnThuHoiRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.MaDATH.Contains(input.Filter) || e.Ten.Contains(input.Filter) ||
                         e.GhiChu.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaDATHFilter), e => e.MaDATH == input.MaDATHFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(input.MinNamQuanLyFilter != null, e => e.NamQuanLy >= input.MinNamQuanLyFilter)
                .WhereIf(input.MaxNamQuanLyFilter != null, e => e.NamQuanLy <= input.MaxNamQuanLyFilter)
                .WhereIf(input.MinThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong >= input.MinThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MaxThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong <= input.MaxThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MinThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng >= input.MinThoiHanBaoLanhTamUngFilter)
                .WhereIf(input.MaxThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng <= input.MaxThoiHanBaoLanhTamUngFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter), e => e.GhiChu == input.GhiChuFilter)
                .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

            var pagedAndFilteredDuAnThuHoies = filteredDuAnThuHoies
                .OrderBy(input.Sorting ?? "id asc")
                .PageBy(input);

            var duAnThuHoies = await (from o in pagedAndFilteredDuAnThuHoies
                select new GetDuAnThuHoiForViewDto()
                {
                    DuAnThuHoi = new DuAnThuHoiDto
                    {
                        MaDATH = o.MaDATH,
                        Ten = o.Ten,
                        NamQuanLy = o.NamQuanLy,
                        ThoiHanBaoLanhHopDong = o.ThoiHanBaoLanhHopDong,
                        ThoiHanBaoLanhTamUng = o.ThoiHanBaoLanhTamUng,
                        GhiChu = o.GhiChu,
                        TrangThai = o.TrangThai,
                        Id = o.Id
                    }
                }).ToListAsync();

            foreach (var da in duAnThuHoies)
            {
                decimal tongDuDuAn = 0;
                decimal tongThuDuAn = 0;

                var listDanhMuc = _danhMucThuHoiRepository.GetAll()
                    .WhereIf(true, p => p.DuAnThuHoiId == da.DuAnThuHoi.Id).ToList();

                if (!listDanhMuc.IsNullOrEmpty())
                {
                    foreach (var dm in listDanhMuc)
                    {
                        var listChiTiet = _chiTietThuHoiRepository.GetAll()
                            .WhereIf(true, p => p.DanhMucThuHoiId == dm.Id).ToList();
                        decimal tongDu = 0;
                        decimal tongThu = 0;
                        if (!listChiTiet.IsNullOrEmpty())
                        {
                            tongDu = listChiTiet.Sum(p => p.TongDu);
                            tongThu = listChiTiet.Sum(p => p.TongThu);
                        }

                        tongDuDuAn += tongDu;
                        tongThuDuAn += tongThu;
                    }
                }

                da.TongDuDuAn = tongDuDuAn;
                da.TongThuDuAn = tongThuDuAn;
            }

            var totalCount = await filteredDuAnThuHoies.CountAsync();

            return new PagedResultDto<GetDuAnThuHoiForViewDto>(
                totalCount,
                duAnThuHoies
            );
        }

        public async Task<GetDuAnThuHoiForViewDto> GetDuAnThuHoiForView(long id)
        {
            var duAnThuHoi = await _duAnThuHoiRepository.GetAsync(id);

            var output = new GetDuAnThuHoiForViewDto {DuAnThuHoi = ObjectMapper.Map<DuAnThuHoiDto>(duAnThuHoi)};

            return output;
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Edit)]
        public async Task<GetDuAnThuHoiForEditOutput> GetDuAnThuHoiForEdit(EntityDto<long> input)
        {
            var duAnThuHoi = await _duAnThuHoiRepository.FirstOrDefaultAsync(input.Id);

            var output = new GetDuAnThuHoiForEditOutput
                {DuAnThuHoi = ObjectMapper.Map<CreateOrEditDuAnThuHoiDto>(duAnThuHoi)};

            return output;
        }

        public async Task CreateOrEdit(CreateOrEditDuAnThuHoiDto input)
        {
            if (input.Id == null)
            {
                await Create(input);
            }
            else
            {
                await Update(input);
            }
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Create)]
        protected virtual async Task Create(CreateOrEditDuAnThuHoiDto input)
        {
            //var duAnThuHoi = ObjectMapper.Map<DuAnThuHoi>(input);
            var duAnThuHoi = new DuAnThuHoi()
            {
                CreationTime = DateTime.Now,
                IsDeleted = false,
                CreatorUserId = AbpSession.UserId,
                Ten = input.Ten,
                ThoiHanBaoLanhHopDong = input.ThoiHanBaoLanhHopDong,
                ThoiHanBaoLanhTamUng = input.ThoiHanBaoLanhTamUng,
                NamQuanLy = input.NamQuanLy,
                GhiChu = input.GhiChu,
                TrangThai = DuAnThuHoiConst.TrangThaiDangXuLi
            };

            var soDuAn = 0;
            string fmt = "000";

            var duAnThuHoiOld = await _duAnThuHoiRepository.GetAll()
                .WhereIf(true, p => p.NamQuanLy == input.NamQuanLy)
                .OrderByDescending(p => p.Id).FirstOrDefaultAsync();
            if (duAnThuHoiOld.IsNullOrDeleted())
            {
                soDuAn = 1;
            }
            else
            {
                soDuAn = duAnThuHoiOld.SoDuAn + 1;
            }

            duAnThuHoi.SoDuAn = soDuAn;
            duAnThuHoi.MaDATH = "TH" + input.NamQuanLy + "-" + soDuAn.ToString(fmt);

            await _duAnThuHoiRepository.InsertAsync(duAnThuHoi);
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Edit)]
        protected virtual async Task Update(CreateOrEditDuAnThuHoiDto input)
        {
            var duAnThuHoi = await _duAnThuHoiRepository.FirstOrDefaultAsync((long) input.Id);
            ObjectMapper.Map(input, duAnThuHoi);
        }

        [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies_Delete)]
        public async Task Delete(EntityDto<long> input)
        {
            await _duAnThuHoiRepository.DeleteAsync(input.Id);
        }

        public async Task<FileDto> GetDuAnThuHoiesToExcel(GetAllDuAnThuHoiesForExcelInput input)
        {
            var filteredDuAnThuHoies = _duAnThuHoiRepository.GetAll()
                .WhereIf(!string.IsNullOrWhiteSpace(input.Filter),
                    e => false || e.MaDATH.Contains(input.Filter) || e.Ten.Contains(input.Filter) ||
                         e.GhiChu.Contains(input.Filter))
                .WhereIf(!string.IsNullOrWhiteSpace(input.MaDATHFilter), e => e.MaDATH == input.MaDATHFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.TenFilter), e => e.Ten == input.TenFilter)
                .WhereIf(input.MinNamQuanLyFilter != null, e => e.NamQuanLy >= input.MinNamQuanLyFilter)
                .WhereIf(input.MaxNamQuanLyFilter != null, e => e.NamQuanLy <= input.MaxNamQuanLyFilter)
                .WhereIf(input.MinThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong >= input.MinThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MaxThoiHanBaoLanhHopDongFilter != null,
                    e => e.ThoiHanBaoLanhHopDong <= input.MaxThoiHanBaoLanhHopDongFilter)
                .WhereIf(input.MinThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng >= input.MinThoiHanBaoLanhTamUngFilter)
                .WhereIf(input.MaxThoiHanBaoLanhTamUngFilter != null,
                    e => e.ThoiHanBaoLanhTamUng <= input.MaxThoiHanBaoLanhTamUngFilter)
                .WhereIf(!string.IsNullOrWhiteSpace(input.GhiChuFilter), e => e.GhiChu == input.GhiChuFilter)
                .WhereIf(input.MinTrangThaiFilter != null, e => e.TrangThai >= input.MinTrangThaiFilter)
                .WhereIf(input.MaxTrangThaiFilter != null, e => e.TrangThai <= input.MaxTrangThaiFilter);

            var query = (from o in filteredDuAnThuHoies
                select new GetDuAnThuHoiForViewDto()
                {
                    DuAnThuHoi = new DuAnThuHoiDto
                    {
                        MaDATH = o.MaDATH,
                        Ten = o.Ten,
                        NamQuanLy = o.NamQuanLy,
                        ThoiHanBaoLanhHopDong = o.ThoiHanBaoLanhHopDong,
                        ThoiHanBaoLanhTamUng = o.ThoiHanBaoLanhTamUng,
                        GhiChu = o.GhiChu,
                        TrangThai = o.TrangThai,
                        Id = o.Id
                    }
                });


            var duAnThuHoiListDtos = await query.ToListAsync();

            return _duAnThuHoiesExcelExporter.ExportToFile(duAnThuHoiListDtos);
        }

        public class TestDto
        {
            public string token { get; set; }
            public string refreshToken { get; set; }
        }

        [HttpPost]
        public async Task<string> SendZalo()
        {
            try
            {
                var client = new HttpClient();
                client.DefaultRequestHeaders.Add("User-Agent", "ASP.Net Core" + AbpSession.UserId + "");
                var request = new HttpRequestMessage(HttpMethod.Post, "https://techber.vn/jwt-login.html");
                var content =
                    new StringContent(
                        "{\r\n    \"username\":\"zalointegrated\",\r\n    \"password\":\"Techber@123\"\r\n}\r\n", null,
                        "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var ketqua = await response.Content.ReadAsStringAsync();

                var ketquaConvert = JsonConvert.DeserializeObject<TestDto>(ketqua);

                var token = ketquaConvert.token;

                var listSDT = new List<string>();
                var sdtZalo = await _sdtZaloRepository.GetAllListAsync();
                foreach (var sdt in sdtZalo)
                {
                    listSDT.Add(sdt.Sdt);
                }
                var stringJoin = string.Join(", ", listSDT);
                
                var message = "Thông báo dự án quá thời hạn thu hồi ";

                var requestZaloMessage =
                    new HttpRequestMessage(HttpMethod.Post, "https://techber.vn/appservices/sendzalo.html");
                requestZaloMessage.Headers.Add("Authorization", "Bearer " + token + "");
                var contentZaloMessage =
                    new StringContent(
                        "{\"listsdt\": [" + stringJoin + "],\r\n    \"message\" : \"" + message + "\"}\r\n", null,
                        "application/json");
                requestZaloMessage.Content = contentZaloMessage;
                var responseZaloMessage = await client.SendAsync(requestZaloMessage);
                responseZaloMessage.EnsureSuccessStatusCode();
                var ketquaZaloMessage = await responseZaloMessage.Content.ReadAsStringAsync();

                var ketquaZaloMessageConvert = JsonConvert.DeserializeObject<TestDto>(ketquaZaloMessage);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "";
        }

        public async Task<string> DocPDF()
        {
            var binObj = await _binaryObjectManager.GetOrNullAsync(Guid.Parse("e9b9b47a-de16-6776-ec58-3a14625ec987"));
            var content = binObj.Bytes;

            // var File(content, "application/pdf");

            var abc = "";
                try
            {
                using (MemoryStream ms = new MemoryStream(content))
                {
                    // Sử dụng PdfReader để đọc từ MemoryStream
                    using (PdfReader reader = new PdfReader(ms))
                    {
                        StringBuilder textBuilder = new StringBuilder();
            
                        // Lặp qua từng trang của tệp PDF
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            // Trích xuất văn bản từ mỗi trang
                            string text = PdfTextExtractor.GetTextFromPage(reader, i);
            
                            // Thêm văn bản vào StringBuilder để xử lý tiếp
                            textBuilder.Append(text);
                        }
            
                        // Lấy văn bản cuối cùng
                        string extractedText = textBuilder.ToString();
            
                        abc = extractedText;
                    }
                }
                
              
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return abc;
        }
        
        public async Task<FileDto> BaoCaoDuAnThuHoiToExcel(BaoCaoDuAnThuHoiToExcelInput input)
        {
            BaoCaoDuAnThuHoi_ExportToFileDto dataTrans = new BaoCaoDuAnThuHoi_ExportToFileDto();
            List<Data_DanhMuc_ListChiTietThuHoiDto> listData = new List<Data_DanhMuc_ListChiTietThuHoiDto>();
            
            var duAnThuHoi = await _duAnThuHoiRepository.FirstOrDefaultAsync(input.id);

            var queryListDanhMuc = await _danhMucThuHoiRepository.GetAllListAsync(p => p.DuAnThuHoiId == duAnThuHoi.Id);

            if (!queryListDanhMuc.IsNullOrEmpty())
            {
                foreach (var ct in queryListDanhMuc)
                {
                    Data_DanhMuc_ListChiTietThuHoiDto data = new Data_DanhMuc_ListChiTietThuHoiDto();
                    var listChiTiet =
                        await _chiTietThuHoiRepository.GetAllListAsync(p => p.DanhMucThuHoiId == ct.Id);
                    data.ListChiTietThuHoi = ObjectMapper.Map<List<ChiTietThuHoiDto>>(listChiTiet);
                    data.DanhMucThuHoi = ObjectMapper.Map<DanhMucThuHoiDto>(ct);
                    
                    listData.Add(data);
                }
            }

            dataTrans.DuAnThuHoi = ObjectMapper.Map<DuAnThuHoiDto>(duAnThuHoi);
            dataTrans.ListData = listData;
            
            return _duAnThuHoiesExcelExporter.BaoCaoDuAnThuHoi_ExportToFile(dataTrans);
        }
        public async Task<FileDto> TongHop_BaoCaoDuAnThuHoiToExcel(BaoCaoDuAnThuHoiToExcelInput input)
        {
            List<BaoCaoDuAnThuHoi_ExportToFileDto> listDataDuAn = new List<BaoCaoDuAnThuHoi_ExportToFileDto>();

            var listDuAnThuHoi = await _duAnThuHoiRepository.GetAllListAsync();
            foreach (var duAnThuHoi in listDuAnThuHoi)
            {
                BaoCaoDuAnThuHoi_ExportToFileDto dataTrans = new BaoCaoDuAnThuHoi_ExportToFileDto();
                List<Data_DanhMuc_ListChiTietThuHoiDto> listData = new List<Data_DanhMuc_ListChiTietThuHoiDto>();
                
                var queryListDanhMuc = await _danhMucThuHoiRepository.GetAllListAsync(p => p.DuAnThuHoiId == duAnThuHoi.Id);

                if (!queryListDanhMuc.IsNullOrEmpty())
                {
                    foreach (var ct in queryListDanhMuc)
                    {
                        Data_DanhMuc_ListChiTietThuHoiDto data = new Data_DanhMuc_ListChiTietThuHoiDto();
                        var listChiTiet =
                            await _chiTietThuHoiRepository.GetAllListAsync(p => p.DanhMucThuHoiId == ct.Id);
                        data.ListChiTietThuHoi = ObjectMapper.Map<List<ChiTietThuHoiDto>>(listChiTiet);
                        data.DanhMucThuHoi = ObjectMapper.Map<DanhMucThuHoiDto>(ct);
                    
                        listData.Add(data);
                    }
                }

                dataTrans.DuAnThuHoi = ObjectMapper.Map<DuAnThuHoiDto>(duAnThuHoi);
                dataTrans.ListData = listData;

                listDataDuAn.Add(dataTrans);
            }
            
            return _duAnThuHoiesExcelExporter.TongHop_BaoCaoDuAnThuHoi_ExportToFile(listDataDuAn);
        }
    }
}