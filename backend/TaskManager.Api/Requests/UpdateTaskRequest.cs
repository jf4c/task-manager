using TaskManager.Domain.Enums;

namespace TaskManager.Api.Requests;

public record UpdateTaskRequest
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EndDate { get; set; }
    public TaskItemStatus Status { get; set; }
}
