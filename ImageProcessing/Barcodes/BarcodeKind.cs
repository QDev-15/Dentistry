using System.Collections.Generic;
using ZXing;

namespace ImageProcessing.Barcodes
{
    /// <summary>Các loại mã vạch 1D mà thư viện này đọc/tạo được.</summary>
    public enum BarcodeKind
    {
        Code128,
        Code39,
        Code93,
        Codabar,
        Ean8,
        Ean13,
        UpcA,
        UpcE,
        AddOn2,
        AddOn5,
        Interleaved2of5,
        Industrial2of5,
        Inverted2of5,
        Matrix2of5,
        Iata2of5,
        DataLogic2of5,
        BcdMatrix
    }

    /// <summary>Ánh xạ <see cref="BarcodeKind"/> sang enum định dạng của ZXing để đọc và ghi.</summary>
    internal static class BarcodeCodecMap
    {
        /// <summary>
        /// (Các) định dạng ZXing có thể khớp với 1 loại mã vạch cho trước khi đọc. Một số loại
        /// 2-of-5 không có bộ đọc riêng trong ZXing nên đều quy về ITF, cách hiện thực 2-of-5 duy
        /// nhất mà ZXing có.
        /// </summary>
        public static IEnumerable<BarcodeFormat> ToReadFormats(BarcodeKind kind)
        {
            switch (kind)
            {
                case BarcodeKind.Code128: yield return BarcodeFormat.CODE_128; break;
                case BarcodeKind.Code39: yield return BarcodeFormat.CODE_39; break;
                case BarcodeKind.Code93: yield return BarcodeFormat.CODE_93; break;
                case BarcodeKind.Codabar: yield return BarcodeFormat.CODABAR; break;
                case BarcodeKind.Ean8: yield return BarcodeFormat.EAN_8; break;
                case BarcodeKind.Ean13: yield return BarcodeFormat.EAN_13; break;
                case BarcodeKind.UpcA: yield return BarcodeFormat.UPC_A; break;
                case BarcodeKind.UpcE: yield return BarcodeFormat.UPC_E; break;
                case BarcodeKind.AddOn2: yield return BarcodeFormat.UPC_EAN_EXTENSION; break;
                case BarcodeKind.AddOn5: yield return BarcodeFormat.UPC_EAN_EXTENSION; break;

                case BarcodeKind.Interleaved2of5:
                case BarcodeKind.Industrial2of5:
                case BarcodeKind.Inverted2of5:
                case BarcodeKind.Matrix2of5:
                case BarcodeKind.Iata2of5:
                case BarcodeKind.DataLogic2of5:
                case BarcodeKind.BcdMatrix:
                    yield return BarcodeFormat.ITF;
                    break;
            }
        }

        /// <summary>Định dạng ZXing duy nhất dùng để tạo 1 loại mã vạch. Ném lỗi với các loại mà ZXing không hỗ trợ tạo.</summary>
        public static BarcodeFormat ToWriteFormat(BarcodeKind kind)
        {
            switch (kind)
            {
                case BarcodeKind.Code128: return BarcodeFormat.CODE_128;
                case BarcodeKind.Code39: return BarcodeFormat.CODE_39;
                case BarcodeKind.Code93: return BarcodeFormat.CODE_93;
                case BarcodeKind.Codabar: return BarcodeFormat.CODABAR;
                case BarcodeKind.Ean8: return BarcodeFormat.EAN_8;
                case BarcodeKind.Ean13: return BarcodeFormat.EAN_13;
                case BarcodeKind.UpcA: return BarcodeFormat.UPC_A;
                case BarcodeKind.UpcE: return BarcodeFormat.UPC_E;
                case BarcodeKind.Interleaved2of5: return BarcodeFormat.ITF;
                default:
                    throw new System.NotSupportedException("ZXing's writer does not support barcode kind '" + kind + "'.");
            }
        }

        public static bool Matches(BarcodeKind configured, BarcodeFormat decoded)
        {
            foreach (BarcodeFormat f in ToReadFormats(configured))
                if (f == decoded)
                    return true;
            return false;
        }

        public static IList<BarcodeFormat> DistinctReadFormats(IEnumerable<BarcodeKind> kinds)
        {
            List<BarcodeFormat> formats = new List<BarcodeFormat>();
            foreach (BarcodeKind kind in kinds)
                foreach (BarcodeFormat f in ToReadFormats(kind))
                    if (!formats.Contains(f))
                        formats.Add(f);
            return formats;
        }
    }
}
