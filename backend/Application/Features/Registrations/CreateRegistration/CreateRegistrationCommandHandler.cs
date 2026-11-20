namespace Application.Features.Registrations.CreateRegistration;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Registrations.Common;
using Domain.Common;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class CreateRegistrationCommandHandler(IAppDbContext dbContext)
    : ICommandHandler<CreateRegistrationCommand, Result<RegistrationResponse>>
{
    public async Task<Result<RegistrationResponse>> HandleAsync(
        CreateRegistrationCommand command,
        CancellationToken cancellationToken = default)
    {
        var plate = command.PlateNumber.Trim().ToUpperInvariant();

        var plateTaken = await dbContext.Registrations
            .AnyAsync(r => r.PlateNumber == plate, cancellationToken);

        if (plateTaken)
        {
            return Result<RegistrationResponse>.Failure(Error.Conflict(
                "Registration.Conflict",
                $"A registration with plate number '{plate}' already exists."));
        }

        // Next sequential number: CS27-0001, CS27-0002, ...
        var last = await dbContext.Registrations
            .OrderByDescending(r => r.RegistrationNumber)
            .Select(r => r.RegistrationNumber)
            .FirstOrDefaultAsync(cancellationToken);

        var next = last is null ? 1 : int.Parse(last[5..]) + 1;

        var registration = new Registration
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = $"CS27-{next:D4}",
            RegistrationDate = DateTime.UtcNow,
            ParticipantName = command.ParticipantName.Trim(),
            ContactNumber = command.ContactNumber,
            Email = command.Email.Trim(),
            Address = command.Address.Trim(),
            CarMakeModel = command.CarMakeModel.Trim(),
            PlateNumber = plate,
            RegistrationType = command.RegistrationType,
            Status = RegistrationStatus.Pending,
            RegistrationFee = RegistrationFees.For(command.RegistrationType),
            AmountPaid = command.AmountPaid
        };

        dbContext.Registrations.Add(registration);
        await dbContext.SaveChangesAsync(cancellationToken);

        return Result<RegistrationResponse>.Success(RegistrationResponse.From(registration));
    }
}