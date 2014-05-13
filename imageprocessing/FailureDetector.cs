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
using log4net;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    unsafe class FailureDetector
    {
        public event ThreadErrorHandler ThreadError;

        // thread synchronization
        private Semaphore sem;

        private static readonly ILog log = LogManager.GetLogger(typeof(FailureDetector));

        // ip variables
        private int minimumContrast;
        private int noiseRange;
        private Boolean enableROI;
        private int updateROIFrequency;
        private Rectangle roi;
        private Boolean enableAutoExposure;
        private int updateExposureFrequency;
        private int targetIntesity;

        private const float INTENSITY_GAIN_THRESHOLD = 0.05f;

        // consumer queue variables
        private const int CONSUMER_QUEUE_SIZE = 1000;
        CircularQueue<QueueElement> consumerQueue;
        private String imageProcessorName;

        // subscriber queue variables
        List<CircularQueue<QueueElement>> subscribers;

        // thread variables
        private Boolean isRunning;
        private Thread processThread;


        private const int DEFAULT_MIN_CONTRAST = 15;
        private const int DEFAULT_MIN_NOISE_LVL = 15;
        private const bool DEFAULT_ENABLE_ROI = false;
        private const int DEFAULT_ROI_UPDATE_FREQUENCY = 100;
        private const bool DEFAULT_ENABLE_AUTO_EXPOSURE = false;
        private const int DEFAULT_EXPOSURE_UPDATE_FREQUENCY = 50;
        private const int DEFAULT_TARGET_INTENSITY = 200;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public FailureDetector(String name)
        {
            sem = new Semaphore(0, 1);

            this.imageProcessorName = name;
            subscribers = new List<CircularQueue<QueueElement>>();

            // initialize processing thread variables
            isRunning = false;

            // defaults
            minimumContrast = DEFAULT_MIN_CONTRAST;
            noiseRange = DEFAULT_MIN_NOISE_LVL;
            enableROI = DEFAULT_ENABLE_ROI;
            updateROIFrequency = DEFAULT_ROI_UPDATE_FREQUENCY;
            enableAutoExposure = DEFAULT_ENABLE_AUTO_EXPOSURE;
            updateExposureFrequency = DEFAULT_EXPOSURE_UPDATE_FREQUENCY;
            targetIntesity = DEFAULT_TARGET_INTENSITY;

            // release control, end of initialization
            sem.Release();
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
            // wait for sem control to enter critical section
            sem.WaitOne();
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
                log.Info("FailureDetector.AddSubscriber : Added subscriber: " + subscriber.Name + ".");
                subscribers.Add(subscriber);
            }
            // exit critical section
            sem.Release();
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
            // wait for sem control to enter critical section
            sem.WaitOne();
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
                log.Info("FailureDetector.AddSubscriber : Removed subscriber: " + subscriber.Name + ".");
                subscribers.RemoveAt(removeIndex);
            }
            // release control, exit critical section
            sem.Release();
            return !doesNotExist;
        }

        /// <summary>
        /// Starts the image processing thread.
        /// </summary>
        /// <exception cref="FailureDetectorException"></exception>
        public void Start()
        {
            sem.WaitOne();
            if (!isRunning)
            {
                processThread = new Thread(new ThreadStart(Process));
                try
                {
                    processThread.Start();
                }
                catch (Exception inner)
                {
                    sem.Release();
                    string errMsg = "FailureDetector.Start : Unable to start process thread.";
                    FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
                isRunning = true;
            }
            else
            {
                sem.Release();
                string errMsg = "FailureDetector.Start : Processor thread is already running.";
                FailureDetectorException ex = new FailureDetectorException(errMsg);
                log.Error(errMsg);
                throw ex;
            }
            sem.Release();
        }

        /// <summary>
        /// Stops the image processing thread. Waits for thread to join.
        /// </summary>
        /// <exception cref="FailureDetectorException"></exception>
        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                try
                {
                    processThread.Join();
                }
                catch (Exception inner)
                {
                    string errMsg = "FailureDetector.Stop : Unable to stop processing thread.";
                    FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }
            else
            {
                string errMsg = "FailureDetector.Stop : Processing thread is already stopped.";
                FailureDetectorException ex = new FailureDetectorException(errMsg);
                log.Error(errMsg);
                throw ex;
            }
        }

        public void SetRange(int range)
        {
            sem.WaitOne();
            this.noiseRange = range;
            sem.Release();
        }
        public void SetContrast(int contrast)
        {
            sem.WaitOne();
            this.minimumContrast = contrast;
            sem.Release();
        }

        public void EnableAutoExposure(bool enable)
        {
            sem.WaitOne();
            this.enableAutoExposure = enable;
            sem.Release();
        }

        private void Process()
        {
            int exposureCounter = 0;
            int roiCounter = 0;
            int imageIntensity = -1;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // keep thread running until told to stop or start
            while (isRunning)
            {
                Thread.Sleep(2);
                sw.Restart();
                List<QueueElement> imageElements = new List<QueueElement>();
                IPData image = null;
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    image = (IPData)imageElements[imageElements.Count - 1].Data;

                    // get the image to process on
                    Bitmap processImage = image.GetRawDataImage();
                    bool isImageSet = true;
                    try
                    {
                        processImage = image.GetRawDataImage();
                    }
                    catch (Exception inner)
                    {
                        isImageSet = false;
                        string errMsg = "FailureDetector.Process : Unable to get raw data image.";
                        FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                        log.Error(errMsg, ex);
                    }

                    if (isImageSet)
                    {
                        // update roi to process
                        if (enableROI)
                        {
                            roiCounter = (roiCounter + 1) % updateROIFrequency;
                            if (roiCounter == 1)
                            {
                                // store old roi incase of an exception
                                Rectangle oldROI = new Rectangle(roi.X,roi.Y,roi.Width,roi.Height);
                                try
                                {
                                    updateROI(processImage);
                                }
                                catch (Exception inner)
                                {
                                    roi = oldROI;
                                    string errMsg = "FailureDetector.Process : Exception thrown while update ROI, reverting to old ROI";
                                    FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                                    log.Error(errMsg,ex);
                                }
                            }
                        }

                        // update exposure of camera
                        if (enableAutoExposure)
                        {
                            exposureCounter = (exposureCounter + 1) % updateExposureFrequency;
                            if (exposureCounter == 0)
                            {
                                // get histogram from image
                                int[] hist = null;
                                try
                                {
                                    hist = histogram(processImage);
                                }
                                catch (Exception inner)
                                {
                                    string errMsg = "FailureDetector.Process : Exception thrown while calculating histogram of image.";
                                    FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                                    log.Error(errMsg, ex);
                                }

                                // get center of mass
                                imageIntensity = -1;
                                try
                                {
                                    imageIntensity = weightedIntensity(hist);
                                }
                                catch (Exception inner)
                                {
                                    string errMsg = "FailureDetector.Process : Exception thrown while calculating average intensity.";
                                    FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                                    log.Error(errMsg, ex);
                                }
                                // verify imageIntensity was successfully calculated
                                if (imageIntensity > -1)
                                {
                                    float desiredGain;
                                    // need a try because there is potential for imageIntensity being 0
                                    try
                                    {
                                        desiredGain = (float)targetIntesity / (float)imageIntensity;
                                    }
                                    catch (Exception)
                                    {
                                        desiredGain = 2.0f;
                                    }
                                    // make sure gain is above change threshold
                                    if (desiredGain > 1.0f + INTENSITY_GAIN_THRESHOLD || desiredGain < 1.0f - INTENSITY_GAIN_THRESHOLD)
                                    {
                                        //cam.GainExposure(desiredGain) ~ this needs to be implemented
                                        exposureCounter -= 1;
                                    }
                                }
                            }
                        }

                        // perform image processing
                        Boolean isImageFiltered = true;
                        try
                        {
                            processImage = filterImage(processImage);
                        }
                        catch (Exception inner)
                        {
                            string errMsg = "FailureDetector.Process : Exception thrown while filtering image.";
                            FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                            log.Error(errMsg, ex);
                            isImageFiltered = false;
                        }

                        if (isImageFiltered)
                        {
                            Boolean isIPDataSet = true;
                            try
                            {
                                image.SetProcessedDataFromImage(processImage);
                            }
                            catch (Exception inner)
                            {
                                string errMsg = "FailureDetector.Process : Exception thrown while storing processed image into IPData.";
                                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                                log.Error(errMsg, ex);
                                isIPDataSet = false;
                            }
                            image.SetIPMetaData(((Double)sw.ElapsedMilliseconds) / 1000, roi, imageIntensity, 0, false, true);

                            if (isIPDataSet)
                            {
                                for (int i = 0; i < subscribers.Count; i++)
                                {
                                    subscribers[i].push(new QueueElement(imageProcessorName, image));
                                }  
                            }
                        }
                    }
                }
            }
        }

       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private Bitmap filterImage(Bitmap b)
        {
            int PixelSize = 3;
            int threshold = noiseRange * 8;
            int result = 0;
            int row_count = 0;
            BitmapData B_data = null;
            // filter
            int[] H = new int[] { 3, 1, -1, -6, -1, 1, 3 };

            // attempt to lock bits on bitmap to process
            try
            {
                B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.filterImage : Unable to lock bits.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.filterImage : Unable to lock bits.", inner);
                throw ex;
            }

            Rectangle regionToProcess;
            if (enableROI)
            {
                regionToProcess = roi;
            }
            else
            {
                regionToProcess = new Rectangle(0, 0, b.Width, b.Height);
            }

                
            // begin filtering of image
            for (int y = regionToProcess.Top; y < regionToProcess.Bottom; y++)
            {
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);
                bool Ishot = false;
                for (int x = regionToProcess.Left + 3; x < regionToProcess.Right - 3; x++)
                {
                    result = 0;
                    int offset = 3;
                    int n = 7;
                    for (int i = 0; i < n; i++)
                    {
                        result += row[(x - offset + i) * PixelSize] * H[i];
                    }
                    if (result > threshold)
                    {
                        if (row[(x - offset) * PixelSize] > row[x * PixelSize] + minimumContrast &&
                            row[(x + offset) * PixelSize] > row[x * PixelSize] + minimumContrast)
                        {
                            Ishot = true;
                            // color pixel green
                            row[x * PixelSize] = 0;   //Blue  0-255
                            row[x * PixelSize + 1] = 255; //Green 0-255
                            row[x * PixelSize + 2] = 0;   //Red   0-255
                        }
                    }
                }
                if (Ishot)
                {
                    row_count++;
                }
            }
            try
            {
                b.UnlockBits(B_data);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.filterImage : Unable to unlock bits.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.filterImage : Unable to unlock bits.", inner);
                throw ex;
            }
            return b;
        }

        /// <summary>
        /// Function updates the ROI.
        /// </summary>
        /// <param name="b"></param>
        public void updateROI(Bitmap b)
        {
            roi = new Rectangle(0, 0, b.Width, b.Height);
            int percentwhite = 80;
            int PixelSize = 3;

            BitmapData B_data = null;

            // try to get histogram of image
            int[] hist;
            try
            {
                 hist = histogram(b);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.updateROI : Exception generated while making histogram of image.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.updateROI : Exception generated while making histogram of image.", inner);
                throw ex;
            }

            // get weighted average of image
            int centerOfMass;
            try
            {
                centerOfMass = weightedIntensity(hist);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.updateROI : Exception generated while calculating weighted intensity.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.updateROI : Exception generated while calculating weighted intensity.", inner);
                throw ex;
            }

            try
            {
                B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.updateROI : Unable to lock bits in bitmap.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.updateROI : Unable to lock bits in bitmap.", inner);
                throw ex;
            }
            
            //Find Top Edge
            for (int y = 0; y < B_data.Height; y++)
            {
                int count = 0;
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);

                for (int x = 0; x < B_data.Width; x++)
                {
                    if (row[x * PixelSize] > centerOfMass * 0.5)
                    {
                        count++;
                    }
                }
                if ((100 * count) / b.Width > percentwhite)
                {
                    roi = new Rectangle(0, y, roi.Width, roi.Height);
                    break;
                }
            }

            //Find Bottom Edge
            for (int y = B_data.Height - 1; y > -1; y--)
            {
                int count = 0;
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);

                for (int x = 0; x < B_data.Width; x++)
                {
                    if (row[x * PixelSize] > centerOfMass * 0.5)
                    {
                        count++;
                    }
                }
                if ((100 * count) / b.Width > percentwhite)
                {
                    roi = new Rectangle(0, roi.Top, roi.Width, y - roi.Top);
                    break;
                }
            }

            try
            {
                b.UnlockBits(B_data);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.updateROI : Unable to unlock bits in bitmap.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.updateROI : Unable to unlock bits in bitmap.", inner);
                throw ex;
            }
            
        }

        private int weightedIntensity(int[] data)
        {
            int weightedSum = 0;
            int sum = 0;
            for (int i = 0; i < data.Length; i++)
            {
                weightedSum += data[i] * i;
                sum += data[i];
            }
            return weightedSum / sum;
        }

        private int[] histogram(Bitmap b)
        {
            int[] data = new int[256];
            BitmapData B_data = null;

            try
            {
                B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.histogram : Unable to lock bits in bitmap.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.histogram : Unable to lock bits in bitmap.", inner);
                throw ex;
            }

            for (int y = 0; y < b.Height; y++)
            {
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);
                for (int x = 0; x < B_data.Width; x++)
                {
                    data[row[x]]++;
                }
            }

            // unlock bits
            try
            {
                b.UnlockBits(B_data);
            }
            catch (Exception inner)
            {
                log.Error("FailureDetector.histogram : Unable to unlock bits in bitmap.", inner);
                FailureDetectorException ex = new FailureDetectorException("FailureDetector.histogram : Unable to unlock bits in bitmap.", inner);
                throw ex;
            }
            
            return data;
        }
    }
    // Use for exceptinos generated in FailureDetector class
    public class FailureDetectorException : System.Exception
    {
        public FailureDetectorException() : base() { }
        public FailureDetectorException(string message) : base(message) { }
        public FailureDetectorException(string message, System.Exception inner) : base(message, inner) { }
        protected FailureDetectorException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
