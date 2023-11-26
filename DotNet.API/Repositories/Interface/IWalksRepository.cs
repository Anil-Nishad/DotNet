using DotNet.API.Models.Domain;

namespace DotNet.API.Repositories.Interface
{
    public interface IWalksRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<List<Walk>> GetAllAsync();
    }
}
