using System;
using System.Collections.Generic;
using System.Linq;

// --- Programme principal ---
List<Client> clients = CreerJeuDeDonnees();

bool continuer = true;
while (continuer)
{
    AfficherMenu();
    string? choix = Console.ReadLine();

    switch (choix)
    {
        case "1":
            AjouterClient(clients);
            break;
        case "2":
            SupprimerClient(clients);
            break;
        case "3":
            RechercherParFragment(clients);
            break;
        case "4":
            AfficherTriesParNom(clients);
            break;
        case "5":
            Console.WriteLine($"Nombre total de clients : {clients.Count}");
            break;
        case "6":
            continuer = false;
            break;
        default:
            Console.WriteLine("Choix invalide.");
            break;
    }
}


// --- Méthodes ---

static List<Client> CreerJeuDeDonnees()
{
    return new List<Client>
    {
        new(1, "Ahmadou Bello", "Yaoundé"),
        new(2, "Fatima Njoya", "Douala"),
        new(3, "Jean Mballa", "Yaoundé"),
        new(4, "Aicha Souley", "Garoua"),
        new(5, "Paul Etoundi", "Douala"),
    };
}

static void AfficherMenu()
{
    Console.WriteLine();
    Console.WriteLine("Gestion des clients");
    Console.WriteLine("1. Ajouter un client");
    Console.WriteLine("2. Supprimer un client");
    Console.WriteLine("3. Rechercher par fragment de nom");
    Console.WriteLine("4. Afficher triés par nom");
    Console.WriteLine("5. Compter les clients");
    Console.WriteLine("6. Quitter");
    Console.Write("Votre choix : ");
}

static void AjouterClient(List<Client> clients)
{
    Console.Write("Nom du client : ");
    string nom = Console.ReadLine() ?? "";

    Console.Write("Ville : ");
    string ville = Console.ReadLine() ?? "";

    int nouvelId = clients.Any() ? clients.Max(c => c.Id) + 1 : 1;

    clients.Add(new Client(nouvelId, nom, ville));
    Console.WriteLine($"Client ajouté avec l'Id {nouvelId}.");
}

static void SupprimerClient(List<Client> clients)
{
    Console.Write("Id du client à supprimer : ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Client? client = clients.FirstOrDefault(c => c.Id == id);

        if (client is not null)
        {
            clients.Remove(client);
            Console.WriteLine("Client supprimé.");
        }
        else
        {
            Console.WriteLine("Aucun client trouvé avec cet Id.");
        }
    }
    else
    {
        Console.WriteLine("Id invalide.");
    }
}

static void RechercherParFragment(List<Client> clients)
{
    Console.Write("Fragment du nom à rechercher : ");
    string fragment = Console.ReadLine() ?? "";

    var resultats = clients
        .Where(c => c.Nom.Contains(fragment, StringComparison.OrdinalIgnoreCase))
        .ToList();

    if (resultats.Count == 0)
    {
        Console.WriteLine("Aucun résultat.");
        return;
    }

    foreach (var c in resultats)
    {
        Console.WriteLine($"[{c.Id}] {c.Nom} - {c.Ville}");
    }
}

static void AfficherTriesParNom(List<Client> clients)
{
    var tries = clients.OrderBy(c => c.Nom).ToList();

    foreach (var c in tries)
    {
        Console.WriteLine($"[{c.Id}] {c.Nom} - {c.Ville}");
    }
}

// --- Modèle (doit être après les instructions top-level) ---
record Client(int Id, string Nom, string Ville);