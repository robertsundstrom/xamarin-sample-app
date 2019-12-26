using System.Net.Http;
using System.Threading.Tasks;

using Xunit;

namespace App1.MobileAppService.Tests
{

    public class BasicTests
        : IClassFixture<CustomWebApplicationFactory<App1.MobileAppService.Startup>>
    {
        private readonly CustomWebApplicationFactory<App1.MobileAppService.Startup> _factory;

        public BasicTests(CustomWebApplicationFactory<App1.MobileAppService.Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/api/items")]
        [InlineData("/api/token")]
        [InlineData("/api/registration")]
        public async Task Get_EndpointReturnsFailure(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var exception = Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode()); // Status Code 200-299
        }


        [Theory]
        [InlineData("/api/items")]
        public async Task Get_EndpointReturnsUnauthorized(string url)
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            var exception = Assert.Throws<HttpRequestException>(() => response.EnsureSuccessStatusCode()); // Status Code 200-299

            Assert.Contains("401", exception.Message);
        }
    }
}
