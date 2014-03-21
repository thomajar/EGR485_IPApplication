using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAF_OpticalFailureDetector.threading;
using SAF_OpticalFailureDetector.camera;
using SAF_OpticalFailureDetector.imageprocessing;
using SAF_OpticalFailureDetector.messenger;
using SAF_OpticalFailureDetector.savequeue;
using System.Threading;

namespace SAF_OpticalFailureDetector
{
    public partial class Form1 : Form
    {
        private Semaphore guiSem;

        // mainQueue is to hold data intended for mainform
        private CircularQueue<QueueElement> mainQueue;

        private Camera cam1;

        private Camera cam2;

        private FailureDetector imagep1;

        private FailureDetector imagep2;

        private Messenger messenger;

        private SaveQueue saveQueue;

        private Bitmap displayBitmap;

        private Rectangle image_roi;

        private Settings program_settings;

        private System.Threading.Timer imageUpdateTimer;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guiSem = new Semaphore(0, 1);
            program_settings = new Settings();
            mainQueue = new CircularQueue<QueueElement>("MAIN", 1000);
            displayBitmap = null;
            image_roi = Rectangle.Empty;
            
            
            

            // initialize camera
            cam1 = new Camera();
            cam1.AddSubscriber(mainQueue);

            // initialize failure detector
            imagep1 = new FailureDetector("Detector1");
            cam1.AddSubscriber(imagep1.GetConsumerQueue());
            
            //cam1.StartCamera();

            //Thread.Sleep(50);


            guiSem.Release();
            // setup timer update
            TimerCallback tcb = new TimerCallback(DisplayImage);
            imageUpdateTimer = new System.Threading.Timer(tcb, imageUpdateTimer, Timeout.Infinite, Timeout.Infinite);
            imageUpdateTimer.Change(1, 50);
            
        }

        private void DisplayImage(object stateInfo)
        {
            List<QueueElement> imageList = new List<QueueElement>();
            if (mainQueue.popAll(ref imageList))
            {
                guiSem.WaitOne();
                displayBitmap = (Bitmap)imageList[imageList.Count - 1].Data;
                guiSem.Release();
                UpdateCameraImage();
            }
        }

        private delegate void UpdateCameraImageCallback();

        private void UpdateCameraImage()
        {
            if (this.pictureBox1.InvokeRequired || InvokeRequired || statusStrip1.InvokeRequired)
            {
                UpdateCameraImageCallback d = new UpdateCameraImageCallback(UpdateCameraImage);
                this.BeginInvoke(d, null);
            }
            else
            {
                guiSem.WaitOne();
                if (this.pictureBox1.Image != null)
                {
                    this.pictureBox1.Image.Dispose();
                }
                this.pictureBox1.Image = displayBitmap;
                guiSem.Release();
            }
        }

        private void btn_LoadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Bitmap (*.bmp)|*.bmp";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                displayBitmap = new Bitmap(ofd.FileName);
                pictureBox1.Image = displayBitmap;
                image_roi = IP.ROI(displayBitmap);
                ProcessImage();
            }
            
        }

        private void ProcessImage()
        {
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
            Bitmap b2 = (Bitmap)displayBitmap.Clone();
            IP.readImg(b2,image_roi,Convert.ToInt32(nud_noise_lvl.Value),
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

        private void tsbtn_Settings_Click(object sender, EventArgs e)
        {
            if (program_settings.ShowDialog() == DialogResult.OK)
            {
                tsbtn_Start.Enabled = true;
            }
        }

        private void tsbtn_Start_Click(object sender, EventArgs e)
        {
            Start();
            tsbtn_Stop.Enabled = true;
            tsbtn_Start.Enabled = false;
            tsbtn_Settings.Enabled = false;
        }

        private void tsbtn_Stop_Click(object sender, EventArgs e)
        {
            Stop();
            tsbtn_Stop.Enabled = false;
            tsbtn_Start.Enabled = false;
            tsbtn_Settings.Enabled = true;
        }

        private void tsbtn_Help_Click(object sender, EventArgs e)
        {

        }

        private void Start()
        {
            messenger = new Messenger(program_settings.EmailAddress,
                program_settings.TestNumber, program_settings.SampleNumber);
        }

        private void Stop()
        {

        }

        
    }
}
