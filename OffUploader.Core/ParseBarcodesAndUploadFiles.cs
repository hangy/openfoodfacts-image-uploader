namespace OffUploader.Core
{
    using MediatR;
    using System.Collections.Generic;

    public class ParseBarcodesAndUploadFiles : IRequest
    {
        public ParseBarcodesAndUploadFiles(ProductOpenerSettings settings, IReadOnlyCollection<string> paths)
        {
            this.Settings = settings;
            this.Paths = paths;
        }

        public IReadOnlyCollection<string> Paths { get; }

        public ProductOpenerSettings Settings { get; }
    }
}