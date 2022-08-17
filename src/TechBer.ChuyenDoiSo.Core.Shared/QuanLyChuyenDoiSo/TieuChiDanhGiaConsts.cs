namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo
{
    public class TieuChiDanhGiaConsts
    {

    }

    public enum XoaTieuChiState : int
    {
        XOA_THANH_CONG = 1,
        CHUA_XOA_HET_CON = 2,
        LOI_KHAC = 3
    }

    public enum DiChuyenTieuChiState : int
    {
        DI_CHUYEN_OK = 1,
        DI_CHUYEN_THAT_BAI = 2
    }

    public enum PhanNhomLevel1State : int
    {
        THONG_TIN_CHUNG = 1,
        NHAN_THUC_SO = 2,
        THE_CHE_SO = 3,
        HA_TANG_SO = 4,
        NHAN_LUC_SO = 5,
        AN_TOAN_THONG_TIN_MANG = 6,
        HOAT_DONG_CHINH_QUYEN_SO = 7,
        HOAT_DONG_KINH_TE_SO = 8,
        HOAT_DONG_XA_HOI_SO = 9,
        DO_THI_THONG_MINH = 10,
        THONG_TIN_VA_DU_LIEU_SO = 11
    }
}