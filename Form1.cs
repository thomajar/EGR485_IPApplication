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
using log4net;

namespace SAF_OpticalFailureDetector
{
    public partial class MainForm : Form
    {
        private const string PROGRAM_NAME = "Optical Failure Detector (V1.0)";

        private static readonly ILog log = LogManager.GetLogger(typeof(MainForm));

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
        private ImageHistoryBuffer saveQueueEngine;

        private System.Threading.Timer imageUpdateTimer;
        private System.Threading.Timer garbageCollector;

        private delegate void UpdateCamera1ImageCallback();
        private delegate void UpdateCamera2ImageCallback();

        public MainForm()
        {
            log4net.Config.XmlConfigurator.Configure();
            log.Info("MainForm.MainForm : Application opened as " + PROGRAM_NAME);

            InitializeComponent();
            this.Text = PROGRAM_NAME;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            guiSem = new Semaphore(0, 1);
            program_settings = new Settings();
            // see how much memory is available on computer and take 80% of it
            float freeMem = 1000;
            try
            {
                System.Diagnostics.PerformanceCounter ramCounter = new System.Diagnostics.PerformanceCounter("Memory", "Available MBytes");
                freeMem = ramCounter.NextValue();
            }
            catch (Exception inner)
            {
                log.Error("MainForm.Form1_Load : Unable to retrieve amount of free memory on system, defaulting to 1GB.",inner);
            }
            // each queue takes 3 MB per item in it
            int queueSize = Convert.ToInt32(freeMem * 0.80) / 12;

            mainQueue = new CircularQueue<QueueElement>("MAIN", queueSize);
            ipQueue1 = new CircularQueue<QueueElement>("IP1", queueSize);
            ipQueue2 = new CircularQueue<QueueElement>("IP2", queueSize);
            saveQueue = new CircularQueue<QueueElement>("save_queue", queueSize);

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
            saveQueueEngine = new ImageHistoryBuffer("save_queue_images", program_settings.LogLocation);
            saveQueueEngine.SetConsumerQueue(saveQueue);

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

            // setup garbage collector
            TimerCallback tcb2 = new TimerCallback(GarbageCollector);
            garbageCollector = new System.Threading.Timer(tcb2, garbageCollector, Timeout.Infinite, Timeout.Infinite);
            garbageCollector.Change(1, 100);
            
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Info("MainForm.Form1_FormClosing : Application is closing, shutting down threads.");

            // stop cameras
            cam1.StopCamera();
            cam2.StopCamera();
            // stop image processors
            imagep1.Stop();
            imagep2.Stop();
            // stop saveQueue Engine
            saveQueueEngine.Stop();

            log.Info("MainForm.Form1_FormClosing : All threads shutdown, program terminating.");
        }

        private void GarbageCollector(object stateInfo)
        {
            GC.Collect();
        }

        /// <summary>
        /// Function is called by timer and checks to see what data is available in mainQueue.
        /// </summary>
        /// <param name="stateInfo"></param>
        private void DisplayImage(object stateInfo)
        {
            // make place to store image data and populate
            List<QueueElement> imageList = new List<QueueElement>();
            if (mainQueue.popAll(ref imageList))
            {
                // check if last element is from image processor 1
                if (imageList[imageList.Count - 1].Type.Equals(imagep1.GetName()))
                {
                    // obtain ownership of gui to enter critical section
                    guiSem.WaitOne();
                    // update camera 1 data to newest data off of imageList
                    camera1Data = (IPData)imageList[imageList.Count - 1].Data;
                    // release ownership of gui to exit critical section
                    guiSem.Release();
                    // draw the updated image data for camera 1
                    UpdateCamera1Image();
                }
                // check if last element is from image processor 2
                else if (imageList[imageList.Count - 1].Type.Equals(imagep2.GetName()))
                {
                    // obtain ownership of gui to enter critical section
                    guiSem.WaitOne();
                    // update camera 2 data to newest data off of imagelist
                    camera2Data = (IPData)imageList[imageList.Count - 1].Data;
                    // release ownership of gui to exit critical section
                    guiSem.Release();
                    // draw the updated image data for camera 2
                    UpdateCamera2Image();
                }

                // dispose all unused imageList data that we were not able to draw to GUI
                for (int i = 0; i < imageList.Count - 1; i++)
                {
                    //((IPData)imageList[i].Data).Dispose();
                    //((IPData)imageList[i].Data).Unlock();
                }
            }
        }

        /// <summary>
        /// Function should be called whenever the data from camera 1 changes.
        /// </summary>
        private void UpdateCamera1Image()
        {
            if (Camera1Display.InvokeRequired || Camera1Process.InvokeRequired)
            {
                UpdateCamera1ImageCallback d = new UpdateCamera1ImageCallback(UpdateCamera1Image);
                this.BeginInvoke(d, null);
            }
            else
            {
                // obtain ownership of gui control to enter critical section
                guiSem.WaitOne();
                // verify camera 1 data is not null
                if (camera1Data != null)
                {
                    try
                    {
                        Camera1Display.SetImage(camera1Data.GetRawDataImage());
                    }
                    catch (Exception inner)
                    {
                        log.Error("MainForm.UpdateCamera1Image : Unable to set camera 1 display image.", inner);
                    }

                    try
                    {
                        Camera1Process.SetImage(camera1Data.GetProcessedDataImage());
                    }
                    catch (Exception inner)
                    {
                        log.Error("MainForm.UpdateCamera1Image : Unable to set camera 1 processed image.", inner);
                    }
                    
                    // update the camera and process frame rates
                    camera1Period = 0.85 * camera1Period + 0.15 * camera1Data.CameraElapsedTime_s;
                    Camera1Display.SetText(String.Format("{0:0.00}", (1 / camera1Period)));
                    process1Period = 0.85 * process1Period + 0.15 * camera1Data.ProcessorElapsedTime_s;
                    Camera1Process.SetText(String.Format("{0:0.00}", (1 / process1Period)));
                }
                // release ownership of critical section
                guiSem.Release();
            }
        }

        /// <summary>
        /// Function should be called whenever the data from camera 2 changes.
        /// </summary>
        private void UpdateCamera2Image()
        {
            if (Camera2Display.InvokeRequired || Camera2Process.InvokeRequired)
            {
                UpdateCamera2ImageCallback d = new UpdateCamera2ImageCallback(UpdateCamera2Image);
                this.BeginInvoke(d, null);
            }
            else
            {
                // obtain ownership of gui control to enter critical section
                guiSem.WaitOne();
                // verify camera2 data is not null
                if (camera2Data != null)
                {
                    try
                    {
                        Camera2Display.SetImage(camera2Data.GetRawDataImage());
                    }
                    catch (Exception inner)
                    {
                        log.Error("MainForm.UpdateCamera1Image : Unable to set camera 2 display image.", inner);
                    }

                    try
                    {
                        Camera2Process.SetImage(camera2Data.GetProcessedDataImage());
                    }
                    catch (Exception inner)
                    {
                        log.Error("MainForm.UpdateCamera1Image : Unable to set camera 2 processed image.", inner);
                    }

                    // update the camera and process frame rates
                    camera2Period = 0.85 * camera2Period + 0.15 * camera2Data.CameraElapsedTime_s;
                    Camera2Display.SetText(String.Format("{0:0.00}", (1 / camera2Period)));
                    process2Period = 0.85 * process2Period + 0.15 * camera2Data.ProcessorElapsedTime_s;
                    Camera2Process.SetText(String.Format("{0:0.00}", (1 / process2Period)));
                }
                // release ownership of critical section
                guiSem.Release();
            }
        }

        /// <summary>
        /// Method is called whenever user presses the settings button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtn_Settings_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.tsbtn_Settings_Click : User pressed settings button.");
            MessageBox.Show("This feature is not yet implemented.", "Settings");
            /*if (program_settings.ShowDialog() == DialogResult.OK)
            {
                tsbtn_Start.Enabled = true;
            }*/
        }

        /// <summary>
        /// Method is called whenever user presses the start processing button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtn_Start_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.tsbtn_Start_Click : User pressed start processing button.");
            Start();
            tsbtn_Stop.Enabled = true;
            tsbtn_Start.Enabled = false;
            //tsbtn_Settings.Enabled = false;
        }

        /// <summary>
        /// Method is called whenever user presses the stop processing button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtn_Stop_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.tsbtn_Stop_Click : User pressed stop processing button.");
            Stop();
            tsbtn_Stop.Enabled = false;
            tsbtn_Start.Enabled = true;
            //tsbtn_Settings.Enabled = true;
        }

        /// <summary>
        /// Method is called whenever user presses the help button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtn_Help_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.tsbtn_Help_Click : User pressed help button.");
            MessageBox.Show("This feature is not yet implemented.", "Help");
        }

        /// <summary>
        /// Method is called whenever user presses the refresh cameras button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbtn_RefreshCamera_Click(object sender, EventArgs e)
        {
            log.Info("MainForm.tsbtn_RefreshCamera_Click : User pressed refresh cameras button.");
            MessageBox.Show("This feature is not yet implemented.", "Refresh Camera");
        }

        private void Start()
        {
            log.Info("MainForm.Start : Starting image processors and save queue engine.");
            imagep1.Start();
            imagep2.Start();
            saveQueueEngine.Start();

            //messenger = new Messenger(program_settings.EmailAddress,
            //    program_settings.TestNumber, program_settings.SampleNumber);
        }

        private void Stop()
        {
            log.Info("MainForm.Stop : Stopping image processors and save queue engine.");
            imagep1.Stop();
            imagep2.Stop();
            saveQueueEngine.Stop();
        }
 
    }
    // Use for exceptinos generated in FailureDetector class
    public class MainFormException : System.Exception
    {
        public MainFormException() : base() { }
        public MainFormException(string message) : base(message) { }
        public MainFormException(string message, System.Exception inner) : base(message, inner) { }
        protected MainFormException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
