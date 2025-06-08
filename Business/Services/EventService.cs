using Business.Factories;
using Business.Interfaces;
using Business.Models;
using Data.Interfaces;

namespace Business.Services;

// EventService handles business logic for event operations.
public class EventService(IEventRepository eventRepository) : IEventService
{
    private readonly IEventRepository _eventRepository = eventRepository;

    // Creates a new event linked to a specific user (creator).
    public async Task<EventResult> CreateEventAsync(CreateEventRequest request, string userId)
    {
        try
        {
            // Convert request DTO to entity using factory.
            var eventEntity = EventFactory.FromRequest(request, userId);

            // Save entity to database.
            var result = await _eventRepository.AddAsync(eventEntity);

            // Return error if saving failed.
            if (!result.Succeeded)
                return EventResult.CreateFailure(result.Error ?? "Failed to create event", result.StatusCode);

            // Return success with 201 Created.
            return EventResult.CreateSuccess(201);
        }
        catch (Exception ex)
        {
            return EventResult.CreateFailure($"Unexpected error: {ex.Message}", 500);
        }
    }

    // Retrieves all events.
    public async Task<EventResult<IEnumerable<Event>>> GetEventsAsync()
    {
        try
        {
            var result = await _eventRepository.GetAllAsync();

            if (!result.Succeeded)
                return EventResult<IEnumerable<Event>>.CreateFailure(result.Error ?? "Failed to fetch events", result.StatusCode);

            // Map entity list to DTOs using factory.
            var events = result.Result!.Select(EventFactory.ToDto);
            return EventResult<IEnumerable<Event>>.CreateSuccess(events);
        }
        catch (Exception ex)
        {
            return EventResult<IEnumerable<Event>>.CreateFailure($"Unexpected error: {ex.Message}", 500);
        }
    }

    // Retrieves a single event by its unique ID, including related packages.
    public async Task<EventResult<Event>> GetEventByIdAsync(Guid id)
    {
        try
        {
            var entity = await _eventRepository.GetOneWithPackagesAsync(id);

            if (entity == null)
                return EventResult<Event>.CreateFailure("Event not found", 404);

            var dto = EventFactory.ToDto(entity);
            return EventResult<Event>.CreateSuccess(dto);
        }
        catch (Exception ex)
        {
            return EventResult<Event>.CreateFailure($"Unexpected error: {ex.Message}", 500);
        }
    }

    // Updates an existing event. Only the original creator can update.
    public async Task<EventResult> UpdateEventAsync(Guid id, UpdateEventRequest request, string userId)
    {
        try
        {
            var entity = await _eventRepository.GetOneAsync(e => e.Id == id);

            if (entity == null)
                return EventResult.CreateFailure("Event not found", 404);

            if (entity.CreatorId.ToString() != userId)
                return EventResult.CreateFailure("Unauthorized", 403);

            // Apply changes.
            entity.EventName = request.EventName;
            entity.Category = request.Category;
            entity.ImageUrl = request.ImageUrl;
            entity.EventDate = request.EventDate;
            entity.Location = request.Location;
            entity.Description = request.Description;

            var result = await _eventRepository.UpdateAsync(entity);

            return result.Succeeded
                ? EventResult.CreateSuccess(204)
                : EventResult.CreateFailure(result.Error ?? "Failed to update event", result.StatusCode);
        }
        catch (Exception ex)
        {
            return EventResult.CreateFailure($"Unexpected error: {ex.Message}", 500);
        }
    }

    // Deletes an event. Only the original creator can delete.
    public async Task<EventResult> DeleteEventAsync(Guid id, string userId)
    {
        try
        {
            var entity = await _eventRepository.GetOneAsync(e => e.Id == id);

            if (entity == null)
                return EventResult.CreateFailure("Event not found", 404);

            if (entity.CreatorId.ToString() != userId)
                return EventResult.CreateFailure("Unauthorized", 403);

            var result = await _eventRepository.DeleteAsync(entity);

            return result.Succeeded
                ? EventResult.CreateSuccess(204)
                : EventResult.CreateFailure(result.Error ?? "Failed to delete event", result.StatusCode);
        }
        catch (Exception ex)
        {
            return EventResult.CreateFailure($"Unexpected error: {ex.Message}", 500);
        }
    }

    // Retrieves all events created by a specific user.
    public async Task<EventResult<IEnumerable<Event>>> GetEventsByCreatorAsync(Guid creatorId)
    {
        try
        {
            var result = await _eventRepository.GetAllByPredicateAsync(e => e.CreatorId == creatorId);

            if (!result.Succeeded)
                return EventResult<IEnumerable<Event>>.CreateFailure(result.Error ?? "Failed to fetch events", result.StatusCode);

            var events = result.Result!.Select(EventFactory.ToDto);
            return EventResult<IEnumerable<Event>>.CreateSuccess(events);
        }
        catch (Exception ex)
        {
            return EventResult<IEnumerable<Event>>.CreateFailure($"Unexpected error: {ex.Message}", 500);
        }
    }
}
