using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    static class MandelbrotCalc
    {
        public static int Calculate(double startX, double startY)
        {
            double a = startX;
            double tempA, tempB;
            double b = startY;
            int mandelNumber;
            int i;
            for (i = 0; pythagorasLength(a, b) <= 2 && i < 100; i++)
            {
                tempA = a * a - b * b + startX;
                tempB = 2 * a * b + startY;
                a = tempA;
                b = tempB;
            }
            return mandelNumber = i;
        }
        private static double pythagorasLength(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }

        public static double[] getParallelBounds(int processorCount, Bitmap bmp)
        {
            double equalParts = (double)bmp.Width / processorCount;
            double partition = Math.Ceiling(equalParts);
            int bound = (int)Math.Ceiling(bmp.Width / equalParts);
            double[] fromToInclusive = new double[bound];
            for (int i = 0; i < bound; i++)
            {
                fromToInclusive[i] = partition;
                partition += equalParts;
            }
            return fromToInclusive;
        }
    }
}
