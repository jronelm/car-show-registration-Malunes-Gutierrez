namespace Application.Features.Registrations.Common;

using Domain.Entities;
using Domain.Enums;

public sealed record RegistrationResponse(
    Guid Id,
    string RegistrationNumber,
    DateTime RegistrationDate,
    string ParticipantName,
    string ContactNumber,
    string Email,
    string Address,
    string CarMakeModel,
    string PlateNumber,
    RegistrationType RegistrationType,
    RegistrationStatus Status,
    decimal RegistrationFee,
    decimal AmountPaid,
    PaymentStatus PaymentStatus)
{
    public static RegistrationResponse From(Registration r) => new(
        r.Id, r.RegistrationNumber, r.RegistrationDate, r.ParticipantName,
        r.ContactNumber, r.Email, r.Address, r.CarMakeModel, r.PlateNumber,
        r.RegistrationType, r.Status, r.RegistrationFee, r.AmountPaid,
        GetPaymentStatus(r.AmountPaid, r.RegistrationFee));

 
    public static PaymentStatus GetPaymentStatus(decimal paid, decimal fee) =>
        paid <= 0 ? PaymentStatus.Unpaid
        : paid >= fee ? PaymentStatus.Paid
        : PaymentStatus.Partial;
}