namespace Api.Features.Registrations;

using Api.Common.Extensions;
using Api.Common.Filters;
using Application.Abstractions.Messaging;
using Application.Features.Registrations.Common;
using Application.Features.Registrations.CreateRegistration;
using Domain.Common;
using Application.Features.Registrations.GetRegistrations;
using Application.Features.Registrations.GetRegistrationById;
using Application.Features.Registrations.GetRegistrationSummary;

public static class RegistrationEndpoints
{
    public static void MapRegistrationEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/registrations").WithTags("Registrations");

        // POST /api/registrations
        group.MapPost("/", async (
                CreateRegistrationCommand command,
                ICommandHandler<CreateRegistrationCommand, Result<RegistrationResponse>> handler,
                CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(command, cancellationToken);

            return result.IsSuccess
                ? Results.Created($"/api/registrations/{result.Value!.Id}", result.Value)
                : result.ToProblemDetails();
        })
            .AddEndpointFilter<ValidationFilter<CreateRegistrationCommand>>()
            .Produces<RegistrationResponse>(StatusCodes.Status201Created)
            .ProducesValidationProblem()
            .ProducesProblem(StatusCodes.Status409Conflict);

        // GET /api/registrations?type=&status=&participantName=&page=&pageSize=
        group.MapGet("/", async (
                [AsParameters] GetRegistrationsQuery query,
                IQueryHandler<GetRegistrationsQuery, Result<GetRegistrationsResponse>> handler,
                CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(query, cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails();
        })
            .AddEndpointFilter<ValidationFilter<GetRegistrationsQuery>>()
            .Produces<GetRegistrationsResponse>()
            .ProducesValidationProblem();

        // GET /api/registrations/{id}
        group.MapGet("/{id:guid}", async (
                Guid id,
                IQueryHandler<GetRegistrationByIdQuery, Result<RegistrationResponse>> handler,
                CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetRegistrationByIdQuery(id), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails();
        })
            .Produces<RegistrationResponse>()
            .ProducesProblem(StatusCodes.Status404NotFound);

        // GET /api/registrations/summary
        group.MapGet("/summary", async (
                IQueryHandler<GetRegistrationSummaryQuery, Result<GetRegistrationSummaryResponse>> handler,
                CancellationToken cancellationToken) =>
        {
            var result = await handler.HandleAsync(new GetRegistrationSummaryQuery(), cancellationToken);

            return result.IsSuccess
                ? Results.Ok(result.Value)
                : result.ToProblemDetails();
        })
            .Produces<GetRegistrationSummaryResponse>();
    }
}