namespace SAF_OpticalFailureDetector
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtn_Settings = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_RefreshCamera = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_Start = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_Stop = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_Help = new System.Windows.Forms.ToolStripButton();
            this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.Camera1Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera1Process = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera2Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera2Process = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.toolStrip1.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtn_Settings,
            this.tsbtn_RefreshCamera,
            this.tsbtn_Start,
            this.tsbtn_Stop,
            this.tsbtn_Help});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1582, 79);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtn_Settings
            // 
            this.tsbtn_Settings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_Settings.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_Settings.Image")));
            this.tsbtn_Settings.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_Settings.Name = "tsbtn_Settings";
            this.tsbtn_Settings.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_Settings.Text = "Settings";
            this.tsbtn_Settings.Click += new System.EventHandler(this.tsbtn_Settings_Click);
            // 
            // tsbtn_RefreshCamera
            // 
            this.tsbtn_RefreshCamera.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_RefreshCamera.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_RefreshCamera.Image")));
            this.tsbtn_RefreshCamera.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_RefreshCamera.Name = "tsbtn_RefreshCamera";
            this.tsbtn_RefreshCamera.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_RefreshCamera.Text = "Refresh Camera";
            this.tsbtn_RefreshCamera.Click += new System.EventHandler(this.tsbtn_RefreshCamera_Click);
            // 
            // tsbtn_Start
            // 
            this.tsbtn_Start.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_Start.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_Start.Image")));
            this.tsbtn_Start.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtn_Start.Name = "tsbtn_Start";
            this.tsbtn_Start.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_Start.Text = "Start";
            this.tsbtn_Start.Click += new System.EventHandler(this.tsbtn_Start_Click);
            // 
            // tsbtn_Stop
            // 
            this.tsbtn_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_Stop.Enabled = false;
            this.tsbtn_Stop.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_Stop.Image")));
            this.tsbtn_Stop.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtn_Stop.Name = "tsbtn_Stop";
            this.tsbtn_Stop.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_Stop.Text = "Stop";
            this.tsbtn_Stop.Click += new System.EventHandler(this.tsbtn_Stop_Click);
            // 
            // tsbtn_Help
            // 
            this.tsbtn_Help.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsbtn_Help.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_Help.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_Help.Image")));
            this.tsbtn_Help.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_Help.Name = "tsbtn_Help";
            this.tsbtn_Help.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_Help.Text = "Help";
            this.tsbtn_Help.Click += new System.EventHandler(this.tsbtn_Help_Click);
            // 
            // tlp_Main
            // 
            this.tlp_Main.ColumnCount = 2;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.Controls.Add(this.Camera1Display, 0, 0);
            this.tlp_Main.Controls.Add(this.Camera1Process, 0, 1);
            this.tlp_Main.Controls.Add(this.Camera2Display, 1, 0);
            this.tlp_Main.Controls.Add(this.Camera2Process, 1, 1);
            this.tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Main.Location = new System.Drawing.Point(0, 79);
            this.tlp_Main.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlp_Main.Name = "tlp_Main";
            this.tlp_Main.RowCount = 2;
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.Size = new System.Drawing.Size(1582, 774);
            this.tlp_Main.TabIndex = 2;
            // 
            // Camera1Display
            // 
            this.Camera1Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera1Display.Location = new System.Drawing.Point(3, 3);
            this.Camera1Display.Name = "Camera1Display";
            this.Camera1Display.Size = new System.Drawing.Size(785, 381);
            this.Camera1Display.TabIndex = 9;
            // 
            // Camera1Process
            // 
            this.Camera1Process.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera1Process.Location = new System.Drawing.Point(3, 390);
            this.Camera1Process.Name = "Camera1Process";
            this.Camera1Process.Size = new System.Drawing.Size(785, 381);
            this.Camera1Process.TabIndex = 10;
            // 
            // Camera2Display
            // 
            this.Camera2Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera2Display.Location = new System.Drawing.Point(794, 3);
            this.Camera2Display.Name = "Camera2Display";
            this.Camera2Display.Size = new System.Drawing.Size(785, 381);
            this.Camera2Display.TabIndex = 11;
            // 
            // Camera2Process
            // 
            this.Camera2Process.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera2Process.Location = new System.Drawing.Point(794, 390);
            this.Camera2Process.Name = "Camera2Process";
            this.Camera2Process.Size = new System.Drawing.Size(785, 381);
            this.Camera2Process.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 853);
            this.Controls.Add(this.tlp_Main);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Optical Failure Detector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tlp_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tlp_Main;
        private System.Windows.Forms.ToolStripButton tsbtn_Settings;
        private System.Windows.Forms.ToolStripButton tsbtn_Start;
        private System.Windows.Forms.ToolStripButton tsbtn_Stop;
        private System.Windows.Forms.ToolStripButton tsbtn_Help;
        private System.Windows.Forms.ToolStripButton tsbtn_RefreshCamera;
        private threading.ZoomImageBox Camera1Display;
        private threading.ZoomImageBox Camera1Process;
        private threading.ZoomImageBox Camera2Display;
        private threading.ZoomImageBox Camera2Process;
    }
}

