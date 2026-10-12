using Microsoft.EntityFrameworkCore;
using GestCredit.Api.Data;
using GestCredit.Api.DTOs;
using GestCredit.Api.Models;

namespace GestCredit.Api.Services;

public class DemandeCreditService : IDemandeCreditService
{
    private readonly GestCreditDbContext _context;

    public DemandeCreditService(GestCreditDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResultDto<DemandeCreditDto>> GetAllAsync(string? statut, int page, int pageSize)
    {
        IQueryable<DemandeCredit> query = _context.DemandesCredit;

        if (!string.IsNullOrWhiteSpace(statut))
        {
            if (!Enum.TryParse<StatutDemande>(statut, true, out var statutFiltre))
            {
                throw new ArgumentException($"Statut inconnu : '{statut}'.", nameof(statut));
            }

            query = query.Where(d => d.Statut == statutFiltre);
        }

        int totalCount = await query.CountAsync();

        // On charge les entités PUIS on mappe en mémoire : Mensualite et CoutTotal
        // sont des propriétés calculées en C#, EF Core ne peut pas les traduire en SQL.
        List<DemandeCredit> demandes = await query
            .OrderBy(d => d.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedResultDto<DemandeCreditDto>(
            demandes.Select(ToDto).ToList(), page, pageSize, totalCount);
    }

    public async Task<DemandeCreditDto?> GetByIdAsync(int id)
    {
        DemandeCredit? demande = await _context.DemandesCredit.FindAsync(id);
        return demande is null ? null : ToDto(demande);
    }

    public async Task<DemandeCreditDto> CreateAsync(CreateDemandeCreditDto dto)
    {
        bool clientExiste = await _context.Clients.AnyAsync(c => c.Id == dto.ClientId);
        if (!clientExiste)
        {
            throw new ArgumentException($"Le client {dto.ClientId} n'existe pas.", nameof(dto.ClientId));
        }

        var demande = new DemandeCredit(dto.ClientId, dto.Montant, dto.TauxAnnuel, dto.DureeMois);

        _context.DemandesCredit.Add(demande);
        await _context.SaveChangesAsync();

        return ToDto(demande);
    }

    public async Task<DemandeCreditDto?> ChangerStatutAsync(int id, StatutDemande nouveauStatut)
    {
        DemandeCredit? demande = await _context.DemandesCredit.FindAsync(id);

        if (demande is null)
        {
            return null;
        }

        // La règle des transitions légales vit dans l'entité (Jour 8).
        // Une transition interdite lève TransitionInvalideException.
        demande.ChangerStatut(nouveauStatut);
        await _context.SaveChangesAsync();

        return ToDto(demande);
    }

    private static DemandeCreditDto ToDto(DemandeCredit d) => new(
        d.Id, d.ClientId, d.Montant, d.TauxAnnuel, d.DureeMois,
        d.Statut.ToString(), d.Mensualite, d.CoutTotal, d.DateCreation);
}