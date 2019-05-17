namespace OffUploader.Core
{
    using MediatR;
    using System.Collections.Generic;

    public class UploadFilesToCodeRequest : IRequest
    {
        public UploadFilesToCodeRequest(ProductOpenerSettings settings, string code, IReadOnlyCollection<string> paths)
        {
            this.Settings = settings;
            this.Code = code;
            this.Paths = paths;
        }

        public ProductOpenerSettings Settings { get; }

        public string Code { get; }

        public IReadOnlyCollection<string> Paths { get; }
    }
}