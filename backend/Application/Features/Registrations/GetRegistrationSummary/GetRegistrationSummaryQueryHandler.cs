namespace Application.Features.Registrations.GetRegistrationSummary;

using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Common;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public sealed class GetRegistrationSummaryQueryHandler(IAppDbContext dbContext)
    : IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>>
{
    public async Task<Result<GetRegistrationSummaryResponse>> HandleAsync(
        GetRegistrationSummaryQuery query,
        CancellationToken cancellationToken = default)
    {
        // Counts per status are calculated in the database
        var counts = await dbContext.Registrations
            .AsNoTracking()
            .GroupBy(r => r.Status)
            .Select(g => new { Status = g.Key, Count = g.Count() })
            .ToListAsync(cancellationToken);

        int CountOf(RegistrationStatus status) =>
            counts.FirstOrDefault(c => c.Status == status)?.Count ?? 0;

        // SQLite cannot SUM decimal columns in SQL, so fetch just the two money
        // columns of non-cancelled registrations and add them up here.
        var money = await dbContext.Registrations
            .AsNoTracking()
            .Where(r => r.Status != RegistrationStatus.Cancelled)
            .Select(r => new { r.RegistrationFee, r.AmountPaid })
            .ToListAsync(cancellationToken);

        var totalFees = money.Sum(m => m.RegistrationFee);
        var totalPayments = money.Sum(m => m.AmountPaid);

        var response = new GetRegistrationSummaryResponse(
            TotalRegistrations: counts.Sum(c => c.Count),
            Confirmed: CountOf(RegistrationStatus.Confirmed),
            CheckedIn: CountOf(RegistrationStatus.CheckedIn),
            Pending: CountOf(RegistrationStatus.Pending),
            Cancelled: CountOf(RegistrationStatus.Cancelled),
            TotalFees: totalFees,
            TotalPayments: totalPayments,
            UncollectedPayments: totalFees - totalPayments);

        return Result<GetRegistrationSummaryResponse>.Success(response);
    }
}