using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using Abp.Linq.Extensions;
using System.Collections.Generic;
using System.Net.Http;
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
using Karion.BusinessSolution.EinvoiceExtension;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechBer.ChuyenDoiSo.Common;

namespace TechBer.ChuyenDoiSo.QuanLyThuHoiTamUng
{
    [AbpAuthorize(AppPermissions.Pages_DuAnThuHoies)]
    public class DuAnThuHoiesAppService : ChuyenDoiSoAppServiceBase, IDuAnThuHoiesAppService
    {
        private readonly IRepository<DuAnThuHoi, long> _duAnThuHoiRepository;
        private readonly IDuAnThuHoiesExcelExporter _duAnThuHoiesExcelExporter;
        private readonly IRepository<ChiTietThuHoi, long> _chiTietThuHoiRepository;
        private readonly IRepository<DanhMucThuHoi, long> _danhMucThuHoiRepository;

        public DuAnThuHoiesAppService(IRepository<DuAnThuHoi, long> duAnThuHoiRepository,
                                      IDuAnThuHoiesExcelExporter duAnThuHoiesExcelExporter,
                                      IRepository<ChiTietThuHoi, long> chiTietThuHoiRepository,
                                      IRepository<DanhMucThuHoi, long> danhMucThuHoiRepository)
        {
            _duAnThuHoiRepository = duAnThuHoiRepository;
            _duAnThuHoiesExcelExporter = duAnThuHoiesExcelExporter;
            _chiTietThuHoiRepository = chiTietThuHoiRepository;
            _danhMucThuHoiRepository = danhMucThuHoiRepository;
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
            duAnThuHoi.MaDATH = "TH" +input.NamQuanLy + "-" + soDuAn.ToString(fmt);

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
                var request = new HttpRequestMessage(HttpMethod.Post, "https://techber.vn/jwt-login.html");
                client.DefaultRequestHeaders.Add("User-Agent", "ASP.Net Core"+ AbpSession.UserId +"");
                var content = new StringContent("{\r\n    \"username\":\"zalointegrated\",\r\n    \"password\":\"Techber@123\"\r\n}\r\n", null, "application/json");
                request.Content = content;
                var response = await client.SendAsync(request);
                response.EnsureSuccessStatusCode();
                var ketqua = await response.Content.ReadAsStringAsync();

                var ketquaConvert = JsonConvert.DeserializeObject<TestDto>(ketqua);

                var token = ketquaConvert.token;

                //string contentType = "application/json;charset=utf-8";
                // string phone = "0949646698";
                //
                // string accessToken =
                //     "ATwL5per0niltf8NCJuD42RLcHyoA7P7NgAq73eQIszPauvZAm9WCbxfZZyAUsj6G_Vr4YTk71KsuiDWNMHJ61U4WtPo5XqE7hE2N71INpmJoC0oP2qYNJkyvofn6taW1lofQLPhIMCstgSHM49gIKhN-X8vTMblM-wZCKHKJLbIohqq9tTSQ5V9lW0fUN96MURd8mjIEdKVx_e8O6XaM3VBYW5UIrj4DQ6-76WOG5KElO4pVW5_GY6ZWHLJ9cSOHvk9NJOvG0HTajK2AIe8QIEUp1zn13va8OxN4cnh5HbX9zEdFpqZ0Hq";
                // List<string> data = new List<string>();
                // data.Add("access_token=" + accessToken);
                // data.Add("data=%7B%22user_id%22%3A%22" + phone + "%22%2C%7D");
                //
                // string result =
                //     CreateRequest.karionGetZaloGetApi("https://openapi.zalo.me/v2.0/oa/getprofile", data);
                //
                // var ketquaTraVe = JObject.Parse(result);
                // bool hasErrors = (int) ketquaTraVe["error"] != 0;
                // if (!hasErrors)
                // {
                //     string user_id = (string) ketquaTraVe["data"]["user_id"];
                //     var message = "test";
                //
                //     string dataMessage = @"{ ""recipient"": { ""user_id"": """ + user_id +
                //                          @""" }, ""message"": { ""text"": """ + message + @""" } }";
                //     string resultMes = CreateRequest.karionGetZaloAPI(
                //         "https://openapi.zalo.me/v2.0/oa/message?access_token=" + accessToken, dataMessage,
                //         "POST", contentType);
                //     var ketquaTraVeMes = JObject.Parse(resultMes);
                //     bool hasErrorsMes = (int) ketquaTraVeMes["error"] != 0;
                // }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return "";
        }
    }
}