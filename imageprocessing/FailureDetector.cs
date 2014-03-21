using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAF_OpticalFailureDetector.threading;
using System.Threading;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    public unsafe class FailureDetector
    {
        // thread synchronization
        private Semaphore sem;

        // ip variables
        private int minimumContrast;
        private int noiseRange;

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

            // release control, end of initialization
            sem.Release();
        }

        /// <summary>
        /// Retrieves the consumer queue from the Failure Detector.  Add
        /// ImageData to this queue in order to get the image processed.
        /// </summary>
        /// <returns>Consumer Queueu</returns>
        public CircularQueue<QueueElement> GetConsumerQueue()
        {
            return consumerQueue;
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

        private void Process()
        {
            // keep thread running until told to stop or start
            while (isRunning)
            {
                List<QueueElement> imageElements = new List<QueueElement>();
                sem.WaitOne();
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    // perform image processing

                    // check histogram update -- feedback to camera

                    // check roi update
                }
                sem.Release();
            }
        }
        

    }
}
