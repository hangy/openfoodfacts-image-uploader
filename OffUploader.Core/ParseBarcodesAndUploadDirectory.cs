namespace OffUploader.Core
{
    using MediatR;

    public class ParseBarcodesAndUploadDirectory : IRequest
    {
        public ParseBarcodesAndUploadDirectory(ProductOpenerSettings settings, string path)
        {
            this.Settings = settings;
            this.Path = path;
        }

        public string Path { get; }

        public ProductOpenerSettings Settings { get; }
    }
}