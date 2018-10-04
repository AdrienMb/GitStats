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
     

        // GET: api/Search/5
        [HttpGet("{project}", Name = "GetSearch")]
        public async Task<string> GetAsync(string project)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync("https://api.github.com/search/repositories?per_page=6&q=" + project);

            dynamic stuff = JObject.Parse(await stringTask);
            Console.WriteLine(stuff);
            var result = JsonConvert.DeserializeObject<JObject>(stuff.ToString());
            var projects = result.Value<JArray>("items").ToObject<List<ProjectModel>>();
            return JsonConvert.SerializeObject(projects);
        }
    }
}
