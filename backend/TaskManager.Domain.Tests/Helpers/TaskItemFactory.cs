using Bogus;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Enums;

namespace TaskManager.Domain.Tests.Helpers;

internal static class TaskItemFactory
{
    public static TaskItem GetMockedObject()
    {
        return GetMockedList()[0];
    }

    public static List<TaskItem> GetMockedList(int quantity = 1)
    {
        var fake = new Faker<TaskItem>().CustomInstantiator(x =>
        {
            var title = x.Commerce.ProductName();
            var description = x.Lorem.Sentences(2);
            var endTime = x.Date.Soon(30).ToUniversalTime();
            var status = x.PickRandom<TaskItemStatus>();

            return TaskItem.Create(title, description, endTime, status);
        });

        return fake.Generate(quantity);
    }
}
