using System;
using System.Drawing;

namespace ImageProcessing.Imaging
{
    /// <summary>
    /// Giá trị bit nào đại diện cho mực trong 1 <see cref="Bitmap"/> lập chỉ mục 1bpp. GDI+ không
    /// ép theo 1 quy ước cố định: nó giữ nguyên bảng màu mà định dạng gốc ngụ ý (ví dụ TIFF gắn cờ
    /// MinIsWhite sẽ giải mã với palette[0]=Trắng, palette[1]=Đen), nên đoạn code đọc trực tiếp bit
    /// đã đóng gói phải tự xác định điều này theo từng bitmap thay vì giả định cố định "index 0 là
    /// đen".
    /// </summary>
    internal enum BitonalPolarity
    {
        /// <summary>Giá trị bit 0 là mực (tối); giá trị bit 1 là giấy (sáng).</summary>
        ZeroIsInk,

        /// <summary>Giá trị bit 1 là mực (tối); giá trị bit 0 là giấy (sáng).</summary>
        OneIsInk
    }

    internal static class BitonalPolarityDetector
    {
        /// <summary>
        /// Kiểm tra 2 mục trong bảng màu của 1 bitmap lập chỉ mục 1bpp và trả về giá trị bit nào
        /// tối hơn (là mực). Trả về mặc định <see cref="BitonalPolarity.OneIsInk"/> (bảng màu mặc
        /// định của GDI+ cho 1 bitmap 1bpp vừa cấp phát) khi bảng màu bị thiếu hoặc suy biến.
        /// </summary>
        public static BitonalPolarity Detect(Bitmap bitonal)
        {
            if (bitonal == null) throw new ArgumentNullException(nameof(bitonal));

            Color[] entries = bitonal.Palette?.Entries;
            if (entries == null || entries.Length < 2)
                return BitonalPolarity.OneIsInk;

            double lum0 = Luminance(entries[0]);
            double lum1 = Luminance(entries[1]);

            // Bảng màu suy biến/giống nhau: giữ mặc định theo quy ước thay vì đoán mò.
            if (Math.Abs(lum0 - lum1) < 1.0)
                return BitonalPolarity.OneIsInk;

            return lum0 < lum1 ? BitonalPolarity.ZeroIsInk : BitonalPolarity.OneIsInk;
        }

        private static double Luminance(Color c)
        {
            return (c.R + c.G + c.B) / 3.0;
        }
    }
}
