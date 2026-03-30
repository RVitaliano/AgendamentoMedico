namespace AgendamentoMedico.Models;


public class Paciente
{
    public Guid Id{get; set;} = Guid.NewGuid();
    public string NomeCompleto { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;
    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
    public ICollection<Agendamento> Agendamentos { get; set; } = [];
}