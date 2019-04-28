namespace OffUploader.Core
{
    using MediatR;
    using OffUploader.Core.Logging;
    using Refit;
    using System;
    using System.Diagnostics;
    using System.IO.Abstractions;
    using System.Threading;
    using System.Threading.Tasks;

    public class UploadFileRequestHandler : IRequestHandler<UploadFileRequest>
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly IFileSystem fileSystem;

        private readonly RestServiceFactory restServiceFactory;

        public UploadFileRequestHandler(RestServiceFactory restServiceFactory, IFileSystem fileSystem)
        {
            this.restServiceFactory = restServiceFactory;
            this.fileSystem = fileSystem;
        }

        public async Task<Unit> Handle(UploadFileRequest request, CancellationToken cancellationToken)
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
                else if  (result.ImgId == -3)
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
