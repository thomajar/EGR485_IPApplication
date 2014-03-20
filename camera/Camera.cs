using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAF_OpticalFailureDetector.threading;
using System.Threading;

namespace SAF_OpticalFailureDetector.camera
{
    public class Camera
    {
        // lock to protect mutual exclusion
        private Semaphore sem;

        private Boolean isRunning;
        private List<CircularQueue<QueueElement>> subscribers;

        public Camera()
        {
            // create camera lock so only one thread can enter at a time
            sem = new Semaphore(0, 1);
            isRunning = false;
            subscribers = new List<CircularQueue<QueueElement>>();
            sem.Release();
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
                if(test.Name == subscriber.Name)
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

        public bool StartCamera()
        {
            Boolean brunning = false;
            // wait for sem control to enter critical section
            sem.WaitOne();
            // verify camera is not already running
            if (!isRunning)
            {
                // start camera
                brunning = true;
                isRunning = true;
            }
            // release control, exit critical section
            sem.Release();
            return brunning;
        }

        public bool StopCamera()
        {
            Boolean brunning = false;
            // wait for sem control to enter critical section
            sem.WaitOne();
            // verify camera is not already stopped
            if(isRunning)
            {
                // stop camera
                brunning = true;
                isRunning = false;
            }
            // release control, exit critical section
            sem.Release();
            return brunning;
        }
    }
}
