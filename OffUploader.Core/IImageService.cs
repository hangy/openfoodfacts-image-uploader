namespace OffUploader.Core
{
    using Refit;
    using System.Threading.Tasks;

    public interface IImageService
    {
        [Multipart]
        [Post("/cgi/product_image_upload.pl")]
        Task<UploadResult> UploadPhoto([AliasAs("\"code\"")]string code, [AliasAs("\"jqueryfileupload\"")]string isJqueryFileUpload, [AliasAs("\"imagefield\"")]string imageField, [AliasAs("\"imgupload_other_de\"")]StreamPart stream);
    }
}