using System.IO;

namespace ImageProcessing.Archiving
{
    /// <summary>
    /// Tối ưu cấu trúc + luồng dữ liệu của 1 file PDF có sẵn mà không đổi nội dung hiển thị
    /// (object/xref stream, nén lại, linearize). Không được phép làm hỏng tính đạt-chuẩn PDF/A -
    /// bên gọi phải kiểm tra lại sau khi tối ưu; bước tối ưu luôn chạy trước bước kiểm tra cuối
    /// cùng.
    /// </summary>
    public interface IPdfOptimizer
    {
        /// <param name="input">Stream PDF nguồn.</param>
        /// <param name="options">Chọn những phép tối ưu nào sẽ áp dụng.</param>
        /// <param name="output">Stream đích; bộ tối ưu không tự đóng stream này.</param>
        OptimizeResult Optimize(Stream input, OptimizeOptions options, Stream output);
    }

    /// <summary>Các tuỳ chọn cho <see cref="IPdfOptimizer.Optimize"/>.</summary>
    public sealed class OptimizeOptions
    {
        /// <summary>Viết lại theo dạng cross-reference/object stream (cấu trúc nhỏ gọn hơn).</summary>
        public bool UseObjectStreams { get; set; } = true;

        /// <summary>Nén lại bằng Flate các luồng dữ liệu chưa nén hoặc nén kém.</summary>
        public bool RecompressStreams { get; set; } = true;

        /// <summary>Linearize file ("fast web view" - xem trước khi tải xong).</summary>
        public bool Linearize { get; set; } = false;

        /// <summary>Bộ tuỳ chọn an toàn, chạy được trên tài liệu PDF/A mà không lo phá vỡ chuẩn.</summary>
        public static OptimizeOptions PdfASafe()
        {
            return new OptimizeOptions { UseObjectStreams = true, RecompressStreams = true, Linearize = false };
        }
    }

    /// <summary>Kết quả trả về của <see cref="IPdfOptimizer.Optimize"/>.</summary>
    public sealed class OptimizeResult
    {
        public OptimizeResult(bool success, long inputBytes, long outputBytes, string message = null)
        {
            Success = success;
            InputBytes = inputBytes;
            OutputBytes = outputBytes;
            Message = message ?? string.Empty;
        }

        public bool Success { get; }

        public long InputBytes { get; }

        public long OutputBytes { get; }

        /// <summary>Số byte tiết kiệm được (có thể âm nếu việc viết lại làm file to hơn).</summary>
        public long BytesSaved { get { return InputBytes - OutputBytes; } }

        /// <summary>Tỉ lệ giảm kích thước, 0..1 (bằng 0 khi không biết kích thước đầu vào).</summary>
        public double Ratio { get { return InputBytes > 0 ? (double)BytesSaved / InputBytes : 0.0; } }

        public string Message { get; }
    }
}
