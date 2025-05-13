using System.ComponentModel.DataAnnotations;

namespace Data.Entities;

public class EventEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required]
    public string EventName { get; set; } = null!;

    [Required]
    public string Category { get; set; } = null!;

    public string? ImageUrl { get; set; }
    
    [Required]
    public DateTime StartDateTime { get; set; }
    
    public DateTime? EndDateTime { get; set; }
    
    [Required]
    public string Location { get; set; } = null!;

    public string Description { get; set; } = null!;

    public decimal Price { get; set; }
}
