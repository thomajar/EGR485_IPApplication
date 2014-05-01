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
        private byte[] processedData;
        private Boolean containsCrack;
        private Size imageSize;

        // time information
        private DateTime timestamp;
        private Double cameraElapsedTime_s;
        private Double processorElapsedTime_s;
        
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
        /// This information is used for calculating the frame rate.</param>
        public IPData(double elapsedTime)
        {
            initData();
            this.cameraElapsedTime_s = elapsedTime;
        }

        /// <summary>
        /// Initializes all objects to a default state.
        /// </summary>
        private void initData()
        {
            rawData = null;
            processedData = null;
            containsCrack = false;
            imageSize = Size.Empty;
            timestamp = DateTime.Now;
            cameraElapsedTime_s = 0;
            processorElapsedTime_s = 0;
        }

        /// <summary>
        /// Sets the raw data byte array equal to the array passed in.
        /// </summary>
        /// <param name="b">Raw data byte array of a bitmap.</param>
        public void SetRawData(Byte[] b)
        {
            rawData = b;
        }

        /// <summary>
        /// Sets the raw data byte array from a bitmap.  This method is used to 
        /// copy in the data from the bitmap and store it in a byte array.
        /// </summary>
        /// <param name="b">Bitmap to copy to raw data byte array.</param>
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
                ImageDataException ex = new ImageDataException("IPData.SetRawDataFromImage : Unable to save data to memory stream.", inner);
                throw ex;
            }
            // convert memory stream to array with byte information
            rawData = ms.ToArray();
            ms.Dispose();
            // store the size of image so it can be recalled later
            imageSize = new Size(b.Width, b.Height);
        }

        /// <summary>
        /// Obtains raw data byte array.  This is not a copy.
        /// </summary>
        /// <returns>Byte array of raw data image.</returns>
        public Byte[] GetRawData()
        {
            return rawData;
        }

        /// <summary>
        /// Obtains a bitmap of the raw data image.  The raw data must be set for this function to work properly otherwise it will generate an exception.
        /// </summary>
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
                ImageDataException ex = new ImageDataException("IPData.GetRawDataImage : Unable to create memory stream.", inner);
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
                ImageDataException ex = new ImageDataException("IPData.GetRawDataImage : Unable to convert memory stream to image.", inner);
                throw ex;
            }
            return b;
        }

        public void SetProcessedData(Byte[] b)
        {
            processedData = b;
        }

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
                ImageDataException ex = new ImageDataException("IPData.SetProcessedDataFromImage : Unable to save data to memory stream.", inner);
                throw ex;
            }
            // convert memory stream to array with byte information
            processedData = ms.ToArray();
            ms.Dispose();
        }

        public Byte[] GetProcessedData()
        {
            return processedData;
        }

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
                ImageDataException ex = new ImageDataException("IPData.GetRawDataImage : Unable to create memory stream.", inner);
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
                ImageDataException ex = new ImageDataException("IPData.GetRawDataImage : Unable to convert memory stream to image.", inner);
                throw ex;
            }
            return b;
        }

        public Boolean ContainsCrack
        {
            get
            {
                return containsCrack;
            }
            set
            {
                containsCrack = value;
            }
        }

        public Double CameraElapsedTime_s
        {
            get
            {
                return cameraElapsedTime_s;
            }
            set
            {
                cameraElapsedTime_s = value;
            }
        }

        public Double ProcessorElapsedTime_s
        {
            get
            {
                return processorElapsedTime_s;
            }
            set
            {
                processorElapsedTime_s = value;
            }
        }

        public Size ImageSize
        {
            get
            {
                return imageSize;
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
