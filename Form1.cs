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
        private CircularQueue<QueueElement> ipQueue1;
        private CircularQueue<QueueElement> ipQueue2;

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
            
            // initialize form
            camera1Label.Parent = camera1ImageBox;
            camera1Label.ForeColor = Color.Red;
            camera1Label.BackColor = Color.Transparent;

            process1Label.Parent = process1ImageBox;
            process1Label.ForeColor = Color.Red;
            process1Label.BackColor = Color.Transparent;

            camera2Label.Parent = camera2ImageBox;
            camera2Label.ForeColor = Color.Red;
            camera2Label.BackColor = Color.Transparent;

            process2Label.Parent = process2ImageBox;
            process2Label.ForeColor = Color.Red;
            process2Label.BackColor = Color.Transparent;
            

            // initialize camera and processor 1
            cam1 = new Camera();
            cam1.AddSubscriber(ipQueue1);
            imagep1 = new FailureDetector("Detector1");
            imagep1.SetConsumerQueue(ipQueue1);
            imagep1.AddSubscriber(mainQueue);

            // initialize camera and processor 2
            cam2 = new Camera();
            cam2.AddSubscriber(ipQueue2);
            imagep2 = new FailureDetector("Detector2");
            imagep2.SetConsumerQueue(ipQueue2);
            imagep2.AddSubscriber(mainQueue);

            cam1.StartCamera();
            imagep1.Start();

            // initialize camera and processor periods
            camera1Period = 0.05;
            camera2Period = 0.05;
            process1Period = 0.05;
            process2Period = 0.05;

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
            if (this.camera1ImageBox.InvokeRequired || this.camera1Label.InvokeRequired ||
                this.process1ImageBox.InvokeRequired || this.process1Label.InvokeRequired)
            {
                UpdateCamera1ImageCallback d = new UpdateCamera1ImageCallback(UpdateCamera1Image);
                this.BeginInvoke(d, null);
            }
            else
            {
                guiSem.WaitOne();

                if (camera1Data != null)
                {
                    camera1ImageBox.Image = camera1Data.GetCameraImage();
                    process1ImageBox.Image = camera1Data.GetProcessedImage();

                    camera1Period = 0.85 * camera1Period + 0.15 * camera1Data.GetElapsedTime();
                    process1Period = 0.85 * process1Period + 0.15 * camera1Data.GetProcessTime();

                    camera1Label.Text = String.Format("{0:0.00}",(1 / camera1Period));
                    process1Label.Text = String.Format("{0:0.00}", (1 / process1Period));
                }
                
                guiSem.Release();
            }
        }

        private void UpdateCamera2Image()
        {
            if (this.camera2ImageBox.InvokeRequired || this.camera2Label.InvokeRequired ||
                this.process2ImageBox.InvokeRequired || this.process2Label.InvokeRequired)
            {
                UpdateCamera2ImageCallback d = new UpdateCamera2ImageCallback(UpdateCamera2Image);
                this.BeginInvoke(d, null);
            }
            else
            {
                guiSem.WaitOne();

                if (camera2Data != null)
                {
                    camera2ImageBox.Image = camera2Data.GetCameraImage();
                    process2ImageBox.Image = camera2Data.GetProcessedImage();

                    camera2Period = 0.85 * camera2Period + 0.15 * camera2Data.GetElapsedTime();
                    process2Period = 0.85 * process2Period + 0.15 * camera2Data.GetProcessTime();

                    camera2Label.Text = String.Format("{0:0.00}", (1 / camera2Period));
                    process2Label.Text = String.Format("{0:0.00}", (1 / process2Period));
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
            MessageBox.Show("This feature is not yet implemented.", "Help");
        }

        private void Start()
        {
            //messenger = new Messenger(program_settings.EmailAddress,
            //    program_settings.TestNumber, program_settings.SampleNumber);
        }

        private void Stop()
        {

        }

        private void tsbtn_RefreshCamera_Click(object sender, EventArgs e)
        {
            MessageBox.Show("This feature is not yet implemented.", "Refresh Camera");
        }

        

        
    }
}
