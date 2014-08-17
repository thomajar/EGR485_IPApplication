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
using SAF_OpticalFailureDetector.camera;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    unsafe class FailureDetector
    {
        private int throttle_period_ms;
        public event ThreadErrorHandler ThreadError;

        // thread synchronization
        private object _ipLock;

        private static readonly ILog log = LogManager.GetLogger(typeof(FailureDetector));

        // ip variables
        private int minimumContrast;
        private int noiseRange;
        private Boolean enableROI;
        private int updateROIPeriod_ms;
        private Rectangle roi;
        private Boolean enableAutoExposure;
        private int updateExposurePeriod_ms;
        private int targetIntesity;
        private int lineLengthThresh;
        private int crackLengthThresh;
	private Rectangle regionToProcess;
        private double connectCrackMaxDistanceThresh;
        private double connectCrackAbsDistanceThresh;

        private Camera cam;

        private const float INTENSITY_GAIN_THRESHOLD = 0.03f;

        // consumer queue variables
        private const int CONSUMER_QUEUE_SIZE = 1000;
        CircularQueue<QueueElement> consumerQueue;
        private String imageProcessorName;

        // subscriber queue variables
        List<CircularQueue<QueueElement>> subscribers;

        // thread variables
        private Boolean isRunning;
        private Thread processThread;

        private const int DEFAULT_THROTTLE_PERIOD = 250;
        private const int DEFAULT_MIN_CONTRAST = 15;
        private const int DEFAULT_MIN_NOISE_LVL = 15;
        private const bool DEFAULT_ENABLE_ROI = false;
        private const int DEFAULT_ROI_UPDATE_FREQUENCY = 100;
        private const bool DEFAULT_ENABLE_AUTO_EXPOSURE = false;
        private const int DEFAULT_EXPOSURE_UPDATE_FREQUENCY = 50;
        private const int DEFAULT_TARGET_INTENSITY = 50;
        private const int DEFAULT_ROI_UPDATE_PERIOD_MS = 5000;
        private const int DEFAULT_EXPOSURE_UPDATE_PERIOD_MS = 2500;
        private const int DEFAULT_LINE_LENGTH_THRESH = 12;
        private const int DEFAULT_CRACK_LENGTH_THRESH = 40;
        private const int DEFAULT_CONNECT_LINES_ABS_PASS_THRESH = 5;
        private const int DEFAULT_CONNECT_LINES_MAXIMUM_PASS_THRESH = 15;


        public bool Running { get { return isRunning; } }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public FailureDetector(String name, ref Camera cam)
        {
            _ipLock = new object();

            this.imageProcessorName = name;
            subscribers = new List<CircularQueue<QueueElement>>();

            // initialize processing thread variables
            isRunning = false;

            this.cam = cam;

            // defaults
            throttle_period_ms = DEFAULT_THROTTLE_PERIOD;
            minimumContrast = DEFAULT_MIN_CONTRAST;
            noiseRange = DEFAULT_MIN_NOISE_LVL;
            enableROI = DEFAULT_ENABLE_ROI;
            updateROIPeriod_ms = DEFAULT_ROI_UPDATE_FREQUENCY;
            enableAutoExposure = DEFAULT_ENABLE_AUTO_EXPOSURE;
            updateExposurePeriod_ms = DEFAULT_EXPOSURE_UPDATE_FREQUENCY;
            targetIntesity = DEFAULT_TARGET_INTENSITY;
            lineLengthThresh = DEFAULT_LINE_LENGTH_THRESH;
            crackLengthThresh = DEFAULT_CRACK_LENGTH_THRESH;
            connectCrackAbsDistanceThresh = DEFAULT_CONNECT_LINES_ABS_PASS_THRESH;
            connectCrackMaxDistanceThresh = DEFAULT_CONNECT_LINES_MAXIMUM_PASS_THRESH;
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

            lock (_ipLock)
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
                    log.Info("FailureDetector.AddSubscriber : Added subscriber: " + subscriber.Name + ".");
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

            lock (_ipLock)
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
                    log.Info("FailureDetector.AddSubscriber : Removed subscriber: " + subscriber.Name + ".");
                    subscribers.RemoveAt(removeIndex);
                }
                
            }
            return !doesNotExist;
        }

        /// <summary>
        /// Starts the image processing thread.
        /// </summary>
        /// <exception cref="FailureDetectorException"></exception>
        public void Start(int throttle_period_ms)
        {
            lock (_ipLock)
            {
                if (!isRunning)
                {
                    this.throttle_period_ms = throttle_period_ms;
                    processThread = new Thread(new ThreadStart(Process));
                    try
                    {
                        processThread.Start();
                    }
                    catch (Exception inner)
                    {
                        string errMsg = "FailureDetector.Start : Unable to start process thread.";
                        FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                        log.Error(errMsg, ex);
                        throw ex;
                    }
                    isRunning = true;
                }
                else
                {
                    string errMsg = "FailureDetector.Start : Processor thread is already running.";
                    FailureDetectorException ex = new FailureDetectorException(errMsg);
                    log.Error(errMsg);
                    throw ex;
                }
                
            }
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
            lock (_ipLock)
            {
                this.noiseRange = range;
            }
        }
        public void SetContrast(int contrast)
        {
            lock (_ipLock)
            {
                this.minimumContrast = contrast; 
            }
        }

        public void SetTargetIntensity(int intensity)
        {
            lock (_ipLock)
            {
                if (intensity > 255)
                {
                    intensity = 255;
                }
                if (intensity < 0)
                {
                    intensity = 0;
                }
                targetIntesity = intensity;
            }
        }

	public void SetRegionToProcess(Rectangle regionToProcess)
	{
            lock (_ipLock)
            {
		this.regionToProcess = regionToProcess;
	    }
	}

        public void EnableAutoExposure(bool enable)
        {
            lock (_ipLock)
            {
                this.enableAutoExposure = enable; 
            }
        }

        public void EnableAutoROI(bool enable)
        {
            lock (_ipLock)
            {
                this.enableROI = enable;
            }
        }

        private void Process()
        {
            int imageIntensity = -1;

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Stopwatch _threadTimer = new Stopwatch();
            _threadTimer.Start();

            Stopwatch _exposureUpdateTimer = new Stopwatch();
            _exposureUpdateTimer.Start();

            Stopwatch _roiUpdateTimer = new Stopwatch();
            _roiUpdateTimer.Start();

            // keep thread running until told to stop or start
            while (isRunning)
            {
                int timeToSleep = throttle_period_ms - Convert.ToInt32(_threadTimer.ElapsedMilliseconds);
                if (timeToSleep > 0)
                {
                    Thread.Sleep(timeToSleep);
                }
                _threadTimer.Restart();
                
                List<QueueElement> imageElements = new List<QueueElement>();
                IPData image = null;
                // grab all image data off of queue
                consumerQueue.popAll(ref imageElements);
                if (imageElements.Count > 0)
                {
                    // convert object to IPData class
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
                            if (_roiUpdateTimer.ElapsedMilliseconds >= updateROIPeriod_ms)
                            {
                                _roiUpdateTimer.Restart();
                                // store old roi incase of an exception
                                Rectangle oldROI = new Rectangle(roi.X,roi.Y,roi.Width,roi.Height);
                                try
                                {
                                    roi = regionToProcess;
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
                        else
                        {
                            roi = new Rectangle(0, 0, processImage.Width, processImage.Height);
                        }

                        // update exposure of camera
                        if (enableAutoExposure)
                        {
                            if (_exposureUpdateTimer.ElapsedMilliseconds >= updateExposurePeriod_ms)
                            {
                                updateExposurePeriod_ms = DEFAULT_EXPOSURE_UPDATE_PERIOD_MS;
                                _exposureUpdateTimer.Restart();
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
                                        try
                                        {
                                            cam.SetExposureByGain(desiredGain);
                                        }
                                        catch (Exception inner)
                                        {
                                            string errMsg = "FailureDetector.Process : Exception thrown changing exposure on cameras.";
                                            FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                                            log.Error(errMsg, ex);
                                        }
                                        updateExposurePeriod_ms = 1000;
                                    }
                                }
                            }
                        }

                        // perform image processing
                        Boolean isImageFiltered = true;
                        int crackCount = 0;
                        int crackConfidence = 0;
                        try
                        {
                            //crackConfidence = filterImage(ref processImage);
                            crackCount = detectCracks(ref processImage);
                        }
                        catch (Exception inner)
                        {
                            string errMsg = "FailureDetector.Process : Exception thrown while filtering image.";
                            FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                            log.Error(errMsg, ex);
                            isImageFiltered = false;
                        }

                        if (enableROI)
                        {
                            try
                            {
                                DrawROI(ref processImage);
                            }
                            catch (Exception inner)
                            {
                                string errMsg = "FailureDetector.Process : Exception thrown while drawing roi.";
                                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                                log.Error(errMsg, ex);
                                isImageFiltered = false;
                                
                            }
                        }

                        if (isImageFiltered)
                        {
                            bool isCracked = false;
                            if (crackConfidence > 80 || crackCount > 0)
                            {
                                isCracked = true;
                            }
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
                            image.SetIPMetaData(((Double)sw.ElapsedMilliseconds) / 1000, roi, imageIntensity, crackCount, isCracked, true);
                            sw.Restart();

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
        /// Function processes a given image(bitmap) to look for cracks running vertically
        /// through the image.  The algorithm is opimized for small cracks less than 10 pixels
        /// wide. The total number of suspected cracks is returned from function.
        /// </summary>
        /// <param name="b">Image to perform image processing on.</param>
        /// <returns>Number of suspected cracks detected.</returns>
        private int detectCracks(ref Bitmap b)
        {
            // list of all pixels that may be part of crack
            List<Point> crackPixels = null;

            // states wheter pixel passed condition to be part of crack
            Boolean[,] isCrackedPixel = null;

            // list of lines found to possibly be a crack
            List<Line> lines = null;

            // connected lines formed from lines
            List<LineCollection> cracks = null;

            // run filter algorithm --> finds pixels that may be part of a crack
            try
            {
                findCrackPixels(ref b, ref crackPixels, ref isCrackedPixel);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.detectCracks : Error occured while finding cracked pixels.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            // run line detector algorithm --> connects cracted pixels together to form lines.
            try
            {
                findCrackLines(ref crackPixels, ref isCrackedPixel, b.Width, b.Height, ref lines);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.detectCracks : Error occured finding crack lines.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            // run line connection algorithm --> connects lines that have start/endpoints close to eachother
            try
            {
                connectCrackLines(ref lines, ref cracks);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.detectCracks : Error occured connecting crack lines.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            // run crack filter algorithm --> filters out cracks/lines that are too short
            int crackCount = 0;
            try
            {
                crackCount = filterLinesAndCracks(ref b, ref lines, ref cracks);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.detectCracks : Error occured filtering lines and cracks.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            
            return crackCount;
        }

        /// <summary>
        /// Goes through list of lines given and draws all of them to an image.  The line is colored
        /// according to the rgb argument given in the parameters.
        /// </summary>
        /// <param name="b">Image to draw to.</param>
        /// <param name="lines">Lines to draw.</param>
        /// <param name="red">Red color value of line 0-255.</param>
        /// <param name="green">Green color value of line 0-255.</param>
        /// <param name="blue">Blue color value of line 0-255.</param>
        private void drawCrackLines(ref Bitmap b, ref List<Line> lines, byte red, byte green, byte blue)
        {
            int PixelSize = 3;
            BitmapData B_data = null;

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

            // draw the best fit line for each line to image 2 pixels wide.
            foreach (Line line in lines)
            {
                for (int x = line.StartX; x < line.EndX; x++)
                {
                    int y1 = line.GetValueAt(x);
                    int y2 = line.GetValueAt(x + 1);
                    for (int i = 0; i < Math.Abs(y1-y2); i++)
                    {
                        int yDraw = y1 + (y2 - y1) / Math.Abs(y2 - y1) * i;

                        if (yDraw > -1 && yDraw < b.Height)
                        {
                            byte* row = (byte*)B_data.Scan0 + (yDraw * B_data.Stride);
                            row[x * PixelSize] = blue;
                            row[x * PixelSize + 1] = green;
                            row[x * PixelSize + 2] = red;
                            row[(x + 1) * PixelSize] = blue;
                            row[(x + 1) * PixelSize + 1] = green;
                            row[(x + 1) * PixelSize + 2] = red;
                        }
                    }
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
        }

        /// <summary>
        /// Filters image looking for pixels that may be part of a crack.  The pixels are found by
        /// applying a high frequency gain by sampling horizontally across the image.  By applying
        /// a gain in this way, a vertical crack can be detected.  A list of all pixels that may be 
        /// a crack is stored in crackPixels while a 2d bool array is filled with true if the pixel
        /// is part of a crack.
        /// </summary>
        /// <param name="b">Image to perform filter on.</param>
        /// <param name="crackPixels">List of points that may be part of crack.</param>
        /// <param name="isCrackedPixel">2d array of image pixels indicating of part of crack.</param>
        private void findCrackPixels(ref Bitmap b, ref List<Point> crackPixels, ref Boolean[,] isCrackedPixel)
        {
            const int PixelSize = 3;
            int[] H = new int[] { 3, 1, -1, -6, -1, 1, 3 };

            BitmapData B_data = null;
            int threshHold = noiseRange * 8;

            crackPixels = new List<Point>();
            isCrackedPixel = new Boolean[b.Height, b.Width];

            // make sure image is correct format
            if (b.PixelFormat != PixelFormat.Format24bppRgb)
            {
                string errMsg = "FailureDetector.findCrackPixels : Image to process has incorrect pixel format, must be PixelFormat.Format24bppRgb.";
                FailureDetectorException ex = new FailureDetectorException(errMsg);
                log.Error(errMsg, ex);
                throw ex;
            }

            // attempt to lock bits on bitmap to process
            try
            {
                B_data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height),
                    ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.findCrackPixels : Unable to lock bits to perform image processing.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            // get region to process
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
            int result = 0;
            int offset = 3;
            const int FILTER_SIZE = 7;
            try
            {
                for (int y = regionToProcess.Top; y < regionToProcess.Bottom; y++)
                {
                    byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);

                    for (int x = regionToProcess.Left + 3; x < regionToProcess.Right - 3; x++)
                    {
                        result = 0;
                        offset = 3;
                        for (int i = 0; i < FILTER_SIZE; i++)
                        {
                            result += row[(x - offset + i) * PixelSize] * H[i];
                        }
                        if (result > threshHold)
                        {
                            if (row[(x - offset) * PixelSize] > row[x * PixelSize] + minimumContrast &&
                                row[(x + offset) * PixelSize] > row[x * PixelSize] + minimumContrast)
                            {
                                // color pixel green
                                row[x * PixelSize] = 0;         // Blue  0-255
                                row[x * PixelSize + 1] = 255;   // Green 0-255
                                row[x * PixelSize + 2] = 0;     // Red   0-255

                                crackPixels.Add(new Point(x, y));
                                isCrackedPixel[y, x] = true;
                            }
                        }
                    }
                }
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.findCrackPixels : Error occured during image processing.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            try
            {
                b.UnlockBits(B_data);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.findCrackPixels : Unable to unlock bits.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
        }

        /// <summary>
        /// Loops through all of the pixels listed in crackPixels and looks for neighbors around it
        /// that may be part of a connected crack.  The cracks are then stored into a list of lines
        /// and returned in lines.
        /// 
        /// Function works by avoiding recursion directly and using a stack to store where the
        /// function is working on and pushing the current position when moving to a new position
        /// and popping from the stack whenever no moves are available in the current position.
        /// </summary>
        /// <param name="crackPixels">List of cracked pixels.</param>
        /// <param name="isCrackedPixel">2d bool array corresponding to image pixels stating if
        /// it is part of crack.</param>
        /// <param name="width">Width of image.</param>
        /// <param name="height">Height of image.</param>
        /// <param name="lines">Number of lines</param>
        private void findCrackLines(ref List<Point> crackPixels, ref Boolean[,] isCrackedPixel, int width, int height, ref List<Line> lines)
        {
            lines = new List<Line>();
            Stack<object> stack = new Stack<object>();
            bool[,] hasPixelBeenUsed = new bool[height, width];

            // loop through all pixels that may be part of a crack.
            foreach (Point testPoint in crackPixels)
            {
                Point p = testPoint;
                Point min = new Point(testPoint.X,testPoint.Y);
                Point max = new Point(testPoint.X,testPoint.Y);

                // check that pixel has not been used yet
                if (!hasPixelBeenUsed[p.Y,p.X])
                {
                    hasPixelBeenUsed[p.Y,p.X] = true;

                    // this is where we look for neighbors
                    bool keepSearching = true;
                    // start looking north and go clockwise around compass looking at 
                    // N, NE, E, SE, S, SW, W, NW
                    ContourState s = ContourState.North;
                    
                    // variables used to fit a line to data.
                    int count = 1;
                    int sumX = p.X;
                    int sumY = p.Y;
                    int sumX2 = (int)Math.Pow(p.X,2);
                    int sumXY = p.X * p.Y;

                    // keep moving through image until no more moves are available
                    while (keepSearching)
                    {
                        Point focus = p;
                        Point direction = Point.Empty;
                        // see what direction to move.
                        switch (s)
                        {
                            case ContourState.North:
                                direction = new Point(0, 1);
                                break;
                            case ContourState.NorthEast:
                                direction = new Point(1, 1);
                                break;
                            case ContourState.East:
                                direction = new Point(1, 0);
                                break;
                            case ContourState.SouthEast:
                                direction = new Point(1, -1);
                                break;
                            case ContourState.South:
                                direction = new Point(0, -1);
                                break;
                            case ContourState.SouthWest:
                                direction = new Point(-1, -1);
                                break;
                            case ContourState.West:
                                direction = new Point(-1, 0);
                                break;
                            case ContourState.NorthWest:
                                direction = new Point(-1, 1);
                                break;
                            default:
                                break;
                        }
                        // check to see if we can go that direction
                        Point checkPoint = new Point(p.X + direction.X, p.Y + direction.Y);
                        bool passedTests = false;
                        if (checkPoint.X >= 0 && checkPoint.Y >= 0 && checkPoint.X < width && checkPoint.Y < height)
                        {
                            if (isCrackedPixel[checkPoint.Y, checkPoint.X] && !hasPixelBeenUsed[checkPoint.Y, checkPoint.X])
                            {
                                // push our current position to stack before moving to new position
                                stack.Push(new Point(p.X, p.Y));
                                stack.Push(s);
                                // move our position to new pixel
                                p = checkPoint;
                                s = ContourState.North;
                                hasPixelBeenUsed[p.Y, p.X] = true;

                                // add to best fit line vars
                                count++;
                                sumX += p.X;
                                sumY += p.Y;
                                sumX2 += (int)Math.Pow(p.X, 2);
                                sumXY += p.X * p.Y;

                                // update min / max for length of line
                                if (p.X < min.X)
                                {
                                    min.X = p.X;
                                }
                                if (p.Y < min.Y)
                                {
                                    min.Y = p.Y;
                                }
                                if (p.X > max.X)
                                {
                                    max.X = p.X;
                                }
                                if (p.Y > max.Y)
                                {
                                    max.Y = p.Y;
                                }
                                passedTests = true;
                            }
                        }
                        if (!passedTests)
                        {
                            // northwest is the last state, must pop backwards
                            if (s == ContourState.NorthWest)
                            {
                                bool foundNewPoint = false;
                                while (!foundNewPoint)
                                {
                                    if (stack.Count > 1)
                                    {
                                        s = (ContourState)stack.Pop();
                                        p = (Point)stack.Pop();
                                        if (s != ContourState.NorthWest)
                                        {
                                            s++;
                                            foundNewPoint = true;
                                        }
                                    }
                                    else
                                    {
                                        // end case
                                        foundNewPoint = true;
                                        keepSearching = false;
                                    }
                                }
                            }
                            else
                            {
                                s++;
                            }
                        } 
                    }
                    // get the length of the line
                    int xdif = max.X - min.X;
                    int ydif = max.Y - min.Y;
                    int hdif = (int)Math.Sqrt(Math.Pow(xdif, 2) + Math.Pow(ydif, 2));

                    // check if line length is acceptable
                    if (hdif >= lineLengthThresh)
                    {
                        // best fit line to data
                        double xMean = (double)sumX / (double)count;
                        double yMean = (double)sumY / (double)count;
                        double slope = (double)(sumXY - sumX * yMean) / (double)(sumX2 - sumX * xMean);
                        double yInt = yMean - slope * xMean;
                        // use best fit line to calculate start point and end point
                        Point startPoint = new Point(min.X, (int)(slope*min.X + yInt));
                        Point endPoint = new Point(max.X, (int)(slope * max.X + yInt));
                        // may be a crack   
                        lines.Add(new Line(startPoint, endPoint, slope, yInt));
                    }
                }
            }
        }

        /// <summary>
        /// Calculates the distance between two points by calculating
        /// the hypotenuse.
        /// </summary>
        /// <param name="p1">Point 1.</param>
        /// <param name="p2">Point 2.</param>
        /// <returns></returns>
        private double hypotenuse(Point p1, Point p2)
        {
            return Math.Sqrt(Math.Pow(p2.X - p1.X, 2) + Math.Pow(p2.Y - p1.Y, 2));
        }

        /// <summary>
        /// Goes through all the lines and looks at the start point and end points.
        /// If the points are close enough together, than they can be connected. Checks
        /// to see that the lines are relatively going in the same direction.
        /// </summary>
        /// <param name="lines">List of lines to attempt to connect.</param>
        /// <param name="cracks">List of connected lines.</param>
        private void connectCrackLines(ref List<Line> lines, ref List<LineCollection> cracks)
        {
            cracks = new List<LineCollection>();
            
            // loop through all lines and attempt to connect them
            for (int lineIndex = 0; lineIndex < lines.Count; lineIndex++)
            {
                Line line = lines[lineIndex];
                // go through cracks
                for (int testIndex = 0; testIndex < cracks.Count; testIndex++)
                {
                    if (testIndex != lineIndex)
                    {
                        Line testLine = new Line(cracks[testIndex].StartPoint, cracks[testIndex].EndPoint, 0, 0);
                        // calculate distance from line start to test end
                        double d1 = hypotenuse(line.StartPoint, testLine.EndPoint);
                        // calculate distance from line end to test start
                        double d2 = hypotenuse(line.EndPoint, testLine.StartPoint);

                        bool addLine = false;

                        // check if distance is within absolute threshold, automaticall connect
                        if (d1 < connectCrackAbsDistanceThresh)
                        {
                            addLine = true;
                            cracks[testIndex].AddToEnd(line);
                        }
                        // check if distance is within absolute threshold, automaticall connect
                        if (d2 < connectCrackAbsDistanceThresh)
                        {
                            addLine = true;
                            cracks[testIndex].AddToStart(line);
                        }

                        if (d1 < connectCrackMaxDistanceThresh)
                        {
                            if (line.Angle > 95)
                            {
                                if (testLine.EndPoint.Y > line.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else if (line.Angle < 85)
                            {
                                if (testLine.EndPoint.Y < line.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else
                            {
                                addLine = true;
                            }
                            if (addLine)
                            {
                                cracks[testIndex].AddToEnd(line);
                            }
                            
                        }

                        if (d2 < connectCrackMaxDistanceThresh)
                        {
                            if (line.Angle > 95)
                            {
                                if (line.EndPoint.Y > testLine.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else if (line.Angle < 85)
                            {
                                if (line.EndPoint.Y < testLine.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else
                            {
                                addLine = true;
                            }
                            if (addLine)
                            {
                                cracks[testIndex].AddToStart(line);
                            }
                        }
                    }
                }

                for (int testIndex = 0; testIndex < lines.Count; testIndex++)
                {
                    if (testIndex != lineIndex)
                    {
                        Line testLine = lines[testIndex];

                        double d1 = hypotenuse(line.StartPoint, testLine.EndPoint);
                        double d2 = hypotenuse(line.EndPoint, testLine.StartPoint);

                        bool addLine = false;

                        if (d1 < connectCrackAbsDistanceThresh || d2 < connectCrackAbsDistanceThresh)
                        {
                            addLine = true;
                        }

                        if (d1 < connectCrackMaxDistanceThresh)
                        {
                            if (line.Angle > 95)
                            {
                                if (testLine.EndPoint.Y > line.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else if (line.Angle < 85)
                            {
                                if (testLine.EndPoint.Y < line.StartPoint.Y)
                                {
                                    addLine = true;   
                                }
                            }
                            else
                            {
                                addLine = true;
                            }
                        }

                        if (d2 < connectCrackMaxDistanceThresh)
                        {
                            if (line.Angle > 95)
                            {
                                if (line.EndPoint.Y > testLine.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else if (line.Angle < 85)
                            {
                                if (line.EndPoint.Y < testLine.StartPoint.Y)
                                {
                                    addLine = true;
                                }
                            }
                            else
                            {
                                addLine = true;
                            }
                        }

                        if (addLine)
                        {
                            LineCollection tmp = new LineCollection();
                            tmp.AddLine(line);
                            tmp.AddLine(testLine);
                            bool add = true;
                            foreach (LineCollection tmplines in cracks)
                            {
                                if (tmplines.Lines.Contains(line) || tmplines.Lines.Contains(testLine))
                                {
                                    add = false;
                                }
                            }
                            if (add)
                            {
                                cracks.Add(tmp);
                            }
                            
                        }

                    }
                }
            }
        }

        /// <summary>
        /// Goes through the list of lines and line collections and checks to see if the length
        /// of the line or line collection is greater than set threshold.  If it is greater than
        /// said threshold, then the crack counter is incremented.  After the function has
        /// cycled through both list, the total number of cracks is returned from the function.
        /// </summary>
        /// <param name="lines">List of purely lines to filter.</param>
        /// <param name="cracks">List of purely connected lines to filter.</param>
        /// <returns>Number of lines / line collections with length greather than threshold.</returns>
        private int filterLinesAndCracks(ref Bitmap b, ref List<Line> lines, ref List<LineCollection> cracks)
        {
            int crackCount = 0;
            List<Line> crackedLines = new List<Line>();
            foreach (Line line in lines)
            {
                if (hypotenuse(line.StartPoint, line.EndPoint) > crackLengthThresh)
                {
                    crackedLines.Add(line);
                    crackCount++;
                }
            }
            foreach (LineCollection crack in cracks)
            {
                if (hypotenuse(crack.StartPoint, crack.EndPoint) > crackLengthThresh)
                {
                    foreach (Line line in crack.Lines)
                    {
                        crackedLines.Add(line);
                    }
                    crackCount++;
                }
            }
            try
            {
                drawCrackLines(ref b, ref crackedLines, 255, 0, 255);
            }
            catch (Exception inner)
            {
                string errMsg = "FailureDetector.filterLinesAndCracks : Error drawing cracked lines.";
                FailureDetectorException ex = new FailureDetectorException(errMsg, inner);
                log.Error(errMsg, inner);
                throw ex;
            }
            
            return crackCount;
        }
       
        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
        private int filterImage(ref Bitmap b)
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
            int percentCrack = 100 * row_count / roi.Height;
            return percentCrack;
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
                    int value = row[x * PixelSize];
                    if (value > centerOfMass * 0.9)
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
                    if (row[x * PixelSize] > centerOfMass * 0.9)
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

        /// <summary>
        /// Calculate the weighted intensity of a histogram.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Creates a histogram of the intensity of a grayscale bitmap
        /// </summary>
        /// <param name="b"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Draws the roi in red
        /// </summary>
        /// <param name="b"></param>
        private void DrawROI(ref Bitmap b)
        {
            int PixelSize = 3;

            BitmapData B_data = null;

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

            byte* topRow1 = (byte*)B_data.Scan0 + (roi.Top * B_data.Stride);
            byte* topRow2 = (byte*)B_data.Scan0 + ((roi.Top + 1) * B_data.Stride);
            byte* bottomRow1 = (byte*)B_data.Scan0 + ((roi.Bottom - 1) * B_data.Stride);
            byte* bottomRow2 = (byte*)B_data.Scan0 + ((roi.Bottom - 2) * B_data.Stride);

            for (int x = 0; x < B_data.Width; x++)
            {
                if ((x % 8) / 2 == 1)
                {
                    topRow1[x * PixelSize] = 0;   //Blue  0-255
                    topRow1[x * PixelSize + 1] = 0; //Green 0-255
                    topRow1[x * PixelSize + 2] = 255;   //Red   0-255
                    bottomRow1[x * PixelSize] = 0;   //Blue  0-255
                    bottomRow1[x * PixelSize + 1] = 0; //Green 0-255
                    bottomRow1[x * PixelSize + 2] = 255;   //Red   0-255
                    topRow2[x * PixelSize] = 0;   //Blue  0-255
                    topRow2[x * PixelSize + 1] = 0; //Green 0-255
                    topRow2[x * PixelSize + 2] = 255;   //Red   0-255
                    bottomRow2[x * PixelSize] = 0;   //Blue  0-255
                    bottomRow2[x * PixelSize + 1] = 0; //Green 0-255
                    bottomRow2[x * PixelSize + 2] = 255;   //Red   0-255
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

    public class Line
    {
        private Point start;
        private Point end;
        private double slope;
        private double yintercept;
        private double angle;


        public Line(Point start, Point end, double slope, double yint)
        {
            this.start = new Point(start.X, start.Y);
            this.end = new Point(end.X, end.Y);
            this.slope = slope;
            this.yintercept = yint;
            //this.angle 
            if (slope > 0)
            {
                this.angle = 180 / Math.PI * Math.Asin(1 / slope);
            }
            else if (slope < 0 )
            {
                this.angle = 90 + 180 / Math.PI * Math.Acos(1 / Math.Abs(slope));
            }
            else
            {
                this.angle = 90;
            }
        }

        public int GetValueAt(int x)
        {
            double y = slope * x + yintercept;
            return (int)y;
        }

        public Point StartPoint { get { return start;} }
        public Point EndPoint { get { return end; } }
        public int StartX { get { return start.X; } }
        public int StartY { get { return start.Y; } }
        public int EndX { get { return end.X; } }
        public int EndY { get { return end.Y; } }
        public double Angle { get { return angle; } }
        

    }

    public class LineCollection
    {
        List<Line> lines;

        private Point startPoint;
        private Point endPoint;

        public List<Line> Lines { get { return lines; } }
        public Point StartPoint { get { return startPoint; } }
        public Point EndPoint { get { return endPoint; } }

        public LineCollection()
        {
            lines = new List<Line>();
        }

        public void AddToEnd(Line line)
        {
            endPoint = line.EndPoint;
            lines.Add(line);
        }

        public void AddToStart(Line line)
        {
            startPoint = line.StartPoint;
            lines.Add(line);
        }

        public void AddLine(Line line)
        {
            startPoint = line.StartPoint;
            endPoint = line.EndPoint;
            lines.Add(line);
        }
    }

    public enum ContourState
    {
        North,
        NorthEast,
        East,
        SouthEast,
        South,
        SouthWest,
        West,
        NorthWest
    }
}
