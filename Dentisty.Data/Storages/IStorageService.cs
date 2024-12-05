using Microsoft.AspNetCore.Http;

namespace Dentistry.Data.Storages
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task<string> SaveFileAsync(IFormFile file);

        Task DeleteFileAsync(string fileName);
    }
}
