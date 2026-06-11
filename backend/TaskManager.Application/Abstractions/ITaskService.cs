using TaskManager.Application.Dtos;

namespace TaskManager.Application.Abstractions;

public interface ITaskService
{
    Task CreateTaskAsync(CreateTaskDTO task);
    Task UpdateTaskAsync(UpdateTaskDTO task);
    Task DeleteTaskAsync(int id);
    Task<List<TaskItemDTO>> GetAllTaskItemsAsync();
    Task<TaskItemDTO> GetTaskItemByIdAsync(int id);
}
