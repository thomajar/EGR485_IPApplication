using log4net;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIS.Imaging;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    class IPData
    {
        // image information
        private byte[] rawData;
        private int imageNumber;
        private Size imageSize;
        private Double exposure_s;
        private int intensity_lsb;

        // time information
        private DateTime timestamp;
        private Double cameraElapsedTime_s;
        private Double processorElapsedTime_s;

        // image processing information
        private byte[] processedData;
        private int potentialCracks;
        private Rectangle roi;
        private Boolean containsCrack;
        private Boolean isProcessed;

        private static readonly ILog log = LogManager.GetLogger(typeof(IPData));

        
        /// <summary>
        /// Initializes an IPData object.
        /// </summary>
        public IPData()
        {
            initData();
        }

        /// <summary>
        /// Initializes an IPData object with specified elapsed time.
        /// </summary>
        /// <param name="elapsedTime">Amount of time elapsed since last image was captured.
        /// <param name="exposure_s">Amount of time exposure is set to.</param>
        /// <param name="isProcessed">States whether the data has been passed through image processor.</param>
        /// This information is used for calculating the frame rate.</param>
        public IPData(double elapsedTime, double exposure_s, Boolean isProcessed, int imageNumber)
        {
            initData();
            this.cameraElapsedTime_s = elapsedTime;
            this.exposure_s = exposure_s;
            this.isProcessed = isProcessed;
            this.imageNumber = imageNumber;
        }

        /// <summary>
        /// Initializes all objects to a default state.
        /// </summary>
        private void initData()
        {
            rawData = null;
            imageSize = Size.Empty;
            exposure_s = 0.0;
            intensity_lsb = -1;
            timestamp = DateTime.Now;
            cameraElapsedTime_s = 0;
            processorElapsedTime_s = 0;
            processedData = null;
            potentialCracks = 0;
            roi = Rectangle.Empty;
            containsCrack = false;
            isProcessed = false;
        }

        /// <summary>
        /// Function is used to set all of the image processing Metadata into the IPData
        /// object.  This data is stored in the object and later passed on.
        /// </summary>
        /// <param name="ipElapsedTime_s">Amount of time elapsed during image processing.</param>
        /// <param name="regionOfInterest">Region within the image that is being processed.</param>
        /// <param name="intensity_lsb">The weighted average of the image intensity.</param>
        /// <param name="potentialCracks">Count of total number of potential cracks.</param>
        /// <param name="containsCrack">States whether a crack is in the image or not.</param>
        /// <param name="isProcessed">States whether image has been through image processor.</param>
        public void SetIPMetaData(double ipElapsedTime_s, Rectangle regionOfInterest,
            int intensity_lsb, int potentialCracks, Boolean containsCrack, Boolean isProcessed)
        {
            this.processorElapsedTime_s = ipElapsedTime_s;
            this.roi = regionOfInterest;
            this.intensity_lsb = intensity_lsb;
            this.potentialCracks = potentialCracks;
            this.containsCrack = containsCrack;
            this.isProcessed = isProcessed;
        }

        /// <summary>
        /// Sets the raw data byte array from a bitmap.  This method is used to 
        /// copy in the data from the bitmap and store it in a byte array.
        /// </summary>
        /// <param name="b">Bitmap to copy to raw data byte array.</param>
        /// <exception cref="ImageDataException"></exception>
        public void SetRawDataFromImage(Bitmap b)
        {
            // create a memory stream to store image data in temporarily
            MemoryStream ms = new MemoryStream();
            try
            {
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch (Exception inner)
            {
                string errMsg = "IPData.SetRawDataFromImage : Unable to save data to memory stream.";
                ImageDataException ex = new ImageDataException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            // convert memory stream to array with byte information
            rawData = ms.ToArray();
            ms.Dispose();
            // store the size of image so it can be recalled later
            imageSize = new Size(b.Width, b.Height);
        }

        /// <summary>
        /// Obtains a bitmap of the raw data image.  The raw data must be set for this function to work properly otherwise it will generate an exception.
        /// </summary>
        /// <exception cref="ImageDataException"></exception>
        /// <returns>Returns raw data in the form of an image.</returns>
        public Bitmap GetRawDataImage()
        {
            // attempt to create a memory stream with raw data byte array in it
            MemoryStream ms;
            try
            {
                ms = new MemoryStream(rawData);
            }
            catch (Exception inner)
            {
                string errMsg = "IPData.GetRawDataImage : Unable to create memory stream of raw image data.";
                ImageDataException ex = new ImageDataException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            // attempt to recreate the bitmap from raw data byte array
            Bitmap b;
            try
            {
                b = (Bitmap)Image.FromStream(ms);
            }
            catch (Exception inner)
            {
                string errMsg = "IPData.GetRawDataImage : Unable to convert memory stream to image.";
                ImageDataException ex = new ImageDataException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            return b;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="b"></param>
        /// <exception cref="ImageDataException"></exception>
        public void SetProcessedDataFromImage(Bitmap b)
        {
            // create a memory stream to store image data in temporarily
            MemoryStream ms = new MemoryStream();
            try
            {
                b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            }
            catch (Exception inner)
            {
                string errMsg = "IPData.SetProcessedDataFromImage : Unable to save data to memory stream.";
                ImageDataException ex = new ImageDataException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            // convert memory stream to array with byte information
            processedData = ms.ToArray();
            ms.Dispose();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <exception cref="ImageDataException"></exception>
        /// <returns></returns>
        public Bitmap GetProcessedDataImage()
        {
            // attempt to create a memory stream with raw data byte array in it
            MemoryStream ms;
            try
            {
                ms = new MemoryStream(processedData);
            }
            catch (Exception inner)
            {
                string errMsg = "IPData.GetRawDataImage : Unable to create memory stream from raw image data.";
                ImageDataException ex = new ImageDataException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            // attempt to recreate the bitmap from raw data byte array
            Bitmap b;
            try
            {
                b = (Bitmap)Image.FromStream(ms);
            }
            catch (Exception inner)
            {
                string errMsg = "IPData.GetRawDataImage : Unable to convert memory stream to image.";
                ImageDataException ex = new ImageDataException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            return b;
        }

        public int ImageNumber
        {
            get
            {
                return imageNumber;
            }
        }

        public Size ImageSize
        {
            get
            {
                return imageSize;
            }
        }

        public Double ImageExposure_s
        {
            get
            {
                return exposure_s;
            }
        }

        public int ImageIntensity_lsb
        {
            get
            {
                return intensity_lsb;
            }
        }

        public DateTime TimeStamp
        {
            get
            {
                return timestamp;
            }
        }

        public Double CameraElapsedTime_s
        {
            get
            {
                return cameraElapsedTime_s;
            }
        }

        public Double ImageProcessorElapsedTime_s
        {
            get
            {
                return processorElapsedTime_s;
            }
        }

        public int PotentialCrackCount
        {
            get
            {
                return potentialCracks;
            }
        }

        public Rectangle RegionOfInterest
        {
            get
            {
                return roi;
            }
        }

        public Boolean ContainsCrack
        {
            get
            {
                return containsCrack;
            }
        }

        public Boolean IsProcessed
        {
            get
            {
                return isProcessed;
            }
        }
    }

    // Use for exceptinos generated in IPData class
    public class ImageDataException : System.Exception
    {
        public ImageDataException() : base() { }
        public ImageDataException(string message) : base(message) { }
        public ImageDataException(string message, System.Exception inner) : base(message, inner) { }
        protected ImageDataException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
