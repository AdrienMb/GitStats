using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GitStats.Models
{
    public class ProjectModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("html_url")]
        public string Html_url { get; set; }

        [JsonProperty("stargazers_count")]
        public int Stargazers_count { get; set; }

        [JsonProperty("owner")]
        public UserModel Owner { get; set; }

    }
}
