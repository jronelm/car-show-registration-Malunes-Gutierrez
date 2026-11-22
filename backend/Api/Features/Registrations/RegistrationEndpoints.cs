namespace Api.Features.Registrations;

using Api.Common.Extensions;
using Api.Common.Filters;
using Application.Abstractions.Messaging;
using Application.Features.Registrations.Common;
using Application.Features.Registrations.CreateRegistration;
using Domain.Common;

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
    }
}