using TaskManager.Api.Requests;
using TaskManager.Api.Responses;
using TaskManager.Application.Dtos;

namespace TaskManager.Api.Mappers;

public static class TaskItemMapper
{
    public static CreateTaskDTO ToCreateTaskDTO(this CreateTaskRequest request)
    {
        return new CreateTaskDTO
        {
            Title = request.Title,
            Description = request.Description,
            EndDate = request.EndDate
        };
    }

    public static UpdateTaskDTO ToUpdateTaskDTO(this UpdateTaskRequest request, int id)
    {
        return new UpdateTaskDTO
        {
            Id = id,
            Title = request.Title,
            Description = request.Description,
            EndDate = request.EndDate,
            Status = request.Status
        };
    }

    public static TaskItemResponse ToTaskItemResponse(this TaskItemDTO dto)
    {
        return new TaskItemResponse
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            StartTime = EnsureUtcKind(dto.StartTime),
            EndDate = EnsureUtcKind(dto.EndDate),
            Status = dto.Status
        };
    }

    private static DateTime EnsureUtcKind(DateTime value)
    {
        return value.Kind switch
        {
            DateTimeKind.Utc => value,
            DateTimeKind.Local => value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(value, DateTimeKind.Utc)
        };
    }

    private static DateTime? EnsureUtcKind(DateTime? value)
    {
        return value.HasValue ? EnsureUtcKind(value.Value) : null;
    }
}
