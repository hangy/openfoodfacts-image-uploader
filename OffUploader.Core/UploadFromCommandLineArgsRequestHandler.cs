namespace OffUploader.Core
{
    using MediatR;
    using System;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class UploadFromCommandLineArgsRequestHandler : IRequestHandler<UploadFromCommandLineArgsRequest>
    {
        private readonly IFileSystem fileSystem;

        private readonly IMediator mediator;

        public UploadFromCommandLineArgsRequestHandler(IFileSystem fileSystem, IMediator mediator)
        {
            this.fileSystem = fileSystem;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UploadFromCommandLineArgsRequest request, CancellationToken cancellationToken)
        {
            var args = request.CommandLineArguments;

            if (args.Count < 1)
            {
                throw new ArgumentException($"Missing arguments. Please specify a directory, or a product code and a file.", nameof(request));
            }

            var firstIsCode = args[0].IsValidCode();
            var firstIsCodeFromFileName = !firstIsCode && args.Count > 0 && this.fileSystem.Path.GetFileName(args[0]).IsValidCode();
            if (firstIsCode && args.Count == 2 && this.fileSystem.Directory.Exists(args[1]))
            {
                await this.mediator.Send(new UploadDirectoryRequest(request.Settings, args[0], args[1]), cancellationToken).ConfigureAwait(false);
            }
            else if (firstIsCode && args.Skip(1).All(f => this.fileSystem.File.Exists(f)))
            {
                await Task.WhenAll(args.Skip(1).Select(f => this.mediator.Send(new UploadFileRequest(request.Settings, args[0], f), cancellationToken))).ConfigureAwait(false);
            }
            else if (!firstIsCode && args.Count == 1 && this.fileSystem.Directory.Exists(args[0]))
            {
                await this.mediator.Send(new ParseBarcodesAndUploadDirectory(request.Settings, args[0]), cancellationToken).ConfigureAwait(false);
            }
            else if (!firstIsCode && firstIsCodeFromFileName && this.fileSystem.Directory.Exists(args[0]))
            {
                await this.mediator.Send(new UploadDirectoryRequest(request.Settings, this.fileSystem.Path.GetFileName(args[0]), args[0]), cancellationToken).ConfigureAwait(false);
            }
            else if (!firstIsCode && args.Skip(1).All(f => this.fileSystem.File.Exists(f)))
            {
                await this.mediator.Send(new ParseBarcodesAndUploadFiles(request.Settings, args.Skip(1).Where(f => this.fileSystem.File.Exists(f)).ToList()), cancellationToken).ConfigureAwait(false);
            }
            else
            {
                throw new ArgumentException($"Invalid arguments. Please specify at least a product code and file or directory.", nameof(request));
            }

            return Unit.Value;
        }
    }
}
