namespace OffUploader.Console
{
    using MediatR;
    using Microsoft.Extensions.Configuration;
    using OffUploader.Core;
    using Serilog;
    using SimpleInjector;
    using SimpleInjector.Lifestyles;
    using System;
    using System.Threading.Tasks;

    internal static class Program
    {
        internal async static Task<int> Main(string[] args)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddUserSecrets(typeof(Program).Assembly)
                    .Build();

                var logger = new LoggerConfiguration()
                    .ReadFrom.Configuration(configuration)
                    .CreateLogger();
                Log.Logger = logger;

                var settings = configuration.GetSection("ProductOpener").Get<ProductOpenerSettings>();

                var container = new Container { Options = { DefaultScopedLifestyle = new AsyncScopedLifestyle() } };
                OffUploaderBootstrapper.Bootstrap(container);
                container.Verify();
                var mediator = container.GetInstance<IMediator>();
                using (AsyncScopedLifestyle.BeginScope(container))
                {
                    await mediator.Send(new LoginRequest(settings)).ConfigureAwait(false);
                    await mediator.Send(new UploadFromCommandLineArgsRequest(settings, args)).ConfigureAwait(false);
                }
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Error executing the uploader");
                throw;
            }
        }
    }
}
