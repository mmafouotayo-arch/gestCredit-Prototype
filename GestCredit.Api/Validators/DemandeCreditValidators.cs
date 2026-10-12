using FluentValidation;
using GestCredit.Api.DTOs;
using GestCredit.Api.Models;

namespace GestCredit.Api.Validators;

public class CreateDemandeCreditDtoValidator : AbstractValidator<CreateDemandeCreditDto>
{
    public CreateDemandeCreditDtoValidator()
    {
        RuleFor(x => x.ClientId)
            .GreaterThan(0).WithMessage("L'identifiant du client est invalide.");

        RuleFor(x => x.Montant)
            .GreaterThan(0).WithMessage("Le montant doit être strictement positif.");

        RuleFor(x => x.TauxAnnuel)
            .GreaterThan(0).WithMessage("Le taux annuel doit être strictement positif.");

        RuleFor(x => x.DureeMois)
            .InclusiveBetween(1, 360).WithMessage("La durée doit être comprise entre 1 et 360 mois.");
    }
}

public class ChangerStatutDtoValidator : AbstractValidator<ChangerStatutDto>
{
    public ChangerStatutDtoValidator()
    {
        RuleFor(x => x.NouveauStatut)
            .NotEmpty().WithMessage("Le nouveau statut est obligatoire.")
            .Must(s => Enum.GetNames<StatutDemande>().Contains(s, StringComparer.OrdinalIgnoreCase))
            .WithMessage("Statut inconnu. Valeurs possibles : Brouillon, Soumise, EnAnalyse, Approuvee, Rejetee.");
    }
}