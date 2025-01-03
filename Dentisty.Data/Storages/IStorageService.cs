using Dentistry.ViewModels.Common;
using Microsoft.AspNetCore.Http;

namespace Dentistry.Data.Storages
{
    public interface IStorageService
    {
        string GetFileUrl(string fileName);

        Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
        Task<string> SaveFileAsync(IFormFile file);
        Task<FileUploadResult> SaveFileToHostingAsync(IFormFile file);
        Task<FileUploadResult> SaveFileToHostingAsync(IFormFile file, string remoteDirectory);

        Task DeleteFileAsync(string fileName);
    }
}
