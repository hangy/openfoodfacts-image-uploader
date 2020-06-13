namespace OffUploader.Core
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Refit;

    public interface ILoginService
    {
        [Post("/cgi/session.pl")]
        Task Login([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}