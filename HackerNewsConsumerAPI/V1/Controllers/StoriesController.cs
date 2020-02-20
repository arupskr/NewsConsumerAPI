using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HackerNewsConsumerAPI.V1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Newtonsoft.Json;

namespace HackerNewsConsumerAPI.V1.Controllers
{
    /// <summary>
    /// Controller to return HackerNews stories, V1.0
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class StoriesController : ControllerBase
    {
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="clientFactory"></param>
        public StoriesController(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Returns 20 best stories.If the number of stories to be returned increases in future considering the batching the parallel calls
        /// Can be configured later to change the number stored externally
        /// Also the end points can be moved to a configuration setting with a base endpoint
        /// caching for 5 mins, assuming that the stories do not change frequently
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ResponseCache(Duration = 300)]
        public async Task<IEnumerable<Story>> Get()
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync("https://hacker-news.firebaseio.com/v0/beststories.json").ConfigureAwait(false);
            var data  = response.Content.ReadAsStringAsync().Result;
            var objects = JsonConvert.DeserializeObject<List<string>>(data);
            return GetBestStories(objects, 20).Result;
        }

        private async Task<Story> GetStory(string id)
        {
            var client = _clientFactory.CreateClient();
            var response = await client.GetAsync($"https://hacker-news.firebaseio.com/v0/item/{id}.json").ConfigureAwait(false);
            var data = response.Content.ReadAsStringAsync().Result;
            var story = JsonConvert.DeserializeObject<Story>(data);
            return story;

        }

        private async Task<IEnumerable<Story>> GetBestStories(IEnumerable<string> ids, int numberOfStories)
        {
            var storyIdsToFetch = ids.Take(numberOfStories);
            var tasks = storyIdsToFetch.Select(GetStory);
            var bestStories = await Task.WhenAll(tasks);
            return bestStories;
        }
    }
}