namespace GestCredit.Console.Modeles;

public class Client
{
    private static int _prochainId = 1;

    public int Id { get; }
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

        Id = _prochainId++;
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