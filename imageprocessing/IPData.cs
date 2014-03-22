using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    class IPData
    {
        private Bitmap camImage;
        private Bitmap processedImage;
        private Boolean containsCrack;

        public IPData()
        {
            camImage = null;
            processedImage = null;
            containsCrack = false;
        }

        public IPData(Bitmap b)
        {
            camImage = b;
            processedImage = null;
            containsCrack = false;
        }

        public void SetCameraImage(Bitmap b)
        {
            camImage = b;
        }
        public Bitmap GetCameraImage()
        {
            Bitmap retImage = (Bitmap)camImage.Clone();
            return retImage;
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
        
    }
}
