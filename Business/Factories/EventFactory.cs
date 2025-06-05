using Business.Models;
using Data.Entities;

namespace Business.Factories;

public static class EventFactory
{
    /// <summary>
    /// Skapar ett nytt EventEntity från en CreateEventRequest.
    /// </summary>
    public static EventEntity FromRequest(CreateEventRequest request, string userId)
    {
        return new EventEntity
        {
            EventName = request.EventName,
            Category = request.Category,
            ImageUrl = request.ImageUrl,
            EventDate = request.EventDate,
            Location = request.Location,
            Description = request.Description,
            CreatorId = Guid.Parse(userId)
        };
    }


    public static Event ToDto(EventEntity entity)
    {
        return new Event
        {
            Id = entity.Id,
            EventName = entity.EventName,
            Category = entity.Category,
            ImageUrl = entity.ImageUrl,
            EventDate = entity.EventDate,
            Location = entity.Location,
            Description = entity.Description,
            Packages = entity.Packages
                .Select(p => p.Package)
                .Select(package => new Package
                {
                    Id = package.Id,
                    PackageName = package.PackageName,
                    SeatingArrangement = package.SeatingArrangement,
                    Placement = package.Placement,
                    Price = package.Price,
                    Currency = package.Currency
                }).ToList()
        };
    }
}
