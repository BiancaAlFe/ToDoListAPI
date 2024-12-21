using Microsoft.EntityFrameworkCore;

namespace Desenhos.Models;

public class DesenhosContext : DbContext
{
    public DesenhosContext(DbContextOptions<DesenhosContext> options)
        : base(options)
    {
    }

    public DbSet<Desenho> Desenhos { get; set; } = null!;
    public DbSet<Parentesco> Parentescos { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Desenho>()
            .HasMany(e => e.Filhos)
            .WithOne(e => e.Pai)
            .HasForeignKey(e => e.PaiId);

        modelBuilder.Entity<Desenho>()
            .HasMany(e => e.Pais)
            .WithOne(e => e.Filho)
            .HasForeignKey(e => e.FilhoId);
    }
}