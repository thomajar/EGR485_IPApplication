using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAF_OpticalFailureDetector.threading;
using System.Threading;
using System.Drawing;
using System.Drawing.Imaging;
using System.Diagnostics;
using SAF_OpticalFailureDetector.imageprocessing;
using log4net;
using System.IO;
using SAF_OpticalFailureDetector.relay;

namespace SAF_OpticalFailureDetector.savequeue
{
    class ImageHistoryBuffer
    {
        private const int DEFAULT_THROTTLE_PERIOD = 250;
        private const int DEFAULT_TEST_HISTORY_SIZE = 50;

        public event ThreadErrorHandler ThreadError;
        // thread synchronization
        private object _saveLock;
        private static readonly ILog log = LogManager.GetLogger(typeof(ImageHistoryBuffer));

        // consumer queue variables
        CircularQueue<QueueElement> consumerQueue;

        // thread variables
        private Boolean isRunning;
        private Thread processThread;
        private int threadThrottlePeriod;

        public bool Running { get { return isRunning; } }

        /// <summary>
        /// Constructor of ImageHistoryBuffer
        /// </summary>
        public ImageHistoryBuffer()
        {
            _saveLock = new object();

            // initialize processing thread variables
            isRunning = false;
            threadThrottlePeriod = DEFAULT_THROTTLE_PERIOD;
        }

        /// <summary>
        /// Retrieves the consumer queue from the Failure Detector.  Add
        /// ImageData to this queue in order to get the image processed.
        /// </summary>
        /// <returns>Consumer Queueu</returns>
        public void SetConsumerQueue(CircularQueue<QueueElement> consumer)
        {
            consumerQueue = consumer;
        }

        /// <summary>
        /// Starts the process thread which enables debug and test saving of IPData.
        /// </summary>
        /// <param name="maximumUpdateFrequency">Limits the maximum frequency of the process thread. </param>
        /// <returns>T if successful, F otherwise.</returns>
        public bool Start(double maximumUpdateFrequency)
        {
            Boolean result = false;
            lock (_saveLock)
            {
                if (!isRunning)
                {
                    try
                    {
                        threadThrottlePeriod = Convert.ToInt32(1000 / maximumUpdateFrequency);
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "ImageHistoryBuffer.Start : Unable to set throttle period, defaulting to 5 FPS.";
                        ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                        log.Info(errMsg, ex);
                        threadThrottlePeriod = DEFAULT_THROTTLE_PERIOD;
                    }
                    result = true;
                    isRunning = true;
                    processThread = new Thread(new ThreadStart(Process));
                    processThread.Start();
                } 
            }
            return result;
        }

        /// <summary>
        /// Stops the image processing thread. Waits for thread to join.
        /// </summary>
        /// <returns>T if successful, F otherwise.</returns>
        public Boolean Stop()
        {
            Boolean result = false;
            lock (_saveLock)
            {
                if (isRunning)
                {
                    result = true;
                    isRunning = false;
                    processThread.Join();
                } 
            }
            return result;
        }

        private void Process()
        {
            // get a local copy of metadata singleton
            MetaData metadata = MetaData.Instance;

            // location of folders in saving directory structure
            string rootLocation = metadata.SaveLocation;
            string cam1TestLocation = "";
            string cam2TestLocation = "";
            string cam1DebugLocation = "";
            string cam2DebugLocation = "";

            // list to keep image data in
            IPData[] cam1History = new IPData[DEFAULT_TEST_HISTORY_SIZE];
            IPData[] cam2History = new IPData[DEFAULT_TEST_HISTORY_SIZE];
            int cam1HistoryInsertIndex = 0;
            int cam2HistoryInsertIndex = 0;

            // tracks consecutive cracks in data and a flag to confirm a crack
            int cam1ConsecutiveCrackedSampleCount = 0;
            int cam2ConsecutiveCrackedSampleCount = 0;
            bool crackConfirmed = false;

            // timers for throttling thread, and updating save requests
            Stopwatch _threadTimer = new Stopwatch();
            Stopwatch _debugSaveUpdateTimer = new Stopwatch();
            Stopwatch _testSaveUpdateTimer = new Stopwatch();

            // flags to reset when timers trigger indicating corresponding data needs to be updated
            bool updateCam1DebugData = true;
            bool updateCam2DebugData = true;
            bool updateCam1TestData = true;
            bool updateCam2TestData = true;

            // attempt to create root directory and unique identify folder
            try
            {
                // updates the rootLocation to include the unique identifier folder generated inside function call
                rootLocation = initRootDirectory(rootLocation);
            }
            catch (Exception inner)
            {
                string errMsg = "ImageHistoryBuffer.Process : Unable to create root directory and unique identifier folder.";
                ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                log.Error(errMsg, ex);
                
                // shutdown thread here
                ThreadErrorEventArgs er = new ThreadErrorEventArgs(errMsg, ex, true);
                ThreadErrorEventArgs.OnThreadError(this, ThreadError, er);
            }

            // attempt to create directory structure for test data
            try
            {
                initTestDirectory(rootLocation, ref cam1TestLocation, ref cam2TestLocation);
            }
            catch (Exception inner)
            {
                string errMsg = "ImageHistoryBuffer.Process : Unable to create directory structure for test data.";
                ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                log.Error(errMsg, ex);

                // shutdown thread here
                ThreadErrorEventArgs er = new ThreadErrorEventArgs(errMsg, ex, true);
                ThreadErrorEventArgs.OnThreadError(this, ThreadError, er);
            }

            // attempt to create directory structure for debug saving if enabled
            if (metadata.EnableDebugSaving)
            {
                try
                {
                    initDebugDirectory(rootLocation, ref cam1DebugLocation, ref cam2DebugLocation);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.Process : Unable to create directory structure for debug saving.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);

                    // shutdown thread here
                    ThreadErrorEventArgs er = new ThreadErrorEventArgs(errMsg, ex, true);
                    ThreadErrorEventArgs.OnThreadError(this, ThreadError, er);
                }
            }

            // start threading timers
            _threadTimer.Start();
            _debugSaveUpdateTimer.Start();
            _testSaveUpdateTimer.Start();

            while (isRunning)
            {
                // throttles the thread to prevent it from consuming 100% of processor it is running on
                int timeToSleep = threadThrottlePeriod - Convert.ToInt32(_threadTimer.ElapsedMilliseconds);
                if (timeToSleep > 0)
                {
                    Thread.Sleep(timeToSleep);
                }
                _threadTimer.Restart();

                // check if debug data needs to be updated
                if (_debugSaveUpdateTimer.ElapsedMilliseconds >= metadata.DebugSaveFrequency * 1000)
                {
                    updateCam1DebugData = true;
                    updateCam2DebugData = true;
                    _debugSaveUpdateTimer.Restart();
                }

                // check to see if test data needs to be updated
                if (_testSaveUpdateTimer.ElapsedMilliseconds >= 250)
                {
                    updateCam1TestData = true;
                    updateCam2TestData = true;
                    _testSaveUpdateTimer.Restart();
                }

                // pop everything off of savequeue
                List<QueueElement> imageElements = new List<QueueElement>();
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    for (int i = 0; i < imageElements.Count; i++)
                    {
                        //figure our where the iamge is coming from
                        IPData data = (IPData)imageElements[i].Data;
                        string type = imageElements[i].Type;
                        if (type.Contains("1"))
                        {
                            // see if image needs to be stored in recent history buffer 1
                            if (updateCam1TestData)
                            {
                                // requires a slot in buffer
                                cam1History[cam1HistoryInsertIndex] = data;
                                cam1HistoryInsertIndex = (cam1HistoryInsertIndex + 1) % DEFAULT_TEST_HISTORY_SIZE;
                                updateCam1TestData = false;
                            }
                            if (metadata.EnableDebugSaving)
                            {
                                if (updateCam1DebugData)
                                {
                                    // need to save image to debug slot
                                    SaveIPData(cam1DebugLocation, ref data);
                                    updateCam1DebugData = false;
                                }
                            }
                            if (data.ContainsCrack)
                            {
                                cam1ConsecutiveCrackedSampleCount++;
                                if (cam1ConsecutiveCrackedSampleCount > 10)
                                {
                                    crackConfirmed = true;
                                }
                            }
                            else
                            {
                                cam1ConsecutiveCrackedSampleCount = 0;
                                crackConfirmed = false;
                            }
                        }
                        else if (type.Contains("2"))
                        {
                            // see if image needs to be stored in recent history buffer 2
                            if (updateCam2TestData)
                            {
                                // requires a slot in buffer
                                cam2History[cam2HistoryInsertIndex] = data;
                                cam2HistoryInsertIndex = (cam2HistoryInsertIndex + 1) % DEFAULT_TEST_HISTORY_SIZE;
                                updateCam2TestData = false;
                            }
                            if (metadata.EnableDebugSaving)
                            {
                                if (updateCam2DebugData)
                                {
                                    // need to save image to debug slot
                                    SaveIPData(cam2DebugLocation, ref data);
                                    updateCam2DebugData = false;
                                }
                            }
                            if (data.ContainsCrack)
                            {
                                cam2ConsecutiveCrackedSampleCount++;
                                if (cam2ConsecutiveCrackedSampleCount > 10)
                                {
                                    crackConfirmed = true;
                                }
                            }
                            else
                            {
                                cam2ConsecutiveCrackedSampleCount = 0;
                                crackConfirmed = false;
                            }
                        }
                        else
                        {
                            // unknown data type
                            log.Info("ImageHistoryBuffer.Process : Received unknown data type.");
                        }
                        // check to see if we have confirmed a crack existing
                        if (crackConfirmed)
                        {
                            // trigger the USB Relay
                            USBRelayController usb_relay = USBRelayController.Instance;
                            if (usb_relay.IsOpen)
                            {
                                usb_relay.SetRelay0Status(true);
                                usb_relay.SetRelay1Status(true);
                            }

                            // save all images in history buffer
                            saveHistoryBuffer(cam1TestLocation, cam1History);
                            saveHistoryBuffer(cam2TestLocation, cam2History);

                            break;
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// Saves raw image, processe image, and metadata file for each IPData inside a folder in rootLocation.
        /// </summary>
        /// <param name="rootLocation">Location to place folder containing IPData.</param>
        /// <param name="data">Data to save.</param>
        /// <exception cref="ImageHistoryBufferException"></exception>
        private void SaveIPData(string rootLocation, ref IPData data)
        {
            const string RAW_DATA_EXTENSION = "//rawimage_";
            const string PROC_DATA_EXTENSION = "//procimage_";
            const string METADATA_EXTENSION = "//data_";
            const string IMAGE_FORMAT = ".bmp";
            const string METADATA_FORMAT = ".txt";

            // create a folder in current directory with timestamp of image data
            DateTime time = data.TimeStamp;
            string directory =
                time.Month.ToString("D2") + time.Day.ToString("D2") + time.Year.ToString("D4") + "_" +
                time.Hour.ToString("D2") + time.Minute.ToString("D2") + time.Second.ToString("D2") + "_" +
                "image" + data.ImageNumber.ToString("D8");
            rootLocation = rootLocation + "//" + directory;

            // create the directory if it does not exist
            if (!Directory.Exists(rootLocation))
            {
                try
                {
                    Directory.CreateDirectory(rootLocation);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Exception thrown creating directory to save IPData in.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }

            // save raw IPData
            Bitmap rawBitmap = null;
            bool isValid = true;
            try
            {
                rawBitmap = data.GetRawDataImage();
            }
            catch (Exception inner)
            {
                string errMsg = "ImageHistoryBuffer.SaveIPData : Unable to retrieve raw data image for saving.";
                ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                log.Error(errMsg, ex);
                isValid = false;
            }
            if (isValid)
            {
                string fileName = rootLocation + RAW_DATA_EXTENSION + data.ImageNumber.ToString("D8") + IMAGE_FORMAT;
                try
                {
                    rawBitmap.Save(fileName);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Unable to save raw data image.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }
            // dispose image to prevent memory leak
            if (rawBitmap != null)
            {
                rawBitmap.Dispose();
            }

            // save processed IPData
            Bitmap processedBitmap = null;
            isValid = true;
            try
            {
                processedBitmap = data.GetProcessedDataImage();
            }
            catch (Exception inner)
            {
                string errMsg = "ImageHistoryBuffer.SaveIPData : Unable to retrieve processed data image for saving.";
                ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                log.Error(errMsg, ex);
                isValid = false;
            }
            if (isValid)
            {
                string fileName = rootLocation + PROC_DATA_EXTENSION + data.ImageNumber.ToString("D8") + IMAGE_FORMAT;
                try
                {
                    processedBitmap.Save(fileName);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Unable to save processed data image.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }

            // dispose image to prevent memory leak
            if (processedBitmap != null)
            {
                processedBitmap.Dispose();
            }

            // create metadata file string to save
            MetaData metadata = MetaData.Instance;
            string dataToWrite = "";
            dataToWrite += "General Settings: " + Environment.NewLine;
            dataToWrite += "Sample Number: " + metadata.SampleNumber + Environment.NewLine;
            dataToWrite += "Test Number: " + metadata.TestNumber + Environment.NewLine;
            dataToWrite += "Imager Noise: " + metadata.ImagerNoise.ToString("D3") + Environment.NewLine;
            dataToWrite += "Minimum Contrast: " + metadata.MinimumContrast.ToString("D3") + Environment.NewLine;
            dataToWrite += "Target Intensity: " + metadata.TargetIntenstiy.ToString("D3") + Environment.NewLine;
            dataToWrite += "Minimum Line Length: " + metadata.MinimumLineLength.ToString("D3") + Environment.NewLine;
            dataToWrite += Environment.NewLine;
            dataToWrite += "Camera Information: " + Environment.NewLine;
            dataToWrite += "Timestamp: " + data.TimeStamp.ToLongTimeString() + Environment.NewLine;
            dataToWrite += "Image Number: " + data.ImageNumber.ToString("D8") + Environment.NewLine;
            dataToWrite += "Image Size: " + data.ImageSize.Width.ToString("D4") + "x" + data.ImageSize.Height.ToString("D4") + Environment.NewLine;
            dataToWrite += "Exposure (s): " + data.ImageExposure_s.ToString() + Environment.NewLine;
            dataToWrite += "Intensity (lsb): " + data.ImageIntensity_lsb.ToString("D3") + Environment.NewLine;
            dataToWrite += "Potential Cracks: " + data.PotentialCrackCount.ToString("D2") + Environment.NewLine;
            dataToWrite += "Contains Crack: " + data.ContainsCrack.ToString() + Environment.NewLine;

            // attempt to write metadata file string to file
            StreamWriter write = null;
            try
            {
                write = new StreamWriter(rootLocation + METADATA_EXTENSION + data.ImageNumber.ToString("D8") + METADATA_FORMAT);
            }
            catch (Exception inner)
            {
                string errMsg = "ImageHistoryBuffer.SaveIPData : Error opening stream writer for metadata file.";
                ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                log.Error(errMsg, inner);
                throw ex;
            }
            if (write != null)
            {
                try
                {
                    write.Write(dataToWrite);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Error writing data to metadata file.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, inner);
                    throw ex;
                }
                try
                {
                    write.Close();
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Error closing stream writer for metadata file.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, inner);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves all contents of history buffer inside of saveLocation.
        /// </summary>
        /// <param name="saveLocation">Location to save history buffer to.</param>
        /// <param name="camHistory">IPData history to save to disk.</param>
        private void saveHistoryBuffer(string saveLocation, IPData[] camHistory)
        {
            for (int j = 0; j < DEFAULT_TEST_HISTORY_SIZE; j++)
            {
                IPData ipdata = camHistory[j];
                if (ipdata != null)
                {
                    try
                    {
                        SaveIPData(saveLocation, ref ipdata);
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "ImageHistoryBuffer : Error saving image data " + j.ToString("D2") + " in image history buffer.";
                        ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                        log.Error(errMsg, ex);
                        // do not throw this error, bc we still want to try to save the rest of data
                    }
                }
            }
        }

        /// <summary>
        /// Creates the rootDirectory if it does not exist and creates a unique identifier folder inside of
        /// it using date and time that save stream was started.
        /// </summary>
        /// <param name="rootLocation">Location to setup folder structure within.</param>
        /// <exception cref="ImageHistoryBufferException"></exception>
        /// <returns>Modified rootDirectory containing unique identifier folder.</returns>
        private string initRootDirectory(string rootLocation)
        {
            // get a local copy of metadata
            MetaData metadata = MetaData.Instance;

            // check if directory chosen by user exists and create it if it does not
            if (!Directory.Exists(rootLocation))
            {
                try
                {
                    Directory.CreateDirectory(rootLocation);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initRootDirectory : Unable to create root directory for saving.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }
            // get date and time to create a folder within user's choosen location to save data, this creates a uniques stamp for the folder name
            DateTime time = DateTime.Now;
            string folderName = "//Test" + metadata.TestNumber + "_Sample" + metadata.SampleNumber + "_" +
                time.Month.ToString("D2") + time.Day.ToString("D2") + time.Year.ToString("D4") + "_" +
                time.Hour.ToString("D2") + time.Minute.ToString("D2") + time.Second.ToString("D2");
            rootLocation += folderName;
            // attempt to create the directory, there is no reason it should already exist
            try
            {
                Directory.CreateDirectory(rootLocation);
            }
            catch (Exception inner)
            {
                string errMsg = "ImageHistoryBuffer.initRootDirectory : Unable to create unique identify folder in root directory.";
                ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            return rootLocation;
        }

        private void initTestDirectory(string rootLocation, ref string cam1Location, ref string cam2Location)
        {
            const string TEST_EXTENSION = "//test";
            const string CAM1_EXTENSION = "//cam1";
            const string CAM2_EXTENSION = "//cam2";

            string testLocation = rootLocation + TEST_EXTENSION;
            // create root test Location folder
            if (!Directory.Exists(testLocation))
            {
                try
                {
                    Directory.CreateDirectory(testLocation);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initTestDirectory : Unable to create test directory root folder.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }

            // create cam1 folder inside of test location folder
            cam1Location = testLocation + CAM1_EXTENSION;
            if (!Directory.Exists(cam1Location))
            {
                try
                {
                    Directory.CreateDirectory(cam1Location);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initTestDirectory : Unable to create cam1 folder inside of test directory";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }

            // create cam2 folder inside of test location folder
            cam2Location = testLocation + CAM2_EXTENSION;
            if (!Directory.Exists(cam2Location))
            {
                try
                {
                    Directory.CreateDirectory(cam2Location);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initTestDirectory : Unable to create cam2 folder inside of test directory";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }
        }

        private void initDebugDirectory(string rootLocation, ref string cam1Location, ref string cam2Location)
        {
            const string DEBUG_EXTENSION = "//debug";
            const string CAM1_EXTENSION = "//cam1";
            const string CAM2_EXTENSION = "//cam2";

            string debugLocation = rootLocation + DEBUG_EXTENSION;
            // create root debug Location folder
            if (!Directory.Exists(debugLocation))
            {
                try
                {
                    Directory.CreateDirectory(debugLocation);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initDebugDirectory : Unable to create debug directory root folder.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }

            // create cam1 folder inside of debug location folder
            cam1Location = debugLocation + CAM1_EXTENSION;
            if (!Directory.Exists(cam1Location))
            {
                try
                {
                    Directory.CreateDirectory(cam1Location);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initDebugDirectory : Unable to create cam1 folder inside of debug directory";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }

            // create cam2 folder inside of debug location folder
            cam2Location = debugLocation + CAM2_EXTENSION;
            if (!Directory.Exists(cam2Location))
            {
                try
                {
                    Directory.CreateDirectory(cam2Location);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.initDebugDirectory : Unable to create cam2 folder inside of debug directory";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }
        }

    }
    // Use for exceptinos generated in ImageHistoryBuffer class
    public class ImageHistoryBufferException : System.Exception
    {
        public ImageHistoryBufferException() : base() { }
        public ImageHistoryBufferException(string message) : base(message) { }
        public ImageHistoryBufferException(string message, System.Exception inner) : base(message, inner) { }
        protected ImageHistoryBufferException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
