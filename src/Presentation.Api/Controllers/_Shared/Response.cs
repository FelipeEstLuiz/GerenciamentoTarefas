using System.Collections.ObjectModel;

namespace Presentation.Api.Controllers._Shared;

public class Response(object? data)
{
    private readonly IList<string> _messages = [];

    public bool Success => Errors?.Any() == false;
    public IEnumerable<string> Errors => new ReadOnlyCollection<string>(_messages);
    public object? Data { get; set; } = data;

    public Response() : this(null) { }

    public static Response Failure(string message)
        => new Response().AddError(message);

    public static Response Failure(IEnumerable<string> messages)
       => new Response().AddError(messages);

    private Response AddError(string message, params object[] parameters)
    {
        if (parameters != null && parameters.Length > 0)
            message = string.Format(message, parameters);

        if (!Errors.Contains(message))
            _messages.Add(message);

        return this;
    }

    private Response AddError(IEnumerable<string> errors, params object[] parameters)
    {
        foreach (string message in errors)
            AddError(message, parameters);

        return this;
    }
}

public class Response<TResponse>
{
    public bool Success { get; set; } = true;
    public required TResponse Data { get; set; }
}