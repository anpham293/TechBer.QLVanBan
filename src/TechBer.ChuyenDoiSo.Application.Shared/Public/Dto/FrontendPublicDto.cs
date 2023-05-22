using System.Collections.Generic;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;

namespace TechBer.ChuyenDoiSo.Public.Dto
{
    public class GetDataFromQrCodeInputDto
    {
        public string Qrstring { get; set; }
    }
    public class GetDataFromQrCodeResultDto
    {
        public ThungHoSoDto ThungHoSo { get; set; }
        public DuAnDto DuAn { get; set; }
        public List<VanBanDuAnDto> ListVanBanDuAn { get; set; }
    }
    
}