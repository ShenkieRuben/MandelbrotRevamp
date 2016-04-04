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

        private Label lb = new Label();
        private Bitmap bmp;

        private double scale { get; set; }
        private double[] coordinates { get; set; }
        private static double offsetX, offsetY;
        private static double midX, midY;
        private Task[] drawTasks;

        private readonly static int processorCount = Environment.ProcessorCount;

        public Mandelbrot()
        {
            InitializeComponent();
            bmp = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            scale = 0.01;
            offsetX = bmp.Width / 2;
            offsetY = bmp.Height / 2;
            DrawImage(bmp, scale);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

            Controls.Remove(lb);
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

        private double[] getParallelBounds(int processorCount)
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

        private void DrawImage(Bitmap bmp, double scale)
        {
            double[] fromToInclusive = getParallelBounds(processorCount);
            OptimizedBitmap b = new OptimizedBitmap(bmp);
            b.LockImage();
            drawTasks = new Task[fromToInclusive.Length];

            Parallel.For(0, bmp.Height, new ParallelOptions { MaxDegreeOfParallelism = processorCount }, y =>
           {
               for (int i = 0; i < offsetX * 2; i++)
               {
                   b.SetPixel(i, y, SetColor((i - offsetX) * scale + midX, (y - offsetY) * scale - midY));

               }
           });
            b.UnlockImage();
            pictureBox1.Image = b.workingBitmap;
        }

        private Color SetColor(double x, double y)
        {
            return MandelbrotDraw.MandelColor(new MandelbrotCalc(x, y).MandelNumber);
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            int x = e.Location.X;
            int y = e.Location.Y;

            midX += (x - offsetX) * scale;
            midY += (offsetY - y) * scale;

            if (e.Button == MouseButtons.Left)
            {
                ClickActions(o => o /= 2);
            }
            else if (e.Button == MouseButtons.Right)
            {
                ClickActions(o => o *= 2);
            }

        }

        private void ClickActions(Func<double, double> scalingDelegate)
        {
            scale = scalingDelegate(scale);
            DrawImage(bmp, scale);
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
