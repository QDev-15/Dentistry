using Dentistry.Common;
using FluentFTP;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Png;
using SixLabors.ImageSharp.Formats.Webp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing.Processors.Quantization;
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

                // Lấy đuôi file gốc
                string fileExtension = Path.GetExtension(file.FileName).ToLower();

                // Nếu là PNG thì giữ nguyên, nếu không thì đổi sang WebP
                bool isPng = fileExtension == ".png";
                string optimizedFileName = "opt_" + DateTime.Now.ToString("ddMMyyyyHHmmss_")
                                        + Path.GetFileNameWithoutExtension(file.FileName).ToLower()
                                        + (isPng ? ".png" : ".webp");

                optimizedFileName = Regex.Replace(optimizedFileName, @"\s+", "-");

                // Đường dẫn file trên server
                string remoteFilePath = Path.Combine(fullRemoteDirectory, optimizedFileName);
                string returnFilePath = Path.Combine(remoteDirectory, optimizedFileName);

                // Lưu file tạm thời
                string tempFilePath = Path.GetTempFileName();

                using (var stream = file.OpenReadStream())
                {
                    stream.Position = 0; // Reset stream về đầu trước khi load ảnh
                    using (var image = Image.Load<Rgba32>(stream))
                    {
                        // 1️⃣ Xóa metadata để giảm dung lượng
                        image.Metadata.ExifProfile = null;
                        image.Metadata.IptcProfile = null;
                        image.Metadata.XmpProfile = null;

                        if (isPng)
                        {
                            // ✅ Nếu là PNG, chỉ xóa metadata và giữ nguyên định dạng
                            image.Save(tempFilePath, new PngEncoder
                            {
                                CompressionLevel = PngCompressionLevel.BestCompression, // Nén tối ưu
                                TransparentColorMode = PngTransparentColorMode.Preserve
                            });
                        }
                        else
                        {
                            // 3️⃣ Nếu ảnh đơn sắc, chuyển sang grayscale
                            if (IsGrayscale(image))
                            {
                                image.Mutate(x => x.Grayscale());
                            }

                            // 4️⃣ Nếu không phải PNG, lưu WebP
                            image.Save(tempFilePath, new WebpEncoder
                            {
                                Quality = 70, // Giảm chất lượng để giảm kích thước
                                Method = WebpEncodingMethod.BestQuality, // Sử dụng nén mạnh
                                NearLossless = true
                            });
                        }
                    }
                }

                // Kết nối FTP
                Connect();

                // Thiết lập timeout
                _client.Config.ConnectTimeout = 15000;
                _client.Config.ReadTimeout = 15000;
                _client.Config.DataConnectionConnectTimeout = 15000;
                _client.Config.DataConnectionReadTimeout = 15000;

                // Upload file tối ưu lên FTP
                _client.UploadFile(tempFilePath, remoteFilePath, FtpRemoteExists.Overwrite);

                // Xóa file tạm
                File.Delete(tempFilePath);

                Disconnect();

                // Trả về object chứa URL ảnh tối ưu
                return new UploadResult
                {
                    ImageUrl = $"{_config.WebHost}/uploads/{returnFilePath.Replace("\\", "/")}",
                    ThumbUrl = string.Empty
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Lỗi: {ex.Message}");
                return null;
            }
        }


        /// <summary>
        /// Kiểm tra xem ảnh có phải là grayscale không
        /// </summary>
        private bool IsGrayscale(Image<Rgba32> image)
        {
            for (int y = 0; y < image.Height; y++)
            {
                for (int x = 0; x < image.Width; x++)
                {
                    var pixel = image[x, y];
                    if (pixel.R != pixel.G || pixel.G != pixel.B)
                    {
                        return false; // Không phải grayscale
                    }
                }
            }
            return true; // Ảnh là grayscale
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



