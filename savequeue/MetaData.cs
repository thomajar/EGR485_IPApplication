using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SAF_OpticalFailureDetector.savequeue
{
    class MetaData
    {
        // metadata general settings
        private string settings_sampleNumber;
        private string settings_testNumber;
        private string settings_saveLocation;
        private bool settings_enableDebugSaving;
        private int settings_debugSavingFrequency;

        // metadata image processing settings
        private int ip_imagerNoise;
        private int ip_imagerContrast;
        private int ip_targetIntensity;
        private int ip_minLineLength;

        private static MetaData instance;
        public static MetaData Instance
        {
            get
            {
                if (instance == null)
	            {
		            instance = new MetaData();
	            }
                return instance;
            }
        }
        private MetaData()
        {
            initData();
        }

        private void initData()
        {
            // metadata general settings
            settings_sampleNumber = "na";
            settings_testNumber = "na";
            settings_saveLocation = "C:\\temp";
            settings_enableDebugSaving = true;
            settings_debugSavingFrequency = 450;

            // metadata image processing settings
            ip_imagerNoise = 12;
            ip_imagerContrast = 8;
            ip_targetIntensity = 200;
            ip_minLineLength = 0;
        }

        public void ResetData()
        {
            initData();
        }

        public void SetGeneralSettings(string sampleNumber, string testNumber, string saveLocation,
            bool enableDebugSave, int debugSaveFrequency)
        {
            this.settings_sampleNumber = sampleNumber;
            this.settings_testNumber = testNumber;
            this.settings_saveLocation = saveLocation;
            this.settings_enableDebugSaving = enableDebugSave;
            this.settings_debugSavingFrequency = debugSaveFrequency;
        }

        public void SetIPSettings(int imagerNoise, int imagerContrast, int imagerTargetIntensity, int minLineLength)
        {
            this.ip_imagerNoise = imagerNoise;
            this.ip_imagerContrast = imagerContrast;
            this.ip_targetIntensity = imagerTargetIntensity;
            this.ip_minLineLength = minLineLength;
        }
    }
}
