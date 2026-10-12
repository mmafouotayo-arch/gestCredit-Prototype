namespace GestCredit.Api.Models;

public class Utilisateur
{
    private Utilisateur() { }

    public Utilisateur(string nomUtilisateur, string motDePasseHash, string email)
    {
        if (string.IsNullOrWhiteSpace(nomUtilisateur))
        {
            throw new ArgumentException("Le nom d'utilisateur ne peut pas être vide.", nameof(nomUtilisateur));
        }

        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("L'email ne peut pas être vide.", nameof(email));
        }

        NomUtilisateur = nomUtilisateur;
        MotDePasseHash = motDePasseHash;
        Email = email;
    }

    public int Id { get; private set; }
    public string NomUtilisateur { get; private set; } = string.Empty;
    public string MotDePasseHash { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public List<Role> Roles { get; private set; } = new();
}