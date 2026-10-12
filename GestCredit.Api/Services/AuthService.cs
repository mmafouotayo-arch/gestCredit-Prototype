using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using GestCredit.Api.Data;
using GestCredit.Api.Models;
using GestCredit.Api.DTOs;

namespace GestCredit.Api.Services;

public class AuthService : IAuthService
{
    private readonly GestCreditDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(GestCreditDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterDto dto)
    {
        bool nomExiste = await _context.Utilisateurs.AnyAsync(u => u.NomUtilisateur == dto.NomUtilisateur);
        if (nomExiste)
        {
            throw new ArgumentException("Ce nom d'utilisateur est déjà pris.", nameof(dto.NomUtilisateur));
        }

        Role? role = await _context.Roles.FirstOrDefaultAsync(r => r.Nom == dto.Role);
        if (role is null)
        {
            throw new ArgumentException($"Le rôle '{dto.Role}' n'existe pas.", nameof(dto.Role));
        }

        string motDePasseHash = BCrypt.Net.BCrypt.HashPassword(dto.MotDePasse);

        var utilisateur = new Utilisateur(dto.NomUtilisateur, motDePasseHash, dto.Email);
        utilisateur.Roles.Add(role);

        _context.Utilisateurs.Add(utilisateur);
        await _context.SaveChangesAsync();

        return GenererReponse(utilisateur);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        Utilisateur? utilisateur = await _context.Utilisateurs
            .Include(u => u.Roles)
            .FirstOrDefaultAsync(u => u.NomUtilisateur == dto.NomUtilisateur);

        if (utilisateur is null || !BCrypt.Net.BCrypt.Verify(dto.MotDePasse, utilisateur.MotDePasseHash))
        {
            throw new UnauthorizedAccessException("Nom d'utilisateur ou mot de passe incorrect.");
        }

        return GenererReponse(utilisateur);
    }

    private AuthResponseDto GenererReponse(Utilisateur utilisateur)
    {
        List<string> nomsRoles = utilisateur.Roles.Select(r => r.Nom).ToList();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, utilisateur.Id.ToString()),
            new(ClaimTypes.Name, utilisateur.NomUtilisateur),
        };

        claims.AddRange(nomsRoles.Select(r => new Claim(ClaimTypes.Role, r)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(double.Parse(_configuration["Jwt:ExpireMinutes"]!)),
            signingCredentials: credentials
        );

        string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return new AuthResponseDto(tokenString, utilisateur.NomUtilisateur, nomsRoles);
    }
}