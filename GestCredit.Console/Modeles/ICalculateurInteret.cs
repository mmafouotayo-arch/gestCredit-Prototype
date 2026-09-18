namespace GestCredit.Console.Modeles;

public interface ICalculateurInteret
{
    string NomMethode { get; }
    decimal CalculerInteret(decimal montant, decimal tauxAnnuel, int dureeMois);
}