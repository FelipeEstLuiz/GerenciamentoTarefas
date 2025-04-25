using Domain.Exceptions;
using Newtonsoft.Json;
using Presentation.Api.Controllers._Shared;
using System.Net;

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

    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        HttpStatusCode httpStatusCode = HttpStatusCode.BadRequest;
        IEnumerable<string> erros = [];

        if (exception is FluentValidation.ValidationException validationException)
        {
            foreach (FluentValidation.Results.ValidationFailure failure in validationException.Errors)
            {
                string message = $"{failure.PropertyName} | {failure.ErrorMessage} | Valor: {failure.AttemptedValue}";
                erros = [.. erros, message];
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

        await context.Response.WriteAsync(JsonConvert.SerializeObject(Response.Failure(erros), settings));
    }
}
