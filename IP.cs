using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing.Imaging;
using System.Drawing;
using System.Diagnostics;

namespace SAF_OpticalFailureDetector
{
    class IP
    {
        public unsafe static double readImg(Bitmap B, Rectangle R, int noise_level, int min_contrast)
        {
            int PixelSize = 3;
            int threshold = noise_level * 8;
            int result = 0;
            int row_count = 0;
            BitmapData B_data = null;
            int[] H = new int[] { 3, 1, -1, -6, -1, 1, 3 };
            if (B.PixelFormat == PixelFormat.Format24bppRgb)
            {
                B_data = B.LockBits(new Rectangle(0, 0, B.Width, B.Height),
                    ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            }

            for (int y = R.Top; y < R.Bottom; y++)
            {
                byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);
                bool Ishot = false;
                for (int x = 3; x < B_data.Width - 3; x++)
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
                        if (row[(x - offset) * PixelSize] > row[x * PixelSize] + min_contrast &&
                            row[(x + offset) * PixelSize] > row[x * PixelSize] + min_contrast)
                        {
                            Ishot = true;
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
            B.UnlockBits(B_data);
            return (double)row_count / (double)R.Height;
        }
        public static Rectangle ROI(Bitmap B)
        {
            Stopwatch SW = new Stopwatch();
            SW.Start();
            Rectangle R = new Rectangle(0, 0, B.Width, B.Height);
            int percentwhite = 80;
            int[] Histogram = new int[256];
            int PixelSize = 3;
            int Hist_greatest = 0;
            int max_index = 0;

            unsafe
            {
                BitmapData B_data = null;
                if (B.PixelFormat == PixelFormat.Format24bppRgb)
                {
                    B_data = B.LockBits(new Rectangle(0, 0, B.Width, B.Height), ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
                }

                //Histogram
                for (int y = 0; y < B_data.Height; y++)
                {
                    byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);

                    for (int x = 0; x < B_data.Width; x++)
                    {
                        Histogram[row[x * PixelSize]]++;
                    }
                }

                //Histogram Sorting
                for (int i = 50; i < Histogram.Length; i++)
                {
                    if (Histogram[i] > Hist_greatest)
                    {
                        Hist_greatest = Histogram[i];
                        max_index = i;
                    }
                }


                //Find Top Edge
                for (int y = 0; y < B_data.Height; y++)
                {
                    int count = 0;
                    byte* row = (byte*)B_data.Scan0 + (y * B_data.Stride);

                    for (int x = 0; x < B_data.Width; x++)
                    {
                        if (row[x * PixelSize] > max_index * 0.5)
                        {
                            count++;
                        }
                    }
                    if ((100 * count) / B.Width > percentwhite)
                    {
                        R = new Rectangle(0, y, R.Width, R.Height);
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
                        if (row[x * PixelSize] > max_index * 0.5)
                        {
                            count++;
                        }
                    }
                    if ((100 * count) / B.Width > percentwhite)
                    {
                        R = new Rectangle(0, R.Top, R.Width, y - R.Top);
                        break;
                    }
                }

                byte* toprow = (byte*)B_data.Scan0 + (R.Top * B_data.Stride);
                byte* bottomrow = (byte*)B_data.Scan0 + (R.Bottom * B_data.Stride);
                for (int x = 0; x < B_data.Width; x++)
                {
                    toprow[x * PixelSize] = 0;   //Blue  0-255
                    toprow[x * PixelSize + 1] = 255; //Green 0-255
                    toprow[x * PixelSize + 2] = 0;   //Red   0-255
                    bottomrow[x * PixelSize] = 0;   //Blue  0-255
                    bottomrow[x * PixelSize + 1] = 255; //Green 0-255
                    bottomrow[x * PixelSize + 2] = 0;   //Red   0-255
                }


                B.UnlockBits(B_data);
                SW.Stop();
                return R;
            }
        }
    }
}
