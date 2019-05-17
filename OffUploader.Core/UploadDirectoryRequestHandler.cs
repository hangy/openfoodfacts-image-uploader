namespace OffUploader.Core
{
    using MediatR;
    using OffUploader.Core.Logging;
    using System.Diagnostics;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UploadDirectoryRequestHandler : IRequestHandler<UploadDirectoryRequest>
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem;

        private readonly IMediator mediator;

        public UploadDirectoryRequestHandler(IFileSystem fileSystem, IMediator mediator)
        {
            this.fileSystem = fileSystem;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UploadDirectoryRequest request, CancellationToken cancellationToken)
        {
            var path = request.Path;
            var jpgs = this.fileSystem.Directory.GetJpegs(path).ToList();
            var code = request.Code;
            var settings = request.Settings;
            log.Info("Uploading JPEGs from {Directory} to product {Code}", path, code);
            var stopwatch = Stopwatch.StartNew();
            await this.mediator.Send(new UploadFilesToCodeRequest(settings, code, jpgs), cancellationToken).ConfigureAwait(false);
            stopwatch.Stop();
            log.Info("Uploaded JPEGs from {Directory} to product {Code} in {Duration}", path, code, stopwatch.Elapsed);
            return Unit.Value;
        }
    }
}
