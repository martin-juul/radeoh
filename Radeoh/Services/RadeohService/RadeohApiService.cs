using System;
using System.Net.Http;

namespace Radeoh.Services.RadeohService
{
    public class RadeohApiService
    {
        private readonly Uri _baseUrl = new Uri("https://radeoh.app/api");
        
        public void GetStations()
        {
            ApiService.ApiService client = new ApiService.ApiService(_baseUrl);
        }
    }
}