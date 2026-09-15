using System;
using ImageProcessing.Imaging;
using OpenCvSharp;

namespace ImageProcessing.Effects
{
    /// <summary>Tuỳ chỉnh cho <see cref="BlankPageDetector"/>.</summary>
    public sealed class BlankPageOptions
    {
        /// <summary>Chia trang thành bao nhiêu cột để kiểm tra chi tiết theo từng ô.</summary>
        public int GridColumns { get; set; } = 4;

        /// <summary>Chia trang thành bao nhiêu hàng để kiểm tra chi tiết theo từng ô.</summary>
        public int GridRows { get; set; } = 4;

        /// <summary>Tỉ lệ phần trăm (0-100) của trang/ô phải là vùng không có mực thì mới tính là trang trắng.</summary>
        public float ThresholdPercent { get; set; } = 99f;

        public static BlankPageOptions Default { get { return new BlankPageOptions(); } }
    }

    /// <summary>
    /// Kiểm tra trang trắng qua 2 tầng: kiểm tra nhanh toàn trang trước, rồi (chỉ khi đã có vẻ
    /// trắng) kiểm tra chi tiết theo lưới từng ô, mỗi ô được làm sạch trước (xoá chấm mực lẻ tẻ,
    /// cắt viền tối, xoá đường kẻ) để các vết bẩn/lỗi quét gần rìa không bị nhầm thành nội dung
    /// thật.
    /// </summary>
    public static class BlankPageDetector
    {
        public static bool IsBlank(string imagePath, BlankPageOptions options = null)
        {
            using (Mat image = ImageEffectFileOps.LoadFirstPage(imagePath))
                return IsBlank(image, options);
        }

        internal static bool IsBlank(Mat image, BlankPageOptions options)
        {
            options = options ?? BlankPageOptions.Default;

            if (!PixelOps.LooksBlank(image, options.ThresholdPercent))
                return false;

            if (options.GridColumns < 2 && options.GridRows < 2)
                return true;

            return AllCellsBlank(image, options);
        }

        private static bool AllCellsBlank(Mat image, BlankPageOptions options)
        {
            int width = image.Cols;
            int height = image.Rows;
            int cellWidth = width / options.GridColumns;
            int cellHeight = height / options.GridRows;

            for (int col = 0; col < options.GridColumns; col++)
            {
                int left = col * cellWidth;
                int cellW = (col + 1 == options.GridColumns) ? width - left : cellWidth;
                if (cellW <= 0) continue;

                for (int row = 0; row < options.GridRows; row++)
                {
                    int top = row * cellHeight;
                    int cellH = (row + 1 == options.GridRows) ? height - top : cellHeight;
                    if (cellH <= 0) continue;

                    Rect cellRect = new Rect(left, top, cellW, cellH).Intersect(new Rect(0, 0, width, height));
                    if (cellRect.Width <= 0 || cellRect.Height <= 0) continue;

                    if (!CellLooksBlank(image, cellRect, options.ThresholdPercent))
                        return false;
                }
            }

            return true;
        }

        private static bool CellLooksBlank(Mat image, Rect cellRect, float thresholdPercent)
        {
            using (Mat cell = new Mat(image, cellRect))
            using (Mat noDots = PixelOps.OpenIsolatedInk(cell))
            using (Mat trimmed = PixelOps.TrimDarkMargins(noDots))
            using (Mat noLines = PixelOps.StripRuledLines(trimmed))
                return PixelOps.LooksBlank(noLines, thresholdPercent);
        }
    }
}
