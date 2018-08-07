using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using KeepNotes;
using KeepNotes.Models;
using KeepNotes.Controllers;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text;

namespace Keeptest
{
    public class KeepIntegrationtest
    {
            private HttpClient _client;

            public KeepIntegrationtest()
            {
                var host = new TestServer(new WebHostBuilder()
                    .UseEnvironment("Testing")
                    .UseStartup<Startup>());
                _client = host.CreateClient();
            }

            [Fact]
            public async Task TestGetAsync()
            {
                var Response = await _client.GetAsync("/api/notes");
                var ResponseBody = await Response.Content.ReadAsStringAsync();
                //Console.WriteLine("Body"+ResponseBody.);
                Assert.Equal(2, ResponseBody.Length);
            }

            [Fact]
            public async Task TestGetById()
            {
                var Response = await _client.GetAsync("/api/notes/99");
                //Console.WriteLine(await Response.Content.ReadAsStringAsync());
                //var ResponseBody = await Response.Content.ReadAsStringAsync();
                Assert.Equal("NotFound", Response.StatusCode.ToString());
            }

            [Fact]
            public async Task TestPost()
            {
                var notes =
                    new Notes()
                    {
                        Title = "My First Note",
                        Text = "This is my plaintext",
                        PinStat = true,
                        checklist = new List<CheckList>()
                        {
                        new CheckList()
                        {
                            list ="checklist data 1",
                        }
                        },
                        label = new List<Label>()
                        {
                        new Label()
                        {
                            label ="labeldata 1"
                        }
                        }
                    };
                var json = JsonConvert.SerializeObject(notes);
                var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");
                var Response = await _client.PostAsync("/api/notes", stringContent);
                var ResponseGet = await _client.GetAsync("/api/notes");
                ResponseGet.EnsureSuccessStatusCode();
            }
        
    }
}
