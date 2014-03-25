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

namespace SAF_OpticalFailureDetector.imageprocessing
{
    unsafe class FailureDetector
    {
        // thread synchronization
        private Semaphore sem;

        // ip variables
        private int minimumContrast;
        private int noiseRange;
        private Boolean enableROI;
        private Rectangle roi;
        private Boolean enableAutoExposure;
        private int targetIntesity;

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

            // initialize processing thread variables
            isRunning = false;
            processThread = new Thread(new ThreadStart(Process));

            // defaults
            minimumContrast = 15;
            noiseRange = 15;
            enableROI = false;
            enableAutoExposure = false;
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

        private void Process()
        {
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

                    Size s = image.GetCameraImage().Size;
                    Bitmap processImage = image.GetCameraImage();//new Bitmap(s.Width,s.Height);
                    //Graphics g = Graphics.FromImage(processImage);
                    //g.DrawImage(image.GetCameraImage(),0,0,s.Width,s.Height);
                    //g.Dispose();

                    // update roi
                    //updateROI(processImage);

                    // perform image processing
                    filterImage(processImage);
                    if (processImage != null)
                    {
                        //processImage.Save("tmp.bmp");
                    }
                    image.SetProcessedImage(processImage);
                    image.SetContainsCrack(true);

                    image.SetProcessTime(((Double)sw.ElapsedMilliseconds) / 1000);
                    sw.Restart();

                    // check histogram update -- feedback to camera

                   

                    // pop IPData onward
                    for (int i = 0; i < subscribers.Count; i++)
                    {
                        subscribers[i].push(new QueueElement(consumerName, image));
                    }

                    for (int i = 0; i < imageElements.Count - 1; i++)
                    {
                        ((IPData)imageElements[i].Data).Dispose();
                        ((IPData)imageElements[i].Data).Unlock();
                    }
                }
                sem.Release();
                
                
            }
        }

        private Bitmap filterImage(Bitmap b)
        {
            int PixelSize = 3;
            int threshold = noiseRange * 8;
            int result = 0;
            int row_count = 0;
            BitmapData B_data = null;
            int[] H = new int[] { 3, 1, -1, -6, -1, 1, 3 };

            try
            {
                B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            }
            catch (Exception)
            {

                return null;
            }
                


            for (int y = 0; y < b.Height; y++)
            {
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);
                bool Ishot = false;
                for (int x = 3; x < B_data.Width - 3; x++)
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
            b.UnlockBits(B_data);
            return b;//(double)row_count / (double)R.Height;
        }

        public int updateROI(Bitmap b)
        {
            roi = new Rectangle(0, 0, b.Width, b.Height);
            int percentwhite = 80;
            int PixelSize = 3;
            int max_index = 0;

            BitmapData B_data = null;

            // get histogram of image
            int[] hist = histogram(b);

            // get weighted average of image
            int centerOfMass = weightedIntensity(hist);


            B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);


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

            b.UnlockBits(B_data);
            return centerOfMass;
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
            int[] H = new int[] { 3, 1, -1, -6, -1, 1, 3 };

            B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);


            for (int y = 0; y < b.Height; y++)
            {
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);
                for (int x = 0; x < B_data.Width; x++)
                {
                    data[row[x]]++;
                }
            }
            b.UnlockBits(B_data);
            return data;
        }


          

    }
}
