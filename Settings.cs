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
                
        }

        private void btnTestRelayOn_Click(object sender, EventArgs e)
        {
            if (relayCtrl.IsOpen)
            {
                try
                {
                    relayCtrl.SetRelay0Status(true);
                    relayCtrl.SetRelay1Status(true);
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
                    relayCtrl.SetRelay0Status(false);
                    relayCtrl.SetRelay1Status(false);
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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetForm()
        {
        }

        private void DisplayError(string errMsg, Exception ex)
        {
            MessageBox.Show(errMsg + Environment.NewLine + ex.ToString(), "Error");
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
