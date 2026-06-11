using Bogus;
using TaskManager.Application.Dtos;
using TaskManager.Domain.Enums;

namespace TaskManager.Application.Tests.Helpers;

internal static class TaskDTOFactory
{
    public static TaskItemDTO GetMockedObject()
    {
        return GetMockedList()[0];
    }

    public static List<TaskItemDTO> GetMockedList(int quantity = 1)
    {
        var fake = new Faker<TaskItemDTO>().CustomInstantiator(x => new TaskItemDTO
        {
            Id = x.Random.Int(1, 10_000),
            Title = x.Commerce.ProductName(),
            Description = x.Lorem.Sentences(2),
            StartTime = DateTime.UtcNow,
            EndDate = x.Date.Soon(30).ToUniversalTime(),
            Status = x.PickRandom<TaskItemStatus>()
        });
        return fake.Generate(quantity);
    }

    public static CreateTaskDTO CreateCreateTaskDTO()
    {
        return new Faker<CreateTaskDTO>()
            .RuleFor(dto => dto.Title, faker => faker.Commerce.ProductName())
            .RuleFor(dto => dto.Description, faker => faker.Lorem.Sentence())
            .RuleFor(dto => dto.EndDate, faker => faker.Date.Soon(30).ToUniversalTime());
    }

    public static UpdateTaskDTO CreateUpdateTaskDTO(int? id = null)
    {
        return new Faker<UpdateTaskDTO>()
            .RuleFor(dto => dto.Id, faker => id ?? faker.Random.Int(1, 10_000))
            .RuleFor(dto => dto.Title, faker => faker.Commerce.ProductName())
            .RuleFor(dto => dto.Description, faker => faker.Lorem.Sentence())
            .RuleFor(dto => dto.EndDate, faker => faker.Date.Soon(30).ToUniversalTime())
            .RuleFor(dto => dto.Status, faker => faker.PickRandom<TaskItemStatus>());
    }
}
