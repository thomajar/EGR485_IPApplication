using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using log4net;
using System.Drawing;

namespace SAF_OpticalFailureDetector
{
    class ReplayManager
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(ReplayManager));

        private const string cam1Name = "cam1";
        private const string cam2Name = "cam2";
        private const string debugName = "debug";
        private const string testName = "test";

        private int debugFrames;
        private int testFrames;
        private bool isDebugVideo;
        private bool isRawVideo;
        private int currentFrameNumber;


        private List<string> cam1DebugDirectories;
        private List<string> cam2DebugDirectories;
        private List<string> cam1TestDirectories;
        private List<string> cam2TestDirectories;


        public int DebugFrames { get { return debugFrames; } }
        public int TestFrames { get { return testFrames; } }


        public ReplayManager(string rootLocation)
        {
            string cam1DebugDir = rootLocation + "//" + debugName + "//" + cam1Name;
            string cam2DebugDir = rootLocation + "//" + debugName + "//" + cam2Name;
            string cam1TestDir = rootLocation + "//" + testName + "//" + cam1Name;
            string cam2TestDir = rootLocation + "//" + testName + "//" + cam2Name;

            cam1DebugDirectories = new List<string>();
            cam2DebugDirectories = new List<string>();
            cam1TestDirectories = new List<string>();
            cam2TestDirectories = new List<string>();

            isDebugVideo = false;
            isRawVideo = false;

            int cam1DebugFrames = 0;
            try
            {
                cam1DebugFrames = GetDirectoires(cam1DebugDir, ref cam1DebugDirectories);
            }
            catch (Exception inner)
            {
                String errMsg = "ReplayManager.ReplayManager : Error getting cam 1 debug directories.";
                ReplayManagerException ex = new ReplayManagerException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            int cam2DebugFrames = 0;
            try
            {
                cam2DebugFrames = GetDirectoires(cam2DebugDir, ref cam2DebugDirectories);
            }
            catch (Exception inner)
            {
                String errMsg = "ReplayManager.ReplayManager : Error getting cam 2 debug directories.";
                ReplayManagerException ex = new ReplayManagerException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            if (cam1DebugFrames <= cam2DebugFrames)
            {
                debugFrames = cam2DebugFrames;
            }
            else
            {
                debugFrames = cam1DebugFrames;
            }


            int cam1TestFrames = 0;
            try
            {
                cam1TestFrames = GetDirectoires(cam1TestDir, ref cam1TestDirectories);
            }
            catch (Exception inner)
            {
                String errMsg = "ReplayManager.ReplayManager : Error getting cam 1 test directories.";
                ReplayManagerException ex = new ReplayManagerException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            int cam2TestFrames = 0;
            try
            {
                cam2TestFrames = GetDirectoires(cam2TestDir, ref cam2TestDirectories);
            }
            catch (Exception inner)
            {
                String errMsg = "ReplayManager.ReplayManager : Error getting cam 2 test directories.";
                ReplayManagerException ex = new ReplayManagerException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }

            if (cam1TestFrames <= cam2TestFrames)
            {
                testFrames = cam2TestFrames;
            }
            else
            {
                testFrames = cam1TestFrames;
            }
        }

        private int GetDirectoires(string searchDirectory, ref List<string> locations)
        {
            if (!Directory.Exists(searchDirectory))
            {
                string errMsg = "ReplayManager.GetDirectoires : Error, directory does not exist.";
                ReplayManagerException ex = new ReplayManagerException(errMsg);
                log.Error(errMsg, ex);
                throw ex;
            }
            string[] dirs = null;
            try
            {
                dirs = Directory.GetDirectories(searchDirectory);
            }
            catch (Exception inner)
            {
                string errMsg = "ReplayManager.GetDirectories : Error searching for subdirectories.";
                ReplayManagerException ex = new ReplayManagerException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
            if (dirs != null)
            {
                foreach (string strLocation in dirs)
                {
                    locations.Add(strLocation);
                }
            }
            else
            {
                string errMsg = "ReplayManager.GetDirectories : List of directories is null, unhandled exception.";
                ReplayManagerException ex = new ReplayManagerException(errMsg);
                log.Error(errMsg, ex);
                throw ex;
            }

            return locations.Count;
        }

        private string GetBitmapLocation(string root)
        {
            string[] files = Directory.GetFiles(root);
            if (isRawVideo)
            {
                foreach (string s in files)
                {
                    if (s.Contains("rawimage"))
                    {
                        return s;
                    }
                }
            }
            else
            {
                foreach (string s in files)
                {
                    if (s.Contains("procimage"))
                    {
                        return s;
                    }
                }
            }
            return "";
        }

        public void SetVideoMode(bool isDebugMode)
        {
            if (this.isDebugVideo != isDebugMode)
            {
                currentFrameNumber = 0;
                this.isDebugVideo = isDebugMode;
            }
        }

        public void SetDataMode(bool isRawMode)
        {
            if (this.isRawVideo != isRawMode)
            {
                this.isRawVideo = isRawMode;
            }
        }

        public int GetCurrentFrame()
        {
            return currentFrameNumber;
        }

        public int GetTotalFrames()
        {
            int totalFrameCount;
            if (isDebugVideo)
            {
                totalFrameCount = debugFrames;
            }
            else
            {
                totalFrameCount = testFrames;
            }
            return totalFrameCount;
        }

        public Bitmap[] GetCurrentBitmaps()
        {
            Bitmap[] frames = new Bitmap[2];
            frames[0] = null;
            frames[1] = null;
            if (isDebugVideo)
            {
                if (currentFrameNumber < cam1DebugDirectories.Count)
                {
                    frames[0] = new Bitmap(GetBitmapLocation(cam1DebugDirectories[currentFrameNumber]));
                }
                if(currentFrameNumber < cam2DebugDirectories.Count)
                {
                    frames[1] = new Bitmap(GetBitmapLocation(cam2DebugDirectories[currentFrameNumber]));
                }
            }
            else
            {
                if (currentFrameNumber < cam1TestDirectories.Count)
                {
                    frames[0] = new Bitmap(GetBitmapLocation(cam1TestDirectories[currentFrameNumber]));
                }
                if (currentFrameNumber < cam2TestDirectories.Count)
                {
                    frames[1] = new Bitmap(GetBitmapLocation(cam2TestDirectories[currentFrameNumber]));
                }
            }
            return frames;
        }

        public string GetTestInfo()
        {
            if (isDebugVideo)
            {
                if (currentFrameNumber < cam1DebugDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    return data[0];
                }
                if (currentFrameNumber < cam2DebugDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    return data[0];
                }
            }
            else
            {
                if (currentFrameNumber < cam1TestDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    return data[0];
                }
                if (currentFrameNumber < cam2TestDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    return data[0];
                }
            }
            return "";
        }

        public string[] GetCurrentFrameInfo()
        {
            string[] frameInfo = new string[2];
            frameInfo[0] = null;
            frameInfo[1] = null;
            if (isDebugVideo)
            {
                if (currentFrameNumber < cam1DebugDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    frameInfo[0] = data[1];
                }
                if (currentFrameNumber < cam2DebugDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    frameInfo[1] = data[1];
                }
            }
            else
            {
                if (currentFrameNumber < cam1TestDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    frameInfo[0] = data[1];
                }
                if (currentFrameNumber < cam2TestDirectories.Count)
                {
                    string[] data = getImageInfo(cam1DebugDirectories[currentFrameNumber]);
                    frameInfo[1] = data[1];
                }
            }
            return frameInfo;
        }

        private String[] getImageInfo(string rootLocation)
        {
            string[] imageInfo = new string[2];
            imageInfo[0] = null;
            imageInfo[1] = null;
            string[] files = Directory.GetFiles(rootLocation);
            foreach (string s in files)
            {
                if (s.Contains(".txt"))
                {
                    
                    int writeIndex = -1;
                    StreamReader sr = new StreamReader(s);
                    string tmp = "";
                    while (!sr.EndOfStream)
                    {
                        string temp = sr.ReadLine();
                        if (writeIndex > -1)
                        {
                            imageInfo[writeIndex] += temp + Environment.NewLine;
                        }
                        if (temp.Contains("General Settings"))
                        {
                            writeIndex++;
                        }
                        if (temp.Contains("Camera Information"))
                        {
                            writeIndex++;
                        }
                    }
                    return imageInfo;
                }
            }
            return imageInfo;
        }

        public int NextFrame()
        {
            if (isDebugVideo)
            {
                if (debugFrames != 0)
                {
                    currentFrameNumber = (currentFrameNumber + 1) % debugFrames;
                }
                
            }
            else
            {
                if (testFrames != 0)
                {
                    currentFrameNumber = (currentFrameNumber + 1) % testFrames;
                }
            }
            return currentFrameNumber;
        }

        public int PreviousFrame()
        {
            if (isDebugVideo)
            {
                currentFrameNumber--;
                if (currentFrameNumber < 0)
                {
                    currentFrameNumber = debugFrames - 1;
                }
            }
            else
            {
                currentFrameNumber--;
                if (currentFrameNumber < 0)
                {
                    currentFrameNumber = testFrames - 1;
                }
            }
            return currentFrameNumber;
        }

        
    }
    public class ReplayManagerException : System.Exception
    {
        public ReplayManagerException() : base() { }
        public ReplayManagerException(string message) : base(message) { }
        public ReplayManagerException(string message, System.Exception inner) : base(message, inner) { }
        protected ReplayManagerException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
