using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TIS.Imaging;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    class IPData
    {
        private ImageBuffer ib;
        private Bitmap camImage;
        private Bitmap processedImage;
        private Boolean containsCrack;
        private Double camElapsedTime;
        private Double procElapsedTime;

        public IPData()
        {
            camImage = null;
            processedImage = null;
            containsCrack = false;
        }

        public IPData(ImageBuffer ib, double elapsedTime)
        {
            this.ib = ib;
            this.ib.Lock();
            camImage = this.ib.Bitmap;
            processedImage = null;
            this.camElapsedTime = elapsedTime;
            containsCrack = false;
        }

        public void Unlock()
        {
            if (this.ib.Locked)
            {
                this.ib.ForceUnlock();
            }
        }

        public void Dispose()
        {
            try
            {
                camImage.Dispose();
                processedImage.Dispose();
                ib.Bitmap.Dispose();
            }
            catch (Exception)
            {
                return;
            }
            
        }

        public void SetCameraImage(Bitmap b)
        {
            camImage = b;
        }
        public Bitmap GetCameraImage()
        {
            Bitmap b = (Bitmap)camImage.Clone();
            return b;
        }
        public void SetProcessedImage(Bitmap b)
        {
            processedImage = b;
        }
        public Bitmap GetProcessedImage()
        {
            return processedImage;
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
