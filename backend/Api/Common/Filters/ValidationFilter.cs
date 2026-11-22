namespace Api.Common.Filters;

using FluentValidation;

public sealed class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(
        EndpointFilterInvocationContext context,
        EndpointFilterDelegate next)
    {
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();

        if (validator is not null)
        {
            var argument = context.Arguments.OfType<T>().FirstOrDefault();

            if (argument is null)
            {
                return Results.Problem(
                    title: "Validation.RequestBody",
                    detail: "The request body is missing or invalid.",
                    statusCode: StatusCodes.Status400BadRequest);
            }

            var result = await validator.ValidateAsync(argument, context.HttpContext.RequestAborted);

            if (!result.IsValid)
            {
                // 400 with an "errors" object: { "ParticipantName": ["..."] }
                return Results.ValidationProblem(result.ToDictionary());
            }
        }

        return await next(context);
    }
}