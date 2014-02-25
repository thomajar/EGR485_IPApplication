using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAF_OpticalFailureDetector
{
    public partial class Form1 : Form
    {
        Bitmap b;
        Rectangle r;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            b = null;
            r = Rectangle.Empty;
        }

        private void btn_LoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap (*.bmp)|*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                b = new Bitmap(ofd.FileName);
                pictureBox1.Image = b;
                r = IP.ROI(b);
                ProcessImage();
            }
            
        }

        private void ProcessImage()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Bitmap b2 = (Bitmap)b.Clone();
            IP.readImg(b2,r,Convert.ToInt32(nud_noise_lvl.Value),
                Convert.ToInt32(nud_min_contrast.Value));
            pictureBox2.Image = b2;
            tslbl_Status.Text = "Elapsed Time: " + sw.ElapsedMilliseconds + " ms"; 
        }

        private void nud_noise_lvl_ValueChanged(object sender, EventArgs e)
        {
            ProcessImage();
        }

        private void nud_min_contrast_ValueChanged(object sender, EventArgs e)
        {
            ProcessImage();
        }

        
    }
}
