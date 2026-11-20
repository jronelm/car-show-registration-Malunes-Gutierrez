namespace Application.Features.Registrations.CreateRegistration;

using Domain.Entities;
using FluentValidation;

public sealed class CreateRegistrationValidator : AbstractValidator<CreateRegistrationCommand>
{
    public CreateRegistrationValidator()
    {
        RuleFor(x => x.ParticipantName).NotEmpty().MaximumLength(150);

        RuleFor(x => x.ContactNumber).NotEmpty()
            .Matches(@"^09\d{9}$")
            .WithMessage("Contact number must be a PH mobile number in the format 09XXXXXXXXX (11 digits).");

        RuleFor(x => x.Email).NotEmpty().EmailAddress().MaximumLength(200);
        RuleFor(x => x.Address).NotEmpty().MaximumLength(300);
        RuleFor(x => x.CarMakeModel).NotEmpty().MaximumLength(100);
        RuleFor(x => x.PlateNumber).NotEmpty().MaximumLength(15);
        RuleFor(x => x.RegistrationType).IsInEnum();

        RuleFor(x => x.AmountPaid)
            .GreaterThanOrEqualTo(0).WithMessage("Amount paid must be 0 or more.");

        RuleFor(x => x.AmountPaid)
            .Must((cmd, paid) => !Enum.IsDefined(cmd.RegistrationType)
                                 || paid <= RegistrationFees.For(cmd.RegistrationType))
            .WithMessage(cmd => $"Amount paid cannot be more than the fee ({RegistrationFees.For(cmd.RegistrationType):N0}).");
    }
}