namespace TaskManager.Domain.Exceptions;

public class DomainException(string message, int statusCode = 400, string title = "Domain error")
    : Exception(message)
{
    public int StatusCode { get; } = statusCode;
    public string Title { get; } = title;
}
