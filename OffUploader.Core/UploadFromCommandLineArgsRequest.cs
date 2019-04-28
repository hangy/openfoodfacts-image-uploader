namespace OffUploader.Core
{
    using MediatR;
    using System.Collections.Generic;

    public class UploadFromCommandLineArgsRequest : IRequest
    {
        public UploadFromCommandLineArgsRequest(ProductOpenerSettings settings, IReadOnlyList<string> commandLineArguments)
        {
            this.Settings = settings;
            this.CommandLineArguments = commandLineArguments;
        }

        public ProductOpenerSettings Settings { get; }

        public IReadOnlyList<string> CommandLineArguments { get; }
    }
}
