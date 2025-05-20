using Data.Entities;
namespace Data.Interfaces;

public interface IEventRepository : IBaseRepository<EventEntity>
{
    Task<IEnumerable<EventEntity>> GetAllWithPackagesAsync();
    Task<EventEntity?> GetOneWithPackagesAsync(Guid id);
}