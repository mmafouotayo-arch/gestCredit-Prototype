namespace GestCredit.Api.Models;


public class DemandeCredit
{
    // Constructeur privé sans paramètre, requis par Entity Framework Core
    private DemandeCredit() { }
    [System.Text.Json.Serialization.JsonConstructor]
    

 public DemandeCredit(int id, int clientId, decimal montant, decimal tauxAnnuel, int dureeMois, DateTime dateCreation, StatutDemande statut)
 {
    Id = id;
    ClientId = clientId;
    Montant = montant;
    TauxAnnuel = tauxAnnuel;
    DureeMois = dureeMois;
    DateCreation = dateCreation;
    Statut = statut;
}
    private static int _prochainId = 1;

    public int Id { get; private set; }
public int ClientId { get; private set; }
public decimal Montant { get; private set; }
public decimal TauxAnnuel { get; private set; }
public int DureeMois { get; private set; }
public DateTime DateCreation { get; private set; }
    // Propriétés calculées : lecture seule, jamais de set public
    public decimal Mensualite
    {
        get
        {
            decimal tauxMensuel = TauxAnnuel / 12m / 100m;
            double facteur = Math.Pow((double)(1 + tauxMensuel), -DureeMois);
            return Montant * tauxMensuel / (1 - (decimal)facteur);
        }
    }

    public decimal CoutTotal => Mensualite * DureeMois;

    public decimal InteretsTotaux => CoutTotal - Montant;

    public DemandeCredit(int clientId, decimal montant, decimal tauxAnnuel, int dureeMois)
    {
        if (montant <= 0)
        {
            throw new ArgumentException("Le montant doit être strictement positif.", nameof(montant));
        }

        if (tauxAnnuel <= 0)
        {
            throw new ArgumentException("Le taux annuel doit être strictement positif.", nameof(tauxAnnuel));
        }

        if (dureeMois <= 0 || dureeMois > 360)
        {
            throw new ArgumentException("La durée doit être comprise entre 1 et 360 mois.", nameof(dureeMois));
        }

        Id = _prochainId++;
        ClientId = clientId;
        Montant = montant;
        TauxAnnuel = tauxAnnuel;
        DureeMois = dureeMois;
        DateCreation = DateTime.Now;
    }

    public override string ToString() =>
        $"[{Id}] Client #{ClientId} - {Montant:N2} FCFA sur {DureeMois} mois → {Mensualite:N2} FCFA/mois";
       
    public StatutDemande Statut { get; private set; } = StatutDemande.Brouillon;

    public void ChangerStatut(StatutDemande nouveauStatut)
 {
    bool transitionValide = (Statut, nouveauStatut) switch
    {
        (StatutDemande.Brouillon, StatutDemande.Soumise) => true,
        (StatutDemande.Soumise, StatutDemande.EnAnalyse) => true,
        (StatutDemande.EnAnalyse, StatutDemande.Approuvee) => true,
        (StatutDemande.EnAnalyse, StatutDemande.Rejetee) => true,
        _ => false
    };

    if (!transitionValide)
    {
        throw new TransitionInvalideException(Statut, nouveauStatut);
    }

    Statut = nouveauStatut;
 }
}
