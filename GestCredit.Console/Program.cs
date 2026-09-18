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