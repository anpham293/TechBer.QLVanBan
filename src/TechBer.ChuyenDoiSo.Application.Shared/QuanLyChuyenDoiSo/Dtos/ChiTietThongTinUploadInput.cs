namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class ChiTietThongTinUploadInput
    {
        public int Id { get; set; }

        public string FileName { get; set; }

        public string FileToken { get; set; }

        public string FileType { get; set; }

        public string OldFile { get; set; }

        public string NoiDung { get; set; }
    }
}
