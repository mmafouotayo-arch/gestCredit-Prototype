namespace GestCredit.Api.DTOs;

// DTO de lecture : ce que l'API renvoie au client
public record ClientDto(int Id, string Nom, string Ville);

// DTO de création : ce que l'API accepte en entrée pour créer un client
public record CreateClientDto(string Nom, string Ville);

// DTO de modification
public record UpdateClientDto(string Nom, string Ville);