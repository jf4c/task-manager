using FluentAssertions;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;
using TaskManager.Domain.Exceptions;
using TaskManager.Domain.Tests.Helpers;

namespace TaskManager.Domain.Tests.Entities;

public class TaskItemTests
{
    [Fact]
    public void Create_ShouldCreateTask_WhenDataIsValid()
    {
        var result = TaskItemFactory.GetMockedObject();

        result.Title.Should().NotBeNullOrWhiteSpace();
        result.Description.Should().NotBeNullOrWhiteSpace();
        result.EndTime.Should().BeAfter(DateTime.UtcNow);
        result.ItemStatus.Should().BeOneOf(TaskItemStatus.Pending, TaskItemStatus.Running, TaskItemStatus.Completed);
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenTitleIsEmpty()
    {
        var action = () => TaskItem.Create("", "Task description", DateTime.UtcNow.AddHours(1), TaskItemStatus.Pending);

        action.Should()
            .Throw<DomainException>()
            .WithMessage("Title is required.");
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenTitleIsLongerThan100Characters()
    {
        var title = new string('A', 101);
        var action = () => TaskItem.Create(title, "Task description", DateTime.UtcNow.AddHours(1), TaskItemStatus.Pending);

        action.Should()
            .Throw<DomainException>()
            .WithMessage("Title cannot be longer than 100 characters.");
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenDescriptionIsEmpty()
    {
        var action = () => TaskItem.Create("Task title", " ", DateTime.UtcNow.AddHours(1), TaskItemStatus.Pending);

        action.Should()
            .Throw<DomainException>()
            .WithMessage("Description is required.");
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenEndTimeIsInThePast()
    {
        var action = () => TaskItem.Create("Task title", "Task description", DateTime.UtcNow.AddMinutes(-1), TaskItemStatus.Pending);

        action.Should()
            .Throw<DomainException>()
            .WithMessage("Completion date cannot be earlier than creation date.");
    }

    [Fact]
    public void Create_ShouldThrowDomainException_WhenStatusIsInvalid()
    {
        var invalidStatus = (TaskItemStatus)999;
        var action = () => TaskItem.Create("Task title", "Task description", DateTime.UtcNow.AddHours(1), invalidStatus);

        action.Should()
            .Throw<DomainException>()
            .WithMessage("Invalid task status.");
    }

    [Fact] 
    public void Update_ShouldUpdateTask_WhenDataIsValid()
    {
        var task = TaskItemFactory.GetMockedObject();
        var updatedEndTime = DateTime.UtcNow.AddHours(4);

        task.Update("New title", "New description", updatedEndTime, TaskItemStatus.Running);

        task.Title.Should().Be("New title");
        task.Description.Should().Be("New description");
        task.EndTime.Should().Be(updatedEndTime);
        task.ItemStatus.Should().Be(TaskItemStatus.Running);
    }
}
