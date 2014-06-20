using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using SAF_OpticalFailureDetector.relay;
using log4net;
using SAF_OpticalFailureDetector.savequeue;

namespace SAF_OpticalFailureDetector
{
    public partial class Settings : Form
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(Settings));
        private USBRelayController relayCtrl;
        private MetaData metadata;

        public Settings()
        {
            this.InitializeComponent();
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            relayCtrl = USBRelayController.Instance;
            metadata = MetaData.Instance;
            DisplayMetadata();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            log.Info("Settings.btnBrowse_Click : User attempted to browse for log file save location.");
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if (fbd.ShowDialog() == DialogResult.OK)
            {
                txtSaveLocation.Text = fbd.SelectedPath;
            }
            else
            {
                string errMsg = "Settings.btnBrowse_Click : Invalid save folder chosen.";
                log.Error(errMsg);
                MessageBox.Show(errMsg,"Error");
            }
        }

        private void btnOpenRelay_Click(object sender, EventArgs e)
        {
            int portNum;
            try
            {
                portNum = Convert.ToInt32(nudPortNum.Value);
            }
            catch (Exception inner)
            {
                string errMsg = "Settings.btnOpenRelay_Click : Exception thrown converting nudPortNum.Value to integer.";
                SettingsException ex = new SettingsException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
                return;
            }

            try
            {
                relayCtrl.Open(portNum);
            }
            catch (Exception inner)
            {
                string errMsg = "Settings.btnOpenRelay_Click : Error connecting to relay controller.";
                SettingsException ex = new SettingsException(errMsg, inner);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
                return;
            }

            // turn USB relay off
            USBRelayController usb_relay = USBRelayController.Instance;
            usb_relay.SetRelay0Status(false);
            usb_relay.SetRelay1Status(false);
                
        }

        private void btnTestRelayOn_Click(object sender, EventArgs e)
        {
            if (relayCtrl.IsOpen)
            {
                try
                {
                    TurnRelayOn();
                }
                catch (Exception inner)
                {
                    string errMsg = "Settings.btnTestRelayOn_Click : Unable to turn relay on.";
                    SettingsException ex = new SettingsException(errMsg, inner);
                    log.Error(errMsg, ex);
                    DisplayError(errMsg, ex);
                }
            }
            else
            {
                string errMsg = "Settings.btnTestRelayOn_Click : Relay controller connection is not opened.";
                SettingsException ex = new SettingsException(errMsg);
                log.Error(errMsg,ex);
                DisplayError(errMsg, ex);
            }
        }

        private void btnTestRelayOff_Click(object sender, EventArgs e)
        {
            if (relayCtrl.IsOpen)
            {
                try
                {
                    TurnRelayOff();
                }
                catch (Exception inner)
                {
                    string errMsg = "Settings.btnTestRelayOff_Click : Unable to turn relay off.";
                    SettingsException ex = new SettingsException(errMsg, inner);
                    log.Error(errMsg, ex);
                    DisplayError(errMsg, ex);
                }
            }
            else
            {
                string errMsg = "Settings.btnTestRelayOff_Click : Relay controller connection is not opened.";
                SettingsException ex = new SettingsException(errMsg);
                log.Error(errMsg, ex);
                DisplayError(errMsg, ex);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            metadata.ResetData();
            ResetForm();
            DisplayMetadata();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveMetaData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            // validate form is setup correctly
            if (isMetaDataValid())
            {
                DialogResult = DialogResult.OK;
                try
                {
                    TurnRelayOff();
                }
                catch (Exception inner)
                {
                    string errMsg = "Settings.btnClose_Click : Unable to turn relays off prior to testing.";
                    SettingsException ex = new SettingsException(errMsg, inner);
                    log.Error(errMsg, ex);
                    DisplayError(errMsg, ex);
                }
            }
            else
            {
                DialogResult = DialogResult.OK;
                MessageBox.Show("Cannot begin testing until all fields are filled in and connection to relay controller is opened.", "Warning");
            }
            SaveMetaData();
            this.Close();
        }

        private bool isMetaDataValid()
        {
            if (txtSampleNumber.Text == "")
            {
                return false;
            }
            if (txtTestNumber.Text == "")
            {
                return false;
            }
            if (txtSaveLocation.Text == "")
            {
                return false;
            }
            if (!relayCtrl.IsOpen)
            {
                return false;
            }

            return true;

        }

        private void TurnRelayOff()
        {
            try
            {
                relayCtrl.SetRelay0Status(false);
                relayCtrl.SetRelay1Status(false);
            }
            catch (Exception inner)
            {
                string errMsg = "Settings.TurnRelayOff : Unable to turn relays off, exception occured.";
                SettingsException ex = new SettingsException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }            
        }

        private void TurnRelayOn()
        {
            try
            {
                relayCtrl.SetRelay0Status(true);
                relayCtrl.SetRelay1Status(true);
            }
            catch (Exception inner)
            {
                string errMsg = "Settings.TurnRelayOn : Unable to turn relays on, exception occured";
                SettingsException ex = new SettingsException(errMsg, inner);
                log.Error(errMsg, ex);
                throw ex;
            }
        }

        private void DisplayMetadata()
        {
            txtSampleNumber.Text = metadata.SampleNumber;
            txtTestNumber.Text = metadata.TestNumber;
            txtSaveLocation.Text = metadata.SaveLocation;
            cbEnableDebugSaving.Checked = metadata.EnableDebugSaving;
            nudImagerNoise.Value = metadata.ImagerNoise;
            nudMinContrast.Value = metadata.MinimumContrast;
            nudTargetIntensity.Value = metadata.TargetIntenstiy;
            nudMinLineLength.Value = metadata.MinimumLineLength;
        }

        private void ResetForm()
        {
            metadata.ResetData();
            DisplayMetadata();
        }

        private void SaveMetaData()
        {
            metadata.SetGeneralSettings(
                txtSampleNumber.Text,
                txtTestNumber.Text,
                txtSaveLocation.Text,
                cbEnableDebugSaving.Checked);
            metadata.SetIPSettings(
                Convert.ToInt32(nudImagerNoise.Value),
                Convert.ToInt32(nudMinContrast.Value),
                Convert.ToInt32(nudTargetIntensity.Value),
                Convert.ToInt32(nudMinLineLength.Value));
        }

        private void DisplayError(string errMsg, Exception ex)
        {
            MessageBox.Show(errMsg + Environment.NewLine + "Detailed Error: " + Environment.NewLine + ex.ToString(), "Error");
        }

        
    }

    public class SettingsException : System.Exception
    {
        public SettingsException() : base() { }
        public SettingsException(string message) : base(message) { }
        public SettingsException(string message, System.Exception inner) : base(message, inner) { }
        protected SettingsException(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context) { }
    }
}
