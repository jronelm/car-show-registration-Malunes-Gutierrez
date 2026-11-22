namespace Application.Features.Registrations.GetRegistrations;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Application.Features.Registrations.Common;
using Domain.Common;
using Microsoft.EntityFrameworkCore;

public sealed class GetRegistrationsQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>>
{
    public async Task<Result<GetRegistrationsResponse>> HandleAsync(
        GetRegistrationsQuery query,
        CancellationToken cancellationToken = default)
    {
        var registrations = dbContext.Registrations.AsNoTracking();

        
        if (!string.IsNullOrWhiteSpace(query.ParticipantName))
        {
            var name = query.ParticipantName.Trim().ToLower();
            registrations = registrations.Where(r => r.ParticipantName.ToLower().Contains(name));
        }

        if (query.Type is not null)
            registrations = registrations.Where(r => r.RegistrationType == query.Type);

        if (query.Status is not null)
            registrations = registrations.Where(r => r.Status == query.Status);

        var totalCount = await registrations.CountAsync(cancellationToken);
        var totalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize);

        
        var items = await registrations
            .OrderByDescending(r => r.RegistrationDate)
            .ThenByDescending(r => r.RegistrationNumber)
            .Skip((query.Page - 1) * query.PageSize)
            .Take(query.PageSize)
            .ToListAsync(cancellationToken);

        var response = new GetRegistrationsResponse(
            items.Select(RegistrationResponse.From).ToList(),
            query.Page,
            query.PageSize,
            totalCount,
            totalPages);

        return Result<GetRegistrationsResponse>.Success(response);
    }
}