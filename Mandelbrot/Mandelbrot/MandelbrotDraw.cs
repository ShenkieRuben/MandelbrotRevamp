using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot
{
    static class MandelbrotDraw
    {
        public static Color MandelColor(int mandelNumber)
        {
            return mandelNumber % 2 == 0 ? Color.Black : Color.White;
        }
             
    }
}
