using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SAF_OpticalFailureDetector.threading;

namespace SAF_OpticalFailureDetector.imageprocessing
{
    public unsafe class FailureDetector
    {
        private int minimumContrast;
        private int noiseRange;

        public static int FindROI();
        public static int ProcessROI();
        public static int Histogram();


        private enum IPState
        {
            RESET,
            INIT,
            ROI,
            PROC
        };
        

    }
}
