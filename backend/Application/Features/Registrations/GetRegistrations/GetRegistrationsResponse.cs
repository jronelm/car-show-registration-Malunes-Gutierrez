namespace Application.Features.Registrations.GetRegistrations;

using Application.Features.Registrations.Common;

public sealed record GetRegistrationsResponse(
    IReadOnlyList<RegistrationResponse> Items,
    int Page,
    int PageSize,
    int TotalCount,
    int TotalPages);