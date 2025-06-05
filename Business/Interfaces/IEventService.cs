using Business.Models;

namespace Business.Interfaces;

public interface IEventService
{
    Task<EventResult> CreateEventAsync(CreateEventRequest request, string userId);
    Task<EventResult<IEnumerable<Event>>> GetEventsAsync();
    Task<EventResult<Event>> GetEventByIdAsync(Guid id);
    Task<EventResult> UpdateEventAsync(Guid id, UpdateEventRequest request, string userId);
    Task<EventResult> DeleteEventAsync(Guid id, string userId);
    Task<EventResult<IEnumerable<Event>>> GetEventsByCreatorAsync(Guid creatorId);
}
