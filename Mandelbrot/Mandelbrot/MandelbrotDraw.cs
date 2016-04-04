using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    static class MandelbrotDraw
    {
        private static double offsetX, offsetY;

        public static Color MandelColor(int mandelNumber)
        {
            return mandelNumber % 2 == 0 ? Color.Black : Color.White;
        }

        public static void DrawImage(Bitmap bmp, double scale, double midX, double midY, ref PictureBox pictureBox)
        {
            offsetX = bmp.Width / 2;
            offsetY = bmp.Height / 2;
            double[] fromToInclusive = MandelbrotCalc.getParallelBounds(Environment.ProcessorCount, bmp);
            OptimizedBitmap b = new OptimizedBitmap(bmp);
            b.LockImage();
            Parallel.For(0, bmp.Height, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, y =>
            {
                for (int i = 0; i < offsetX * 2; i++)
                {
                    b.SetPixel(i, y, SetColor((i - offsetX) * scale + midX, (y - offsetY) * scale - midY));

                }
            });
            b.UnlockImage();
            pictureBox.Image = b.workingBitmap;
        }

        private static Color SetColor(double x, double y)
        {
            int mandelNumber = MandelbrotCalc.Calculate(x, y);
            return MandelbrotDraw.MandelColor(mandelNumber);
        }

    }
}
