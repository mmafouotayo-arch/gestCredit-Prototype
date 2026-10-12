namespace GestCredit.Api.Models;

public class Role
{
    private Role() { }

    public Role(string nom)
    {
        if (string.IsNullOrWhiteSpace(nom))
        {
            throw new ArgumentException("Le nom du rôle ne peut pas être vide.", nameof(nom));
        }

        Nom = nom;
    }

    public int Id { get; private set; }
    public string Nom { get; private set; } = string.Empty;
}