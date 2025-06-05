using System.ComponentModel.DataAnnotations;

namespace Business.Models;

public class CreateEventRequest
{
    [Required]
    public string EventName { get; set; } = null!;

    [Required]
    public string Category { get; set; } = null!;

    public string? ImageUrl { get; set; }

    [Required]
    public DateTime EventDate { get; set; }

    [Required]
    public string Location { get; set; } = null!;

    public string Description { get; set; } = null!;

    public Guid? CreatorId { get; set; }


}
