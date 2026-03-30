using AgendamentoMedico.Data;
using AgendamentoMedico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoMedico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacientesController : ControllerBase
{
    private readonly AppDbContext _context;

    public PacientesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var pacientes = await _context.Pacientes.ToListAsync();
        return Ok(pacientes);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByID(Guid id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente is null)
            return NotFound();
        return Ok(paciente);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Paciente paciente)
    {
        _context.Pacientes.Add(paciente);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetByID), new {id =  paciente.Id}, paciente);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Paciente paciente)
    {
        if (id != paciente.Id)
            return BadRequest();

        _context.Entry(paciente).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var paciente = await _context.Pacientes.FindAsync(id);
        if (paciente is null)
            return NotFound();

        _context.Pacientes.Remove(paciente);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}