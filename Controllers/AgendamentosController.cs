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
        var agendamentos = await _context.Agendamentos.ToListAsync();

        foreach (var a in agendamentos)
        {
            a.Paciente = await _context.Pacientes.FindAsync(a.PacienteId);
            a.Medico = await _context.Medicos.FindAsync(a.MedicoId);
        }

        return Ok(agendamentos);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var idStr = id.ToString().ToLower();

        var agendamentos = await _context.Agendamentos
            .FromSqlRaw($"SELECT * FROM Agendamentos WHERE LOWER(Id) = '{idStr}'")
            .ToListAsync();

        var agendamento = agendamentos.FirstOrDefault();

        if (agendamento is null)
            return NotFound();

        var pacientes = await _context.Pacientes
            .FromSqlRaw($"SELECT * FROM Pacientes WHERE LOWER(Id) = '{agendamento.PacienteId.ToString().ToLower()}'")
            .ToListAsync();

        var medicos = await _context.Medicos
            .FromSqlRaw($"SELECT * FROM Medicos WHERE LOWER(Id) = '{agendamento.MedicoId.ToString().ToLower()}'")
            .ToListAsync();

        agendamento.Paciente = pacientes.FirstOrDefault();
        agendamento.Medico = medicos.FirstOrDefault();

        return Ok(agendamento);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] Agendamento agendamento)
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