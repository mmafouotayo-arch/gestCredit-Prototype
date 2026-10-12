namespace GestCredit.Api.DTOs;

public record RegisterDto(string NomUtilisateur, string MotDePasse, string Email, string Role);
public record LoginDto(string NomUtilisateur, string MotDePasse);
public record AuthResponseDto(string Token, string NomUtilisateur, List<string> Roles);