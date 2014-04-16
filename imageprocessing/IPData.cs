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
        private Boolean containsCrack;
        private Double camElapsedTime;
        private Double procElapsedTime;

        private DateTime timestamp;
        private byte[] rawData;
        private byte[] processedData;

        public IPData()
        {
            timestamp = DateTime.Now;
            containsCrack = false;
        }

        public IPData(double elapsedTime)
        {
            timestamp = DateTime.Now;
            this.camElapsedTime = elapsedTime;
            containsCrack = false;
        }

        public byte[] GetRawData()
        {
            return rawData;
        }

        public void SetProcessedData(byte[] pData)
        {
            processedData = pData;
        }

        public void SetCameraImage(Bitmap b)
        {
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            rawData = ms.ToArray();
            ms.Dispose();
        }

        public Bitmap GetCameraImage()
        {
            MemoryStream ms = new MemoryStream(rawData);
            Bitmap b = (Bitmap) Image.FromStream(ms);
            return b;
        }
        public void SetProcessedImage(Bitmap b)
        {
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);
            processedData = ms.ToArray();
            ms.Dispose();
        }
        public Bitmap GetProcessedImage()
        {
            MemoryStream ms = new MemoryStream(processedData);
            Bitmap b = (Bitmap)Image.FromStream(ms);
            return b;
        }




        public void SetContainsCrack(Boolean b)
        {
            containsCrack = b;
        }
        public Boolean ContainsCrack()
        {
            return containsCrack;
        }
        public Double GetElapsedTime()
        {
            return camElapsedTime;
        }

        public void SetProcessTime(Double elapsedTime)
        {
            procElapsedTime = elapsedTime;
        }
        public Double GetProcessTime()
        {
            return procElapsedTime;
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
