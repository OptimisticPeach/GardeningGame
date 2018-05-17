using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace Perlin_Noise_Gen
{
    public class Program
    {
        public static double Map(double n, double start1, double stop1, double start2, double stop2)
        {
            return ((n - start1) / (stop1 - start1)) * (stop2 - start2) + start2;
        }
        public const double max = 105.063372278828;
        public const double min = 4.42914116324341;
        static void Main(string[] args)
        {
            using (Bitmap b = new Bitmap(10, 10))
            {
                for (int i = 0; i != 10; i++)
                {
                    for (int j = 0; j != 10; j++)
                    {
                        var P = Perlin.OctavePerlin(i / 120.0, j / 120.0, (i + j) / 240.0, 3, 10);
                        P = Map(P, min, max, 0, 255);
                        Color c = Color.FromArgb(255, (int)P, (int)P, (int)P);
                        b.SetPixel(i, j, c);
                    }
                }
                b.Save(@".\green.png", ImageFormat.Png);
            }
        }
    }
}
