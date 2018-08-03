using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using KeepNotes.Models;
using System.Data;

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

                return Ok(temp);
            
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

            return Ok(temp);

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
            //Notes[] temp = new Notes[_context.Notes.Count()];
            //int i = 0;
            await _context.Notes.Include(x => x.checklist).Include(x => x.label).ForEachAsync(x =>
            {

                if (x.ID == id)
                {
                    // temp[i] = x;
                    x = notes;
                }
                //i++;
            });
            await _context.SaveChangesAsync();
            return Ok(notes);
            //  _context.Entry(notes).State = EntityState.Modified;
            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!NotesExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}
            //return NoContent();
        }

        // POST: api/Notes
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