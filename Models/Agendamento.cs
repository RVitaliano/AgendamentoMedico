namespace AgendamentoMedico.Models;

public class Agendamento
{
	public Guid Id { get; set; } = Guid.NewGuid();
	public Guid PacienteId { get; set; }
	public Guid MedicoId { get; set; }
	public DateTime DataHora { get; set; }
	public int DuracaoMinutos { get; set; } = 30;
	public StatusAgendamento Status { get; set; } = StatusAgendamento.Pendente;
	public string? Observacoes { get; set; }
	public DateTime DataCriacao { get; set; } = DateTime.UtcNow;
	public Paciente? Paciente { get; set; }
	public Medico? Medico { get; set; }
}