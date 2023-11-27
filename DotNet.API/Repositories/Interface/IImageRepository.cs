using DotNet.API.Models.Domain;

namespace DotNet.API.Repositories.Interface
{
    public interface IImageRepository
    {
        Task<Image> UploadImage(Image image);
    }
}
