using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UntoldApp.Models;

namespace UntoldApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShowController : ControllerBase
    {
        private readonly ShowContext _context;

        public ShowController(ShowContext context)
        {
            _context = context;
        }

        // GET: api/Show
        [HttpGet]
        [Authorize(Roles = "Admin,Cashier")]
        public async Task<ActionResult<IEnumerable<ShowModel>>> GetShow()
        {
            return await _context.Show.ToListAsync();
        }

        // GET: api/Show/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ShowModel>> GetShowModel(int id)
        {
            var showModel = await _context.Show.FindAsync(id);

            if (showModel == null)
            {
                return NotFound();
            }

            return showModel;
        }

        // PUT: api/Show/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutShowModel(int id, ShowModel showModel)
        {
            if (id != showModel.ShowId)
            {
                return BadRequest();
            }

            _context.Entry(showModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ShowModelExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Show
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ShowModel>> PostShowModel(ShowModel showModel)
        {
            _context.Show.Add(showModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetShowModel", new { id = showModel.ShowId }, showModel);
        }

        // DELETE: api/Show/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteShowModel(int id)
        {
            var showModel = await _context.Show.FindAsync(id);
            if (showModel == null)
            {
                return NotFound();
            }

            _context.Show.Remove(showModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ShowModelExists(int id)
        {
            return _context.Show.Any(e => e.ShowId == id);
        }
    }
}
