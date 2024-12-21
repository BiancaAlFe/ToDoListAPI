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
    public class DesenhosController : ControllerBase
    {
        private readonly DesenhosContext _context;

        public DesenhosController(DesenhosContext context)
        {
            _context = context;
        }

        // GET: api/Desenhoss
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DesenhoDTO>>> GetDesenhos()
        {
            return await _context.Desenhos
                                 .Select(d => DesenhoToDTO(d))
                                 .ToListAsync();
        }

        // GET: api/Desenhoss/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DesenhoDTO>> GetDesenho(long id)
        {
            var desenho = await _context.Desenhos.FindAsync(id);

            if (desenho == null)
            {
                return NotFound();
            }

            return DesenhoToDTO(desenho);
        }

        [HttpGet("Peso/{id}")]
        public async Task<ActionResult<int>> GetFilhosOfDesenho(long id)
        {
            //var desenhoOld = await _context.Desenhos.FindAsync(id);

            var desenho = await _context.Desenhos
                                  .Include(d => d.Pais)
                                  .Include(d => d.Filhos)
                                  .FirstOrDefaultAsync(d => d.Id == id);

            

            if (desenho == null)
            {
                return NotFound();
            }

            /*if (desenho.Pais != null)
                foreach (var p in desenho.Pais)
                {
                    var pai = await _context.Desenhos.FindAsync(p.PaiId);
                    var filho = await _context.Desenhos.FindAsync(p.FilhoId);
                    Console.WriteLine($"{filho.Name} é filho de {pai.Name}");
                }

            if (desenho.Filhos != null)
                foreach (var f in desenho.Filhos)
                {
                    var pai = await _context.Desenhos.FindAsync(f.PaiId);
                    var filho = await _context.Desenhos.FindAsync(f.FilhoId);
                    Console.WriteLine($"{pai.Name} é pai de {filho.Name}");
                }*/

            return Ok(await PesoFilhos(id));
        }

        // PUT: api/Desenhoss/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDesenho(long id, DesenhoDTO desenhoDTO)
        {
            if (id != desenhoDTO.Id)
            {
                return BadRequest();
            }

            var desenho = await _context.Desenhos.FindAsync(id);
            if (desenho == null)
            {
                return NotFound();
            }

            desenho.Name = desenhoDTO.Name;
            desenho.Peso = desenhoDTO.Peso;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DesenhoExists(id))
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
        public async Task<ActionResult<DesenhoDTO>> PostDesenho(DesenhoDTO desenhoDTO)
        {
            var desenho = new Desenho
            {
                Name = desenhoDTO.Name,
                Peso = desenhoDTO.Peso
            };

            _context.Desenhos.Add(desenho);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetDesenho), new { id = desenho.Id }, DesenhoToDTO(desenho));
        }

        // DELETE: api/Desenhoss/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDesenho(long id)
        {
            var desenho = await _context.Desenhos.FindAsync(id);
            if (desenho == null)
            {
                return NotFound();
            }

            _context.Desenhos.Remove(desenho);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private bool DesenhoExists(long id)
        {
            return _context.Desenhos.Any(e => e.Id == id);
        }

        private static DesenhoDTO DesenhoToDTO(Desenho desenho)
        {
            return new DesenhoDTO
            {
                Id = desenho.Id,
                Name = desenho.Name,
                Peso = desenho.Peso
            };
        }

        private async Task<int> PesoFilhos(long id)
        {
            var desenho = await _context.Desenhos
                                  .Include(d => d.Filhos)
                                  .FirstOrDefaultAsync(d => d.Id == id);

            if (desenho == null)
            {
                return 0;
            }

            int soma = 0;

            if (desenho.Filhos != null)
                foreach (var f in desenho.Filhos)
                {
                    soma += await PesoFilhos(f.FilhoId);
                }
            
            return soma + desenho.Peso;
        }
    }
}
