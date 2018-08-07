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
        private KeepNotesContext _context;


            public KeepIntegrationtest()
            {
                var host = new TestServer(new WebHostBuilder()
                    .UseEnvironment("Testing")
                    .UseStartup<Startup>());
            _context = host.Host.Services.GetService(typeof(KeepNotesContext)) as KeepNotesContext;
                _client = host.CreateClient();
            List<Notes> note1 = new List<Notes>(){ new Notes { Title = "First", Text = "First sentence", PinStat = true, checklist = new List<CheckList>() { new CheckList { list = "hello" }, new CheckList { list = "brother" } },
                label = new List<Label>() { new Label { label = "number1" }, new Label { label = "number2" } } },new Notes
                {
                    Title = "Second",
                    Text = "Second sentence",
                    PinStat = true,
                    checklist = new List<CheckList>() { new CheckList { list = "hello2" }, new CheckList { list = "brother2" } },
                    label = new List<Label>() { new Label { label = "number3" }, new Label { label = "number4" } }
                } };
            _context.Notes.AddRange(note1);
            _context.SaveChanges();
            //_context = host.Host.Services.GetService(typeof(NoteContext)) as NoteContext;
            //_client = host.CreateClient();
            //_context.Note.AddRange(TestNoteProper);
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

        [Fact]
            public async Task TestGetAsync()
            {
                var Response = await _client.GetAsync("/api/notes");
                var ResponseBody = await Response.Content.ReadAsStringAsync();
            //Console.WriteLine("Body"+ResponseBody.);
            //Assert.Equal(2, ResponseBody.Length);
            Response.EnsureSuccessStatusCode();
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
        public async Task TestGetByTitle()
        {
            var Response = await _client.GetAsync("/api/notes/title/First");
            var reponseString = await Response.Content.ReadAsStringAsync();
            var responsedata = JsonConvert.DeserializeObject<List<Notes>>(reponseString);
            Response.EnsureSuccessStatusCode();
        }
        [Fact]
        public async Task TestPut()
        {
            var notes =
               new Notes()
               {
                   ID = 1,
                   Title = "My First Note in PUT",
                   Text = "Put test",
                   PinStat = false,
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
            var Response = await _client.PutAsync("/api/Notes/1",stringContent);
            var responseString = await Response.Content.ReadAsStringAsync();
            var responsedata = JsonConvert.DeserializeObject<Notes>(responseString);
            Response.EnsureSuccessStatusCode();

        }
        [Fact]
        public async Task TestDelete()
        {
            var Response = await _client.DeleteAsync("/api/notes/1");
            var reponseString = await Response.Content.ReadAsStringAsync();
            var responsedata = JsonConvert.DeserializeObject<Notes>(reponseString);
            Response.EnsureSuccessStatusCode();

        }

    }
}
