using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Desenhos.Models;

namespace ToDoListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ParentescosController : ControllerBase
    {
        private readonly DesenhosContext _context;

        public ParentescosController(DesenhosContext context)
        {
            _context = context;
        }

        // GET: api/Desenhoss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ParentescoDTO>>> GetParentescos()
        {
            return await _context.Parentescos
                                 .Select(p => ParentescoToDTO(p))
                                 .ToListAsync();
        }

        // GET: api/Desenhoss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ParentescoDTO>> GetParentesco(long id)
        {
            var parentesco = await _context.Parentescos.FindAsync(id);

            if (parentesco == null)
            {
                return NotFound();
            }

            return ParentescoToDTO(parentesco);
        }

        // PUT: api/Desenhoss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutParentesco(long id, ParentescoDTO parentescoDTO)
        {
            if (id != parentescoDTO.Id)
            {
                return BadRequest();
            }

            var parenteco = await _context.Parentescos.FindAsync(id);
            if (parenteco == null)
            {
                return NotFound();
            }

            parenteco.PaiId = parentescoDTO.PaiId;
            parenteco.FilhoId = parentescoDTO.FilhoId;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ParentescoExists(id))
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

        // POST: api/Desenhoss
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ParentescoDTO>> PostParentesco(ParentescoDTO parentescoDTO)
        {
            var parentesco = new Parentesco
            {
                PaiId = parentescoDTO.PaiId,
                FilhoId = parentescoDTO.FilhoId
            };

            _context.Parentescos.Add(parentesco);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetParentesco), new { id = parentesco.Id }, ParentescoToDTO(parentesco));
        }

        // DELETE: api/Desenhoss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteParentesco(long id)
        {
            var parentesco = await _context.Parentescos.FindAsync(id);
            if (parentesco == null)
            {
                return NotFound();
            }

            _context.Parentescos.Remove(parentesco);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ParentescoExists(long id)
        {
            return _context.Parentescos.Any(e => e.Id == id);
        }

        private static ParentescoDTO ParentescoToDTO(Parentesco parentesco)
        {
            return new ParentescoDTO
            {
                Id = parentesco.Id,
                PaiId = parentesco.PaiId,
                FilhoId = parentesco.FilhoId
            };
        }
    }
}
