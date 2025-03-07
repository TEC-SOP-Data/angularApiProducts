using angularApiProducts.Data;
using angularApiProducts.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace angularApiProducts.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CompanyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Company
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Companies>>> GetCompanies()
        {
            return await _context.Company.ToListAsync();
        }

        // GET: api/Company/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Companies>> GetCompany(int id)
        {
            var company = await _context.Company.FindAsync(id);

            if (company == null)
            {
                return NotFound();
            }

            return company;
        }

        // POST: api/Company
        [HttpPost]
        public async Task<ActionResult<Companies>> CreateCompany(Companies company)
        {
            _context.Company.Add(company);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCompany), new { id = company.Id }, company);
        }

        // PUT: api/Company/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompany(int id, [FromBody] Companies updatedCompany)
        {
            if (id != updatedCompany.Id)
            {
                return BadRequest("Virksomhed-ID matcher ikke.");
            }

            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound("Virksomhed ikke fundet.");
            }

            // Opdater kun relevante felter (du kan tilføje flere felter her)
            company.Name = updatedCompany.Name;
            company.EducationalBranch = updatedCompany.EducationalBranch;
            company.Homepage = updatedCompany.Homepage;
            company.Notes = updatedCompany.Notes;
            company.CompanySize = updatedCompany.CompanySize;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompanyExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(company); // Returner det opdaterede virksomhed
        }

        // DELETE: api/Company/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompany(int id)
        {
            var company = await _context.Company.FindAsync(id);
            if (company == null)
            {
                return NotFound();
            }

            _context.Company.Remove(company);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompanyExists(int id)
        {
            return _context.Company.Any(e => e.Id == id);
        }
    }
}
