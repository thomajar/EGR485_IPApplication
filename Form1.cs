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
using SAF_OpticalFailureDetector.relay;

namespace SAF_OpticalFailureDetector
{
    public partial class MainForm : Form
    {
        private const string PROGRAM_NAME = "Optical Failure Detector (V1.0)";
        private const String DISPLAY_TYPE_NORMAL = "Camera Image";
        private const String DISPLAY_TYPE_PROCESSED = "Raw Processed";

        private const string DEFAULT_CAM1_NAME = "cam1";
        private const string DEFAULT_CAM2_NAME = "cam2";
        private const string DEFAULT_IMAGE_PROCESSOR1_NAME = "camproc1";
        private const string DEFAULT_IMAGE_PROCESSOR2_NAME = "camproc2";

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
        private ImageHistoryBuffer saveEngine;

        private string Cam1DisplayType;
        private string Cam2DisplayType;

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
                log.Error("MainForm.Form1_Load : Unable to retrieve amount of free memory on system, defaulting to 1GB.", inner);
            }
            if (freeMem > 2100)
            {
                freeMem = 2100;
            }

            // each queue takes 6 MB per item in it
            int queueSize = 50;//Convert.ToInt32(freeMem * 0.90) / 32;
            mainQueue = new CircularQueue<QueueElement>("MAIN", queueSize);
            ipQueue1 = new CircularQueue<QueueElement>("IP1", queueSize);
            ipQueue2 = new CircularQueue<QueueElement>("IP2", queueSize);
            saveQueue = new CircularQueue<QueueElement>("save_queue", queueSize);

            // initialize camera and processor 1
            cam1 = new Camera();
            cam1.AddSubscriber(ipQueue1);
            cam1.AddSubscriber(mainQueue);
            imagep1 = new FailureDetector(DEFAULT_IMAGE_PROCESSOR1_NAME, ref cam1);
            imagep1.SetConsumerQueue(ipQueue1);
            imagep1.AddSubscriber(saveQueue);
            imagep1.AddSubscriber(mainQueue);
            imagep1.EnableAutoExposure(true);
            imagep1.EnableAutoROI(true);

            // initialize camera and processor 2
            cam2 = new Camera();
            cam2.AddSubscriber(ipQueue2);
            cam2.AddSubscriber(mainQueue);
            imagep2 = new FailureDetector(DEFAULT_IMAGE_PROCESSOR2_NAME, ref cam2);
            imagep2.SetConsumerQueue(ipQueue2);
            imagep2.AddSubscriber(saveQueue);
            imagep2.AddSubscriber(mainQueue);
            imagep2.EnableAutoExposure(true);
            imagep2.EnableAutoROI(true);

            // sets image queue
            saveEngine = new ImageHistoryBuffer();
            saveEngine.SetConsumerQueue(saveQueue);

            // add thread error handlers
            cam1.ThreadError += new ThreadErrorHandler(Camera0ThreadError);
            cam2.ThreadError += new ThreadErrorHandler(Camera1ThreadError);
            imagep1.ThreadError += new ThreadErrorHandler(ImageProcessor0ThreadError);
            imagep2.ThreadError += new ThreadErrorHandler(ImageProcessor0ThreadError);
            saveEngine.ThreadError += new ThreadErrorHandler(SaveQueueThreadError);

            // start the cameras
            RefreshCameras();


            // initialize camera and processor periods
            camera1Period = 0.03;
            camera2Period = 0.03;
            process1Period = 0.03;
            process2Period = 0.03;

            // need to update comboboxes
            cmboCam1View.Items.Add(DISPLAY_TYPE_NORMAL);
            cmboCam1View.Items.Add(DISPLAY_TYPE_PROCESSED);
            cmboCam1View.SelectedIndex = 0;

            cmboCam2View.Items.Add(DISPLAY_TYPE_NORMAL);
            cmboCam2View.Items.Add(DISPLAY_TYPE_PROCESSED);
            cmboCam2View.SelectedIndex = 0;

            guiSem.Release();
            // setup timer update
            TimerCallback tcb = new TimerCallback(DisplayImage);
            imageUpdateTimer = new System.Threading.Timer(tcb, imageUpdateTimer, Timeout.Infinite, Timeout.Infinite);
            imageUpdateTimer.Change(1, 200);

            // setup garbage collector
            TimerCallback tcb2 = new TimerCallback(GarbageCollector);
            garbageCollector = new System.Threading.Timer(tcb2, garbageCollector, Timeout.Infinite, Timeout.Infinite);
            garbageCollector.Change(1, 100);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            log.Info("MainForm.Form1_FormClosing : Application is closing, shutting down threads.");

            // stop cameras
            if (cam1.Running)
            {
                try
                {
                    cam1.StopCamera();
                }
                catch (Exception inner)
                {
                    string errMsg = "MainForm.Form1_FormClosing : Unable to stop camera 1.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                } 
            }
            if (cam2.Running)
            {
                try
                {
                    cam2.StopCamera();
                }
                catch (Exception inner)
                {
                    string errMsg = "MainForm.Form1_FormClosing : Unable to stop camera 2.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                } 
            }
            // stop image processors
            if (imagep1.Running)
            {
                try
                {
                    imagep1.Stop();
                }
                catch (Exception inner)
                {
                    string errMsg = "MainForm.Form1_FormClosing : Unable to stop image processor 1.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                }
            }
            if (imagep2.Running)
            {
                try
                {
                    imagep2.Stop();
                }
                catch (Exception inner)
                {
                    string errMsg = "MainForm.Form1_FormClosing : Unable to stop image processor 2.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                }
            }
            // stop saveQueue Engine
            if (saveEngine.Running)
            {
                try
                {
                    saveEngine.Stop();
                }
                catch (Exception inner)
                {
                    string errMsg = "MainForm.Form1_FormClosing : Unable to stop save engine.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                }
            }

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
	            /*
                 * Store all of the inter-thread communication data types into 
                 * a list of queue elements.  The list will consist of all inter-thread
                 * messages that have been sent since the last time this function was called.
                 */
                List<QueueElement> dataList = new List<QueueElement>();
                if (mainQueue.popAll(ref dataList))
                {
                    int cam1Index = -1;
                    int cam2Index = -1;
                    int proc1Index = -1;
                    int proc2Index = -1;
                    for (int i = dataList.Count - 1; i > -1 && ( cam1Index == -1 || cam2Index == -1 || proc1Index == -1 || proc2Index == -1); i--)
                    {
                        if (cam1Index == -1)
                        {
                            if (dataList[i].Type.Contains(DEFAULT_CAM1_NAME))
                            {
                                cam1Index = i;
                            }
                        }
                        if (cam2Index == -1)
                        {
                            if (dataList[i].Type.Contains(DEFAULT_CAM2_NAME))
                            {
                                cam2Index = i;
                            }
                        }
                        if (proc1Index == -1)
                        {
                            if (dataList[i].Type.Contains(DEFAULT_IMAGE_PROCESSOR1_NAME))
                            {
                                proc1Index = i;
                            }
                        }
                        if (proc2Index == -1)
                        {
                            if (dataList[i].Type.Contains(DEFAULT_IMAGE_PROCESSOR2_NAME))
                            {
                                proc2Index = i;
                            }
                        }
                    
                    
                    }

                    // obtain ownership of GUI thread
                    guiSem.WaitOne();
                    if (cam1Index > -1)
                    {
                        // update camera 1 data to newest data off of imageList
                        camera1Data = (IPData)dataList[cam1Index].Data;
                        UpdateCamera1Image();
                    }
                    if (cam2Index > -1)
                    {
                        // update camera 2 data to newest data off of imagelist
                        camera2Data = (IPData)dataList[cam2Index].Data;
                        UpdateCamera2Image();
                    }

                    switch (Cam1DisplayType)
                    {
                        case DISPLAY_TYPE_NORMAL:
                            if (cam1Index > -1)
                            {
                                // update camera 1 data to newest data off of imageList
                                camera1Data = (IPData)dataList[cam1Index].Data;
                                UpdateCamera1Image();
                            }
                            break;
                        case DISPLAY_TYPE_PROCESSED:
                            if (proc1Index > -1)
                            {
                                // update camera 1 data to newest data off of imageList
                                camera1Data = (IPData)dataList[proc1Index].Data;
                                UpdateCamera1Image();
                            }
                            break;
                        default:
                            break;
                    }

                    switch (Cam2DisplayType)
                    {
                        case DISPLAY_TYPE_NORMAL:
                            if (cam2Index > -1)
                            {
                                // update camera 2 data to newest data off of imagelist
                                camera2Data = (IPData)dataList[cam2Index].Data;
                                UpdateCamera2Image();
                            }
                            break;
                        case DISPLAY_TYPE_PROCESSED:
                            if (proc2Index > -1)
                            {
                                // update camera 2 data to newest data off of imagelist
                                camera2Data = (IPData)dataList[proc2Index].Data;
                                UpdateCamera2Image();
                            }
                            break;
                        default:
                            break;
                    }
                    guiSem.Release();
                } 
        }

        /// <summary>
        /// Function should be called whenever the data from camera 1 changes.
        /// </summary>
        private void UpdateCamera1Image()
        {
            if (Camera1Display.InvokeRequired || gbCamera1.InvokeRequired)
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
                    switch (Cam1DisplayType)
                    {
                        case DISPLAY_TYPE_NORMAL:
                            try
                            {
                                if (!camera1Data.IsProcessed)
                                {
                                    Camera1Display.SetImage(camera1Data.GetRawDataImage());
                                }
                            }
                            catch (Exception inner)
                            {
                                log.Error("MainForm.UpdateCamera1Image : Unable to set camera 1 display image.", inner);
                            }
                            break;
                        case DISPLAY_TYPE_PROCESSED:
                            try
                            {
                                if (camera1Data.IsProcessed)
                                {
                                    Camera1Display.SetImage(camera1Data.GetProcessedDataImage());
                                }
                            }
                            catch (Exception inner)
                            {
                                log.Error("MainForm.UpdateCamera1Image : Unable to set camera 1 display image.", inner);
                            }
                            break;
                        default:
                            break;
                    }
                    
                    
                    // update camera 1 information
                    camera1Period = 0.85 * camera1Period + 0.15 * camera1Data.CameraElapsedTime_s;
                    lblCam1FPS.Text = "Frames Per Second: " + String.Format("{0:0.00}", (1 / camera1Period));
                    if (camera1Data.ImageProcessorElapsedTime_s != 0)
                    {
                        process1Period = 0.95 * process1Period + 0.05 * camera1Data.ImageProcessorElapsedTime_s;
                        lblCam1IPFPS.Text = "Image Processor FPS: " + String.Format("{0:0.00}", (1 / process1Period));
                    }
                    lblCam1Exposure.Text = "Exposure: " + String.Format("{0:0.00}", camera1Data.ImageExposure_s * 1000);
                    if (camera1Data.IsProcessed)
                    {
                        lblCam1Intensity.Text = "Intensity: " + camera1Data.ImageIntensity_lsb.ToString();
                        lblCam1CracksDetected.Text = "Crack Detected: " + camera1Data.ContainsCrack.ToString();
                        lblCam1PotentialCracks.Text = "Potential Cracks: " + camera1Data.PotentialCrackCount.ToString();
                    }
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
            if (Camera2Display.InvokeRequired || gbCamera2.InvokeRequired)
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
                    switch (Cam2DisplayType)
                    {
                        case DISPLAY_TYPE_NORMAL:
                            try
                            {
                                if (!camera2Data.IsProcessed)
                                {
                                    Camera2Display.SetImage(camera2Data.GetRawDataImage());
                                }
                            }
                            catch (Exception inner)
                            {
                                log.Error("MainForm.UpdateCamera2Image : Unable to set camera 2 display image.", inner);
                            }
                            break;
                        case DISPLAY_TYPE_PROCESSED:
                            try
                            {
                                if (camera2Data.IsProcessed)
                                {
                                    Camera2Display.SetImage(camera2Data.GetProcessedDataImage());
                                }
                            }
                            catch (Exception inner)
                            {
                                log.Error("MainForm.UpdateCamera2Image : Unable to set camera 2 display image.", inner);
                            }
                            break;
                        default:
                            break;
                    }

                    // update camera 2 information
                    camera2Period = 0.85 * camera2Period + 0.15 * camera2Data.CameraElapsedTime_s;
                    lblCam2FPS.Text = "Frames Per Second: " + String.Format("{0:0.00}", (1 / camera2Period));
                    if (camera2Data.ImageProcessorElapsedTime_s != 0)
                    {
                        process2Period = 0.95 * process2Period + 0.05 * camera2Data.ImageProcessorElapsedTime_s;
                        lblCam2IPFPS.Text = "Image Processor FPS: " + String.Format("{0:0.00}", (1 / process2Period));
                    }
                    
                    lblCam2Exposure.Text = "Exposure: " + String.Format("{0:0.00}", camera2Data.ImageExposure_s * 1000);
                    if (camera2Data.IsProcessed)
                    {
                        lblCam2Intensity.Text = "Intensity: " + camera2Data.ImageIntensity_lsb.ToString();
                        lblCam2CracksDetected.Text = "Crack Detected: " + camera2Data.ContainsCrack.ToString();
                        lblCam2PotentialCracks.Text = "Potential Cracks: " + camera2Data.PotentialCrackCount.ToString();
                    }
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
            program_settings.ShowDialog();
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
            ipQueue1.reset();
            ipQueue2.reset();
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
            RefreshCameras();
        }

        private void DisplayError(string errMsg, Exception ex)
        {
            MessageBox.Show(errMsg + Environment.NewLine + "Detailed Error: " + Environment.NewLine + ex.ToString(), "Error");
        }

        private void RefreshCameras()
        {
            // make sure camera 1 is shutdown
            if (cam1.Running)
            {
                try
                {
                    cam1.StopCamera();
                }
                catch (Exception inner)
                {
                    string errMsg = "Form1.RefreshCameras : Unable to stop camera 1.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                    DisplayError(errMsg, ex);
                    return;
                }
            }
            // attempt to start camera 1
            try
            {
                cam1.StartCamera(DEFAULT_CAM1_NAME, 0);
            }
            catch (Exception inner)
            {
                string errMsg = "Form1.RefreshCameras : Unable to start camera 1.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
            }

            // make sure camera 2 is shutdown
            if (cam2.Running)
            {
                try
                {
                    cam2.StopCamera();
                }
                catch (Exception inner)
                {
                    string errMsg = "Form1.RefreshCameras : Unable to stop camera 2.";
                    MainFormException ex = new MainFormException(errMsg, inner);
                    log.Error(errMsg, ex);
                    DisplayError(errMsg, ex);
                    return;
                }
            }
            // attempt to start camera 2
            try
            {
                cam2.StartCamera(DEFAULT_CAM2_NAME, 1);
            }
            catch (Exception inner)
            {
                string errMsg = "Form1.RefreshCameras : Unable to start camera 2.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
            }
        }

        private bool Start()
        {
            log.Info("MainForm.Start : Starting image processors and save queue engine.");
            // set parameters for image processors
            MetaData metadata = MetaData.Instance;
            imagep1.SetContrast(metadata.MinimumContrast);
            imagep1.SetRange(metadata.ImagerNoise);
            imagep1.SetTargetIntensity(metadata.TargetIntenstiy);
            imagep2.SetContrast(metadata.MinimumContrast);
            imagep2.SetRange(metadata.ImagerNoise);
            imagep2.SetTargetIntensity(metadata.TargetIntenstiy);
            try
            {
                imagep1.Start();
            }
            catch (Exception inner)
            {
                string errMsg = "MainForm.Start : Error starting image processor 1.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
                return false;
            }
            Thread.Sleep(100);
            try
            {
                imagep2.Start();
            }
            catch (Exception inner)
            {
                string errMsg = "MainForm.Start : Error starting image processor 2.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
                return false;
            }
            try
            {
                saveEngine.Start(5);
            }
            catch (Exception inner)
            {
                string errMsg = "MainForm.Start : Error starting save engine.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
                return false;
            }
            return true;
        }

        private void Stop()
        {
            log.Info("MainForm.Stop : Stopping image processors and save queue engine.");
            try
            {
                imagep1.Stop();
            }
            catch (Exception inner)
            {
                string errMsg = "MainForm.Stop : Error stopping image processor 1.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
            }
            try
            {
                imagep2.Stop();
            }
            catch (Exception inner)
            {
                string errMsg = "MainForm.Stop : Error stopping image processor 2.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
            }
            try
            {
                saveEngine.Stop();
            }
            catch (Exception inner)
            {
                string errMsg = "MainForm.Stop : Error stopping save engine.";
                MainFormException ex = new MainFormException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
            }
            return;
        }

        private void Camera0ThreadError( object sender, ThreadErrorEventArgs e)
        {
            e.ShowErrorMsgBoxEx();
            if (e.StoppingThread)
            {
                // handle camera 1 shutdown accordingly

            }
        }

        private void Camera1ThreadError(object sender, ThreadErrorEventArgs e)
        {
            e.ShowErrorMsgBoxEx();
            if (e.StoppingThread)
            {
                // handle camera 2 shutdown accordingly
            }
        }

        private void ImageProcessor0ThreadError(object sender, ThreadErrorEventArgs e)
        {
            e.ShowErrorMsgBoxEx();
            if (e.StoppingThread)
            {
                // handle image processor 1 shutdown accordingly
            }
        }

        private void ImageProcessor1ThreadError(object sender, ThreadErrorEventArgs e)
        {
            e.ShowErrorMsgBoxEx();
            if (e.StoppingThread)
            {
                // handle image processor 2 shutdown accordingly
            }
        }

        private void SaveQueueThreadError(object sender, ThreadErrorEventArgs e)
        {
            e.ShowErrorMsgBoxEx();
            if (e.StoppingThread)
            {
                // handle save queue shutdown accordingly
            }
        }

        private void cmboCam1View_TextChanged(object sender, EventArgs e)
        {
            Cam1DisplayType = cmboCam1View.Text;
        }

        private void cmboCam2View_TextChanged(object sender, EventArgs e)
        {
            Cam2DisplayType = cmboCam2View.Text;
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
