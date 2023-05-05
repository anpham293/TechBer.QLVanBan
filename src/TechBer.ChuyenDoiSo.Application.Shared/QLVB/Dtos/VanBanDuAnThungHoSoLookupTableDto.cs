namespace TechBer.ChuyenDoiSo.QLVB.Dtos
{
    public class VanBanDuAnThungHoSoLookupTableDto
    {
        public int Id { get; set; }

        public string DisplayName { get; set; }
    }

    public class SapXepHoSoVaoThungDto
    {
        public int? ThungHoSoId { get; set; }
        public int VanBanDuAnId { get; set; }
    }
}