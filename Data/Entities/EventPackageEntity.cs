using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;
public class EventPackageEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    [ForeignKey(nameof(Event))]
    public Guid EventId { get; set; }
    public EventEntity Event { get; set; } = null!;

    [ForeignKey(nameof(Package))]
    public Guid PackageId { get; set; }
    public PackageEntity Package { get; set; } = null!;
}
