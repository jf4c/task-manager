using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dtos;

public record CreateTaskDTO()
{
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EndDate { get; set; }
}
