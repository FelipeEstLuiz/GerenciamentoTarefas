using Domain.Exceptions;
using Domain.Extension;
using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace Presentation.Api.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
        IEnumerable<string> erros = [];

        if (exception is FluentValidation.ValidationException validationException)
        {
            foreach (FluentValidation.Results.ValidationFailure failure in validationException.Errors)
            {
                StringBuilder message = new StringBuilder()
                    .Append($"{failure.PropertyName}")
                    .Append(" | ")
                    .Append($"{failure.ErrorMessage}")
                    .Append(" | ")
                    .Append("Valor: ");

                try
                {
                    string? enumName = failure.AttemptedValue is Enum enumValue
                        ? enumValue.GetEnumName()
                        : null;

                    if (enumName is not null) message.Append(enumName);
                    else message.Append($"{failure.AttemptedValue}");
                }
                catch (Exception)
                {
                    message.Append($"{failure.AttemptedValue}");
                }

                erros = [.. erros, message.ToString()];
            }
        }
        else if (exception is UnauthorizedAccessException)
        {
            httpStatusCode = HttpStatusCode.Unauthorized;
            erros = ["Usuário nao autorizado"];
        }
        else if (exception is ValidacaoException validacaoException)
        {
            httpStatusCode = validacaoException.HttpStatusCode;
            erros = [validacaoException.Message];
        }
        else
        {
            httpStatusCode = HttpStatusCode.InternalServerError;
            erros = ["Erro ao processar requisição"];
        }

        context.Response.StatusCode = (int)httpStatusCode;

        JsonSerializerSettings settings = new()
        {
            ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver(),
            Formatting = Formatting.Indented
        };

        await context.Response.WriteAsync(JsonConvert.SerializeObject(erros, settings));
    }
}
