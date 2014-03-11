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
        public Settings()
        {
            InitializeComponent();
        }

        private void btnLogFileBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            if(fbd.ShowDialog() == DialogResult.OK)
            {
                txtLogFile.Text = fbd.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            bool isFilledOut = true;
            if (cmboCam1.Text == "Select...")
            {
                isFilledOut = false;
            }
            if (cmboCam2.Text == "Select...")
            {
                isFilledOut = false;
            }
            if (txtEmail.Text == "")
            {
                isFilledOut = false;
            }
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
            
            //messenger.Messenger mess = new messenger.Messenger(txtEmail.Text,txtTestNumber.Text,txtSampleNumber.Text);
            //1mess.SendMessage();/
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            txtSampleNumber.Text = "";
            txtTestNumber.Text = "";
            txtLogFile.Text = "";
            txtEmail.Text = "";
            cmboCam1.Text = "Select...";
            cmboCam2.Text = "Select...";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Abort;
            Close();
        }
    }
}
