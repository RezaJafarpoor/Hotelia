using FluentValidation;
using FluentValidation.Results;

namespace Hotelia.Shared.EndpointFilters;

public class ValidationFilter<T> : IEndpointFilter where T : class
{
    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var model = context.Arguments.OfType<T>().FirstOrDefault();
        if (model is null)
            return Results.BadRequest();
        var validator = context.HttpContext.RequestServices.GetService<IValidator<T>>();
        if (validator is not null)
        {
            var result = await validator.ValidateAsync(model);
            if (!result.IsValid)
            {
                return Results.BadRequest(GetErrors(result.Errors));
            }
        }

        return await next(context);
    }

    private Dictionary<string, Dictionary<string,string>> GetErrors(List<ValidationFailure> result)
    {
        var dict = new Dictionary<string, Dictionary<string,string>>();
        var list = new Dictionary<string,string>();
        result.ForEach(x => list.Add(x.PropertyName,x.ErrorMessage));
        dict.Add("Validation problem", list);
        return dict;
    }
}