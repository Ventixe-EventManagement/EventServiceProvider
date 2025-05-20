using Data.Context;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories;

public class EventRepository : BaseRepository<EventEntity>, IEventRepository
{
    public EventRepository(DataContext context) : base(context)
    {
    }

    public Task<IEnumerable<EventEntity>> GetAllWithPackagesAsync()
    {
        return GetAllWithDetailsAsync(query => query.Include(e => e.Packages));
    }

    public Task<EventEntity?> GetOneWithPackagesAsync(Guid id)
    {
        return GetOneWithDetailsAsync(
            query => query.Include(e => e.Packages),
            e => e.Id == id);
    }
}
