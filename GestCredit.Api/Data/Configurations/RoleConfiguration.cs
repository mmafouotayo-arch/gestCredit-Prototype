using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestCredit.Api.Models;

namespace GestCredit.Api.Data.Configurations;

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.ToTable("Roles");
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Nom).IsRequired().HasMaxLength(50);
        builder.HasIndex(r => r.Nom).IsUnique();
        builder.HasData(
    new { Id = 1, Nom = "Agent" },
    new { Id = 2, Nom = "Analyste" },
    new { Id = 3, Nom = "Administrateur" }
);
    }
}