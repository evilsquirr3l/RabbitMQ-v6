using EventBus.Base.Standard;

namespace Subscriber.Events;

public class ItemCreatedIntegrationEventHandler : IIntegrationEventHandler<ItemCreatedIntegrationEvent>
{
    public ItemCreatedIntegrationEventHandler()
    {
    }

    public async Task Handle(ItemCreatedIntegrationEvent @event)
    {
        //TODO: put into database
        Console.WriteLine($"Handled event: {@event}");
    }
}