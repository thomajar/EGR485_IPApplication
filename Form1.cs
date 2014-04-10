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
using System.Drawing.Imaging;

namespace SAF_OpticalFailureDetector
{
    public partial class Form1 : Form
    {
        private Semaphore guiSem;

        // mainQueue is to hold data intended for mainform
        private CircularQueue<QueueElement> mainQueue;
        private CircularQueue<QueueElement> ipQueue1;
        private CircularQueue<QueueElement> ipQueue2;
        private CircularQueue<QueueElement> saveQueue;

        // camera and processor 
        private Camera cam1;
        private Camera cam2;
        private FailureDetector imagep1;
        private FailureDetector imagep2;
        private IPData camera1Data;
        private IPData camera2Data;
        private Double camera1Period;
        private Double camera2Period;
        private Double process1Period;
        private Double process2Period;

        private Settings program_settings;
        private SaveQueue saveQueueEnginie;

        private System.Threading.Timer imageUpdateTimer;

        private delegate void UpdateCamera1ImageCallback();
        private delegate void UpdateCamera2ImageCallback();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guiSem = new Semaphore(0, 1);
            program_settings = new Settings();
            mainQueue = new CircularQueue<QueueElement>("MAIN", 100);
            ipQueue1 = new CircularQueue<QueueElement>("IP1",100);
            ipQueue2 = new CircularQueue<QueueElement>("IP2",100);
            saveQueue = new CircularQueue<QueueElement>("save_queue", 100);
            

            // initialize camera and processor 1
            cam1 = new Camera();
            cam1.AddSubscriber(ipQueue1);
            imagep1 = new FailureDetector("Detector1");
            imagep1.SetConsumerQueue(ipQueue1);
            imagep1.AddSubscriber(mainQueue);
            imagep1.AddSubscriber(saveQueue);

            // initialize camera and processor 2
            cam2 = new Camera();
            cam2.AddSubscriber(ipQueue2);
            imagep2 = new FailureDetector("Detector2");
            imagep2.SetConsumerQueue(ipQueue2);
            imagep2.AddSubscriber(mainQueue);
            imagep2.AddSubscriber(saveQueue);

            // sets image queue
            saveQueueEnginie = new SaveQueue("save_queue_images", program_settings.LogLocation);
            saveQueueEnginie.SetConsumerQueue(saveQueue);

            // start the cameras
            cam1.StartCamera(0);
            cam2.StartCamera(1);


            // initialize camera and processor periods
            camera1Period = 0.03;
            camera2Period = 0.03;
            process1Period = 0.03;
            process2Period = 0.03;

            guiSem.Release();
            // setup timer update
            TimerCallback tcb = new TimerCallback(DisplayImage);
            imageUpdateTimer = new System.Threading.Timer(tcb, imageUpdateTimer, Timeout.Infinite, Timeout.Infinite);
            imageUpdateTimer.Change(1, 50);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // stop camera and processor threads
            cam1.StopCamera();
            cam2.StopCamera();
            imagep1.Stop();
            imagep2.Stop();
            saveQueueEnginie.Stop();
        }

        private void DisplayImage(object stateInfo)
        {
            List<QueueElement> imageList = new List<QueueElement>();
            if (mainQueue.popAll(ref imageList))
            {
                if (imageList[imageList.Count - 1].Type.Equals(imagep1.GetName()))
                {
                    guiSem.WaitOne();
                    if (camera1Data != null)
                    {
                        camera1Data.Dispose();
                        camera1Data.Unlock();
                    }
                    camera1Data = (IPData)imageList[imageList.Count - 1].Data;
                    guiSem.Release();
                    UpdateCamera1Image();
                }
                else if (imageList[imageList.Count - 1].Type.Equals(imagep2.GetName()))
                {
                    guiSem.WaitOne();
                    if (camera2Data != null)
                    {
                        camera2Data.Dispose();
                        camera2Data.Unlock();
                    }
                    camera2Data = (IPData)imageList[imageList.Count - 1].Data;
                    guiSem.Release();
                    UpdateCamera2Image();
                }

                for (int i = 0; i < imageList.Count - 1; i++)
                {
                    ((IPData)imageList[i].Data).Dispose();
                    ((IPData)imageList[i].Data).Unlock();
                }
            }
        }

        private void UpdateCamera1Image()
        {
            if (Camera1Display.InvokeRequired || Camera1Process.InvokeRequired)
            {
                UpdateCamera1ImageCallback d = new UpdateCamera1ImageCallback(UpdateCamera1Image);
                this.BeginInvoke(d, null);
            }
            else
            {
                guiSem.WaitOne();

                if (camera1Data != null)
                {
                    Bitmap cameraImage = camera1Data.GetCameraImage();
                    //camera1ImageBox.Image = ScaleImage(ref cameraImage, new Point(0, 0), new Size(100, 100), 1);
                    Camera1Display.SetImage(camera1Data.GetCameraImage());
                    Camera1Process.SetImage(camera1Data.GetProcessedImage());

                    camera1Period = 0.85 * camera1Period + 0.15 * camera1Data.GetElapsedTime();
                    process1Period = 0.85 * process1Period + 0.15 * camera1Data.GetProcessTime();

                    Camera1Display.SetText(String.Format("{0:0.00}",(1 / camera1Period)));
                    Camera1Process.SetText(String.Format("{0:0.00}", (1 / process1Period)));
                }
                
                guiSem.Release();
            }
        }

        private void UpdateCamera2Image()
        {
            if (Camera2Display.InvokeRequired || Camera2Process.InvokeRequired)
            {
                UpdateCamera2ImageCallback d = new UpdateCamera2ImageCallback(UpdateCamera2Image);
                this.BeginInvoke(d, null);
            }
            else
            {
                guiSem.WaitOne();

                if (camera2Data != null)
                {
                    Camera2Display.SetImage(camera2Data.GetCameraImage());
                    Camera2Process.SetImage(camera2Data.GetProcessedImage());

                    camera2Period = 0.85 * camera2Period + 0.15 * camera2Data.GetElapsedTime();
                    process2Period = 0.85 * process2Period + 0.15 * camera2Data.GetProcessTime();

                    Camera2Display.SetText(String.Format("{0:0.00}", (1 / camera2Period)));
                    Camera2Process.SetText(String.Format("{0:0.00}", (1 / process2Period)));
                }

                guiSem.Release();
            }
        }

        /// <summary>
        /// Scales an image and returns a copy of the image
        /// </summary>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <param name="zoomlvl"></param>
        /// <returns></returns>
        private Bitmap ScaleImage(ref Bitmap b, Point p, Size s, int zoomlvl)
        {
            Bitmap scaledImage;

            scaledImage = new Bitmap(s.Width, s.Height);

            // apply gain to the image

            float displayGain = 1.0f;

            float[][] matrix = {
                    new float[] {displayGain, 0, 0, 0, 0},        // red scaling factor of 2
                    new float[] {0, displayGain, 0, 0, 0},        // green scaling factor of 1
                    new float[] {0, 0, displayGain, 0, 0},        // blue scaling factor of 1
                    new float[] {0, 0, 0, displayGain, 0},        // alpha scaling factor of 1
                    new float[] {0, 0, 0, 0, 1}};    // three translations of 0.2;

            ColorMatrix colorMatrix = new ColorMatrix(matrix);

            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Graphics g_scaled = Graphics.FromImage(scaledImage);
            g_scaled.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            try
            {
                g_scaled.DrawImage(b, new Rectangle(0, 0, scaledImage.Width, scaledImage.Height),
                    0, 0, b.Width, b.Height, GraphicsUnit.Pixel, imageAttr);
            }
            catch (Exception)
            {
                
                throw;
            }

            g_scaled.Dispose();
            return scaledImage;

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
            MessageBox.Show("This feature is not yet implemented.", "Settings");
            /*if (program_settings.ShowDialog() == DialogResult.OK)
            {
                tsbtn_Start.Enabled = true;
            }*/
        }

        private void tsbtn_Start_Click(object sender, EventArgs e)
        {
            Start();
            tsbtn_Stop.Enabled = true;
            tsbtn_Start.Enabled = false;
            //tsbtn_Settings.Enabled = false;
        }

        private void tsbtn_Stop_Click(object sender, EventArgs e)
        {
            Stop();
            tsbtn_Stop.Enabled = false;
            tsbtn_Start.Enabled = true;
            //tsbtn_Settings.Enabled = true;
        }

        private void tsbtn_Help_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented.", "Help");
        }

        private void Start()
        {
            imagep1.Start();
            imagep2.Start();
            saveQueueEnginie.Start();

            //messenger = new Messenger(program_settings.EmailAddress,
            //    program_settings.TestNumber, program_settings.SampleNumber);
        }

        private void Stop()
        {
            imagep1.Stop();
            imagep2.Stop();
            saveQueueEnginie.Stop();
        }

        private void tsbtn_RefreshCamera_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented.", "Refresh Camera");
        }

        

        
    }
}
