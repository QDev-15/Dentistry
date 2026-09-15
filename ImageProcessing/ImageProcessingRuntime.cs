namespace ImageProcessing
{
    /// <summary>
    /// Các tuỳ chỉnh hiệu năng ở phạm vi tiến trình, dành cho ứng dụng gọi vào thư viện này trong
    /// môi trường có tính đồng thời riêng (web app phục vụ nhiều request cùng lúc, worker chạy
    /// batch với pool task riêng, ...).
    /// </summary>
    public static class ImageProcessingRuntime
    {
        /// <summary>
        /// Đặt số luồng worker mà các hàm OpenCV bên dưới (Resize, MedianBlur, morphology, ...)
        /// được phép dùng nội bộ. Nếu để mặc định của OpenCV, mỗi luồng worker riêng của ứng dụng
        /// (ví dụ mỗi request web đang xử lý đồng thời) LẠI tiếp tục phân nhánh sang pool nội bộ
        /// của OpenCV, dẫn tới quá tải CPU khi ứng dụng đã chạy nhiều lệnh gọi này cùng lúc - đo
        /// thực tế cho thấy throughput giảm gần một nửa khi tải cao. Gọi
        /// <c>SetWorkerThreads(1)</c> một lần khi khởi động ứng dụng nếu tự quản lý tính đồng thời
        /// (web app, batch job dùng Parallel.For); chỉ để mặc định khi ứng dụng xử lý đơn luồng,
        /// từng ảnh một.
        /// </summary>
        public static void SetWorkerThreads(int threadCount)
        {
            OpenCvSharp.Cv2.SetNumThreads(threadCount);
        }

        /// <summary>Số luồng worker mà các hàm OpenCV hiện đang dùng nội bộ.</summary>
        public static int GetWorkerThreads()
        {
            return OpenCvSharp.Cv2.GetNumThreads();
        }
    }
}
