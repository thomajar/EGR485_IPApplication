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
using log4net;
using System.Diagnostics;


namespace SAF_OpticalFailureDetector.camera
{
    class Camera
    {
        // thread synchronization, thread error, and logger
        private object _camLock;
        public event ThreadErrorHandler ThreadError;
        private static readonly ILog log = LogManager.GetLogger(typeof(Camera));

        // list of subscribers to send image data to
        private List<CircularQueue<QueueElement>> subscribers;

        // camera variables
        private ICImagingControl cam;
        private Boolean isRunning;
        private double lastTime;
        private int imageNumber;
        private String cameraName;
        private double exposure_s;

        // default settings
        private const float DEFAULT_FRAME_RATE = 15.0f;
        private const string DEFAULT_VIDEO_FORMAT = "Y800 (1280x960)";
        private const bool DEFAULT_ENABLE_AUTO_EXPOSURE = false;
        private const double DEFAULT_EXPOSURE_MS = 35.0;
        private const bool DEFAULT_ENABLE_AUTO_GAIN = false;
        private const double DEFAULT_GAIN_DB = 0.0;
        private const int DEFAULT_IMAGE_BUFFER_SIZE = 50;

        public bool Running { get { return isRunning; } }

        /// <summary>
        /// Constructor for Camera
        /// </summary>
        public Camera()
        {
            // create camera lock so only one thread can enter at a time
            _camLock = new object();

            // initialize variables
            subscribers = new List<CircularQueue<QueueElement>>();
            cam = new ICImagingControl();
            isRunning = false;
            lastTime = -1;
            imageNumber = 0;
            cameraName = "";
            exposure_s = 0;
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

            lock (_camLock)
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

            lock (_camLock)
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
        /// Attempts to start the camera specified by camNumber.  Exceptions will be thrown if the camera
        /// is unable to start up correctly.
        /// </summary>
        /// <param name="camName">Used for producer name in thread communication objects.</param>
        /// <param name="camNumber">Index of camera.</param>
        /// <exception cref="CameraException">Thrown by internal exceptions when attempting to start the camera.</exception>
        public void StartCamera(string camName, int camNumber)
        {
            // verify camera is not already running
            if (!isRunning)
            {
                this.cameraName = camName;
                // make sure we have enough devices connected to computer to attempt to connect to camera
                if(cam.Devices.Length > camNumber)
                {
                    cam.Device = cam.Devices[camNumber];
                    cam.VideoFormat = DEFAULT_VIDEO_FORMAT;
                    cam.DeviceFrameRate = DEFAULT_FRAME_RATE;
                    try
                    {
                        SetExposureAuto(DEFAULT_ENABLE_AUTO_EXPOSURE);
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "Camera.StartCamera : Unable to set auto exposure.";
                        CameraException ex = new CameraException(errMsg, inner);
                        log.Error(errMsg, ex);
                        throw ex;
                    }

                    try
                    {
                        SetExposure(DEFAULT_EXPOSURE_MS);
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "Camera.StartCamera : Unable to set exposure.";
                        CameraException ex = new CameraException(errMsg, inner);
                        log.Error(errMsg, ex);
                        throw ex;
                    }

                    try
                    {
                        SetGainAuto(DEFAULT_ENABLE_AUTO_GAIN);
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "Camera.StartCamera : Unable to set auto gain.";
                        CameraException ex = new CameraException(errMsg, inner);
                        log.Error(errMsg, ex);
                        throw ex;
                    }

                    try
                    {
                        SetGain(DEFAULT_GAIN_DB);
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "Camera.StartCamera : Unable to set gain.";
                        CameraException ex = new CameraException(errMsg, inner);
                        log.Error(errMsg, ex);
                        throw ex;
                    }
                    
                    cam.ImageRingBufferSize = DEFAULT_IMAGE_BUFFER_SIZE;
                    cam.DeviceLostExecutionMode = EventExecutionMode.AsyncInvoke;
                    cam.ImageAvailableExecutionMode = EventExecutionMode.MultiThreaded;
                    cam.OverlayBitmapPosition = PathPositions.None;
                    cam.LiveCaptureLastImage = false;
                    cam.DeviceLost += new EventHandler<ICImagingControl.DeviceLostEventArgs>(CameraLost);
                    cam.ImageAvailable += new EventHandler<ICImagingControl.ImageAvailableEventArgs>(ImageAvailable);
                    cam.LiveCaptureContinuous = true;

                    // attempt to start the camera, this validates all the properties we attemtped to set
                    try
                    {
                        cam.LiveStart();
                    }
                    catch (Exception inner)
                    {
                        isRunning = false;
                        string errMsg = "Camera.StartCamera : Unable to execute LiveStart.";
                        CameraException ex = new CameraException(errMsg, inner);
                        log.Error(errMsg, ex);
                        throw ex;
                    }
                    this.isRunning = true;
                }
                else
                {
                    string errMsg = "Camera.StartCamera : Cannot connect to camera index, it is higher than the number of cameras currently connected to computer.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                }
            }
            else
            {
                string errMsg = "Camera.StartCamera : Unable to start camera because it is already running.";
                CameraException ex = new CameraException(errMsg);
                log.Error(errMsg, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Stops the camera from recording images.
        /// </summary>
        /// <exception cref="CameraException"></exception>
        /// <returns>True if successful, False otherwise.</returns>
        public void StopCamera()
        {

            lock (_camLock)
            {
                // verify camera is not already stopped
                if (isRunning)
                {
                    isRunning = false;
                    cam.ImageAvailable -= new EventHandler<ICImagingControl.ImageAvailableEventArgs>(ImageAvailable);
                    cam.DeviceLost -= new EventHandler<ICImagingControl.DeviceLostEventArgs>(CameraLost);
                    try
                    {
                        cam.LiveStop();
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "Camera.StopCamera : Unable to execute live stop.";
                        CameraException ex = new CameraException(errMsg, inner);
                        log.Error(errMsg, ex);
                    }
                } 
            }

        }


        public void SetExposureByGain(double gain)
        {
            double tmp_exposure = exposure_s * gain;

            try
            {
                SetExposure(tmp_exposure);
            }
            catch (Exception inner)
            {
                string errMsg = "Camera.SetExposureByGain : Unable to set exposure.";
                CameraException ex = new CameraException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
        }
        //cam.GainExposure(desiredGain);

        /// <summary>
        /// Changes the state of camera's auto exposure to either on or off.
        /// </summary>
        /// <param name="enable">True if enabling, false if disabling.</param>
        /// <exception cref="CameraException"></exception>
        public void SetExposureAuto( Boolean enable)
        {
            lock (_camLock)
            {
                // check that device is valid before attempting to change it.
                if (!cam.DeviceValid)
                {
                    string errMsg = "Camera.SetExposureAuto : Camera is not valid, cannot change auto exposure state.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                }

                VCDSwitchProperty autoExposureInterface;
                try
                {
                    autoExposureInterface = (VCDSwitchProperty)cam.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure +
                        ":" + VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                }
                catch (Exception inner)
                {
                    string errMsg = "Camera.SetExposureAuto : Unable to obtain auto exposure interface.";
                    CameraException ex = new CameraException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
                if (autoExposureInterface != null && autoExposureInterface.Available)
                {
                    autoExposureInterface.Switch = enable;
                }
                else
                {
                    string errMsg = "Camera.SetExposureAuto : Error setting auto exposure value -- parameter not available.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                } 
            }
        }

        /// <summary>
        /// Changes the duration of the camera's exposure time.
        /// </summary>
        /// <param name="exposure_ms">Duration to exposure camera's pixels.</param>
        /// <exception cref="CameraException"></exception>
        public void SetExposure(double exposure_ms)
        {
            lock (_camLock)
            {
                // check that device is valid before attempting to change it.
                if (!cam.DeviceValid)
                {
                    string errMsg = "Camera.SetExposure : Camera is not valid, cannot change exposure.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                }

                VCDAbsoluteValueProperty exposureInterface;
                try
                {
                    exposureInterface = (VCDAbsoluteValueProperty)cam.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Exposure +
                        ":" + VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_AbsoluteValue);
                }
                catch (Exception inner)
                {

                    string errMsg = "Camera.SetExposure : Unable to obtain exposure interface.";
                    CameraException ex = new CameraException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
                if (exposureInterface != null && exposureInterface.Available)
                {
                    exposureInterface.Value = exposure_ms / 1000.0;
                    Thread.Sleep(10);
                    exposure_s = Convert.ToDouble(exposureInterface.Value);
                }
                else
                {

                    string errMsg = "Camera.SetExposure : Error setting exposure value -- parameter not available.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                }
                
            }
        }

        /// <summary>
        /// Attempts to set the camera's auto gain mode.
        /// </summary>
        /// <param name="enable">True to enable auto gain, false otherwise.</param>
        /// <exception cref="CameraException"></exception>
        public void SetGainAuto(Boolean enable)
        {
            lock (_camLock)
            {
                // check that device is valid before attempting to change it.
                if (!cam.DeviceValid)
                {
                    string errMsg = "Camera.SetGainAuto : Camera is not valid, cannot change auto gain status.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                }

                VCDSwitchProperty exposureInterface;
                try
                {
                    exposureInterface = (VCDSwitchProperty)cam.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain +
                        ":" + VCDIDs.VCDElement_Auto + ":" + VCDIDs.VCDInterface_Switch);
                }
                catch (Exception inner)
                {
                    string errMsg = "Camera.SetGainAuto : Unable to obtain auto gain interface.";
                    CameraException ex = new CameraException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
                if (exposureInterface != null && exposureInterface.Available)
                {
                    exposureInterface.Switch = enable;
                }
                else
                {
                    string errMsg = "Camera.SetGainAuto : Error setting auto gain status -- parameter not available.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                } 
            }
        }

        /// <summary>
        /// Method changes the gain setting on camera.
        /// </summary>
        /// <param name="gain_dB">Value of camera gain.</param>
        /// <exception cref="CameraException"></exception>
        public void SetGain(double gain_dB)
        {
            lock (_camLock)
            {
                // check that device is valid before attempting to change it.
                if (!cam.DeviceValid)
                {
                    string errMsg = "Camera.SetGain : Camera is not valid, cannot change gain.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                }

                VCDAbsoluteValueProperty exposureInterface;
                try
                {
                    exposureInterface = (VCDAbsoluteValueProperty)cam.VCDPropertyItems.FindInterface(VCDIDs.VCDID_Gain +
                        ":" + VCDIDs.VCDElement_Value + ":" + VCDIDs.VCDInterface_AbsoluteValue);
                }
                catch (Exception inner)
                {
                    string errMsg = "Camera.SetGain : Unable to obtain gain interface.";
                    CameraException ex = new CameraException(errMsg, inner);
                    log.Error(errMsg, ex);
                    throw ex;
                }
                if (exposureInterface != null && exposureInterface.Available)
                {
                    exposureInterface.Value = gain_dB;
                }
                else
                {
                    string errMsg = "Camera.SetGain : Error setting gain value -- parameter not available.";
                    CameraException ex = new CameraException(errMsg);
                    log.Error(errMsg, ex);
                    throw ex;
                } 
            }
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
            catch (Exception inner)
            {
                string errMsg = "Camera.CameraLost : Attempted to execute LiveStop after losing camera, exception thrown.";
                CameraException ex = new CameraException(errMsg, inner);
                log.Error(ex);
            }


            isRunning = false;
            cam.ImageAvailable -= new EventHandler<ICImagingControl.ImageAvailableEventArgs>(ImageAvailable);
            cam.DeviceLost -= new EventHandler<ICImagingControl.DeviceLostEventArgs>(CameraLost);

            // generate exception to pass to any subscribers of the ThreadError event
            string errMsg2 = "Camera.CameraLost : Camera disconnected from system.";
            CameraException ex2 = new CameraException(errMsg2);
            ThreadErrorEventArgs er = new ThreadErrorEventArgs(errMsg2, ex2, true);
            ThreadErrorEventArgs.OnThreadError(this, ThreadError, er);
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
            
            // lock the image buffer so that it is not overwritten while we are saving it.
            buff.Lock();
            bool isImageSet = true;
            imageNumber++;
            IPData data = new IPData(elapsedTime,exposure_s, false, imageNumber);
            try
            {
                data.SetRawDataFromImage(buff.Bitmap);
            }
            catch (Exception inner)
            {
                string errMsg = "Camera.ImageAvailable : Unable to save image into raw data of IPData object.";
                CameraException ex = new CameraException(errMsg, inner);
                log.Error(errMsg, ex);
                isImageSet = false;
            }
            buff.Unlock();


            lock (_camLock)
            {
                // only push the image if stored successfully
                if (isImageSet)
                {
                    for (int i = 0; i < subscribers.Count; i++)
                    {
                        subscribers[i].push(new QueueElement(cameraName, data));
                    }
                } 
            }
            
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
