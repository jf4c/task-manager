using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;

namespace TaskManager.Domain.Entities;

public class TaskItem : Entity
{
    private TaskItem()
    {
        Title = string.Empty;
        Description = string.Empty;
        StartTime = DateTime.UtcNow;
        EndTime = DateTime.UtcNow;
        ItemStatus = TaskItemStatus.Pending;
    }
    
    private TaskItem(string title, string description, DateTime? endTime, TaskItemStatus itemStatus)
    {
        Title = title;
        Description = description;
        StartTime = DateTime.UtcNow;
        EndTime = NormalizeToUtc(endTime);
        ItemStatus = itemStatus;
    }
    
    public static TaskItem Create(string title, string description, DateTime? endTime)
    {
        Validate(title, description, endTime, TaskItemStatus.Pending);

        return new TaskItem(title, description, endTime, TaskItemStatus.Pending);
    }

    public static TaskItem Create(string title, string description, DateTime? endTime, TaskItemStatus itemStatus)
    {
        Validate(title, description, endTime, itemStatus);

        return new TaskItem(title, description, endTime, itemStatus);
    }

    public void Update(string title, string description, DateTime? endTime, TaskItemStatus itemStatus)
    {
        Validate(title, description, endTime, itemStatus);

        Title = title;
        Description = description;
        EndTime = NormalizeToUtc(endTime);
        ItemStatus = itemStatus;
    }

    private static void Validate(string title, string description, DateTime? endTime, TaskItemStatus itemStatus)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title is required.");

        if (title.Length > 100)
            throw new DomainException("Title cannot be longer than 100 characters.");

        if (string.IsNullOrWhiteSpace(description))
            throw new DomainException("Description is required.");

        var normalizedEndTime = NormalizeToUtc(endTime);
        if (normalizedEndTime.HasValue && DateTime.UtcNow > normalizedEndTime.Value)
            throw new DomainException("Completion date cannot be earlier than creation date.");

        if (!Enum.IsDefined(itemStatus))
            throw new DomainException("Invalid task status.");
    }

    private static DateTime? NormalizeToUtc(DateTime? dateTime)
    {
        if (!dateTime.HasValue)
            return null;

        return dateTime.Value.Kind switch
        {
            DateTimeKind.Utc => dateTime.Value,
            DateTimeKind.Local => dateTime.Value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(dateTime.Value, DateTimeKind.Local).ToUniversalTime()
        };
    }

    public string Title { get; private set; }
    public string Description { get; private set; }
    public DateTime StartTime { get; private set; } 
    public DateTime? EndTime { get; private set; }  
    public TaskItemStatus ItemStatus { get; private set; }
}
