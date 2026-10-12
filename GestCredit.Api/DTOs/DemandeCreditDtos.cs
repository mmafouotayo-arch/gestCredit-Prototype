namespace GestCredit.Api.DTOs;

public record DemandeCreditDto(
    int Id,
    int ClientId,
    decimal Montant,
    decimal TauxAnnuel,
    int DureeMois,
    string Statut,
    decimal Mensualite,
    decimal CoutTotal,
    DateTime DateCreation);

public record CreateDemandeCreditDto(int ClientId, decimal Montant, decimal TauxAnnuel, int DureeMois);

public record ChangerStatutDto(string NouveauStatut);