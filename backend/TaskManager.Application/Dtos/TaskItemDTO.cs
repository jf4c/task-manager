using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dtos;

public record TaskItemDTO
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime? EndDate { get; set; }
    public TaskItemStatus Status { get; set; }
}
