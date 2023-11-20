namespace OffUploader.Core
{
    using System;
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using MediatR;
    using OffUploader.Core.Logging;

    public class LoginRequestHandler : IRequestHandler<LoginRequest>
    {
        private readonly static ILog log = LogProvider.GetCurrentClassLogger();

        private readonly RestServiceFactory restServiceFactory;

        public LoginRequestHandler(RestServiceFactory restServiceFactory)
        {
            this.restServiceFactory = restServiceFactory ?? throw new ArgumentNullException(nameof(restServiceFactory));
        }

        public async Task Handle(LoginRequest request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var settings = request.Settings;
            if (settings == null)
            {
                throw new InvalidOperationException("Settings are invalid");
            }

            var loginService = this.restServiceFactory.GetService<ILoginService>(settings);
            var data = new Dictionary<string, object> {
#pragma warning disable CS8604
                { "user_id", settings.UserName },
                { "password", settings.Password },
#pragma warning restore CS8604 // Mögliches Nullverweisargument.
                { ".submit", "Sign-in" }
            };

            await loginService.Login(data).ConfigureAwait(false);
            log.Info("Login successful");
        }
    }
}
