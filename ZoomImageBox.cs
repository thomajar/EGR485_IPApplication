using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;

namespace SAF_OpticalFailureDetector.threading
{
    public partial class ZoomImageBox : UserControl
    {
        private const int ZOOM_MAX = 4;
        private const int ZOOM_MIN = -4;

        private Semaphore ctrlSem;
        private Bitmap unscaledImage;
        private Bitmap scaledImage;
        
        // image settings
        private int zoomlvl;
        private Point focusPoint;

        public ZoomImageBox()
        {
            InitializeComponent();
            ctrlSem = new Semaphore(0, 1);
            this.DisplayText.MouseWheel += this.DisplayImage_MouseScroll;
            DisplayText.Parent = DisplayImageBox;
            DisplayText.ForeColor = Color.Red;
            DisplayText.BackColor = Color.Transparent;

            zoomlvl = 0;
            focusPoint = Point.Empty;

            ctrlSem.Release();
        }

        public void ZoomIn(Point p)
        {
            // verify image may zoom in
            if (zoomlvl + 1 > ZOOM_MAX)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomIn : Cannot zoom in any further.");
                throw ex;
            }

            // update the zoom lvl and focus point
            zoomlvl++;
            focusPoint = p;

            // attempt to draw the image to DisplayImage
            try
            {
                DrawImage();
            }
            catch (Exception inner)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomIn : Exception thrown drawing image.", inner);
                throw ex;
            }
            
        }

        public void ZoomOut(Point p)
        {
            // verify image can zoom out
            if (zoomlvl - 1 < ZOOM_MIN)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomOut : Cannot zoom out any further.");
            }

            // update the zoom lvl and focus point
            zoomlvl--;
            focusPoint = p;

            // attempt to draw the image to DisplayImage
            try
            {
                DrawImage();
            }
            catch (Exception inner)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomOut : Exception thrown drawing image.", inner);
                throw ex;
            }
        }

        public void SetImage(Bitmap b)
        {
            ctrlSem.WaitOne();
            unscaledImage = b;
            ctrlSem.Release();
            DrawImage();
        }

        public void SetText(String s)
        {
            DisplayText.Text = s;
        }

        private void DrawImage()
        {
            ctrlSem.WaitOne();

            // store portion to draw in mem bitmap
            Bitmap buffer;
            Point displayImageOffset = new Point(0, 0);

            // check to see what desired image size is
            int desiredImageWidth = 
                Convert.ToInt32(unscaledImage.Width * Math.Pow(2, zoomlvl));
            int desiredImageHeight = 
                Convert.ToInt32(unscaledImage.Height * Math.Pow(2,zoomlvl));
            
            // grab requested output size
            int requestedOutputWidth = DisplayImageBox.Width;
            int requestedOutputHeight = DisplayImageBox.Height;

            // create the scaled bitmap, just size at this point
            if (desiredImageWidth > requestedOutputWidth ||
                desiredImageHeight > requestedOutputHeight)
            {
                // use the requested size by user form
                scaledImage = new Bitmap(requestedOutputWidth, requestedOutputHeight);

                Point requestedFocusPoint = focusPoint;

                // create a cropping rectanble around the focus point in terms of original unscaled image
                Rectangle cropRectangle = new Rectangle(
                    Convert.ToInt32(focusPoint.X - requestedOutputWidth / 2 / Math.Pow(2,zoomlvl)),
                    Convert.ToInt32(focusPoint.Y - requestedOutputHeight / 2 / Math.Pow(2,zoomlvl)),
                    Convert.ToInt32(requestedOutputWidth / Math.Pow(2,zoomlvl)),
                    Convert.ToInt32(requestedOutputHeight / Math.Pow(2,zoomlvl))
                    );
                // verify cropping rectangle is within unscaledBitmap's width and height
                
                // check left side
                if (cropRectangle.X < 0)
                {
                    cropRectangle.X = 0;
                    if (cropRectangle.Right > unscaledImage.Width)
                    {
                        cropRectangle.Width = unscaledImage.Width;
                    }
                }

                // check top side
                if (cropRectangle.Y < 0)
                {
                    cropRectangle.Y = 0;
                    if (cropRectangle.Bottom > unscaledImage.Height)
                    {
                        cropRectangle.Height = unscaledImage.Height;
                    }
                }

                // check right side
                if (cropRectangle.Right > unscaledImage.Width)
                {
                    cropRectangle.X -= cropRectangle.Right - unscaledImage.Width;
                    if (cropRectangle.X < 0)
                    {
                        cropRectangle.X = 0;
                        cropRectangle.Width = unscaledImage.Width;
                    }
                }

                // check bottom side
                if (cropRectangle.Bottom > unscaledImage.Height)
                {
                    cropRectangle.Y -= cropRectangle.Bottom - unscaledImage.Height;
                    if (cropRectangle.Y < 0)
                    {
                        cropRectangle.Y = 0;
                        cropRectangle.Height = unscaledImage.Height;
                    }
                }

                // set the top left coordinate to copy image from
                displayImageOffset = new Point(cropRectangle.X, cropRectangle.Y);

                // try to get the cropped image
                try
                {
                    buffer = unscaledImage.Clone(cropRectangle, unscaledImage.PixelFormat);
                }
                catch (Exception inner)
                {
                    ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unable to draw scaled image.", inner);
                    ctrlSem.Release();
                    throw ex;
                }
            }
            else
            {
                // use size dictated by zoom lvl, this is smaller than requested size
                displayImageOffset = new Point(0, 0);
                scaledImage = new Bitmap(desiredImageWidth, desiredImageHeight);
                buffer = unscaledImage;
            }

            // apply gain to the image
            float displayGain = 1.0f;
            float[][] matrix = {
                    new float[] {displayGain, 0, 0, 0, 0},        // red scaling factor of 2
                    new float[] {0, displayGain, 0, 0, 0},        // green scaling factor of 1
                    new float[] {0, 0, displayGain, 0, 0},        // blue scaling factor of 1
                    new float[] {0, 0, 0, displayGain, 0},        // alpha scaling factor of 1
                    new float[] {0, 0, 0, 0, 1}};    // three translations of 0.2;

            ColorMatrix colorMatrix = new ColorMatrix(matrix);

            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // make the scaled bitmap
            Graphics g = Graphics.FromImage(scaledImage);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            try
            {
                g.DrawImage(buffer, new Rectangle(0, 0, scaledImage.Width, scaledImage.Height),
                    0, 0, buffer.Width, buffer.Height, GraphicsUnit.Pixel, imageAttr);
            }
            catch (Exception inner)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Exception redrawing scaled image.", inner);
                ctrlSem.Release();
                throw ex;
            }

            g.Dispose();

            DisplayImageBox.Image = scaledImage;


            ctrlSem.Release();
        }


        /// <summary>
        /// Scales an image and returns a copy of the image
        /// </summary>
        /// <param name="b"></param>
        /// <param name="p"></param>
        /// <param name="s"></param>
        /// <param name="zoomlvl"></param>
        /// <returns></returns>
        private Bitmap ScaleImage(ref Bitmap b, Point p, Size s, int zoomlvl)
        {
            Bitmap scaledImage;

            scaledImage = new Bitmap(s.Width, s.Height);

            // apply gain to the image

            float displayGain = 1.0f;

            float[][] matrix = {
                    new float[] {displayGain, 0, 0, 0, 0},        // red scaling factor of 2
                    new float[] {0, displayGain, 0, 0, 0},        // green scaling factor of 1
                    new float[] {0, 0, displayGain, 0, 0},        // blue scaling factor of 1
                    new float[] {0, 0, 0, displayGain, 0},        // alpha scaling factor of 1
                    new float[] {0, 0, 0, 0, 1}};    // three translations of 0.2;

            ColorMatrix colorMatrix = new ColorMatrix(matrix);

            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            Graphics g_scaled = Graphics.FromImage(scaledImage);
            g_scaled.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            try
            {
                g_scaled.DrawImage(b, new Rectangle(0, 0, scaledImage.Width, scaledImage.Height),
                    0, 0, b.Width, b.Height, GraphicsUnit.Pixel, imageAttr);
            }
            catch (Exception)
            {

                throw;
            }

            g_scaled.Dispose();
            return scaledImage;

        }

        void DisplayImage_MouseScroll(object sender, MouseEventArgs e)
        {
            // zoom in
            if (e.Delta > 0)
            {
                ZoomIn(new Point(e.X, e.Y));
            }
            // zoom out
            else if(e.Delta < 0)
            {
                ZoomOut(new Point(e.X, e.Y));
            }
        }

        private void DisplayText_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayText.Focus();
        }
    }

    // A SaveStream exception class
    public class ZoomImageBoxException : System.Exception
    {
        public ZoomImageBoxException() : base() { }
        public ZoomImageBoxException(string message) : base(message) { }
        public ZoomImageBoxException(string message, System.Exception inner) : base(message, inner) { }
        protected ZoomImageBoxException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
