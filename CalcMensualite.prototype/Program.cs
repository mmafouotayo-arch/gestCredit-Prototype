using System;
using System.Globalization;

Console.WriteLine("Calculateur de mensualité de prêt");

// --- Saisie du montant ---
decimal montant;
while (true)
{
    Console.Write("Montant du prêt : ");
    string? saisieMontant = Console.ReadLine();

    if (decimal.TryParse(saisieMontant, NumberStyles.Number, CultureInfo.InvariantCulture, out montant) && montant > 0)
    {
        break;
    }
    Console.WriteLine("Montant invalide. Veuillez saisir un nombre positif.");
}

// --- Saisie du taux annuel ---
decimal tauxAnnuel;
while (true)
{
    Console.Write("Taux annuel (%) : ");
    string? saisieTaux = Console.ReadLine();

    if (decimal.TryParse(saisieTaux, NumberStyles.Number, CultureInfo.InvariantCulture, out tauxAnnuel) && tauxAnnuel > 0)
    {
        break;
    }
    Console.WriteLine("Taux invalide. Veuillez saisir un nombre positif.");
}

// --- Saisie de la durée ---
int dureeMois;
while (true)
{
    Console.Write("Durée (en mois) : ");
    string? saisieDuree = Console.ReadLine();

    if (int.TryParse(saisieDuree, out dureeMois) && dureeMois > 0)
    {
        break;
    }
    Console.WriteLine("Durée invalide. Veuillez saisir un nombre entier positif.");
}

// --- Calcul de la mensualité ---
decimal tauxMensuel = tauxAnnuel / 12m / 100m;

// Conversion en double uniquement pour la puissance (Math.Pow n'existe pas pour decimal)
double facteur = Math.Pow((double)(1 + tauxMensuel), -dureeMois);
decimal mensualite = montant * tauxMensuel / (1 - (decimal)facteur);

decimal coutTotal = mensualite * dureeMois;
decimal interetsTotaux = coutTotal - montant;

// --- Affichage formaté ---
Console.WriteLine();
Console.WriteLine($"Mensualité    : {mensualite:N2} FCFA");
Console.WriteLine($"Coût total    : {coutTotal:N2} FCFA");
Console.WriteLine($"Intérêts payés: {interetsTotaux:N2} FCFA");