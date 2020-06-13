namespace OffUploader.Core
{
    using System;
    using MediatR;

    public class UploadFileRequest : IRequest
    {
        public UploadFileRequest(ProductOpenerSettings settings, string code, string path)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace", nameof(path));
            }

            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.Code = code;
            this.Path = path;
        }

        public ProductOpenerSettings Settings { get; }

        public string Code { get; }

        public string Path { get; }
    }
}