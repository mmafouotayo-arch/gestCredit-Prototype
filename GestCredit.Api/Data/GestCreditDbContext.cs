using Microsoft.EntityFrameworkCore;
using GestCredit.Api.Models;

namespace GestCredit.Api.Data;

public class GestCreditDbContext : DbContext
{
    public GestCreditDbContext(DbContextOptions<GestCreditDbContext> options)
        : base(options)
    {
    }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Utilisateur> Utilisateurs => Set<Utilisateur>();
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<DemandeCredit> DemandesCredit => Set<DemandeCredit>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(GestCreditDbContext).Assembly);
    }
}