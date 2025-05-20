namespace Business.Models;

public class Package
{
    public Guid Id { get; set; }
    public string PackageName { get; set; } = null!;
    public string? SeatingArrangement { get; set; }
    public string? Placement { get; set; }
    public decimal? Price { get; set; }
    public string? Currency { get; set; }
}
