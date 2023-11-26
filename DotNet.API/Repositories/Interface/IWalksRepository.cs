using DotNet.API.Models.Domain;

namespace DotNet.API.Repositories.Interface
{
    public interface IWalksRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync();
        Task<Walk?> GetAsync(Guid id);
        Task<Walk?> UpdateAsync(Guid id, Walk walk);
    }
}
