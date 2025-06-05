using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public class EventEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid CreatorId { get; set; }

    [Required]
    public string EventName { get; set; } = null!;

    [Required]
    public string Category { get; set; } = null!;

    public string? ImageUrl { get; set; }
    
    [Required]
    [Column(TypeName = "datetime2")]
    public DateTime EventDate { get; set; }

    [Required]
    public string Location { get; set; } = null!;

    public string Description { get; set; } = null!;

    public ICollection <EventPackageEntity> Packages { get; set; } = [];
}
