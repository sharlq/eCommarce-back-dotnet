using Microsoft.AspNetCore.Mvc.Testing;
using Infrastructure.Data;

namespace dotnet.tests
{
    [TestClass]
    public class ApiTest
    {
        private readonly AppDbContext _context;
        private readonly HttpClient _httpClient;
      

        public ApiTest()
        {
            var wepAppFactory = new WebApplicationFactory<Program>();
            _httpClient = wepAppFactory.CreateDefaultClient();
          
        }

        [TestMethod]
        public async Task DefaultRoute_returnHelloWorld()
        {

            var response = await _httpClient.GetAsync("/api/Product/1005");
            var stringResult = await response.Content.ReadAsStringAsync();

            Assert.AreEqual("OK", response.StatusCode.ToString());

        }


    }
}