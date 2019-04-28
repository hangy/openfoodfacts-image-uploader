namespace OffUploader.Core
{
    public class UploadResult
    {
        public string? Status { get; set; }

        public int? ImgId { get; set; }

        public string? Error { get; set; }

        public UploadedImage? Image { get; set; }
    }
}