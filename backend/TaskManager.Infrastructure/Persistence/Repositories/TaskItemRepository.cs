using Microsoft.EntityFrameworkCore;
using TaskManager.Application.Abstractions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions;
using TaskManager.Infrastructure.Persistence.Context;

namespace TaskManager.Infrastructure.Persistence.Repositories;

public class TaskItemRepository(TaskDbContext context) : ITaskRepository
{
    private readonly TaskDbContext _context = context;

    public async Task<IEnumerable<TaskItem>> GetAllTasksAsync()
    {
        return await _context.Tasks
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TaskItem> GetTaskByIdAsync(int id)
    {
        var task = await _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        return task ?? throw new DomainException(
            $"Task with id '{id}' was not found.",
            404,
            "Resource not found");
    }

    public async Task AddTaskAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateTaskAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteTaskAsync(TaskItem task)
    {
        ArgumentNullException.ThrowIfNull(task);

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }
}
