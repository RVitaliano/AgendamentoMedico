using AgendamentoMedico.Models;
using Microsoft.EntityFrameworkCore;

namespace AgendamentoMedico.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Paciente> Pacientes { get; set; }
    public DbSet<Medico> Medicos {  get; set; }
    public DbSet<Agendamento> Agendamentos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.CPF)
            .IsUnique();

        modelBuilder.Entity<Paciente>()
            .HasIndex(p => p.Email)
            .IsUnique();

        modelBuilder.Entity<Medico>()
            .HasIndex(m => m.CRM)
            .IsUnique();

        modelBuilder.Entity<Agendamento>()
            .Property(a => a.Status)
            .HasConversion<string>();

        modelBuilder.Entity<Agendamento>()
            .HasOne(a => a.Paciente)
            .WithMany(p => p.Agendamentos)
            .HasForeignKey(a => a.PacienteId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Agendamento>()
            .HasOne(a => a.Medico)
            .WithMany(m => m.Agendamentos)
            .HasForeignKey(a => a.MedicoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}