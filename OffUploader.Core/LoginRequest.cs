namespace OffUploader.Core
{
    using MediatR;

    public class LoginRequest : IRequest
    {
        public LoginRequest(ProductOpenerSettings settings)
        {
            this.Settings = settings;
        }

        public ProductOpenerSettings Settings { get; }
    }
}
