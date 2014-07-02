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
using log4net;

namespace SAF_OpticalFailureDetector.threading
{
    public partial class ZoomImageBox : UserControl
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ZoomImageBox));
        /// <summary>
        /// Limit user can zoom in is 2 ^ ZOOM_MAX x.
        /// </summary>
        private const int ZOOM_MAX = 4;

        /// <summary>
        /// Limit user can zoom out is 2 & ZOOM_MIN x.
        /// </summary>
        private const int ZOOM_MIN = -4;

        /// <summary>
        /// Used to synchronize critical sections in class.
        /// </summary>
        private Semaphore ctrlSem;

        /// <summary>
        /// Original image is stored in the unscaledImage variable.
        /// </summary>
        private Bitmap unscaledImage;

        /// <summary>
        /// The most recently draw image is stored in scaledImage variable.
        /// </summary>
        private Bitmap scaledImage;
        
        /// <summary>
        /// The current zoom level is 2 ^ zoomlvl.
        /// </summary>
        private int zoomlvl;

        /// <summary>
        /// Point in unscaled image to focus on.
        /// </summary>
        private Point focusPoint;

        /// <summary>
        /// Offset to top-left corner of display image in unscaled image coordinates.
        /// </summary>
        private Point displayImageOffset;

        /// <summary>
        /// Keeps track of the mouse state for panning.
        /// </summary>
        private MouseState mousestate;

        /// <summary>
        /// Point on unscaled image where mouse was pressed.
        /// </summary>
        private Point mousePointDown;

        /// <summary>
        /// Point on unscaled image where mouse was released.
        /// </summary>
        private Point mousePointUp;

        /// <summary>
        /// Mouse is either pressed or released.
        /// </summary>
        private enum MouseState
        {
            Released,
            Pressed
        }

        /// <summary>
        /// Default constructor for ZoomImageBox.
        /// </summary>
        public ZoomImageBox()
        {
            // must be called before editing controls
            InitializeComponent();

            // initialize semaphore and obtain ownership
            ctrlSem = new Semaphore(0, 1);

            // add event handler for mouse wheel events on display text label
            this.DisplayText.MouseWheel += this.DisplayImage_MouseScroll;

            // need to set DisplayText parent to Display Image box in order to have transparent background on label
            DisplayText.Parent = DisplayImageBox;
            DisplayText.ForeColor = Color.Red;
            DisplayText.BackColor = Color.Transparent;

            // default zoom lvl is 0x
            zoomlvl = -1;
            focusPoint = Point.Empty;
            displayImageOffset = new Point(0, 0);
            mousestate = MouseState.Released;
            mousePointDown = new Point(0, 0);
            mousePointUp = new Point(0, 0);

            // release semaphore allowing control to others
            ctrlSem.Release();
        }

        /// <summary>
        /// Zooms in on unscaled image at point p.
        /// </summary>
        /// <param name="p">Point on unscaled image to focus on.</param>
        public void ZoomIn(Point p)
        {
            // verify image may zoom in
            if (zoomlvl + 1 > ZOOM_MAX)
            {
                log.Error("ZoomImageBox.ZoomIn : Cannot zoom in any further.");
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
                log.Error("ZoomImageBox.ZoomIn : Exception thrown drawing image.", inner);
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomIn : Exception thrown drawing image.", inner);
                throw ex;
            }
        }

        /// <summary>
        /// Zooms out on unscaled image at point p.
        /// </summary>
        /// <param name="p">Point on unscaled image to focus on.</param>
        public void ZoomOut(Point p)
        {
            // verify image can zoom out
            if (zoomlvl - 1 < ZOOM_MIN)
            {
                log.Error("ZoomImageBox.ZoomOut : Cannot zoom out any further.");
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomOut : Cannot zoom out any further.");
                throw ex;
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
                log.Error("ZoomImageBox.ZoomOut : Exception thrown drawing image.", inner);
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.ZoomOut : Exception thrown drawing image.", inner);
                throw ex;
            }
        }

        /// <summary>
        /// Sets the unscaled image and draws it with the current zoom level and focus point settings.
        /// </summary>
        /// <param name="b">Bitmap to set display image to.</param>
        public void SetImage(Bitmap b)
        {
            Bitmap currentImage = (Bitmap)this.DisplayImageBox.Image;
            // obtain semaphore control before changing the image
            ctrlSem.WaitOne();
            unscaledImage = b;
            ctrlSem.Release();

            // attempt to draw the image
            try
            {
                DrawImage();
            }
            catch (Exception inner)
            {
                log.Error("ZoomImageBox.SetImage : Unable to set and draw image.", inner);
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.SetImage : Unable to set and draw image.", inner);
                throw ex;
            }
            if (currentImage != null)
            {
                currentImage.Dispose();
            }
            
        }

        /// <summary>
        /// Sets the text on the display text label.
        /// </summary>
        /// <param name="s">String to place on display text label.</param>
        public void SetText(String s)
        {
            DisplayText.Text = s;
        }

        /// <summary>
        /// Draws the image stored in unscaled image and scales it into the scaled image.
        /// </summary>
        private void DrawImage()
        {
            // we dont want settings changed while drawing image
            ctrlSem.WaitOne();

            // make sure unscaled image is not null
            if (unscaledImage == null)
            {
                log.Error("ZoomImageBox.DrawImage : Unscaled image is null, cannot draw zoomed image.");
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unscaled image is null, cannot draw zoomed image.");
                ctrlSem.Release();
                throw ex;
            }

            // store portion to draw in mem bitmap
            Bitmap buffer;

            // size the image wants to be
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
                log.Error("ZoomImageBox.DrawImage : Error obtaining desired image width/height.", inner);
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Error obtaining desired image width/height.", inner);
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
                int outputWidth;
                int outputHeight;
                // get image width to set
                if (desiredImageWidth > requestedOutputWidth)
                {
                    outputWidth = requestedOutputWidth;
                }
                else
                {
                    outputWidth = desiredImageWidth;
                }
                // get image height to set
                if (desiredImageHeight > requestedOutputHeight )
                {
                    outputHeight = requestedOutputHeight;
                }
                else
                {
                    outputHeight = desiredImageHeight;
                }
                
                // use the requested size by user form
                try
                {
                    scaledImage = new Bitmap(outputWidth, outputHeight);
                }
                catch (Exception inner)
                {
                    log.Error("ZoomImageBox.DrawImage : Unable to create scaledImage Bitmap.", inner);
                    ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unable to create scaledImage Bitmap.", inner);
                    ctrlSem.Release();
                    throw ex;
                }

                Point requestedFocusPoint = focusPoint;

                // create a cropping rectanble around the focus point in terms of original unscaled image
                Rectangle cropRectangle = new Rectangle(
                    Convert.ToInt32(focusPoint.X - outputWidth / 2 / Math.Pow(2, zoomlvl)),
                    Convert.ToInt32(focusPoint.Y - outputHeight / 2 / Math.Pow(2, zoomlvl)),
                    Convert.ToInt32(outputWidth / Math.Pow(2, zoomlvl)),
                    Convert.ToInt32(outputHeight / Math.Pow(2, zoomlvl))
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
                    log.Error("ZoomImageBox.DrawImage : Unable to draw scaled image.", inner);
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
                    log.Error("ZoomImageBox.DrawImage : Unable to draw scaled image.", inner);
                    ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unable to draw scaled image.", inner);
                    ctrlSem.Release();
                    throw ex;
                }
                
                buffer = unscaledImage;
            }

            // apply gain to the image
            float displayGain = 1.0f;
            float[][] matrix = {
                    new float[] {displayGain, 0, 0, 0, 0},
                    new float[] {0, displayGain, 0, 0, 0},
                    new float[] {0, 0, displayGain, 0, 0},
                    new float[] {0, 0, 0, displayGain, 0},
                    new float[] {0, 0, 0, 0, 1}};

            ColorMatrix colorMatrix = new ColorMatrix(matrix);

            ImageAttributes imageAttr = new ImageAttributes();
            imageAttr.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);

            // make the scaled bitmap
            Graphics g;
            try
            {
                g = Graphics.FromImage(scaledImage);
            }
            catch (Exception inner)
            {
                log.Error("ZoomImageBox.DrawImage : Unable to get graphics handle for scaled image.", inner);
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Unable to get graphics handle for scaled image.", inner);
                ctrlSem.Release();
                throw ex;
            }
            // grab the nearest pixel for color
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            try
            {
                g.DrawImage(buffer, new Rectangle(0, 0, scaledImage.Width, scaledImage.Height),
                    0, 0, buffer.Width, buffer.Height, GraphicsUnit.Pixel, imageAttr);
            }
            catch (Exception inner)
            {
                log.Error("ZoomImageBox.DrawImage : Exception redrawing scaled image.", inner);
                ZoomImageBoxException ex = new ZoomImageBoxException("ZoomImageBox.DrawImage : Exception redrawing scaled image.", inner);
                ctrlSem.Release();
                throw ex;
            }
            // dispose graphics ~ avoids memory leak
            g.Dispose();

            // update the image shown on ZoomImageBox control
            DisplayImageBox.Image = scaledImage;

            
            ctrlSem.Release();
        }

        /// <summary>
        /// This event is called when user scrolls mouse on control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DisplayImage_MouseScroll(object sender, MouseEventArgs e)
        {
            // zoom in
            if (e.Delta > 0)
            {
                try
                {
                    log.Info("ZoomImageBox.DisplayImage_MouseScroll : User scrolled mouse to zoom in.");
                    ZoomIn(new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl))));
                }
                catch (Exception inner)
                {
                    log.Error("ZoomImageBox.DisplayImage_MouseScroll : Unable to zoom in and draw image.", inner);
                }
                
            }
            // zoom out
            else if(e.Delta < 0)
            {
                try
                {
                    log.Info("ZoomImageBox.DisplayImage_MouseScroll : User scrolled mouse to zoom out.");
                    ZoomOut(new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl))));
                }
                catch (Exception inner)
                {
                    log.Error("ZoomImageBox.DisplayImage_MouseScroll : Unable to zoom out and draw image.", inner);
                }
            }
        }

        /// <summary>
        /// Event is called when mouse enters the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayText_MouseEnter(object sender, EventArgs e)
        {
            this.DisplayText.Focus();
        }

        /// <summary>
        /// Event occurs when user presses the mouse down on control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayImageBox_MouseDown(object sender, MouseEventArgs e)
        {
            mousePointDown = new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl)));
            mousestate = MouseState.Pressed;
        }

        /// <summary>
        /// Event occurs when user releases the mouse on control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayImageBox_MouseUp(object sender, MouseEventArgs e)
        {
            // mouse needs to be down
            if (mousestate == MouseState.Pressed)
            {
                // unscaled image must not be null
                if (unscaledImage == null)
                {
                    mousestate = MouseState.Released;
                    return;
                }
                log.Info("ZoomImageBox.DisplayImageBox_MouseUp : User panned around image.");

                // set point up in terms of unscaled image.
                mousePointUp = new Point(Convert.ToInt32(e.X / Math.Pow(2, zoomlvl)), Convert.ToInt32(e.Y / Math.Pow(2, zoomlvl)));
                // adjust the focuspoint by amount moved from mousePointDown to mousePOintUp
                int xCoordinate = focusPoint.X + (mousePointDown.X - mousePointUp.X);
                int yCoordinate = focusPoint.Y + (mousePointDown.Y - mousePointUp.Y);

                try
                {
                    // make sure point is within image
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
                }
                catch (Exception inner)
                {
                    log.Error("ZoomImageBox.DisplayImageBox_MouseUp : Exception thrown while panning.", inner);
                    mousestate = MouseState.Released;
                    return;
                }
                
                // edit the display image offset and focus point
                ctrlSem.WaitOne();
                displayImageOffset = new Point(xCoordinate, yCoordinate);
                focusPoint = new Point(xCoordinate, yCoordinate);
                ctrlSem.Release();
                mousestate = MouseState.Released;
            }
        }

        /// <summary>
        /// Event occurs when mouse leaves the control.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DisplayImageBox_MouseLeave(object sender, EventArgs e)
        {
            mousestate = MouseState.Released;
        }
    }

    // Use for exceptinos generated in ZoomImageBox class
    public class ZoomImageBoxException : System.Exception
    {
        public ZoomImageBoxException() : base() { }
        public ZoomImageBoxException(string message) : base(message) { }
        public ZoomImageBoxException(string message, System.Exception inner) : base(message, inner) { }
        protected ZoomImageBoxException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
