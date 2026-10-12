using FluentValidation;
using GestCredit.Api.DTOs;

namespace GestCredit.Api.Validators;

public class RegisterDtoValidator : AbstractValidator<RegisterDto>
{
    public RegisterDtoValidator()
    {
        RuleFor(x => x.NomUtilisateur)
            .NotEmpty().WithMessage("Le nom d'utilisateur est obligatoire.")
            .MinimumLength(3).WithMessage("Le nom d'utilisateur doit contenir au moins 3 caractères.")
            .MaximumLength(100).WithMessage("Le nom d'utilisateur ne peut pas dépasser 100 caractères.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("L'email est obligatoire.")
            .EmailAddress().WithMessage("L'email n'est pas valide.")
            .MaximumLength(150).WithMessage("L'email ne peut pas dépasser 150 caractères.");

        RuleFor(x => x.MotDePasse)
            .NotEmpty().WithMessage("Le mot de passe est obligatoire.")
            .MinimumLength(8).WithMessage("Le mot de passe doit contenir au moins 8 caractères.")
            .MaximumLength(64).WithMessage("Le mot de passe ne peut pas dépasser 64 caractères.")
            .Matches("[A-Z]").WithMessage("Le mot de passe doit contenir au moins une majuscule.")
            .Matches("[a-z]").WithMessage("Le mot de passe doit contenir au moins une minuscule.")
            .Matches("[0-9]").WithMessage("Le mot de passe doit contenir au moins un chiffre.");
    }
}

public class LoginDtoValidator : AbstractValidator<LoginDto>
{
    public LoginDtoValidator()
    {
        RuleFor(x => x.NomUtilisateur).NotEmpty().WithMessage("Le nom d'utilisateur est obligatoire.");
        RuleFor(x => x.MotDePasse).NotEmpty().WithMessage("Le mot de passe est obligatoire.");
    }
}