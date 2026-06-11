using FluentAssertions;
using Moq;
using TaskManager.Application.Abstractions;
using TaskManager.Application.Services;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Application.Tests.Helpers;

namespace TaskManager.Application.Tests.Services;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _taskRepositoryMock = new();
    private readonly TaskService _sut;

    public TaskServiceTests()
    {
        _sut = new TaskService(_taskRepositoryMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateTask_WhenDataIsValid()
    {
        var dto = TaskDTOFactory.CreateCreateTaskDTO();
        _taskRepositoryMock.MockITaskRepositoryAdd();

        await _sut.CreateTaskAsync(dto);

        _taskRepositoryMock.Verify(
            repository => repository.AddTaskAsync(
                It.Is<TaskItem>(task =>
                    task.Title == dto.Title &&
                    task.Description == dto.Description &&
                    task.EndTime == dto.EndDate &&
                    task.ItemStatus == TaskItemStatus.Pending)),
            Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldPreserveNullDescription_WhenDescriptionIsOmitted()
    {
        var dto = TaskDTOFactory.CreateCreateTaskDTO();
        dto.Description = null;
        _taskRepositoryMock.MockITaskRepositoryAdd();

        await _sut.CreateTaskAsync(dto);

        _taskRepositoryMock.Verify(
            repository => repository.AddTaskAsync(
                It.Is<TaskItem>(task =>
                    task.Title == dto.Title &&
                    task.Description == null &&
                    task.EndTime == dto.EndDate &&
                    task.ItemStatus == TaskItemStatus.Pending)),
            Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTask_WhenDataIsValid()
    {
        const int taskId = 10;
        var existingTask = TaskItemFactory.GetMockedObject();

        var dto = TaskDTOFactory.CreateUpdateTaskDTO(taskId);

        _taskRepositoryMock.MockITaskRepositoryGetById(taskId, existingTask);
        _taskRepositoryMock.MockITaskRepositoryUpdate();

        await _sut.UpdateTaskAsync(dto);

        _taskRepositoryMock.Verify(
            repository => repository.UpdateTaskAsync(
                It.Is<TaskItem>(task => ReferenceEquals(task, existingTask))),
            Times.Once);

        existingTask.Should().BeEquivalentTo(new
        {
            dto.Title,
            dto.Description,
            EndTime = (DateTime?)dto.EndDate,
            ItemStatus = dto.Status
        });
    }

    [Fact]
    public async Task UpdateAsync_ShouldPreserveNullDescription_WhenDescriptionIsOmitted()
    {
        const int taskId = 10;
        var existingTask = TaskItemFactory.GetMockedObject();
        var dto = TaskDTOFactory.CreateUpdateTaskDTO(taskId);
        dto.Description = null;

        _taskRepositoryMock.MockITaskRepositoryGetById(taskId, existingTask);
        _taskRepositoryMock.MockITaskRepositoryUpdate();

        await _sut.UpdateTaskAsync(dto);

        existingTask.Description.Should().BeNull();
        _taskRepositoryMock.Verify(
            repository => repository.UpdateTaskAsync(
                It.Is<TaskItem>(task => ReferenceEquals(task, existingTask))),
            Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ShouldDeleteTask_WhenTaskExists()
    {
        const int taskId = 7;
        var existingTask = TaskItem.Create(
            "Task to delete",
            "Description",
            DateTime.UtcNow.AddHours(1));

        _taskRepositoryMock.MockITaskRepositoryGetById(taskId, existingTask);
        _taskRepositoryMock.MockITaskRepositoryDelete();

        await _sut.DeleteTaskAsync(taskId);

        _taskRepositoryMock.Verify(repository => repository.DeleteTaskAsync(existingTask), Times.Once);
    }

    [Fact]
    public async Task GetTaskItemByIdAsync_ShouldReturnMappedDTO_WhenTaskExists()
    {
        const int taskId = 2;
        var mockedDTO = TaskDTOFactory.GetMockedObject();
        var task = TaskItem.Create(
            mockedDTO.Title,
            mockedDTO.Description,
            DateTime.UtcNow.AddHours(5));
        task.Update(task.Title, task.Description, task.EndTime, TaskItemStatus.Completed);

        _taskRepositoryMock.MockITaskRepositoryGetById(taskId, task);

        var result = await _sut.GetTaskItemByIdAsync(taskId);

        result.Should().NotBeNull();
        result.Title.Should().Be(task.Title);
        result.Description.Should().Be(task.Description);
        result.EndDate.Should().Be(task.EndTime);
        result.Status.Should().Be(task.ItemStatus);
    }

    [Fact]
    public async Task GetTaskItemByIdAsync_ShouldReturnMappedDTO_WhenSetupUsesAnyId()
    {
        var mockedDTO = TaskDTOFactory.GetMockedObject();
        var task = TaskItem.Create(
            mockedDTO.Title,
            mockedDTO.Description,
            DateTime.UtcNow.AddHours(2));
        task.Update(task.Title, task.Description, task.EndTime, TaskItemStatus.Running);

        _taskRepositoryMock.MockITaskRepositoryGetById(task);

        var result = await _sut.GetTaskItemByIdAsync(999);

        result.Should().NotBeNull();
        result.Title.Should().Be(task.Title);
        result.Description.Should().Be(task.Description);
        result.EndDate.Should().Be(task.EndTime);
        result.Status.Should().Be(task.ItemStatus);
    }

    [Fact]
    public async Task GetAllTaskItemsAsync_ShouldReturnMappedDTOs_WhenTasksExist()
    {
        var tasks = TaskItemFactory.GetMockedList(3);

        _taskRepositoryMock.MockITaskRepositoryGetAll(tasks);

        var result = await _sut.GetAllTaskItemsAsync();
        var expected = tasks.Select(task => new
        {
            task.Title,
            task.Description,
            EndDate = task.EndTime,
            Status = task.ItemStatus
        });

        result.Should().HaveCount(tasks.Count);
        result.Should().BeEquivalentTo(expected);
    }
}