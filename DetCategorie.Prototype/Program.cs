using System;
using System.Globalization;

// --- Programme principal ---
decimal montant = SaisirMontant();
decimal tauxAnnuel = SaisirTauxAnnuel();
int dureeMois = SaisirDuree();

var (mensualite, coutTotal, interetsTotaux) = CalculerPret(montant, tauxAnnuel, dureeMois);
string categorie = DeterminerCategorie(montant);

AfficherResultats(mensualite, coutTotal, interetsTotaux, categorie);


// --- Méthodes ---

static decimal SaisirMontant()
{
    while (true)
    {
        Console.Write("Montant du prêt : ");
        string? saisie = Console.ReadLine();

        if (decimal.TryParse(saisie, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal montant) && montant > 0)
        {
            return montant;
        }
        Console.WriteLine("Montant invalide. Veuillez saisir un nombre positif.");
    }
}

static decimal SaisirTauxAnnuel()
{
    while (true)
    {
        Console.Write("Taux annuel (%) : ");
        string? saisie = Console.ReadLine();

        if (decimal.TryParse(saisie, NumberStyles.Number, CultureInfo.InvariantCulture, out decimal taux) && taux > 0)
        {
            return taux;
        }
        Console.WriteLine("Taux invalide. Veuillez saisir un nombre positif.");
    }
}

static int SaisirDuree()
{
    while (true)
    {
        Console.Write("Durée (en mois) : ");
        string? saisie = Console.ReadLine();

        if (int.TryParse(saisie, out int duree) && duree > 0)
        {
            return duree;
        }
        Console.WriteLine("Durée invalide. Veuillez saisir un nombre entier positif.");
    }
}

static (decimal Mensualite, decimal CoutTotal, decimal Interets) CalculerPret(decimal montant, decimal tauxAnnuel, int dureeMois)
{
    decimal tauxMensuel = tauxAnnuel / 12m / 100m;
    double facteur = Math.Pow((double)(1 + tauxMensuel), -dureeMois);
    decimal mensualite = montant * tauxMensuel / (1 - (decimal)facteur);

    decimal coutTotal = mensualite * dureeMois;
    decimal interets = coutTotal - montant;

    return (mensualite, coutTotal, interets);
}

static string DeterminerCategorie(decimal montant) => montant switch
{
    < 500_000m => "Micro-crédit",
    >= 500_000m and < 5_000_000m => "Standard",
    >= 5_000_000m => "Grand compte",
};

static void AfficherResultats(decimal mensualite, decimal coutTotal, decimal interets, string categorie)
{
    Console.WriteLine();
    Console.WriteLine($"Catégorie     : {categorie}");
    Console.WriteLine($"Mensualité    : {mensualite:N2} FCFA");
    Console.WriteLine($"Coût total    : {coutTotal:N2} FCFA");
    Console.WriteLine($"Intérêts payés: {interets:N2} FCFA");
}