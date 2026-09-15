using System;
using System.IO;
using ImageProcessing.Effects;

namespace ImageProcessing.Assembly
{
    /// <summary>
    /// Biến 1 trang vừa scan xong (dữ liệu thô) thành dạng lưu trữ cuối cùng: chạy pipeline hiệu
    /// ứng ngay tại chỗ, sau đó chuyển kết quả vào thư mục đích với tên mới (TIFF CCITT G4 cho
    /// trang đen-trắng, JPEG cho các trường hợp còn lại).
    /// </summary>
    public static class ScannedPageProcessor
    {
        public static string Process(string sourceImagePath, EffectPipelineOptions effects, string destinationFolder)
        {
            if (string.IsNullOrEmpty(sourceImagePath)) throw new ArgumentException("sourceImagePath is required.", nameof(sourceImagePath));
            if (string.IsNullOrEmpty(destinationFolder)) throw new ArgumentException("destinationFolder is required.", nameof(destinationFolder));
            effects = effects ?? EffectPipelineOptions.None;

            ImageEffectFileOps.ApplyPipelineInPlace(sourceImagePath, effects);

            string extension = effects.ColorImage ? ".jpg" : ".tif";
            string finalPath = Path.Combine(destinationFolder, Guid.NewGuid().ToString("N") + extension);
            File.Move(sourceImagePath, finalPath);
            return finalPath;
        }
    }
}
