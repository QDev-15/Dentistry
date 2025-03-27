using Dentistry.ViewModels.Common;
using Microsoft.AspNetCore.Http;

namespace Dentistry.Data.Storages
{
    public interface IStorageService
    {
        #region Save to Directory Config or Manual

            string GetFileToHostingUrl(string fileName);
            Task<FileUploadResult> SaveFileToHostingAsync(IFormFile file);
            Task<FileUploadResult> SaveFileToHostingAsync(IFormFile file, string remoteDirectory);

            bool DeleteFileToHostingAsync(string urlImage);

        #endregion
        #region Save to Folder Constans 
            Task SaveFileAsync(Stream mediaBinaryStream, string fileName);
            Task<string> SaveFileAsync(IFormFile file);
            Task DeleteFileAsync(string fileName);
        #endregion

    }
}
