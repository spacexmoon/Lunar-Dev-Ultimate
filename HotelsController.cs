using System.Net.Http.Headers;
using System.Net.Sockets;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelListing.Api.Data;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using Microsoft.EntityFrameworkCore.Metadata;

namespace HotelListing.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelsController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        public HotelsController(HotelListingDbContext context)
        {
            _context = context;
        }

        // GET: api/Hotels
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> Get()
        {
            var hotels = await _context.Hotels.ToListAsync();
            return Ok(hotels);
        }

        // GET api/Hotels/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> Get(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound();
            }

            return Ok(hotel);
        }

        // POST api/Hotels
        [HttpPost]
        public async Task<ActionResult<Hotel>> Post([FromBody] Hotel newHotel)
        {
            await _context.Hotels.AddAsync(newHotel);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newHotel.Id }, newHotel);
        }

        // PUT api/Hotels
        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> Put(int id, [FromBody] Hotel updatedHotel)
        {
            if (id != updatedHotel.Id)
            {
                return BadRequest(new { message = "Hotel ID mismatch" });
            }

            _context.Entry(updatedHotel).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Hotels.AnyAsync(h => h.Id == id))
                {
                    return NotFound(new { message = "Hotel not found" });
                }
            }

            return NoContent();
        }

        // DELETE api/Hotels
        [HttpDelete("{id}")]
        public async Task<ActionResult<Hotel>> Delete(int id)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
            {
                return NotFound(new { message = "Hotel not found" });
            }

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
