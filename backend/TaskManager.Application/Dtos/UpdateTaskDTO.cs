using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dtos;

public record UpdateTaskDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
    public TaskItemStatus Status { get; set; }
}
