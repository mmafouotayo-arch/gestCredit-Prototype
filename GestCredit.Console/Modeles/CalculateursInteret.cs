namespace GestCredit.Console.Modeles;

// Intérêt simple : le taux s'applique uniquement au capital initial, chaque année
public class InteretSimple : ICalculateurInteret
{
    public string NomMethode => "Intérêt simple";

    public decimal CalculerInteret(decimal montant, decimal tauxAnnuel, int dureeMois)
    {
        decimal dureeAnnees = dureeMois / 12m;
        return montant * (tauxAnnuel / 100m) * dureeAnnees;
    }
}

// Intérêt composé : les intérêts précédents génèrent eux-mêmes des intérêts
public class InteretCompose : ICalculateurInteret
{
    public string NomMethode => "Intérêt composé";

    public decimal CalculerInteret(decimal montant, decimal tauxAnnuel, int dureeMois)
    {
        decimal dureeAnnees = dureeMois / 12m;
        double montantFinal = (double)montant * Math.Pow(1 + (double)(tauxAnnuel / 100m), (double)dureeAnnees);
        return (decimal)montantFinal - montant;
    }
}

// Intérêt dégressif : appliqué sur le capital restant dû (diminue à chaque mensualité)
public class InteretDegressif : ICalculateurInteret
{
    public string NomMethode => "Intérêt dégressif";

    public decimal CalculerInteret(decimal montant, decimal tauxAnnuel, int dureeMois)
    {
        decimal tauxMensuel = tauxAnnuel / 12m / 100m;
        double facteur = Math.Pow((double)(1 + tauxMensuel), -dureeMois);
        decimal mensualite = montant * tauxMensuel / (1 - (decimal)facteur);
        return (mensualite * dureeMois) - montant;
    }
}