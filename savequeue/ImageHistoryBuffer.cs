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

namespace SAF_OpticalFailureDetector.savequeue
{
    class ImageHistoryBuffer
    {
        // thread synchronization
        private Semaphore sem;

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

        public ImageHistoryBuffer(String name, String LogFileLocation)
        {
            sem = new Semaphore(0, 1);

            // initialize queues
            consumerLogFileLocation = LogFileLocation;
            consumerName = CONSUMER_ROOTNAME + name;
            consumerQueue = new CircularQueue<QueueElement>(consumerName,CONSUMER_QUEUE_SIZE);
            subscribers = new List<CircularQueue<QueueElement>>();

            // initialize processing thread variables
            isRunning = false;

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

        /// <summary>
        /// This is the process that is run in a seperate thread
        /// </summary>
        private void Process()
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
                sem.WaitOne();
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    image = (IPData)imageElements[imageElements.Count - 1].Data;
                    if(sw.ElapsedMilliseconds >= 5000)
                    {
                        sw.Restart();
                        bufferImages[imageIndex] = image.GetCameraImage();
                        imageIndex = (imageIndex + 1) % 100;
                    }
                    if(!image.ContainsCrack())
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
                sem.Release();


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
