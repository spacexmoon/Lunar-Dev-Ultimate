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
    public class CountriesController : ControllerBase
    {
        private readonly HotelListingDbContext _context;
        public CountriesController(HotelListingDbContext context)
        {
            _context = context;
        }

        // GET: api/Countries
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Country>>> Get()
        {
            var countries = await _context.Countries.ToListAsync();
            return Ok(countries);
        }

        // GET api/Countries/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Country>> Get(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }

            return Ok(country);
        }

        // POST api/Countries
        [HttpPost]
        public async Task<ActionResult<Country>> Post([FromBody] Country newCountry)
        {
            await _context.Countries.AddAsync(newCountry);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = newCountry.CountryId }, newCountry);
        }

        // PUT api/Countries
        [HttpPut("{id}")]
        public async Task<ActionResult<Hotel>> Put(int id, [FromBody] Country updatedCountry)
        {
            if (id != updatedCountry.CountryId)
            {
                return BadRequest(new { message = "Country ID mismatch" });
            }

            _context.Entry(updatedCountry).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Countries.AnyAsync(c => c.CountryId == id))
                {
                    return NotFound(new { message = "Country not found" });
                }
            }

            return NoContent();
        }

        // DELETE api/Countries
        [HttpDelete("{id}")]
        public async Task<ActionResult<Country>> Delete(int id)
        {
            var country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound(new { message = "Country not found" });
            }

            _context.Countries.Remove(country);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
