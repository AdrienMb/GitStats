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
        private readonly string githubUrl = "https://api.github.com/repos/";

        // GET: api/Stats/project/owner
        [HttpGet("{project}/{owner}", Name = "GetStats")]
        public async Task<string> GetAsync(string project, string owner)
        {
            Task<JArray> jContributorsT = CallContributorsAsync(project, owner);
            Task<JArray> jCommitsT = CallCommitsAsync(project, owner);
            JArray[] responses = await Task.WhenAll(jContributorsT, jCommitsT);
            List<UserModel> contributors = new List<UserModel>();
            foreach (JObject jauthor in responses[0])
            {
                UserModel author = jauthor.Value<JObject>("author").ToObject<UserModel>();
                author.Commits = new List<DateTime>();
                contributors.Add(author);
            }
            foreach (JObject jcommit in responses[1])
            {
                JObject jauthor = jcommit.Value<JObject>("author");
                if (jauthor != null)
                {
                    int committerId = jauthor.Value<int>("id");
                    DateTime date = jcommit.Value<JObject>("commit").Value<JObject>("author").Value<DateTime>("date");
                    int delta = DayOfWeek.Monday - date.DayOfWeek;
                    DateTime monday = date.AddDays(delta);
                    UserModel committer = contributors.Find(contributor => contributor.Id == committerId);
                    System.Diagnostics.Debug.WriteLine(date);
                    if (committer != null)
                        committer.Commits.Add(monday);
                }
            }
            return JsonConvert.SerializeObject(contributors, Formatting.Indented);
        }

        public async Task<JArray> CallContributorsAsync(string project, string owner)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync(githubUrl + owner + "/" + project + "/stats/contributors");

            return JArray.Parse(await stringTask);
        }
        public async Task<JArray> CallCommitsAsync(string project, string owner)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync(githubUrl + owner + "/" + project + "/commits?per_page=100");
            return JArray.Parse(await stringTask);
        }
    }
}