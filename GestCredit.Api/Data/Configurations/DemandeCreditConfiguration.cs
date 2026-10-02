using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using GestCredit.Api.Models;

namespace GestCredit.Api.Data.Configurations;

public class DemandeCreditConfiguration : IEntityTypeConfiguration<DemandeCredit>
{
    public void Configure(EntityTypeBuilder<DemandeCredit> builder)
    {
        builder.ToTable("DemandesCredit");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Montant)
            .HasColumnType("decimal(18,2)")
            .IsRequired();

        builder.Property(d => d.TauxAnnuel)
            .HasColumnType("decimal(5,2)")
            .IsRequired();

        builder.Property(d => d.Statut)
            .HasConversion<string>()
            .HasMaxLength(20)
            .IsRequired();

        builder.HasIndex(d => d.Statut)
            .HasDatabaseName("IX_DemandesCredit_Statut");

        builder.HasIndex(d => d.ClientId)
            .HasDatabaseName("IX_DemandesCredit_ClientId");

        builder.HasOne<Client>()
            .WithMany()
            .HasForeignKey(d => d.ClientId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}