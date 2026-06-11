using Moq;
using TaskManager.Application.Abstractions;
using TaskManager.Domain.Entities;

namespace TaskManager.Application.Tests.Helpers;

public static class GenerateMocks
{
    public static void MockITaskRepositoryGetById(this Mock<ITaskRepository> mock, TaskItem entity)
    {
        mock.Setup(x => x.GetTaskByIdAsync(It.IsAny<int>())).ReturnsAsync(entity);
    }

    public static void MockITaskRepositoryGetById(this Mock<ITaskRepository> mock, int id, TaskItem entity)
    {
        mock.Setup(x => x.GetTaskByIdAsync(id)).ReturnsAsync(entity);
    }

    public static void MockITaskRepositoryGetAll(this Mock<ITaskRepository> mock, IEnumerable<TaskItem> entities)
    {
        mock.Setup(x => x.GetAllTasksAsync()).ReturnsAsync(entities);
    }

    public static void MockITaskRepositoryAdd(this Mock<ITaskRepository> mock)
    {
        mock.Setup(x => x.AddTaskAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
    }

    public static void MockITaskRepositoryUpdate(this Mock<ITaskRepository> mock)
    {
        mock.Setup(x => x.UpdateTaskAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
    }

    public static void MockITaskRepositoryDelete(this Mock<ITaskRepository> mock)
    {
        mock.Setup(x => x.DeleteTaskAsync(It.IsAny<TaskItem>())).Returns(Task.CompletedTask);
    }
}

