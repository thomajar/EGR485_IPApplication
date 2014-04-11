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
        private Point displayImageOffset;

        private MouseState mousestate;
        private Point mousePointDown;
        private Point mousePointUp;


        private enum MouseState
        {
            Released,
            Pressed
        }

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
            displayImageOffset = new Point(0, 0);
            mousestate = MouseState.Released;
            mousePointDown = new Point(0, 0);
            mousePointUp = new Point(0, 0);

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
            ctrlSem.WaitOne();
            zoomlvl++;
            focusPoint = new Point(p.X + displayImageOffset.X, p.Y + displayImageOffset.Y);
            ctrlSem.Release();

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
            ctrlSem.WaitOne();
            zoomlvl--;
            focusPoint = new Point(p.X + displayImageOffset.X, p.Y + displayImageOffset.Y);
            ctrlSem.Release();

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
            try
            {
                DrawImage();
            }
            catch (Exception inner)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.SetImage : Unable to set and draw image.", inner);
                throw ex;
            }
            
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

            int desiredImageWidth;
            int desiredImageHeight;

            // check to see what desired image size is
            try
            {
                desiredImageWidth =
                    Convert.ToInt32(unscaledImage.Width * Math.Pow(2, zoomlvl));
                desiredImageHeight =
                    Convert.ToInt32(unscaledImage.Height * Math.Pow(2, zoomlvl));
            }
            catch (Exception inner)
            {
                ZoomImageBoxException ex = new ZoomImageBoxException(
                    "ZoomImageBox.DrawImage : Error obtaining desired image width/height.",
                    inner);
                ctrlSem.Release();
                throw ex;
            }
            
            
            // grab requested output size
            int requestedOutputWidth = DisplayImageBox.Width;
            int requestedOutputHeight = DisplayImageBox.Height;

            // create the scaled bitmap, just size at this point
            if (desiredImageWidth > requestedOutputWidth ||
                desiredImageHeight > requestedOutputHeight)
            {
                // use the requested size by user form
                try
                {
                    scaledImage = new Bitmap(requestedOutputWidth, requestedOutputHeight);
                }
                catch (Exception inner)
                {
                    ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unable to create scaledImage Bitmap.", inner);
                    ctrlSem.Release();
                    throw ex;
                }

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
                    //buffer.Save("Image.bmp");
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
                try
                {
                    scaledImage = new Bitmap(desiredImageWidth, desiredImageHeight);
                }
                catch (Exception inner)
                {
                    ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unable to draw scaled image.", inner);
                    ctrlSem.Release();
                    throw ex;
                }
                
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

        void DisplayImage_MouseScroll(object sender, MouseEventArgs e)
        {
            // zoom in
            if (e.Delta > 0)
            {
                try
                {
                    ZoomIn(new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl))));
                }
                catch (Exception)
                {
                }
                
            }
            // zoom out
            else if(e.Delta < 0)
            {
                try
                {
                    ZoomOut(new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl))));
                }
                catch (Exception)
                {
                }
                
            }
        }

        private void DisplayText_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayText.Focus();
        }

        private void DisplayImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            mousePointDown = new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl)));
            mousestate = MouseState.Pressed;
        }

        private void DisplayImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            
            if (mousestate == MouseState.Pressed)
            {
                if (unscaledImage == null)
                {
                    mousestate = MouseState.Released;
                    return;
                }
                mousePointUp = new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl)));
                int xCoordinate = focusPoint.X + (mousePointDown.X - mousePointUp.X);
                int yCoordinate = focusPoint.Y + (mousePointDown.Y - mousePointUp.Y);
                displayImageOffset = new Point(xCoordinate, yCoordinate);
                if (xCoordinate < 0)
                {
                    xCoordinate = 0;
                }
                if (xCoordinate > unscaledImage.Size.Width)
                {
                    xCoordinate = unscaledImage.Size.Width;
                }
                if (yCoordinate < 0)
                {
                    yCoordinate = 0;
                }
                if (yCoordinate > unscaledImage.Size.Height)
                {
                    yCoordinate = unscaledImage.Size.Height;
                }
                ctrlSem.WaitOne();
                focusPoint = new Point(xCoordinate, yCoordinate);
                ctrlSem.Release();
                mousestate = MouseState.Released;
            }
        }

        private void DisplayImageBox_MouseLeave(object sender, EventArgs e)
        {
            mousestate = MouseState.Released;
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
