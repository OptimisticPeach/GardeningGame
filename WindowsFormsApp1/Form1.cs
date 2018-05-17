using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            backgroundWorker1.RunWorkerAsync();
        }
        Bitmap b = new Bitmap(150, 150);
        double time = 0;
        private void pictureBox1_Paint( object sender, PaintEventArgs e)
        {
            time = DateTime.Now.TimeOfDay.TotalSeconds / 5;
            for (int i = 0; i != 150; i++)
            {
                for (int j = 0; j != 150; j++)
                {
                    var P = Perlin_Noise_Gen.Perlin.OctavePerlin((i / 120.0), (j / 120.0), ((i + j) / 240.0) + time, 3, 10);
                    P = Perlin_Noise_Gen.Program.Map(P, Perlin_Noise_Gen.Program.min, Perlin_Noise_Gen.Program.max, 0, 255);
                    Color c = Color.FromArgb(255, (int)P, (int)P, (int)P);
                    b.SetPixel(i, j, c);
                }
            }
            e.Graphics.DrawImage(b, new Point(0, 0));
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            EventHandler eh = new EventHandler(DoRefresh);
            while (true)
                try
                {
                    Invoke(eh, this, null);
                }
                catch
                {
                    break;
                }
        }
        private void DoRefresh(object sender, EventArgs e)
        {
            pictureBox1.Refresh();
        }
    }
}
