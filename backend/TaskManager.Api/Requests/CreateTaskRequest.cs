namespace TaskManager.Api.Requests;

public record CreateTaskRequest
{
    public string Title { get; set; } = String.Empty;
    public string? Description { get; set; }
    public DateTime EndDate { get; set; }
}