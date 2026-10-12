using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestCredit.Api.Models;

namespace GestCredit.Api.Data.Configurations;

public class UtilisateurConfiguration : IEntityTypeConfiguration<Utilisateur>
{
    public void Configure(EntityTypeBuilder<Utilisateur> builder)
    {
        builder.ToTable("Utilisateurs");
        builder.HasKey(u => u.Id);

        builder.Property(u => u.NomUtilisateur).IsRequired().HasMaxLength(100);
        builder.HasIndex(u => u.NomUtilisateur).IsUnique();

        builder.Property(u => u.Email).IsRequired().HasMaxLength(150);
        builder.HasIndex(u => u.Email).IsUnique();

        builder.Property(u => u.MotDePasseHash).IsRequired().HasMaxLength(255);

        builder.HasMany(u => u.Roles)
            .WithMany()
            .UsingEntity(j => j.ToTable("UtilisateursRoles"));
    }
}