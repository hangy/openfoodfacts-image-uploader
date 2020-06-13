namespace OffUploader.Core
{
    using System;
    using System.Collections.Generic;
    using MediatR;

    public class UploadFromCommandLineArgsRequest : IRequest
    {
        public UploadFromCommandLineArgsRequest(ProductOpenerSettings settings, IReadOnlyList<string> commandLineArguments)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
            this.CommandLineArguments = commandLineArguments ?? throw new ArgumentNullException(nameof(commandLineArguments));
        }

        public ProductOpenerSettings Settings { get; }

        public IReadOnlyList<string> CommandLineArguments { get; }
    }
}
