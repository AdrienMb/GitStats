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


        // GET: api/Stats/:project&:owner
        [HttpGet("{projectWithOwner}", Name = "GetStats")]
        public async Task<string> GetAsync(string projectWithOwner)
        {

            string project = projectWithOwner.Split('&')[0];
            string owner = projectWithOwner.Split('&')[1];
            List<UserModel> contributors = await GetContributorsAsync(project, owner);
            List<UserModel> contributorsWithCommits = await GetCommitsAsync(project, owner, contributors);
            return JsonConvert.SerializeObject(contributorsWithCommits, Formatting.Indented);
        }

        public async Task<List<UserModel>> GetContributorsAsync(string project, string owner)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync("https://api.github.com/repos/" + owner + "/" + project + "/stats/contributors");

            JArray arr = JArray.Parse(await stringTask);
            List<UserModel> authors = new List<UserModel>();
            foreach(JObject jauthor in arr)
            {
                UserModel author = jauthor.Value<JObject>("author").ToObject<UserModel>();
                author.Commits = new List<DateTime>();
                authors.Add(author);
            }
            return authors;
        }
        public async Task<List<UserModel>> GetCommitsAsync(string project, string owner, List<UserModel> contributors)
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/vnd.github.v3+json"));
            client.DefaultRequestHeaders.Add("User-Agent", "request");

            var stringTask = client.GetStringAsync("https://api.github.com/repos/" + owner + "/" + project + "/commits?per_page=100");
            JArray arr = JArray.Parse(await stringTask);
             foreach(JObject jcommit in arr)
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
                    if (committer!=null)
                        committer.Commits.Add(monday);
                }
            }

            return contributors;

        }
    }
}