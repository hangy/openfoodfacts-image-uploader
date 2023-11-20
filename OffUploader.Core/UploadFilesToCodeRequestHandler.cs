namespace OffUploader.Core
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OffUploader.Core.Logging;

    public class UploadFilesToCodeRequestHandler : IRequestHandler<UploadFilesToCodeRequest>
    {
        private readonly static ILog log = LogProvider.GetCurrentClassLogger();

        private readonly IMediator mediator;

        public UploadFilesToCodeRequestHandler(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public Task Handle(UploadFilesToCodeRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return this.HandleImpl(request, cancellationToken);
        }

        private async Task HandleImpl(UploadFilesToCodeRequest request, CancellationToken cancellationToken)
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
        }
    }
}
