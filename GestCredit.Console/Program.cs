using GestCredit.Console.Modeles;

const string CheminClients = "clients.json";
const string CheminDemandes = "demandes.json";

List<Client> clients = await DepotJson.ChargerClientsAsync(CheminClients);
List<DemandeCredit> demandes = await DepotJson.ChargerDemandesAsync(CheminDemandes);

System.Console.WriteLine($"{clients.Count} client(s) et {demandes.Count} demande(s) chargés.");

bool continuer = true;
while (continuer)
{
    AfficherMenu();
    string? choix = System.Console.ReadLine();

    switch (choix)
    {
        case "1":
            AjouterClient(clients);
            break;
        case "2":
            AjouterDemande(clients, demandes);
            break;
        case "3":
            ChangerStatutDemande(demandes);
            break;
        case "4":
            AfficherRapport(clients, demandes);
            break;
        case "5":
            await DepotJson.SauvegarderClientsAsync(clients, CheminClients);
            await DepotJson.SauvegarderDemandesAsync(demandes, CheminDemandes);
            System.Console.WriteLine("Données sauvegardées.");
            break;
        case "6":
            await DepotJson.SauvegarderClientsAsync(clients, CheminClients);
            await DepotJson.SauvegarderDemandesAsync(demandes, CheminDemandes);
            continuer = false;
            System.Console.WriteLine("Données sauvegardées. Au revoir !");
            break;
        default:
            System.Console.WriteLine("Choix invalide.");
            break;
    }
}

static void AfficherMenu()
{
    System.Console.WriteLine();
    System.Console.WriteLine("GestCredit");
    System.Console.WriteLine("1. Ajouter un client");
    System.Console.WriteLine("2. Ajouter une demande de crédit");
    System.Console.WriteLine("3. Changer le statut d'une demande");
    System.Console.WriteLine("4. Afficher le rapport");
    System.Console.WriteLine("5. Sauvegarder");
    System.Console.WriteLine("6. Sauvegarder et quitter");
    System.Console.Write("Votre choix : ");
}

static void AjouterClient(List<Client> clients)
{
    System.Console.Write("Nom : ");
    string nom = System.Console.ReadLine() ?? "";
    System.Console.Write("Ville : ");
    string ville = System.Console.ReadLine() ?? "";

    try
    {
        clients.Add(new Client(nom, ville));
        System.Console.WriteLine("Client ajouté.");
    }
    catch (ArgumentException ex)
    {
        System.Console.WriteLine($"Erreur : {ex.Message}");
    }
}

static void AjouterDemande(List<Client> clients, List<DemandeCredit> demandes)
{
    if (!clients.Any())
    {
        System.Console.WriteLine("Aucun client. Ajoutez d'abord un client.");
        return;
    }

    foreach (var c in clients) System.Console.WriteLine(c);
    System.Console.Write("Id du client : ");

    if (!int.TryParse(System.Console.ReadLine(), out int clientId) || !clients.Any(c => c.Id == clientId))
    {
        System.Console.WriteLine("Client invalide.");
        return;
    }

    System.Console.Write("Montant : ");
    decimal.TryParse(System.Console.ReadLine(), out decimal montant);
    System.Console.Write("Taux annuel (%) : ");
    decimal.TryParse(System.Console.ReadLine(), out decimal taux);
    System.Console.Write("Durée (mois) : ");
    int.TryParse(System.Console.ReadLine(), out int duree);

    try
    {
        demandes.Add(new DemandeCredit(clientId, montant, taux, duree));
        System.Console.WriteLine("Demande ajoutée.");
    }
    catch (ArgumentException ex)
    {
        System.Console.WriteLine($"Erreur : {ex.Message}");
    }
}

static void ChangerStatutDemande(List<DemandeCredit> demandes)
{
    foreach (var d in demandes) System.Console.WriteLine(d);
    System.Console.Write("Id de la demande : ");

    if (!int.TryParse(System.Console.ReadLine(), out int id))
    {
        System.Console.WriteLine("Id invalide.");
        return;
    }

    DemandeCredit? demande = demandes.FirstOrDefault(d => d.Id == id);
    if (demande is null)
    {
        System.Console.WriteLine("Demande introuvable.");
        return;
    }

    System.Console.WriteLine("Nouveau statut : 1-Soumise 2-EnAnalyse 3-Approuvee 4-Rejetee");
    string? choix = System.Console.ReadLine();

    StatutDemande? nouveauStatut = choix switch
    {
        "1" => StatutDemande.Soumise,
        "2" => StatutDemande.EnAnalyse,
        "3" => StatutDemande.Approuvee,
        "4" => StatutDemande.Rejetee,
        _ => null
    };

    if (nouveauStatut is null)
    {
        System.Console.WriteLine("Choix invalide.");
        return;
    }

    try
    {
        demande.ChangerStatut(nouveauStatut.Value);
        System.Console.WriteLine("Statut mis à jour.");
    }
    catch (TransitionInvalideException ex)
    {
        System.Console.WriteLine($"Erreur : {ex.Message}");
    }
}

static void AfficherRapport(List<Client> clients, List<DemandeCredit> demandes)
{
    System.Console.WriteLine();
    System.Console.WriteLine("Encours par statut");
    foreach (var g in demandes.GroupBy(d => d.Statut))
    {
        System.Console.WriteLine($"{g.Key,-12} : {g.Count()} demande(s) - {g.Sum(d => d.Montant):N2} FCFA");
    }

    System.Console.WriteLine();
    System.Console.WriteLine("Clients sans demande");
    foreach (var c in clients.Where(c => !demandes.Any(d => d.ClientId == c.Id)))
    {
        System.Console.WriteLine(c);
    }
}