using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata;

namespace Data.Entities;

public class PackageEntity
{
    [Key]
    public Guid Id { get; set; } = Guid.NewGuid();

    public string PackageName { get; set; } = null!;

    public string? SeatingArrangement { get; set; }

    public string? Placement { get; set; }

    [Column(TypeName= "decimal(18,2)")]
    public decimal? Price { get; set; }

    public string? Currency { get; set; }

}