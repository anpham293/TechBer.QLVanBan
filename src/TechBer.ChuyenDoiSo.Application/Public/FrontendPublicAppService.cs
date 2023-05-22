using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Collections.Extensions;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.UI;
using TechBer.ChuyenDoiSo.Public.Dto;
using TechBer.ChuyenDoiSo.QLVB;
using TechBer.ChuyenDoiSo.QLVB.Dtos;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo;
using TechBer.ChuyenDoiSo.QuanLyKhoHoSo.Dtos;

namespace TechBer.ChuyenDoiSo.Public
{
    public class FrontendPublicAppService : ChuyenDoiSoAppServiceBase, IFrontendPublicAppService
    {
        private readonly IRepository<VanBanDuAn> _vanBanDuAnRepository;
        private readonly IRepository<ThungHoSo> _thungHoSoRepository;
        private readonly IRepository<DuAn> _duAnRepository;

        public FrontendPublicAppService(
            IRepository<VanBanDuAn> vanBanDuAnRepository,
            IRepository<ThungHoSo> thungHoSoRepository,
            IRepository<DuAn> duAnRepository
        )
        {
            _vanBanDuAnRepository = vanBanDuAnRepository;
            _thungHoSoRepository = thungHoSoRepository;
            _duAnRepository = duAnRepository;
        }

        public async Task<GetDataFromQrCodeResultDto> GetDataFromQrCode(GetDataFromQrCodeInputDto input)
        {
            ThungHoSo thungHoSo =
                await _thungHoSoRepository.FirstOrDefaultAsync(p => p.QrString.Equals(input.Qrstring));
            if (thungHoSo.IsNullOrDeleted())
            {
                throw new UserFriendlyException("QR không chính xác");
            }
            DuAn duAn = await  _duAnRepository.FirstOrDefaultAsync(thungHoSo.DuAnId.Value);

            List<VanBanDuAn> listvanBanDuAn = new List<VanBanDuAn>(); 
            listvanBanDuAn = _vanBanDuAnRepository.GetAll().WhereIf(true, p => p.ThungHoSoId == thungHoSo.Id).ToList();
            
            return new GetDataFromQrCodeResultDto()
            {
               ThungHoSo = ObjectMapper.Map<ThungHoSoDto>(thungHoSo),
               DuAn = ObjectMapper.Map<DuAnDto>(duAn),
               ListVanBanDuAn = ObjectMapper.Map<List<VanBanDuAnDto>>(listvanBanDuAn)
            };
        }
    }
}