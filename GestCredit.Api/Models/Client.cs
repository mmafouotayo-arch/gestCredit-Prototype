namespace GestCredit.Api.Models;

public class Client
{
    // Constructeur privé sans paramètre, requis par Entity Framework Core
 private Client()
{
    Nom = string.Empty;
    Ville = string.Empty;
}
    [System.Text.Json.Serialization.JsonConstructor]
    
 public Client(int id, string nom, string ville)
 {
    Id = id;
    Nom = nom;
    Ville = ville;
 }
    public int Id { get; private set; }
public string Nom { get; private set; }
public string Ville { get; private set; }

public Client(string nom, string ville)
{
    if (string.IsNullOrWhiteSpace(nom))
    {
        throw new ArgumentException("Le nom du client ne peut pas être vide.", nameof(nom));
    }

    if (string.IsNullOrWhiteSpace(ville))
    {
        throw new ArgumentException("La ville du client ne peut pas être vide.", nameof(ville));
    }

    Nom = nom;
    Ville = ville;

    }

    public void Renommer(string nouveauNom)
    {
        if (string.IsNullOrWhiteSpace(nouveauNom))
        {
            throw new ArgumentException("Le nouveau nom ne peut pas être vide.", nameof(nouveauNom));
        }

        Nom = nouveauNom;
    }

    public override string ToString() => $"[{Id}] {Nom} - {Ville}";
}