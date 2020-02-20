using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HackerNewsConsumerAPI;
using HackerNewsConsumerAPI.V1.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Xunit;

namespace HackerNewsConsumerAPITest
{
    /// <summary>
    /// Integration tests for Stories Controller
    /// </summary>
    public class StoriesControllerTests
    {
        private readonly HttpClient _client;
        public StoriesControllerTests()
        {
            var builder = new WebHostBuilder().UseEnvironment("Testing").UseStartup<Startup>();
            var server = new TestServer(builder);
            _client = server.CreateClient();
        }

        [Fact]
        public async Task Test_GetBestStories()
        {
            var response = await _client.GetAsync("/api/v1/stories");
            response.EnsureSuccessStatusCode();
            var stringResponse = await response.Content.ReadAsStringAsync();
            var stories = JsonConvert.DeserializeObject<IEnumerable<Story>>(stringResponse);
            Assert.Contains(stories, p => p.Title == "Explorabl.es");
            Assert.Equal(20, stories.Count());
        }


    }
}
