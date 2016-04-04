using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    class MandelbrotCalc
    {
        public int MandelNumber { get; }
        public MandelbrotCalc(double startX, double startY)
        {
            double a = startX;
            double tempA, tempB;
            double b = startY;
            int i;
            for (i = 0; pythagorasLength(a, b) <= 2 && i < 100; i++)
            {
                tempA = a * a - b * b + startX;
                tempB = 2 * a * b + startY;
                a = tempA;
                b = tempB;
            }
            MandelNumber = i;
        }

        private double pythagorasLength(double a, double b)
        {
            return Math.Sqrt(a * a + b * b);
        }

    }
}
