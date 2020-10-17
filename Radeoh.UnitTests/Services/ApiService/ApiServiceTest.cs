using System.Net.Http;
using Xunit;
using FluentAssertions;

namespace Radeoh.UnitTests.Services.ApiService
{
    public class ApiServiceTest
    {
        [Fact]
        public void TestCanInstantiateApiService()
        {
            var client = new Radeoh.Services.ApiService.ApiService(new HttpClient());

            client.Should().BeOfType<Radeoh.Services.ApiService.ApiService>();
        }
    }
}