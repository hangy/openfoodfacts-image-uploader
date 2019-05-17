namespace OffUploader.Core
{
    using MediatR;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class ParseBarcodesAndUploadDirectoryHandler : IRequestHandler<ParseBarcodesAndUploadDirectory>
    {
        private readonly IFileSystem fileSystem;

        private readonly IMediator mediator;

        public ParseBarcodesAndUploadDirectoryHandler(IFileSystem fileSystem, IMediator mediator)
        {
            this.fileSystem = fileSystem;
            this.mediator = mediator;
        }

        public Task<Unit> Handle(ParseBarcodesAndUploadDirectory request, CancellationToken cancellationToken)
        {
            var path = request.Path;
            var jpgs = this.fileSystem.Directory.GetJpegs(path).ToList();
            return this.mediator.Send(new ParseBarcodesAndUploadFiles(request.Settings, jpgs), cancellationToken);
        }
    }
}
