using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using GitStats.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace GitStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatsController : ControllerBase
    {
        private static readonly HttpClient client = new HttpClient();


        // GET: api/Stats/5
        [HttpGet("{projectWithOwner}", Name = "GetStats")]
        public async Task<string> GetAsync(string projectWithOwner)
        {
            return await GetContributorsAsync(projectWithOwner);
        }

        public async Task<string> GetContributorsAsync(string projectWithOwner)
        {
            string project = projectWithOwner.Split('-')[0];
            string owner = projectWithOwner.Split('-')[1];
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync("https://api.github.com/repos/" + owner + "/" + project + "/stats/contributors");

            JArray arr = JArray.Parse(await stringTask);
            List<UserModel> authors = new List<UserModel>();
            foreach(JObject jauthor in arr)
            {
                var jObjectauthor = JsonConvert.DeserializeObject<JObject>(jauthor.ToString());
                authors.Add(jObjectauthor.Value<JObject>("author").ToObject<UserModel>());
            }
            return JsonConvert.SerializeObject(authors);
        }
    }
}