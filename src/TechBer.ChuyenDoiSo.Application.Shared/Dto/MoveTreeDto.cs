namespace TechBer.ChuyenDoiSo.Dto
{
    public class MoveTreeDto
    {
        public int Id { get; set; }

        public int? NewParentId { get; set; }
        public int Position { get; set; }
    }
    public enum XoaTreeState : int
    {
        XOA_THANH_CONG = 1,
        CHUA_XOA_HET_CON = 2,
        LOI_KHAC = 3
    }

    public enum DiChuyenTreeState : int
    {
        DI_CHUYEN_OK = 1,
        DI_CHUYEN_THAT_BAI = 2
    }
}