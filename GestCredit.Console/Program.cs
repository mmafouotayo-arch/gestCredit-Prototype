using GestCredit.Console.Modeles;

// --- Génération d'un jeu de données ---
List<Client> clients = new()
{
    new Client("Ahmadou Bello", "Yaoundé"),
    new Client("Fatima Njoya", "Douala"),
    new Client("Jean Mballa", "Yaoundé"),
    new Client("Aicha Souley", "Garoua"),
    new Client("Paul Etoundi", "Douala"),
    new Client("Marie Ngo", "Yaoundé"),
    new Client("Ibrahim Sali", "Garoua"),
};

Random random = new(42); // seed fixe pour des résultats reproductibles
string[] villesParClient = clients.Select(c => c.Ville).ToArray();

List<DemandeCredit> demandes = new();
for (int i = 0; i < 20; i++)
{
    // Exclut le dernier client (Ibrahim Sali) pour tester "clients sans demande"
    Client client = clients[random.Next(clients.Count - 1)];
    decimal montant = random.Next(500_000, 10_000_000);
    decimal taux = random.Next(8, 18);
    int duree = random.Next(6, 60);

    DemandeCredit demande = new(client.Id, montant, taux, duree);
    demandes.Add(demande);
}

// --- Rapport LINQ ---

System.Console.WriteLine("Encours par statut");
var encoursParStatut = demandes
    .GroupBy(d => d.Statut)
    .Select(g => new { Statut = g.Key, Total = g.Sum(d => d.Montant), Nombre = g.Count() });

foreach (var ligne in encoursParStatut)
{
    System.Console.WriteLine($"{ligne.Statut,-12} : {ligne.Nombre} demande(s) - {ligne.Total:N2} FCFA");
}

System.Console.WriteLine();
System.Console.WriteLine("Top 5 des plus gros montants");
var top5 = demandes.OrderByDescending(d => d.Montant).Take(5);

foreach (var d in top5)
{
    System.Console.WriteLine(d);
}

System.Console.WriteLine();
System.Console.WriteLine("Montant moyen par ville");
var moyenneParVille = demandes
    .Join(clients, d => d.ClientId, c => c.Id, (d, c) => new { d.Montant, c.Ville })
    .GroupBy(x => x.Ville)
    .Select(g => new { Ville = g.Key, Moyenne = g.Average(x => x.Montant) })
    .OrderByDescending(x => x.Moyenne);

foreach (var ligne in moyenneParVille)
{
    System.Console.WriteLine($"{ligne.Ville,-10} : {ligne.Moyenne:N2} FCFA");
}

System.Console.WriteLine();
System.Console.WriteLine("Clients sans demande");
var clientsSansDemande = clients
    .Where(c => !demandes.Any(d => d.ClientId == c.Id));

foreach (var c in clientsSansDemande)
{
    System.Console.WriteLine(c);
}