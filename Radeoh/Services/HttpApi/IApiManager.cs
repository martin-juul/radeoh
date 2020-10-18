using System.Net.Http;
using System.Threading.Tasks;

namespace Radeoh.Services.HttpApi
{
    public interface IApiManager
    {
        Task<HttpResponseMessage> GetStations();
    }
}