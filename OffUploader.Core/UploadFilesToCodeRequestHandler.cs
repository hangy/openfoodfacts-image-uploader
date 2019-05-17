namespace OffUploader.Core
{
    using MediatR;
    using OffUploader.Core.Logging;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;

    public class UploadFilesToCodeRequestHandler : IRequestHandler<UploadFilesToCodeRequest>
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly IMediator mediator;

        public UploadFilesToCodeRequestHandler(IMediator mediator)
        {
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UploadFilesToCodeRequest request, CancellationToken cancellationToken)
        {
            var settings = request.Settings;
            var code = request.Code;
            var jpgs = request.Paths;
            log.Info("Uploading {FileCount} JPEGs from  to product {Code}", jpgs, code);
            var uploaded = 0;
            var stopwatch = Stopwatch.StartNew();
            foreach (var jpg in jpgs)
            {
                await this.mediator.Send(new UploadFileRequest(settings, code, jpg), cancellationToken).ConfigureAwait(false);
                ++uploaded;
            }

            stopwatch.Stop();
            log.Info("Uploaded {UploadedImagesCount} JPEGs from {@Paths} to product {Code} in {Duration}", uploaded, jpgs, code, stopwatch.Elapsed);
            return Unit.Value;
        }
    }
}
