namespace Business.Models;

public class Event
{
    public Guid Id { get; set; }
    public string EventName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string? ImageUrl { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = null!;
    public string Description { get; set; } = null!;

    public List<Package> Packages { get; set; } = [];
}