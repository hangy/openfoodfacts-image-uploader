namespace OffUploader.Core
{
    using System;
    using System.Net.Http;
    using Refit;

    public class RestServiceFactory
    {
        private readonly HttpMessageHandler httpMessageHandler;

        public RestServiceFactory(HttpMessageHandler httpMessageHandler)
        {
            this.httpMessageHandler = httpMessageHandler ?? throw new ArgumentNullException(nameof(httpMessageHandler));
        }

        public TService GetService<TService>(ProductOpenerSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.Uri == null)
            {
                throw new InvalidOperationException($"{nameof(settings)}.{nameof(settings.Uri)} cannot be null");
            }

            return RestService.For<TService>(settings.Uri.ToString(), new RefitSettings { HttpMessageHandlerFactory = () => this.httpMessageHandler });
        }
    }
}
