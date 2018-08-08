using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeepNotes.Models;
using System.Data;
using System.Configuration;

namespace KeepNotes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly KeepNotesContext _context;

        public NotesController(KeepNotesContext context)
        {
            _context = context;
        }

        // GET: api/Notes
        [HttpGet]
        public IEnumerable<Notes> GetNotes()
        {
            return _context.Notes.Include(x => x.label).Include(x => x.checklist);
            //return Ok();
        }

        // GET: api/Notes/5 
        [HttpGet("{id}")]
        public async Task<IActionResult> GetNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.Include(y => y.label).Include(y => y.checklist).FirstOrDefaultAsync(x => x.ID == id);

            if (notes == null)
            {
                return NotFound();
            }

            return Ok(notes);
        }
        [HttpGet("title/{title}")]
        public async Task<IActionResult> GetNotes([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // var notes = await _context.Notes.Include(y => y.label).Include(y => y.checklist).FirstOrDefaultAsync(x => x.Title == title);
            Notes[] temp = new Notes[_context.Notes.Count()];
            int i = 0;
            await _context.Notes.Include(x => x.checklist).Include(x => x.label).ForEachAsync(x =>
            {
            
                if (x.Title == title)
                {
                    temp[i] = x;

                }
                i++;
            });
            //foreach (var u in temp) {
                if ( temp == null)
                {
                    return NotFound();
                }

                return Ok(temp.ToList());
            
        }
        [HttpGet("pin/{pin}")]
        public async Task<IActionResult> GetNotes([FromRoute] bool pin)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List<Notes> temp = new List<Notes>();
            await _context.Notes.Include(x => x.checklist).Include(x => x.label).ForEachAsync(x =>
            {

                if (x.PinStat == pin)
                {
                    temp.Add(x);

                }
            });
            if (temp == null)
            {
                return NotFound();
            }

            return Ok(temp.ToList());

        }
        [HttpGet("label/{label}")]
        public IActionResult GetNotesLabel([FromRoute] string label)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            //List < Notes > temp  = new List<Notes>();
            var notes = _context.Notes.Include(x => x.checklist).Include(x=>x.label).Where(x => x.label.Exists(z => z.label == label)).ToList();
            //foreach(var x in GetNotes())
            //{
            //    foreach(var z in x.label)
            //    {
            //        if(z.label == label)
            //        {
            //            temp.Add(x);
            //        }
            //    }
            //}
            if (notes == null) return NotFound();
            return Ok(notes);
        }

        // PUT: api/Notes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNotes([FromRoute] int id, [FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != notes.ID)
            {
                return BadRequest();
            }
            //var temp2 = GetNotes().SingleOrDefault(x => x.ID == id);
            await _context.Notes.Include(x => x.checklist).Include(x => x.label).ForEachAsync(x =>
            {

                if (x.ID == id)
                {
                   // x = notes;
                    //_context.UpdateRange();
                    x.Title = notes.Title;
                    x.Text = notes.Text;
                    x.PinStat = notes.PinStat;
                    _context.Label.RemoveRange(x.label);
                    _context.Label.AddRange(notes.label);
                    _context.CheckList.RemoveRange(x.checklist);
                    _context.CheckList.AddRange(notes.checklist);

                }
            });
            //_context.Entry(notes).State = EntityState.Modified;
            // _context.Notes.Update(notes);
            await _context.SaveChangesAsync();

            return Ok(_context.Notes.Include(y => y.label).Include(y => y.checklist).First(x => x.ID == notes.ID));
        }
        [HttpPost]
        public async Task<IActionResult> PostNotes([FromBody] Notes notes)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.Notes.Add(notes);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetNotes", new { id = notes.ID }, notes);
        }
        [HttpPost("check/{id}")]
        public async Task<IActionResult> PostCheck ([FromRoute]int id, [FromBody] CheckList item)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            await _context.Notes.Include(x => x.checklist).Include(x => x.label).ForEachAsync(x =>
            {
                if (x.ID == id)
                {
                    // temp[i] = x;
                    x.checklist.Add(item);
                }
            });
            await _context.SaveChangesAsync();
            return Ok("Added A Checklist");
        }

        // DELETE: api/Notes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var notes = await _context.Notes.Include(y => y.label).Include(y => y.checklist).FirstOrDefaultAsync(x => x.ID == id);

            _context.Notes.Remove(notes);
            await _context.SaveChangesAsync();

            return Ok(notes);
        }
        [HttpDelete("del/{title}")]
        public async Task<IActionResult> DeleteNotes([FromRoute] string title)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int count = _context.Notes.Count();
            Notes[] temp = new Notes[count];
            int i = 0;
            await _context.Notes.Include(x => x.checklist).Include(x => x.label).ForEachAsync(x =>
            {

                if (x.Title == title)
                {
                    temp[i] = x;
                    _context.Notes.Remove(temp[i]);   
                }
                i++;
            });
            await _context.SaveChangesAsync();
            return Ok(temp);
            // if (temp == null) return NotFound)();
        }
        private bool NotesExists(int id)
        {
            return _context.Notes.Any(e => e.ID == id);
        }
    }
}