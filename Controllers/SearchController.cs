using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using GitStats.Models;

namespace GitStats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {

        private static readonly HttpClient client = new HttpClient();
        private readonly string githubUrl = "https://api.github.com/search/repositories?per_page=6&q=";

        // GET: api/Search/project/page
        [HttpGet("{project}/{page}", Name = "GetSearch")]
        public async Task<string> GetAsync(string project, string page)
        {

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync(githubUrl + project + "&page=" + page);
            JObject response = JObject.Parse(await stringTask);
            var projects = response.Value<JArray>("items").ToObject<List<ProjectModel>>();
            return JsonConvert.SerializeObject(projects, Formatting.Indented);
        }
    }
}
