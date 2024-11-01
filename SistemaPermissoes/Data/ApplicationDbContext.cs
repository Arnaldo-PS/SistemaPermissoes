using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Papel> Papeis { get; set; }
    public DbSet<Tarefa> Tarefas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configura muitos-para-muitos entre Usuario e Papel
        modelBuilder.Entity<Usuario>()
            .HasMany(u => u.Papeis)
            .WithMany();

        // Configura muitos-para-muitos entre Papel e Tarefa
        modelBuilder.Entity<Papel>()
            .HasMany(p => p.Tarefas)
            .WithMany();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("server=localhost;database=SistemaPermissoes;user=root;password=lojalolis",
                new MySqlServerVersion(new Version(9, 1, 0))); // Substitua pela sua versão do MySQL
        }
    }
}
