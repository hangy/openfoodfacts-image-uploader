namespace OffUploader.Core
{
    using Refit;
    using System.Net.Http;

    public class RestServiceFactory
    {
        private readonly HttpMessageHandler httpMessageHandler;

        public RestServiceFactory(HttpMessageHandler httpMessageHandler)
        {
            this.httpMessageHandler = httpMessageHandler;
        }

        public TService GetService<TService>(ProductOpenerSettings settings) => RestService.For<TService>(settings.Uri.ToString(), new RefitSettings { HttpMessageHandlerFactory = () => this.httpMessageHandler });
    }
}
