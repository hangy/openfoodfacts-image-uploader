namespace OffUploader.Core
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public class ParseBarcodesAndUploadFiles : IRequest
    {
        public ParseBarcodesAndUploadFiles(ProductOpenerSettings settings, IReadOnlyCollection<string> paths)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.Paths = paths ?? throw new ArgumentNullException(nameof(paths));
        }

        public IReadOnlyCollection<string> Paths { get; }

        public ProductOpenerSettings Settings { get; }
    }
}