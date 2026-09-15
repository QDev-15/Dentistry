using System;
using System.Drawing;

namespace ImageProcessing.Effects
{
    /// <summary>Draws a text label with an opaque background swatch onto a copy of an image.</summary>
    public static class TextStamper
    {
        public static Bitmap Stamp(
            Bitmap source,
            string text,
            int left,
            int top,
            float fontSize,
            FontStyle fontStyle,
            Color textColor,
            Color backgroundColor,
            string fontFamily)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));

            Bitmap result = new Bitmap(source);
            using (Graphics g = Graphics.FromImage(result))
            using (Font font = new Font(fontFamily, fontSize, fontStyle, GraphicsUnit.Point))
            using (Brush backBrush = new SolidBrush(backgroundColor))
            using (Brush textBrush = new SolidBrush(textColor))
            {
                g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                SizeF size = g.MeasureString(text, font);
                g.FillRectangle(backBrush, left, top, size.Width, size.Height);
                g.DrawString(text, font, textBrush, left, top);
            }
            return result;
        }
    }
}
