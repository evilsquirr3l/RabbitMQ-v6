using EventBus.Base.Standard;

namespace Subscriber.Events;

public class ItemCreatedIntegrationEvent : IntegrationEvent
{
    public ItemCreatedIntegrationEvent(string id, string description)
    {
        Id = id;
        Description = description;
    }

    public string Id { get; set; }
    public string Description { get; set; }
}