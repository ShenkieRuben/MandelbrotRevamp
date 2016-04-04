using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mandelbrot
{
    public partial class Mandelbrot : Form
    {
        public string Coordinates { get; set; }

        private Bitmap bmp;

        private double scale { get; set; }
        private double[] coordinates { get; set; }
        private static double midX, midY;

        private readonly static int processorCount = Environment.ProcessorCount;

        public Mandelbrot()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            scale = 0.01;
            MandelbrotDraw.DrawImage(bmp, scale, 0, 0, ref pictureBox1);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = sender as TextBox;
            tb.BackColor = Color.White;
            tb.Text = Regex.Replace(tb.Text, @"\s+", "");
            Regex regex = new Regex(@"^\(\d{1,10}\,?\d*;\d{1,10}\,?\d*\)$", RegexOptions.IgnorePatternWhitespace);
            if (regex.IsMatch(tb.Text))
            {
                Coordinates = tb.Text;
                tb.BackColor = Color.LightGreen;
                coordinates = Coordinates.Split(new char[] { ';', '(', ')' }, StringSplitOptions.RemoveEmptyEntries).Select(double.Parse).ToArray();
                midX = coordinates[0];
                midY = coordinates[1];
            }
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;

            midX += (x - bmp.Width / 2) * scale;
            midY += (bmp.Height / 2 - y) * scale;

            if (e.Button == MouseButtons.Left)
            {
                ClickActions(o => o /= 2, midX, midY);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ClickActions(o => o *= 2, midX, midY);
            }

        }

        private void ClickActions(Func<double, double> scalingDelegate, double midX, double midY)
        {
            scale = scalingDelegate(scale);
            MandelbrotDraw.DrawImage(bmp, scale, midX, midY, ref pictureBox1);
            scaleTB.Text = scale.ToString();
            coordTB.Text = $"(" + Math.Round(midX, 6) + ";" + Math.Round(midY, 6) + ")";
        }

        private void scaleTB_TextChanged(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            tb.Text = scale.ToString();
        }
    }
}
