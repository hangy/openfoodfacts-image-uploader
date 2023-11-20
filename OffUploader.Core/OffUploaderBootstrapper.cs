namespace OffUploader.Core
{
    using System;
    using System.Collections.Generic;
    using System.IO.Abstractions;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Reflection;
    using ImageMagick;
    using MediatR;
    using MediatR.Pipeline;
    using SimpleInjector;
    using ZXing;

    public static class OffUploaderBootstrapper
    {
        public static void Bootstrap(Container container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            container.Register<IFileSystem, FileSystem>(Lifestyle.Singleton);

            BootstrapMediator(container);
            BootstrapRefit(container);

            container.Register<IMagickImageFactory<byte>, MagickImageFactory>(Lifestyle.Singleton);
            container.Register<IBarcodeReaderGeneric>(() => new BarcodeReaderGeneric() { AutoRotate = true, Options = { TryHarder = true, TryInverted = true } }, Lifestyle.Singleton);
        }

        // Thanks! https://github.com/haison8x/MediatR.SimpleInjector/blob/master/src/MediatR.SimpleInjector/ContainerExtension.cs
        private static void BootstrapMediator(Container container)
        {
            var assemblies = new[] { typeof(OffUploaderBootstrapper).Assembly };
            var allAssemblies = new List<Assembly> { typeof(IMediator).GetTypeInfo().Assembly };
            allAssemblies.AddRange(assemblies);
            container.RegisterSingleton<IMediator, Mediator>();
            container.Register(typeof(IRequestHandler<,>), allAssemblies);

            // we have to do this because by default, generic type definitions (such as the Constrained Notification Handler) won't be registered
            var notificationHandlerTypes = container.GetTypesToRegister(typeof(INotificationHandler<>), assemblies, new TypesToRegisterOptions
            {
                IncludeGenericTypeDefinitions = true,
                IncludeComposites = false,
            });
            container.Register(typeof(INotificationHandler<>), notificationHandlerTypes);

            container.Collection.Register(typeof(IPipelineBehavior<,>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPreProcessor<>), Enumerable.Empty<Type>());
            container.Collection.Register(typeof(IRequestPostProcessor<,>), Enumerable.Empty<Type>());
        }

        private static void BootstrapRefit(Container container)
        {
            container.Register<HttpMessageHandler>(() => new HttpClientHandler
            {
                CookieContainer = new CookieContainer(),
                UseCookies = true,
                Credentials = new NetworkCredential("off", "off")
            }, Lifestyle.Scoped);

            container.Register<RestServiceFactory>(Lifestyle.Scoped);
        }
    }
}
