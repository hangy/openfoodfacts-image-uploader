namespace OffUploader.Core
{
    using System.Threading.Tasks;
    using Refit;

    public interface IImageService
    {
        [Multipart]
        [Post("/cgi/product_image_upload.pl")]
        Task<UploadResult> UploadPhoto([AliasAs("\"code\"")]string code, [AliasAs("\"jqueryfileupload\"")]string isJqueryFileUpload, [AliasAs("\"imagefield\"")]string imageField, [AliasAs("\"imgupload_other_de\"")]StreamPart stream);
    }
}