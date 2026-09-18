using GestCredit.Console.Modeles;

var client = new Client("Ahmadou Bello", "Yaoundé");
System.Console.WriteLine(client);

var demande = new DemandeCredit(client.Id, 5_000_000m, 12m, 24);
System.Console.WriteLine(demande);
System.Console.WriteLine($"Coût total : {demande.CoutTotal:N2} FCFA");
System.Console.WriteLine($"Intérêts   : {demande.InteretsTotaux:N2} FCFA");

// Test de validation : doit lever une exception
try
{
    var demandeInvalide = new DemandeCredit(client.Id, -1000m, 12m, 24);
}
catch (ArgumentException ex)
{
    System.Console.WriteLine($"Erreur attendue : {ex.Message}");
}

decimal montant = 5_000_000m;
decimal taux = 12m;
int duree = 24;

List<ICalculateurInteret> calculateurs = new()
{
    new InteretSimple(),
    new InteretCompose(),
    new InteretDegressif(),
};

foreach (ICalculateurInteret calculateur in calculateurs)
{
    decimal interet = calculateur.CalculerInteret(montant, taux, duree);
    System.Console.WriteLine($"{calculateur.NomMethode,-20} : {interet:N2} FCFA");
}