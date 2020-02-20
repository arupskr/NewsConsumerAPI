using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HackerNewsConsumerAPI.V1.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Story
    {
        /// <summary>
        /// Story Identifier
        /// </summary>

        [JsonIgnore]
        [JsonProperty("id")]
        public int Id { get; set; }

        /// <summary>
        /// Story Title
        /// </summary>
        [JsonProperty("title")]
        public string Title { get; set; }

        /// <summary>
        /// The URL of the story
        /// </summary>
        [JsonProperty("url")]
        public string Uri { get; set; }

        /// <summary>
        /// The username of the item's author
        /// </summary>
        [JsonProperty("by")]
        public string PostedBy { get; set;  }

        /// <summary>
        /// Creation date of the story, Unix datetime
        /// </summary>
        [JsonProperty("time", NullValueHandling = NullValueHandling.Ignore)]
        public string Time { get; set; }

        /// <summary>
        /// The story's score or the votes
        /// </summary>
        [JsonProperty("score", NullValueHandling = NullValueHandling.Ignore)]
        public int Score { get; set; }

        /// <summary>
        /// Count of the number of comments
        /// </summary>
        [JsonProperty("descendants", NullValueHandling = NullValueHandling.Ignore)]
        public int CommentCount { get; set; }

    }
}
