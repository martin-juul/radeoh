using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Refit;

namespace Radeoh.Services.HttpApi
{
    [Headers("Content-Type: application/json", "Accept: application/json")]
    public interface IRadeohApi
    {
        [Get("/api/stations")]
        Task<HttpResponseMessage> GetStations(CancellationToken cancellationToken);
    }
}