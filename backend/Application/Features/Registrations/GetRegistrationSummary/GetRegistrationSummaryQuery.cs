namespace Application.Features.Registrations.GetRegistrationSummary;

using Application.Abstractions.Messaging;
using Domain.Common;

public sealed record GetRegistrationSummaryQuery : IQuery<Result<GetRegistrationSummaryResponse>>;