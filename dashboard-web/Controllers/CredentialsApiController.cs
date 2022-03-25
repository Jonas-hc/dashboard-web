using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using dashboard_web.Data;
using dashboard_web.Models;
using Microsoft.AspNetCore.Authorization;

namespace dashboard_web.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CredentialsApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CredentialsApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/CredentialsApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Credentials>>> GetCredentials()
        {
            return await _context.Credentials.ToListAsync();
        }

        // GET: api/CredentialsApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Credentials>> GetCredentials(int id)
        {
            var credentials = await _context.Credentials.FindAsync(id);

            if (credentials == null)
            {
                return NotFound();
            }

            return credentials;
        }

        // PUT: api/CredentialsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCredentials(int id, Credentials credentials)
        {
            if (id != credentials.ID)
            {
                return BadRequest();
            }

            _context.Entry(credentials).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CredentialsExists(id))
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

        // POST: api/CredentialsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Credentials>> PostCredentials(Credentials credentials)
        {
            _context.Credentials.Add(credentials);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCredentials", new { id = credentials.ID }, credentials);
        }

        // DELETE: api/CredentialsApi/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Credentials>> DeleteCredentials(int id)
        {
            var credentials = await _context.Credentials.FindAsync(id);
            if (credentials == null)
            {
                return NotFound();
            }

            _context.Credentials.Remove(credentials);
            await _context.SaveChangesAsync();

            return credentials;
        }

        private bool CredentialsExists(int id)
        {
            return _context.Credentials.Any(e => e.ID == id);
        }
    }
}
