using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UntoldApp.Export;
using UntoldApp.Models;

namespace UntoldApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly TicketContext _context;
        private readonly ShowContext _showContext;

        public TicketController(TicketContext context, ShowContext showContext)
        {
            _context = context;
            _showContext = showContext;
        }

        // GET: api/Ticket
        [HttpGet]
        [Authorize(Roles = "Cashier")]
        public async Task<ActionResult<IEnumerable<TicketModel>>> GetTicket()
        {
            return await _context.Ticket.ToListAsync();
        }

        // GET: api/Ticket/5
        [HttpGet("{id}")]
        [Authorize(Roles = "Cashier")]
        public async Task<ActionResult<TicketModel>> GetTicketModel(int id)
        {
            var ticketModel = await _context.Ticket.FindAsync(id);

            if (ticketModel == null)
            {
                return NotFound();
            }

            return ticketModel;
        }

        // PUT: api/Ticket/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> PutTicketModel(int id, TicketModel ticketModel)
        {
            if (id != ticketModel.TicketId)
            {
                return BadRequest();
            }

            _context.Entry(ticketModel).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TicketModelExists(id))
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

        // POST: api/Ticket
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Cashier")]
        public async Task<ActionResult<TicketModel>> PostTicketModel(TicketModel ticketModel)
        {
            var shows = await _showContext.Show.Where(show => show.ShowId == ticketModel.ShowId).ToListAsync();
            var show = shows[0];

            var tickets = await _context.Ticket.Where(ticket => ticket.ShowId == show.ShowId).ToListAsync();

            int numberOfTicketsBought = 0;
            foreach(var ticket in tickets)
            {
                numberOfTicketsBought += ticket.NumberOfPlaces;
            }
            
            if(show.MaximumNoOfTickets - numberOfTicketsBought - ticketModel.NumberOfPlaces < 0)
            {
                return BadRequest();
            }
            

            _context.Ticket.Add(ticketModel);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTicketModel", new { id = ticketModel.TicketId }, ticketModel);
        }

        // GET: api/Ticket/Export
        [HttpGet]
        [Route("Export/{showId}/{type}")]
        public async Task<ActionResult> Export(int showId, string type)
        {

            var tickets = await _context.Ticket.Where(ticket => ticket.ShowId == showId).ToListAsync();

            var exportType = type.Equals("csv") ? ExportType.CSV : ExportType.XML;

            var exporter = ExportFactory.Create(exportType);

            exporter.Export(tickets);

            var filePath = type.Equals("csv") ? $"tickets.csv" : $"tickets.xml";

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, "text/plain", Path.GetFileName(filePath));
        }


        // DELETE: api/Ticket/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Cashier")]
        public async Task<IActionResult> DeleteTicketModel(int id)
        {
            var ticketModel = await _context.Ticket.FindAsync(id);
            if (ticketModel == null)
            {
                return NotFound();
            }

            _context.Ticket.Remove(ticketModel);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TicketModelExists(int id)
        {
            return _context.Ticket.Any(e => e.TicketId == id);
        }
    }
}
