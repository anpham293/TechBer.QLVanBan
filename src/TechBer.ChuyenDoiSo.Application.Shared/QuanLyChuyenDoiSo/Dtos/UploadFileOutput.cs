using Abp.Web.Models;

namespace TechBer.ChuyenDoiSo.QuanLyChuyenDoiSo.Dtos
{
    public class UploadFileOutput : ErrorInfo
    {
        public string FileName { get; set; }

        public string FileType { get; set; }

        public string FileToken { get; set; }

        public UploadFileOutput()
        {

        }

        public UploadFileOutput(ErrorInfo error)
        {
            Code = error.Code;
            Details = error.Details;
            Message = error.Message;
            ValidationErrors = error.ValidationErrors;
        }
    }
}
