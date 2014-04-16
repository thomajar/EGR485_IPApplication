using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAF_OpticalFailureDetector.threading;
using System.Threading;
using TIS.Imaging;
using SAF_OpticalFailureDetector.imageprocessing;
using System.Drawing;


namespace SAF_OpticalFailureDetector.camera
{
    class Camera
    {
        /// <summary>
        /// Used to synchronize critical sections in camera.
        /// </summary>
        private Semaphore sem;

        /// <summary>
        /// Flag stating true if camera is running.
        /// </summary>
        private Boolean isRunning;

        /// <summary>
        /// List of subscribers that camera populates with data.
        /// </summary>
        private List<CircularQueue<QueueElement>> subscribers;

        /// <summary>
        /// Interface to TIS camera.
        /// </summary>
        private ICImagingControl cam;

        /// <summary>
        /// Used to keep track of the last time image snapped on camera.
        /// </summary>
        private double lastTime = -1.0;

        /// <summary>
        /// Constructor for Camera
        /// </summary>
        public Camera()
        {
            // create camera lock so only one thread can enter at a time
            sem = new Semaphore(0, 1);

            // initialize variables
            isRunning = false;
            subscribers = new List<CircularQueue<QueueElement>>();
            cam = new ICImagingControl();

            // unlock critical section
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

        /// <summary>
        /// Starts up camera and begins populating consumers with
        /// image data.
        /// </summary>
        /// <param name="camNumber">Index of camera starting at 0.</param>
        /// <returns>True if started, False otherwise.</returns>
        public bool StartCamera(int camNumber)
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
                if(cam.Devices.Length > camNumber)
                {
                    cam.Device = cam.Devices[camNumber];
                    cam.VideoFormat = cam.VideoFormats[44];
                    cam.DeviceFrameRate = cam.DeviceFrameRates[8];
                    cam.DeviceLostExecutionMode = EventExecutionMode.AsyncInvoke;
                    cam.ImageAvailableExecutionMode = EventExecutionMode.MultiThreaded;
                    cam.OverlayBitmapPosition = PathPositions.None;
                    cam.LiveCaptureLastImage = false;
                    cam.ImageRingBufferSize = 50;
                    cam.DeviceLost += new EventHandler<ICImagingControl.DeviceLostEventArgs>(CameraLost);
                    cam.ImageAvailable += new EventHandler<ICImagingControl.ImageAvailableEventArgs>(ImageAvailable);
                    cam.LiveCaptureContinuous = true;
                    try
                    {
                        cam.LiveStart();
                    }
                    catch (Exception)
                    {
                        isRunning = false;
                        brunning = false;
                    }
                }
            }
            // release control, exit critical section
            sem.Release();
            return brunning;
        }

        /// <summary>
        /// Stops the camera from recording images.
        /// </summary>
        /// <returns>True if successful, False otherwise.</returns>
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
                cam.ImageAvailable -= new EventHandler<ICImagingControl.ImageAvailableEventArgs>(ImageAvailable);
                cam.LiveCaptureContinuous = true;
                cam.LiveStop();
            }
            // release control, exit critical section
            sem.Release();
            return brunning;
        }

        public void SetExposure(double exposure)
        {

        }

        /// <summary>
        /// Called when camera is disconnected from PC.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CameraLost(object sender, ICImagingControl.DeviceLostEventArgs e)
        {
            try
            {
                cam.LiveStop();
            }
            catch (Exception) { }
            isRunning = false;
            cam.ImageAvailable -= new EventHandler<ICImagingControl.ImageAvailableEventArgs>(ImageAvailable);
        }

        /// <summary>
        /// Called when an image becomes available from camera.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ImageAvailable(object sender, ICImagingControl.ImageAvailableEventArgs e)
        {
            ImageBuffer buff = cam.ImageBuffers[e.bufferIndex];

            double elapsedTime = -1.0;
            double time = cam.ReferenceTimeCurrent;
            // get time elapsed
            if (lastTime > 0.0)
            {
                elapsedTime = time - lastTime;
            }
            lastTime = time;
            buff.Lock();
            IPData data = new IPData(elapsedTime);
            data.SetCameraImage(buff.Bitmap);
            buff.Unlock();

            sem.WaitOne();
            for (int i = 0; i < subscribers.Count; i++)
            {
                subscribers[i].push(new QueueElement("Camera", data));
            }
            sem.Release();
        }
    }

    // Use for exceptinos generated in Camera class
    public class CameraException : System.Exception
    {
        public CameraException() : base() { }
        public CameraException(string message) : base(message) { }
        public CameraException(string message, System.Exception inner) : base(message, inner) { }
        protected CameraException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
