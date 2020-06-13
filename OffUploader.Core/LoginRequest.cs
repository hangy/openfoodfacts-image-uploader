namespace OffUploader.Core
{
    using System;
    using MediatR;

    public class LoginRequest : IRequest
    {
        public LoginRequest(ProductOpenerSettings settings)
        {
            this.Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        public ProductOpenerSettings Settings { get; }
    }
}
