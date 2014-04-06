using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SAF_OpticalFailureDetector
{
    public partial class Settings : Form
    {
        private String camera1;
        private String camera2;
        private String emailAddress;
        private String sampleNumber;
        private String testNumber;
        private String logLocation;

        public String Camera1Name
        {
            get
            {
                return camera1;
            }
        }

        public String Camera2Name
        {
            get
            {
                return camera2;
            }
        }

        public String EmailAddress
        {
            get
            {
                return emailAddress;
            }
        }

        public String SampleNumber
        {
            get
            {
                return sampleNumber;
            }
        }

        public String TestNumber
        {
            get
            {
                return testNumber;
            }
        }

        public String LogLocation
        {
            get
            {
                return logLocation;
            }
        }

        public Settings()
        {
            InitializeComponent();
            camera1 = "";
            camera2 = "";
            emailAddress = "";
            sampleNumber = "";
            testNumber = "";
            logLocation = "";
        }

        private void btnLogFileBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                logLocation = fbd.SelectedPath;
                txtLogFile.Text = logLocation;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isFilledOut = true;
            if (txtLogFile.Text == "")
            {
                isFilledOut = false;
            }
            if (txtSampleNumber.Text == "")
            {
                isFilledOut = false;
            }
            if (txtTestNumber.Text == "")
            {
                isFilledOut = false;
            }
            if (!isFilledOut)
            {
                MessageBox.Show("Please fill in all fields.");
            }

            else
            {
                DialogResult = DialogResult.OK;
                Close();
                
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSampleNumber.Text = "";
            txtTestNumber.Text = "";
            txtLogFile.Text = "";
            sampleNumber = "";
            testNumber = "";
            logLocation = "";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            Close();
        }

        private void txtSampleNumber_TextChanged(object sender, EventArgs e)
        {
            sampleNumber = txtSampleNumber.Text;
        }

        private void txtTestNumber_TextChanged(object sender, EventArgs e)
        {
            testNumber = txtTestNumber.Text;
        }
    }
}
