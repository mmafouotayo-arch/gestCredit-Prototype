namespace GestCredit.Console.Modeles;

public class TransitionInvalideException : Exception
{
    public StatutDemande StatutActuel { get; }
    public StatutDemande StatutDemande { get; }

    public TransitionInvalideException(StatutDemande statutActuel, StatutDemande statutDemande)
        : base($"Transition invalide : impossible de passer de '{statutActuel}' à '{statutDemande}'.")
    {
        StatutActuel = statutActuel;
        StatutDemande = statutDemande;
    }
}