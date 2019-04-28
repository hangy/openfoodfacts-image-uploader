namespace OffUploader.Core
{
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface ILoginService
    {
        [Post("/cgi/session.pl")]
        Task Login([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }
}