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

        private CircularQueue<QueueElement> ipQueue;

        private Camera cam1;

        private Camera cam2;

        private FailureDetector imagep1;

        private FailureDetector imagep2;

        private Messenger messenger;

        private SaveQueue saveQueue;

        private IPData imageData;

        private Bitmap displayBitmap1;

        private Bitmap displayBitmap2;

        private Rectangle image_roi;

        private Settings program_settings;

        private System.Threading.Timer imageUpdateTimer;

        private delegate void UpdateCameraImageCallback();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guiSem = new Semaphore(0, 1);
            program_settings = new Settings();
            mainQueue = new CircularQueue<QueueElement>("MAIN", 1000);
            ipQueue = new CircularQueue<QueueElement>("IP",1000);
            displayBitmap1 = null;
            image_roi = Rectangle.Empty;
            
            
            

            // initialize camera
            cam1 = new Camera();
            //cam1.AddSubscriber(mainQueue);
            cam1.AddSubscriber(ipQueue);

            // initialize failure detector
            imagep1 = new FailureDetector("Detector1");
            imagep1.SetConsumerQueue(ipQueue);
            imagep1.AddSubscriber(mainQueue);
            
            
            //imagep1.AddSubscriber(mainQueue);
            
            cam1.StartCamera();
            imagep1.Start();

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
                if (imageData != null)
                {
                    imageData.Dispose();
                    imageData.Unlock();
                }
                imageData = (IPData)imageList[imageList.Count - 1].Data;
                guiSem.Release();
                UpdateCameraImage();

                for (int i = 0; i < imageList.Count - 1; i++)
                {
                    ((IPData)imageList[i].Data).Dispose();
                    ((IPData)imageList[i].Data).Unlock();
                }
            }
        }

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

                if (imageData != null)
                {
                    pictureBox1.Image = imageData.GetCameraImage();
                    pictureBox2.Image = imageData.GetProcessedImage();
                    
                    toolStripStatusLabel1.Text = (1 / imageData.GetElapsedTime()).ToString();
                    toolStripStatusLabel2.Text = (1 / imageData.GetProcessTime()).ToString();
                }
                
                guiSem.Release();
            }
        }

        private void nud_noise_lvl_ValueChanged(object sender, EventArgs e)
        {
            imagep1.SetContrast(Convert.ToInt32(nud_noise_lvl.Value));
        }

        private void nud_min_contrast_ValueChanged(object sender, EventArgs e)
        {
            imagep1.SetRange(Convert.ToInt32(nud_min_contrast.Value));
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
