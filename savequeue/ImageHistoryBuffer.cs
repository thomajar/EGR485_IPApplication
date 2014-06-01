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

namespace SAF_OpticalFailureDetector.savequeue
{
    class ImageHistoryBuffer
    {

        public event ThreadErrorHandler ThreadError;
        // thread synchronization
        private object _saveLock;
        private static readonly ILog log = LogManager.GetLogger(typeof(ImageHistoryBuffer));

        // consumer queue variables
        private const String CONSUMER_ROOTNAME = "SAVE_";
        private const int CONSUMER_QUEUE_SIZE = 1000;
        CircularQueue<QueueElement> consumerQueue;
        private String consumerName;
        private String consumerLogFileLocation;

        // subscriber queue variables
        List<CircularQueue<QueueElement>> subscribers;

        // thread variables
        private Boolean isRunning;
        private Thread processThread;

        public bool Running { get { return isRunning; } }

        public ImageHistoryBuffer(String name, String LogFileLocation)
        {
            _saveLock = new object();

            // initialize queues
            consumerLogFileLocation = LogFileLocation;
            consumerName = CONSUMER_ROOTNAME + name;
            //consumerQueue = new CircularQueue<QueueElement>(consumerName,CONSUMER_QUEUE_SIZE);
            subscribers = new List<CircularQueue<QueueElement>>();

            // initialize processing thread variables
            isRunning = false;

            // release control, end of initialization
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
        /// Function adds a subscriber to the cameras queue.  The camera will 
        /// send all of its images to each subscriber in its queue.
        /// </summary>
        /// <param name="subscriber">Subscriber to add to queue.</param>
        /// <returns>True if successful, False if already exists.</returns>
        public bool AddSubscriber(CircularQueue<QueueElement> subscriber)
        {
            bool doesNotExist = true;

            lock (_saveLock)
            {
                // loop through all subscribers and check if subscriber exists
                foreach (CircularQueue<QueueElement> test in subscribers)
                {
                    // only care if the names match
                    if (test.Name == subscriber.Name)
                    {
                        doesNotExist = false;
                    }
                }
                // verify subsriber not already in camera queue
                if (doesNotExist)
                {
                    subscribers.Add(subscriber);
                } 
            }

            return doesNotExist;
        }

        /// <summary>
        /// Function removes subscriber from camera's queue if the subscriber
        /// exists.
        /// </summary>
        /// <param name="subscriber">Subscriber to remove from camera.</param>
        /// <returns>True if successful, False otherwise.</returns>
        public bool RemoveSubscriber(CircularQueue<QueueElement> subscriber)
        {
            bool doesNotExist = true;
            int removeIndex = -1;
            int i = 0;

            lock (_saveLock)
            {
                foreach (CircularQueue<QueueElement> test in subscribers)
                {
                    // only name needs to match
                    if (test.Name == subscriber.Name)
                    {
                        doesNotExist = false;
                        // take note of what index subsriber located at
                        removeIndex = i;
                    }
                    i++;
                }
                // if it exists, remove it
                if (!doesNotExist)
                {
                    subscribers.RemoveAt(removeIndex);
                } 
            }

            return !doesNotExist;
        }

        /// <summary>
        /// Starts the image processing thread.
        /// </summary>
        /// <returns>T if successful, F otherwise.</returns>
        public bool Start()
        {
            Boolean result = false;
            if (!isRunning)
            {
                result = true;
                isRunning = true;
                processThread = new Thread(new ThreadStart(Process));
                processThread.Start();
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
            if (isRunning)
            {
                result = true;
                isRunning = false;
                processThread.Join();
            }
            return result;
        }

        private int DEFAULT_SAVE_FRAME_COUNT = 100;
        private int DEFAULT_SAVE_FRAME_FREQUENCY = 5;

        private void SaveIPData(string rootLocation, ref IPData data)
        {
            DateTime time = DateTime.Now;
            string directory = 
                time.Month.ToString("D2") + time.Day.ToString("D2") + time.Year.ToString("D4") + "_" +
                time.Hour.ToString("D2") + time.Minute.ToString("D2") + time.Second.ToString("D2") + "_" +
                "image" + data.ImageNumber.ToString("D8");
            rootLocation = rootLocation + "//" + directory;
            if (!Directory.Exists(rootLocation))
            {
                Directory.CreateDirectory(rootLocation);
            } 

            // save raw
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
                log.Error(errMsg,ex);
                isValid = false;
            }
            if (isValid)
            {
                string fileName = rootLocation + "//rawimage_" + data.ImageNumber.ToString("D8") + ".bmp";
                try
                {
                    rawBitmap.Save(fileName);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Unable to save raw data image.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                }
            }
            // save processed
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
                string fileName = rootLocation + "//procimage_" + data.ImageNumber.ToString("D8") + ".bmp";
                try
                {
                    processedBitmap.Save(fileName);
                }
                catch (Exception inner)
                {
                    string errMsg = "ImageHistoryBuffer.SaveIPData : Unable to save processed data image.";
                    ImageHistoryBufferException ex = new ImageHistoryBufferException(errMsg, inner);
                    log.Error(errMsg, ex);
                }
            }
            // save metadata
            StreamWriter write = new StreamWriter(rootLocation + "//info" + data.ImageNumber.ToString("D8") + ".txt");
            MetaData metadata = MetaData.Instance;
            write.WriteLine("General Settings: ");
            write.WriteLine("Sample Number: " + metadata.SampleNumber);
            write.WriteLine("Test Number: " + metadata.TestNumber);
            write.WriteLine("Imager Noise: " + metadata.ImagerNoise.ToString("D3"));
            write.WriteLine("Minimum Contrast: " + metadata.MinimumContrast.ToString("D3"));
            write.WriteLine("Target Intensity: " + metadata.TargetIntenstiy.ToString("D3"));
            write.WriteLine("Minimum Line Length: " + metadata.MinimumLineLength.ToString("D3"));
            write.WriteLine();
            write.WriteLine("Camera Information: ");
            write.WriteLine("Timestamp: " + data.TimeStamp.ToLongTimeString());
            write.WriteLine("Image Number: " + data.ImageNumber.ToString("D8"));
            write.WriteLine("Image Size: " + data.ImageSize.Width.ToString("D4") + "x" + data.ImageSize.Height.ToString("D4"));
            write.WriteLine("Exposure (s): " + data.ImageExposure_s.ToString());
            write.WriteLine("Intensity (lsb): " + data.ImageIntensity_lsb.ToString("D3"));
            write.WriteLine("Potential Cracks: " + data.PotentialCrackCount.ToString("D2"));
            write.WriteLine("Contains Crack: " + data.ContainsCrack.ToString());
            write.Close();
        }

        private void Process()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool saveImage1 = true;
            bool saveImage2 = true;

            // setup directories
            string rootLocation = MetaData.Instance.SaveLocation;
            if (!Directory.Exists(rootLocation))
            {
                Directory.CreateDirectory(rootLocation);
            }
            DateTime time = DateTime.Now;
            // add a date and time of test to test
            string folderName = "//Test" + MetaData.Instance.TestNumber + "_Sample" +
                MetaData.Instance.SampleNumber + "_" +
                time.Month.ToString("D2") + time.Day.ToString("D2") + time.Year.ToString("D4") + "_" +
                time.Hour.ToString("D2") + time.Minute.ToString("D2") + time.Second.ToString("D2");
            rootLocation += folderName;
            if (!Directory.Exists(rootLocation))
            {
                Directory.CreateDirectory(rootLocation);
            }
            // crack detected folder --> cam1 /cam2
            string crackedRootLocation = rootLocation + "//cracked";
            string cam1CrackedRootLocation = crackedRootLocation + "//cam1";
            string cam2CrackedRootLocation = crackedRootLocation + "//cam2";
            if (!Directory.Exists(crackedRootLocation))
            {
                Directory.CreateDirectory(crackedRootLocation);
            }
            if (!Directory.Exists(cam1CrackedRootLocation))
            {
                Directory.CreateDirectory(cam1CrackedRootLocation);
            }
            if (!Directory.Exists(cam2CrackedRootLocation))
            {
                Directory.CreateDirectory(cam2CrackedRootLocation);
            }

            // debug folder --> cam1 / cam2
            string debugLocation = rootLocation + "//debug";
            string cam1DebugRootLocation = debugLocation + "//cam1";
            string cam2DebugRootLocation = debugLocation + "//cam2";
            if (MetaData.Instance.EnableDebugSaving)
            {
                if (!Directory.Exists(debugLocation))
                {
                    Directory.CreateDirectory(debugLocation);
                }
                if (!Directory.Exists(cam1DebugRootLocation))
                {
                    Directory.CreateDirectory(cam1DebugRootLocation);
                }
                if (!Directory.Exists(cam2DebugRootLocation))
                {
                    Directory.CreateDirectory(cam2DebugRootLocation);
                }
            }
            

            while (isRunning)
            {
                
                Thread.Sleep(5);
                List<QueueElement> imageElements = new List<QueueElement>();

                MetaData metadata = MetaData.Instance;

                if (sw.ElapsedMilliseconds >= metadata.DebugSaveFrequency * 1000)
                {
                    saveImage1 = true;
                    saveImage2 = true;
                    sw.Restart();
                }

                List<IPData> CameraOneHistory = new List<IPData>(DEFAULT_SAVE_FRAME_COUNT);
                List<IPData> CameraTwoHistory = new List<IPData>(DEFAULT_SAVE_FRAME_COUNT);
                int camOneCounter = 0;
                int camTwoCounter = 0;
                int camOneIndex = 0;
                int camTwoIndex = 0;
                
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
                            if (data.ImageNumber % DEFAULT_SAVE_FRAME_FREQUENCY != camOneCounter % DEFAULT_SAVE_FRAME_FREQUENCY)
                            {
                                // requires a slot in buffer
                                CameraOneHistory.Insert(camOneIndex, data);
                                camOneIndex = (camOneIndex + 1) % DEFAULT_SAVE_FRAME_COUNT;
                            }
                            if (metadata.EnableDebugSaving)
                            {
                                if (saveImage1)
                                {
                                    // need to save image to debug slot
                                    SaveIPData(cam1DebugRootLocation,  ref data);
                                    saveImage1 = false;
                                }
                            }
                            camOneCounter = data.ImageNumber;
                        }
                        else if (type.Contains("2"))
                        {
                            // see if image needs to be stored in recent history buffer 2
                            if (data.ImageNumber % DEFAULT_SAVE_FRAME_FREQUENCY != camTwoCounter % DEFAULT_SAVE_FRAME_FREQUENCY)
                            {
                                // requires a slot in buffer
                                CameraTwoHistory.Insert(camTwoIndex, data);
                                camTwoIndex = (camTwoIndex + 1) % DEFAULT_SAVE_FRAME_COUNT;
                            }
                            if (metadata.EnableDebugSaving)
                            {
                                if (saveImage2)
                                {
                                    // need to save image to debug slot
                                    SaveIPData(cam2DebugRootLocation, ref data);
                                    saveImage2 = false;
                                }
                            }
                            camTwoCounter = data.ImageNumber;
                        }
                        else
                        {
                            // unknown data type
                        }
                    }
                }
            }
            return;
        }

        /// <summary>
        /// This is the process that is run in a seperate thread
        /// </summary>
        private void Process2()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Bitmap[] bufferImages = new Bitmap[100];
            int imageIndex = 0;
            string date_time;
            // keep thread running until told to stop or start
            while (isRunning)
            {
                List<QueueElement> imageElements = new List<QueueElement>();
                IPData image = null;
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    image = (IPData)imageElements[imageElements.Count - 1].Data;
                    if(sw.ElapsedMilliseconds >= 5000)
                    {
                        sw.Restart();
                        bufferImages[imageIndex] = image.GetRawDataImage();//image.GetCameraImage();
                        imageIndex = (imageIndex + 1) % 100;
                    }
                    if(!image.ContainsCrack)
                    {
                        for(int counter = imageIndex; counter < 100; counter++)
                        {
                             Bitmap tmpBmp = bufferImages[counter];
                            if (tmpBmp != null)
                            {
                                date_time = DateTime.Now.Date.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day +
                                     "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();
                                tmpBmp.Save(consumerLogFileLocation + "_" + date_time + ".bmp");
                            }
                         
                        }
                        for(int counter = 0; counter < imageIndex; counter++)
                        {
                            Bitmap tmpBmp = bufferImages[counter];
                            if (tmpBmp != null)
                            {
                                date_time = DateTime.Now.Date.Year + "_" + DateTime.Now.Month + "_" + DateTime.Now.Day +
                                    "_" + DateTime.Now.Hour.ToString() + "_" + DateTime.Now.Minute.ToString() + "_" + DateTime.Now.Second.ToString();
                                tmpBmp.Save(consumerLogFileLocation + "_" + date_time + ".bmp");
                            }
                        }
                    }
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
