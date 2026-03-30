using AgendamentoMedico.Data;
using AgendamentoMedico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoMedico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MedicosController : ControllerBase
{
    private readonly AppDbContext _context;

    public MedicosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var medicos = await _context.Medicos.ToListAsync();
        return Ok(medicos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByID(Guid id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico is null)
            return NotFound();
        return Ok(medico);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Medico medico)
    {
        _context.Medicos.Add(medico);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByID), new { id = medico.Id }, medico);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Medico medico)
    {
        if (id != medico.Id)
            return BadRequest();

        _context.Entry(medico).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var medico = await _context.Medicos.FindAsync(id);
        if (medico is null)
            return NotFound();

        _context.Medicos.Remove(medico);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}