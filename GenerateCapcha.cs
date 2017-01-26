using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenerateCapcha_VN
{
    class GenerateCapcha
    {
        public GenerateCapcha()
        {

        }
        int iHeight = 80;
        int iWidth = 190;
        Random rd = new Random();

        int[] aBackgroundNoiseColor = new int[] { 150, 150, 150 };
        int[] aTextColor = new int[] { 0, 0, 0 };
        int[] aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };
        string[] aFontNames = new string[] { "Comic Sans MS", "Arial", "Times New Roman", "Georgia", "Verdana", "Geneva" };
        #region FontStyle
        FontStyle[] aFontStyles = new FontStyle[]
{  
 FontStyle.Bold,
 FontStyle.Italic,
 FontStyle.Regular,
 FontStyle.Strikeout,
 FontStyle.Underline
};
        #endregion
        #region Background
        HatchStyle[] aHatchStyles = new HatchStyle[]
{
 HatchStyle.BackwardDiagonal, HatchStyle.Cross, 
	HatchStyle.DashedDownwardDiagonal, HatchStyle.DashedHorizontal,
 HatchStyle.DashedUpwardDiagonal, HatchStyle.DashedVertical
};
        #endregion

        public byte[] GetCapcha(string sCaptchaText)
        {
            //Creates an output Bitmap
            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight, PixelFormat.Format24bppRgb);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            //Create a Drawing area
            RectangleF oRectangleF = new RectangleF(0, 0, iWidth, iHeight);
            Brush oBrush = default(Brush);

            //Draw background (Lighter colors RGB 100 to 255)
            oBrush = new HatchBrush(aHatchStyles[rd.Next(aHatchStyles.Length - 1)], Color.FromArgb((rd.Next(100, 255)),(rd.Next(100, 255)), (rd.Next(100, 255))), Color.White);
            oGraphics.FillRectangle(oBrush, oRectangleF);

            //Draw text
            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            int i = 0;
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;

                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                //Rotate text Random
                oMatrix.RotateAt(rd.Next(-30, 30), new PointF(x, y));
                oGraphics.Transform = oMatrix;


                //Draw the letters with Random Font Type, Size and Color
                oGraphics.DrawString
                (
                    //Text
                sCaptchaText.Substring(i, 1),
                    //Random Font Name and Style
                new Font(aFontNames[rd.Next(aFontNames.Length - 1)],aFontEmSizes[rd.Next(aFontEmSizes.Length - 1)],aFontStyles[rd.Next(aFontStyles.Length - 1)]),
                    //Random Color (Darker colors RGB 0 to 100)
                new SolidBrush(Color.FromArgb(rd.Next(0, 100),rd.Next(0, 100), rd.Next(0, 100))),
                x,
                rd.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }
            MemoryStream oMemoryStream = new MemoryStream();
            oOutputBitmap.Save(oMemoryStream, System.Drawing.Imaging.ImageFormat.Png);
            byte[] oBytes = oMemoryStream.ToArray();
            return oBytes;
        }
    }
}
