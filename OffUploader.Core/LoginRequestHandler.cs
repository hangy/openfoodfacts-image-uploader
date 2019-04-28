namespace OffUploader.Core
{
    using MediatR;
    using OffUploader.Core.Logging;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;

    public class LoginRequestHandler : IRequestHandler<LoginRequest>
    {
        private static readonly ILog log = LogProvider.GetCurrentClassLogger();

        private readonly RestServiceFactory restServiceFactory;

        public LoginRequestHandler(RestServiceFactory restServiceFactory)
        {
            this.restServiceFactory = restServiceFactory;
        }

        public async Task<Unit> Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            var settings = request.Settings;
            var loginService = this.restServiceFactory.GetService<ILoginService>(settings);
            var data = new Dictionary<string, object> {
                { "user_id", settings.UserName },
                { "password", settings.Password },
                { ".submit", "Sign-in" }
            };

            await loginService.Login(data).ConfigureAwait(false);
            log.Info("Login successful");
            return Unit.Value;
        }
    }
}
