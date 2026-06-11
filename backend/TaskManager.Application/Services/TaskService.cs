using TaskManager.Application.Abstractions;
using TaskManager.Application.Dtos;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Services;

public class TaskService(ITaskRepository taskRepository) : ITaskService
{
    private readonly ITaskRepository _taskRepository = taskRepository;

    public async Task CreateTaskAsync(CreateTaskDTO dto)
    {
        var taskItem = TaskItem.Create(dto.Title, dto.Description, dto.EndDate);
        
        await _taskRepository.AddTaskAsync(taskItem);
    }

    public async Task UpdateTaskAsync(UpdateTaskDTO dto)
    {
        var taskItem = await _taskRepository.GetTaskByIdAsync(dto.Id);
        taskItem.Update(dto.Title, dto.Description, dto.EndDate, dto.Status);

        await _taskRepository.UpdateTaskAsync(taskItem);
    }

    public async Task DeleteTaskAsync(int id)
    {
        var taskItem = await _taskRepository.GetTaskByIdAsync(id);
        await _taskRepository.DeleteTaskAsync(taskItem);
    }

    public async Task<List<TaskItemDTO>> GetAllTaskItemsAsync()
    {
        var tasks = await _taskRepository.GetAllTasksAsync();
        return tasks.Select(MapToTaskItemDto).ToList();
    }

    public async Task<TaskItemDTO> GetTaskItemByIdAsync(int id)
    {
        var task = await _taskRepository.GetTaskByIdAsync(id);
        return MapToTaskItemDto(task);
    }

    private static TaskItemDTO MapToTaskItemDto(TaskItem task)
    {
        return new TaskItemDTO
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            StartTime = EnsureUtcKind(task.StartTime),
            EndDate = EnsureUtcKind(task.EndTime),
            Status = task.ItemStatus
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
