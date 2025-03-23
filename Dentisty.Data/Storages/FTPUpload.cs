using Dentistry.Common;
using FluentFTP;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Formats.Webp;
using System.Text.RegularExpressions;

namespace Dentisty.Data.Storages
{
    public class FtpUploader
    {
        private readonly HostingConfig _config;
        private FtpClient _client;

        public FtpUploader(IOptions<HostingConfig> config)
        {
            _config = config.Value;
            _client = new FtpClient(_config.HostAddress)
            {
                Credentials = new System.Net.NetworkCredential(_config.UserFTP, _config.PassWordFTP)
            };
        }

        // Kết nối đến FTP
        private void Connect()
        {
            if (!_client.IsConnected)
            {
                _client.Connect();
            }
        }

        // Ngắt kết nối FTP
        private void Disconnect()
        {
            if (_client.IsConnected)
            {
                _client.Disconnect();
            }
        }

        // Kiểm tra và tạo thư mục nếu chưa tồn tại
        private void CreateDirectoryIfNotExists(string remoteDirectory)
        {
            Connect();

            // Kiểm tra xem thư mục đã tồn tại trên server chưa
            if (!_client.DirectoryExists(remoteDirectory))
            {
                _client.CreateDirectory(remoteDirectory);
            }

            Disconnect();
        }
        private string GetHostDirectory()
        {
            return string.IsNullOrEmpty(_config.HostDirectory.Replace("/", "")) ? "wwwroot/uploads/"  : _config.HostDirectory + "/wwwroot/uploads/";
        }

        // Upload ảnh lên FTP
        public string UploadImage(IFormFile file, string? remoteDirectory)
        {
            try
            {
                if (string.IsNullOrEmpty(remoteDirectory))
                {
                    remoteDirectory = _config.UploadDirectory;
                }
                // Kiểm tra file hợp lệ
                if (file == null || file.Length <= 0)
                {
                    throw new ArgumentException("File không hợp lệ.");
                }

                string fullRemoteDirectory = GetHostDirectory() + remoteDirectory;
                // Tạo thư mục trên server nếu chưa tồn tại
                CreateDirectoryIfNotExists(fullRemoteDirectory);

                // Lấy tên file
                string fileName = DateTime.Now.ToString("ddMMyyyyHHmmss_") + Path.GetFileName(file.FileName).ToLower();
                fileName = Regex.Replace(fileName, @"\s+", "-");
                // file path upload
                string remoteFilePath = Path.Combine(fullRemoteDirectory, fileName);
                // file path return
                string returnFilePath = Path.Combine(remoteDirectory, fileName);

                // Lưu file tạm thời
                string tempFilePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                Connect();

                // Upload file từ tạm lên FTP
                _client.UploadFile(tempFilePath, remoteFilePath, FtpRemoteExists.Overwrite);

                // Trả về URL file
                string imageUrl = $"{_config.WebHost}/uploads/{returnFilePath.Replace("\\", "/")}";

                // Xóa file tạm
                if (File.Exists(tempFilePath))
                {
                    File.Delete(tempFilePath);
                }

                Disconnect();

                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }
        public class UploadResult
        {
            public string ImageUrl { get; set; }     // Ảnh gốc
            public string ThumbUrl { get; set; }     // Ảnh thumbnail
        }

        /// <summary>
        /// Upload image and create thumb image
        /// </summary>
        /// <param name="file"></param>
        /// <param name="remoteDirectory"></param>
        /// <returns></returns>
        public UploadResult UploadImageV2(IFormFile file, string? remoteDirectory)
        {
            try
            {
                if (string.IsNullOrEmpty(remoteDirectory))
                {
                    remoteDirectory = _config.UploadDirectory;
                }

                // Kiểm tra file hợp lệ
                if (file == null || file.Length <= 0)
                {
                    throw new ArgumentException("File không hợp lệ.");
                }

                string fullRemoteDirectory = GetHostDirectory() + remoteDirectory;
                CreateDirectoryIfNotExists(fullRemoteDirectory);

                // Tạo tên file ảnh gốc
                string originalFileName = DateTime.Now.ToString("ddMMyyyyHHmmss_") + Path.GetFileName(file.FileName).ToLower();
                originalFileName = Regex.Replace(originalFileName, @"\s+", "-");

                // Đường dẫn file gốc trên server
                string remoteFilePath = Path.Combine(fullRemoteDirectory, originalFileName);
                string returnFilePath = Path.Combine(remoteDirectory, originalFileName);

                // Lưu file tạm thời
                string tempFilePath = Path.GetTempFileName();
                using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    file.CopyTo(stream);
                }

                // Tạo file thumb
                string thumbFileName = "thumb_" + originalFileName + ".webp";
                string remoteThumbPath = Path.Combine(fullRemoteDirectory, thumbFileName);
                string returnThumbPath = Path.Combine(remoteDirectory, thumbFileName);

                string tempThumbPath = Path.GetTempFileName();
                using (var stream = file.OpenReadStream())
                using (var image = Image.Load(stream))
                {
                    // Xóa metadata để giảm dung lượng
                    image.Metadata.ExifProfile = null;

                    // Lấy kích thước gốc của ảnh
                    int originalWidth = image.Width;
                    int originalHeight = image.Height;

                    // Xác định kích thước tối đa
                    int maxWidth = 800;
                    int maxHeight = 800;

                    // Tính tỷ lệ scale sao cho ảnh giữ đúng tỷ lệ gốc và không vượt quá maxWidth, maxHeight
                    float scale = Math.Min((float)maxWidth / originalWidth, (float)maxHeight / originalHeight);
                    int newWidth = (int)(originalWidth * scale);
                    int newHeight = (int)(originalHeight * scale);

                    image.Mutate(x => x.Resize(new ResizeOptions
                    {
                        Size = new Size(newWidth, newHeight),
                        Mode = ResizeMode.Max // Giữ nguyên tỷ lệ gốc
                    }));

                    // Lưu ảnh với chất lượng cao hơn
                    image.Save(tempThumbPath, new WebpEncoder { Quality = 90 });
                }

                // Kết nối FTP với timeout
                Connect();

                // Thiết lập timeout
                _client.Config.ConnectTimeout = 15000; // 15 giây
                _client.Config.ReadTimeout = 15000; // 15 giây
                _client.Config.DataConnectionConnectTimeout = 15000; // Timeout khi kết nối dữ liệu
                _client.Config.DataConnectionReadTimeout = 15000; // Timeout khi đọc dữ liệu

                // Upload file gốc lên FTP
                _client.UploadFile(tempFilePath, remoteFilePath, FtpRemoteExists.Overwrite);

                // Upload file thumb lên FTP
                _client.UploadFile(tempThumbPath, remoteThumbPath, FtpRemoteExists.Overwrite);

                // Xóa file tạm
                File.Delete(tempFilePath);
                File.Delete(tempThumbPath);

                Disconnect();

                // Trả về object chứa URL ảnh gốc và thumbnail
                return new UploadResult
                {
                    ImageUrl = $"{_config.WebHost}/uploads/{returnFilePath.Replace("\\", "/")}",
                    ThumbUrl = $"{_config.WebHost}/uploads/{returnThumbPath.Replace("\\", "/")}"
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }


        // Xóa file dựa trên URL
        public bool DeleteFileFromUrl(string imageUrl)
        {
            try
            {
                // Xử lý URL để lấy đường dẫn file từ xa
                if (string.IsNullOrEmpty(imageUrl))
                {
                    throw new ArgumentException("URL không hợp lệ.");
                }

                // Loại bỏ phần host để lấy đường dẫn file
                string relativePath = imageUrl.Replace($"{_config.WebHost}/uploads/", "").Replace("/", "\\");

                // Tạo đường dẫn đầy đủ đến file trên server FTP
                string remoteFilePath = Path.Combine(GetHostDirectory(), relativePath);

                Connect();

                // Kiểm tra và xóa file
                if (_client.FileExists(remoteFilePath))
                {
                    _client.DeleteFile(remoteFilePath);
                    Console.WriteLine($"Đã xóa file: {remoteFilePath}");
                }
                else
                {
                    throw new ArgumentException($"File không tồn tại: {remoteFilePath}");
                }

                Disconnect();

                return true;
            }
            catch (Exception ex)
            {
                throw new ArgumentException($"Lỗi khi xóa file từ URL: {ex.Message}");
            }
        }

    }
    public class UploadResult
    {
        public string ImageUrl { get; set; }     // Ảnh gốc
        public string ThumbUrl { get; set; }     // Ảnh thumbnail
    }
}



