using Business.Models;

namespace Business.Interfaces;

public interface IEventService
{
    Task<EventResult> CreateEventAsync(CreateEventRequest request);
    Task<EventResult<IEnumerable<Event>>> GetEventsAsync();
    Task<EventResult<Event>> GetEventByIdAsync(Guid id);
    Task<EventResult> UpdateEventAsync(Guid id, UpdateEventRequest request);
    Task<EventResult> DeleteEventAsync(Guid id);
}
