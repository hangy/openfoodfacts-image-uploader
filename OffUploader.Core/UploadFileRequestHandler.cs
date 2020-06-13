namespace OffUploader.Core
{
    using System;
    using System.Diagnostics;
    using System.IO.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OffUploader.Core.Logging;
    using Refit;

    public class UploadFileRequestHandler : IRequestHandler<UploadFileRequest>
    {
        private readonly static ILog log = LogProvider.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem;

        private readonly RestServiceFactory restServiceFactory;

        public UploadFileRequestHandler(RestServiceFactory restServiceFactory, IFileSystem fileSystem)
        {
            this.restServiceFactory = restServiceFactory ?? throw new ArgumentNullException(nameof(restServiceFactory));
            this.fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
        }

        public Task<Unit> Handle(UploadFileRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            return this.HandleImpl(request);
        }

        private async Task<Unit> HandleImpl(UploadFileRequest request)
        {
            var imageService = this.restServiceFactory.GetService<IImageService>(request.Settings);
            using (var stream = this.fileSystem.File.OpenRead(request.Path))
            {
                var code = request.Code;
                var path = request.Path;
                var part = new StreamPart(stream, this.fileSystem.Path.GetFileName(path));
                log.Info("Uploading {File} to product {Code}", path, code);
                var stopwatch = Stopwatch.StartNew();
                var result = await imageService.UploadPhoto(code, "1", "other_de", part).ConfigureAwait(false);
                stopwatch.Stop();
                if (result.Image != null)
                {
                    log.Info("Uploaded {File} to product {Code} in {Duration}: {@UploadResult}", path, code, stopwatch.Elapsed, result);
                }
                else if (result.ImgId == -3)
                {
                    log.Info("Image {File} was already uploaded to {Code} before. Upload took {Duration}: {@UploadResult}", path, code, stopwatch.Elapsed, result);
                }
                else
                {
                    log.Error("Could not upload {File} to product {Code} in {Duration}: {@UploadResult}", path, code, stopwatch.Elapsed, result);
                    throw new InvalidOperationException(result.Error ?? result.Status ?? "Unknown Error");
                }
            }

            return Unit.Value;
        }
    }
}
