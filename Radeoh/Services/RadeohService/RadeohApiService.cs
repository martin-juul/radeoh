using System;
using System.Net.Http;

namespace Radeoh.Services.RadeohService
{
    public class RadeohApiService
    {
        private readonly NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly Uri _baseUrl = new Uri("https://radeoh.app/api");
        
        public void GetStations()
        {
            ApiService.ApiService client = new ApiService.ApiService(_baseUrl);

            var res = client.SendJsonRequest(HttpMethod.Get, new Uri("/stations"), null);
            
            _logger.Debug(res.Result.ToString());
        }
    }
}