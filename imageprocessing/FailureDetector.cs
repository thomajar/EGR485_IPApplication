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

        private const float INTENSITY_GAIN_THRESHOLD = 1.05f;

        // consumer queue variables
        private const String CONSUMER_ROOTNAME = "IP_";
        private const int CONSUMER_QUEUE_SIZE = 1000;
        CircularQueue<QueueElement> consumerQueue;
        private String consumerName;

        // subscriber queue variables
        List<CircularQueue<QueueElement>> subscribers;

        // thread variables
        private Boolean isRunning;
        private Thread processThread;

        public FailureDetector(String name)
        {
            sem = new Semaphore(0, 1);

            // initialize queues
            consumerName = CONSUMER_ROOTNAME + name;
            consumerQueue = new CircularQueue<QueueElement>(consumerName,CONSUMER_QUEUE_SIZE);
            subscribers = new List<CircularQueue<QueueElement>>();
            log.Info("FailureDetector.FailureDetector : Created consumer queue : " + consumerName);

            // initialize processing thread variables
            isRunning = false;

            // defaults
            minimumContrast = 15;
            noiseRange = 15;
            enableROI = false;
            updateROIFrequency = 100;
            enableAutoExposure = false;
            updateExposureFrequency = 50;
            targetIntesity = 200;

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
                log.Info("FailureDetector.AddSubscriber : Added subscriber: " + subscriber.Name + " to " + consumerName + " queue.");
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
                log.Info("FailureDetector.AddSubscriber : Removed subscriber: " + subscriber.Name + " from " + consumerName + " queue.");
                subscribers.RemoveAt(removeIndex);
            }
            // release control, exit critical section
            sem.Release();
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
                processThread = new Thread(new ThreadStart(Process2));
                try
                {
                    processThread.Start();
                }
                catch (Exception inner)
                {
                    log.Error("FailureDetector.Start : Unable to start process thread.", inner);
                    FailureDetectorException ex = new FailureDetectorException("FailureDetector.Start : Unable to start process thread.", inner);
                    throw ex;
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
            if (isRunning)
            {
                result = true;
                isRunning = false;
                try
                {
                    processThread.Join();
                }
                catch (Exception inner)
                {
                    log.Error("FailureDetector.Start : Unable to stop process thread.", inner);
                    FailureDetectorException ex = new FailureDetectorException("FailureDetector.Start : Unable to stop process thread.", inner);
                    throw ex;
                }
                
            }
            return result;
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

        public String GetName()
        {
            return consumerName;
        }

        private void Process()
        {
            int exposureCounter = 0;
            int roiCounter = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // keep thread running until told to stop or start
            while (isRunning)
            {
                List<QueueElement> imageElements = new List<QueueElement>();
                IPData image = null;
                sem.WaitOne();
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    image = (IPData)imageElements[imageElements.Count - 1].Data;


                    // get the image to process on
                    Bitmap processImage = image.GetRawDataImage();//image.GetCameraImage();
                    /*Stopwatch awatch = new Stopwatch();
                    awatch.Start();
                    byte[] imageData = image.GetRawData();
                    long times = awatch.ElapsedMilliseconds;
                    awatch.Restart();
                    image.SetProcessedData(imageData);
                    long times2 = awatch.ElapsedMilliseconds;
                    awatch.Stop();*/

                    // update roi to process
                    if (enableROI)
                    {
                        roiCounter = (roiCounter + 1) % updateROIFrequency;
                        if (roiCounter == 0)
                        {
                            // store old roi incase of an exception
                            Rectangle oldROI = roi;
                            try
                            {
                                updateROI(processImage);
                            }
                            catch (Exception inner)
                            {
                                log.Error("FailureDetetor.Process : Exception thrown while updating ROI, reverting to old ROI.",inner);
                                roi = oldROI;
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
                                log.Error("FailureDetector.Process : Exception thrown while calculating histogram of iamge.", inner);
                            }

                            // get center of mass
                            int imageIntensity = -1;
                            try
                            {
                                imageIntensity = weightedIntensity(hist);
                            }
                            catch (Exception inner)
                            {
                                log.Error("FailureDetector.Process : Exception thrown while calculating weighted intensity.", inner);
                            }
                            // verify imageIntensity was successfully calculated
                            if (imageIntensity > -1)
                            {
                                // update ROI
                                float desiredGain = (float)targetIntesity / (float)imageIntensity;
                                // make sure gain is above change threshold
                                if (desiredGain > INTENSITY_GAIN_THRESHOLD)
                                {
                                    //cam.GainExposure(desiredGain) ~ this needs to be implemented
                                }
                                
                            }
                        }
                    }

                    // perform image processing
                    try
                    {
                        processImage = filterImage(processImage);
                    }
                    catch (Exception inner)
                    {
                        log.Error("MainForm.Process : Exception thrown while filtering image.", inner);
                    }
                    
                    // set the processed image after processing
                    image.SetProcessedDataFromImage(processImage);

                    // set image to contain crack
                    image.ContainsCrack = true;

                    // set the amount of time spent processing
                    image.ProcessorElapsedTime_s = ((Double)sw.ElapsedMilliseconds) / 1000;
                    sw.Restart();

                    // pop IPData onward
                    for (int i = 0; i < subscribers.Count; i++)
                    {
                        subscribers[i].push(new QueueElement(consumerName, image));
                    }
                    //processImage.Dispose();

                    // dispose and unlock unused images
                    for (int i = 0; i < imageElements.Count - 1; i++)
                    {
                        //((IPData)imageElements[i].Data).Dispose();
                        //((IPData)imageElements[i].Data).Unlock();
                    }
                }
                sem.Release();
            }
        }

        private void Process2()
        {
            int exposureCounter = 0;
            int roiCounter = 0;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            // keep thread running until told to stop or start
            while (isRunning)
            {
                List<QueueElement> imageElements = new List<QueueElement>();
                IPData image = null;
                sem.WaitOne();
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    image = (IPData)imageElements[imageElements.Count - 1].Data;

                    Byte[] data = image.GetRawData();

                    if (data != null)
                    {
                        if (data.Length > image.Image_Offset)
                        {
                            Byte[] processed = null;
                            // image is valid at this point
                            try
                            {
                                processed = filterImage2(data, image.Image_Offset, image.BytesPerPixel, image.ImageSize);
                            }
                            catch (Exception inner)
                            {
                                log.Error("MainForm.Process : Exception thrown while filtering image.", inner);
                            }
                            if (processed != null)
                            {
                                image.SetProcessedData(processed);
                            }
                            
                        }
                    }


                    //// get the image to process on
                    //Bitmap processImage = image.GetRawDataImage();//image.GetCameraImage();
                    ///*Stopwatch awatch = new Stopwatch();
                    //awatch.Start();
                    //byte[] imageData = image.GetRawData();
                    //long times = awatch.ElapsedMilliseconds;
                    //awatch.Restart();
                    //image.SetProcessedData(imageData);
                    //long times2 = awatch.ElapsedMilliseconds;
                    //awatch.Stop();*/

                    //// update roi to process
                    //if (enableROI)
                    //{
                    //    roiCounter = (roiCounter + 1) % updateROIFrequency;
                    //    if (roiCounter == 0)
                    //    {
                    //        // store old roi incase of an exception
                    //        Rectangle oldROI = roi;
                    //        try
                    //        {
                    //            //updateROI(processImage);
                    //        }
                    //        catch (Exception inner)
                    //        {
                    //            log.Error("FailureDetetor.Process : Exception thrown while updating ROI, reverting to old ROI.", inner);
                    //            roi = oldROI;
                    //        }
                    //    }
                    //}

                    //// update exposure of camera
                    //if (enableAutoExposure)
                    //{
                    //    exposureCounter = (exposureCounter + 1) % updateExposureFrequency;
                    //    if (exposureCounter == 0)
                    //    {
                    //        // get histogram from image
                    //        int[] hist = null;
                    //        try
                    //        {
                    //            //hist = histogram(processImage);
                    //        }
                    //        catch (Exception inner)
                    //        {
                    //            log.Error("FailureDetector.Process : Exception thrown while calculating histogram of iamge.", inner);
                    //        }

                    //        // get center of mass
                    //        int imageIntensity = -1;
                    //        try
                    //        {
                    //            //imageIntensity = weightedIntensity(hist);
                    //        }
                    //        catch (Exception inner)
                    //        {
                    //            log.Error("FailureDetector.Process : Exception thrown while calculating weighted intensity.", inner);
                    //        }
                    //        // verify imageIntensity was successfully calculated
                    //        if (imageIntensity > -1)
                    //        {
                    //            // update ROI
                    //            float desiredGain = (float)targetIntesity / (float)imageIntensity;
                    //            // make sure gain is above change threshold
                    //            if (desiredGain > INTENSITY_GAIN_THRESHOLD)
                    //            {
                    //                //cam.GainExposure(desiredGain) ~ this needs to be implemented
                    //            }

                    //        }
                    //    }
                    //}

                    //// perform image processing
                    //try
                    //{
                    //    processImage = filterImage(processImage);
                    //}
                    //catch (Exception inner)
                    //{
                    //    log.Error("MainForm.Process : Exception thrown while filtering image.", inner);
                    //}

                    // set the processed image after processing
                    //image.SetProcessedDataFromImage(processImage);

                    // set image to contain crack
                    image.ContainsCrack = true;

                    // set the amount of time spent processing
                    image.ProcessorElapsedTime_s = ((Double)sw.ElapsedMilliseconds) / 1000;
                    sw.Restart();

                    // pop IPData onward
                    for (int i = 0; i < subscribers.Count; i++)
                    {
                        subscribers[i].push(new QueueElement(consumerName, image));
                    }
                    //processImage.Dispose();

                    // dispose and unlock unused images
                    for (int i = 0; i < imageElements.Count - 1; i++)
                    {
                        //((IPData)imageElements[i].Data).Dispose();
                        //((IPData)imageElements[i].Data).Unlock();
                    }
                }
                sem.Release();
            }
        }

        private Byte[] filterImage2(Byte[] data, int offset, int bpp, Size s)
        {
            int threshold = noiseRange * 8;
            int result = 0;
            // filter
            int[] H = new int[] { 3, 1, -1, -6, -1, 1, 3 };


            Rectangle regionToProcess;
            if (enableROI)
            {
                regionToProcess = roi;
            }
            else
            {
                regionToProcess = new Rectangle(0, 0, s.Width, s.Height);
            }

            for (int y = regionToProcess.Top; y < regionToProcess.Bottom; y++)
            {
                for (int x = regionToProcess.Left; x < regionToProcess.Right; x++)
                {
                    result = 0;
                    int filterOffset = 3;
                    int n = 7;
                    for (int i = 0; i < n; i++)
                    {
                        result += data[(y * s.Width + (x - filterOffset + i)) * bpp + offset] * H[i];
                    }
                    if (result > threshold)
                    {
                        if (data[(y * s.Width + (x - filterOffset)) * bpp + offset] > data[(y * s.Width + x) * bpp + offset] + minimumContrast &&
                            data[(y * s.Width + (x + filterOffset)) * bpp + offset] > data[(y * s.Width + x) * bpp + offset] + minimumContrast)
                        {
                            // color pixel green
                            data[(y * s.Width + x) * bpp + offset + 0] = 0;   //Blue  0-255
                            data[(y * s.Width + x) * bpp + offset + 1] = 255; //Green 0-255
                            data[(y * s.Width + x) * bpp + offset + 2] = 0;   //Red   0-255
                        }
                    }
                }
            }
            return data;
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
