using TaskManager.Domain.Enums;

namespace TaskManager.Application.Dtos;

public record CreateTaskDTO()
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime EndDate { get; set; }
}
