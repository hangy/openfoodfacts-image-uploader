namespace OffUploader.Core
{
    using System;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;

    public class ParseBarcodesAndUploadDirectoryHandler : IRequestHandler<ParseBarcodesAndUploadDirectory>
    {
        private readonly IFileSystem fileSystem;

        private readonly IMediator mediator;

        public ParseBarcodesAndUploadDirectoryHandler(IFileSystem fileSystem, IMediator mediator)
        {
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task Handle(ParseBarcodesAndUploadDirectory request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var path = request.Path;
            var jpgs = this.fileSystem.Directory.GetJpegs(path).ToList();
            return this.mediator.Send(new ParseBarcodesAndUploadFiles(request.Settings, jpgs), cancellationToken);
        }
    }
}
