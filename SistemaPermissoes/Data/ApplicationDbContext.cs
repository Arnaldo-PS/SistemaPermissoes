﻿using Microsoft.EntityFrameworkCore;
using SistemaPermissoes.Models;
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
    public DbSet<UsuarioPapel> UsuarioPapel { get; set; }
    public DbSet<PapelTarefa> PapelTarefa { get; set; }


    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("server=localhost;database=SistemaPermissoes2;user=root;password=lojalolis",
                new MySqlServerVersion(new Version(9, 1, 0)));
        }
    }
}
