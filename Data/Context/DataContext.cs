using Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Context;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<EventEntity>? Events { get; set; }

    public DbSet<PackageEntity>? Packages { get; set; }
    public DbSet<EventPackageEntity>? EventPackages { get; set; }

}
