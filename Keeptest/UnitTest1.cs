using System;
using Xunit;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

using KeepNotes;
using KeepNotes.Models;
using KeepNotes.Controllers;

namespace Keeptest
{
    public class KeepContext
    {
        NotesController _controller;
        public KeepContext()
        {
            //var optionBuilder = new DbContextOptionsBuilder<KeepNotesContext>();
            //optionBuilder.UseInMemoryDatabase("TestDB");
            var optionsBuilder = new DbContextOptionsBuilder<KeepNotesContext>();
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            var todocontext = new KeepNotesContext(optionsBuilder.Options);
            _controller = new NotesController(todocontext);
            createtestdata(todocontext);
        }

        public void createtestdata(KeepNotesContext todocontext)
        {
            List<Notes> note1 = new List<Notes>(){ new Notes { Title = "First", Text = "First sentence", PinStat = true, checklist = new List<CheckList>() { new CheckList { list = "hello" }, new CheckList { list = "brother" } },
                label = new List<Label>() { new Label { label = "number1" }, new Label { label = "number2" } } },new Notes
                {
                    Title = "First",
                    Text = "First sentence",
                    PinStat = true,
                    checklist = new List<CheckList>() { new CheckList { list = "hello2" }, new CheckList { list = "brother2" } },
                    label = new List<Label>() { new Label { label = "number3" }, new Label { label = "number4" } }
                } };
            todocontext.AddRange(note1);
            todocontext.SaveChanges();
        }
        [Fact]
        public void TestGet()
        {
            var result = _controller.GetNotes().ToList();
            Console.Write("{0}" + '\n' + result[0].ID);
            //var objectresult = result as OkObjectResult;
            //var notes = objectresult.Value as List<Notes>;
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async void TestGetId()
        {
            var tofindID = _controller.GetNotes().ToList();
            var result = await _controller.GetNotes(tofindID[0].ID);
            //Assert.True(condition: result is OkObjectResult);
            var OkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = OkObjectResult.Value as Notes;
            Assert.Equal(notes.ID, tofindID[0].ID);
        }

        [Fact]
        public async void TestGetTitle()
        {
            var result = await _controller.GetNotes("First");
            var resultAsOkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as List<Notes>;
            Assert.NotNull(notes);
            // Assert.Equal(notes.Select(x => x.Title == "First").Count == notes.Count);

        }
        [Fact]
        public async void TestGetPin()
        {
            var result = await _controller.GetNotes(true);
            var resultAsOkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as List<Notes>;
            Assert.NotNull(notes);

        }
        [Fact]
        public async void TestPost()
        {
            Notes note = new Notes
            {
                Title = "Post",
                Text = "Post sentence",
                PinStat = true,
                checklist = new List<CheckList>() { new CheckList { list = "hello2" }, new CheckList { list = "brother2" } },
                label = new List<Label>() { new Label { label = "number3" }, new Label { label = "number4" } }
            };
            var result = await _controller.PostNotes(note);
            var resultAsOkObjectResult = result as CreatedAtActionResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(notes, note);
        }
        [Fact]
        public async void TestPut()
        {
            var tofindID = _controller.GetNotes().ToList();
            Notes note = new Notes
            {
                ID = tofindID[1].ID,
                Title = "Put",
                Text = "put sentence",
                PinStat = false,
                checklist = new List<CheckList>() { new CheckList { list = "hello2" }, new CheckList { list = "brother2" } },
                label = new List<Label>() { new Label { label = "number3" }, new Label { label = "number4" } }
            };
            var result = await _controller.PutNotes(tofindID[1].ID, note);
            //Console.WriteLine(note.Id);
            var resultAsOkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(notes.Title, note.Title);
        }
        [Fact]
        public async void TestDel()
        {
            var tofindID = _controller.GetNotes().ToList();
            var result = await _controller.DeleteNotes(tofindID[0].ID);
            //Console.WriteLine(note.Id);
            var resultAsOkObjectResult = result as OkObjectResult;
            //Assert.True(condition: result, OkObjectResult);
            var notes = resultAsOkObjectResult.Value as Notes;
            Assert.Equal(notes.ID, tofindID[0].ID);

        }

    }


}
