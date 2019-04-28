﻿namespace OffUploader.Core
{
    using MediatR;

    public class UploadDirectoryRequest : IRequest
    {
        public UploadDirectoryRequest(ProductOpenerSettings settings, string code, string path)
        {
            this.Settings = settings;
            this.Code = code;
            this.Path = path;
        }

        public ProductOpenerSettings Settings { get; }

        public string Code { get; }

        public string Path { get; }

    }
}
