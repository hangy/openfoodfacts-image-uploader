namespace OffUploader.Core
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public class UploadFilesToCodeRequest : IRequest
    {
        public UploadFilesToCodeRequest(ProductOpenerSettings settings, string code, IReadOnlyCollection<string> paths)
        {
            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException($"'{nameof(code)}' cannot be null or whitespace", nameof(code));
            }

            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.Code = code;
            this.Paths = paths ?? throw new ArgumentNullException(nameof(paths));
        }

        public ProductOpenerSettings Settings { get; }

        public string Code { get; }

        public IReadOnlyCollection<string> Paths { get; }
    }
}