namespace OffUploader.Core
{
    using System;
    using MediatR;

    public class ParseBarcodesAndUploadDirectory : IRequest
    {
        public ParseBarcodesAndUploadDirectory(ProductOpenerSettings settings, string path)
        {
            if (string.IsNullOrWhiteSpace(path))
            {
                throw new ArgumentException($"'{nameof(path)}' cannot be null or whitespace", nameof(path));
            }

            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.Path = path;
        }

        public string Path { get; }

        public ProductOpenerSettings Settings { get; }
    }
}