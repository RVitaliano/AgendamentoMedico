using AgendamentoMedico.Data;
using AgendamentoMedico.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoMedico.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AgendamentosController : ControllerBase
{
    private readonly AppDbContext _context;

    public AgendamentosController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var agendamentos = await _context.Agendamentos
            .Include(a => a.Paciente)
            .Include(a => a.Medico)
            .ToListAsync();
        return Ok(agendamentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var agendamento = await _context.Agendamentos
            .Include(a => a.Paciente)
            .Include(a => a.Medico)
            .FirstOrDefaultAsync(a => a.Id == id);

        if (agendamento is null)
            return NotFound();

        return Ok(agendamento);
    }

    [HttpPost]
    public async Task<IActionResult> Create(Agendamento agendamento)
    {
        _context.Agendamentos.Add(agendamento);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetById), new { id = agendamento.Id }, agendamento);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, Agendamento agendamento)
    {
        if (id != agendamento.Id)
            return BadRequest();

        _context.Entry(agendamento).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var agendamento = await _context.Agendamentos.FindAsync(id);
        if (agendamento is null)
            return NotFound();

        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}