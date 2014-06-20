namespace SAF_OpticalFailureDetector
{
    using threading;
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsbtn_ReplayMode = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_CameraMode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtn_Settings = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_RefreshCamera = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_Start = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_Stop = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_Help = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.cmboCam1View = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmboCam2View = new System.Windows.Forms.ComboBox();
            this.gbCamera1 = new System.Windows.Forms.GroupBox();
            this.lblCam1PotentialCracks = new System.Windows.Forms.Label();
            this.lblCam1CracksDetected = new System.Windows.Forms.Label();
            this.lblCam1Intensity = new System.Windows.Forms.Label();
            this.lblCam1Exposure = new System.Windows.Forms.Label();
            this.lblCam1IPFPS = new System.Windows.Forms.Label();
            this.lblCam1FPS = new System.Windows.Forms.Label();
            this.gbCamera2 = new System.Windows.Forms.GroupBox();
            this.lblCam2PotentialCracks = new System.Windows.Forms.Label();
            this.lblCam2CracksDetected = new System.Windows.Forms.Label();
            this.lblCam2Intensity = new System.Windows.Forms.Label();
            this.lblCam2Exposure = new System.Windows.Forms.Label();
            this.lblCam2IPFPS = new System.Windows.Forms.Label();
            this.lblCam2FPS = new System.Windows.Forms.Label();
            this.tlp_ReplayMode = new System.Windows.Forms.TableLayoutPanel();
            this.tsbtn_PreviosFrame = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_PlayFrame = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_StopFrame = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_NextFrame = new System.Windows.Forms.ToolStripButton();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tlp_CamView = new System.Windows.Forms.TableLayoutPanel();
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tlp_testSettings = new System.Windows.Forms.TableLayoutPanel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.Camera1Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera2Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.zoomImageBox1 = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.zoomImageBox2 = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.toolStrip1.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            this.gbCamera1.SuspendLayout();
            this.gbCamera2.SuspendLayout();
            this.tlp_ReplayMode.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tlp_CamView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.tlp_testSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbtn_ReplayMode,
            this.tsbtn_CameraMode,
            this.toolStripSeparator1,
            this.tsbtn_Settings,
            this.tsbtn_RefreshCamera,
            this.tsbtn_Start,
            this.tsbtn_Stop,
            this.tsbtn_Help,
            this.toolStripSeparator2,
            this.tsbtn_PreviosFrame,
            this.tsbtn_PlayFrame,
            this.tsbtn_StopFrame,
            this.tsbtn_NextFrame});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1582, 79);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsbtn_ReplayMode
            // 
            this.tsbtn_ReplayMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_ReplayMode.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_ReplayMode.Image")));
            this.tsbtn_ReplayMode.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtn_ReplayMode.Name = "tsbtn_ReplayMode";
            this.tsbtn_ReplayMode.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_ReplayMode.Text = "toolStripButton2";
            this.tsbtn_ReplayMode.ToolTipText = "Load Video";
            this.tsbtn_ReplayMode.Click += new System.EventHandler(this.tsbtn_ReplayMode_Click);
            // 
            // tsbtn_CameraMode
            // 
            this.tsbtn_CameraMode.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_CameraMode.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_CameraMode.Image")));
            this.tsbtn_CameraMode.ImageTransparentColor = System.Drawing.Color.White;
            this.tsbtn_CameraMode.Name = "tsbtn_CameraMode";
            this.tsbtn_CameraMode.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_CameraMode.Text = "toolStripButton1";
            this.tsbtn_CameraMode.ToolTipText = "Capture Video";
            this.tsbtn_CameraMode.Click += new System.EventHandler(this.tsbtn_CameraMode_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 79);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 79);
            // 
            // tlp_Main
            // 
            this.tlp_Main.ColumnCount = 5;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 92F));
            this.tlp_Main.Controls.Add(this.label1, 0, 0);
            this.tlp_Main.Controls.Add(this.cmboCam1View, 1, 0);
            this.tlp_Main.Controls.Add(this.label2, 2, 0);
            this.tlp_Main.Controls.Add(this.cmboCam2View, 3, 0);
            this.tlp_Main.Controls.Add(this.Camera1Display, 0, 1);
            this.tlp_Main.Controls.Add(this.Camera2Display, 2, 1);
            this.tlp_Main.Controls.Add(this.gbCamera1, 0, 2);
            this.tlp_Main.Controls.Add(this.gbCamera2, 2, 2);
            this.tlp_Main.Controls.Add(this.tlp_ReplayMode, 4, 0);
            this.tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Main.Location = new System.Drawing.Point(0, 79);
            this.tlp_Main.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlp_Main.Name = "tlp_Main";
            this.tlp_Main.RowCount = 3;
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tlp_Main.Size = new System.Drawing.Size(1582, 699);
            this.tlp_Main.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(25, 40);
            this.label1.TabIndex = 0;
            this.label1.Text = "Camera 1 View:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmboCam1View
            // 
            this.cmboCam1View.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmboCam1View.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboCam1View.FormattingEnabled = true;
            this.cmboCam1View.Location = new System.Drawing.Point(34, 7);
            this.cmboCam1View.Name = "cmboCam1View";
            this.cmboCam1View.Size = new System.Drawing.Size(25, 24);
            this.cmboCam1View.TabIndex = 1;
            this.cmboCam1View.TextChanged += new System.EventHandler(this.cmboCam1View_TextChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(65, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 40);
            this.label2.TabIndex = 2;
            this.label2.Text = "Camera 2 View:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cmboCam2View
            // 
            this.cmboCam2View.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.cmboCam2View.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmboCam2View.FormattingEnabled = true;
            this.cmboCam2View.Location = new System.Drawing.Point(96, 7);
            this.cmboCam2View.Name = "cmboCam2View";
            this.cmboCam2View.Size = new System.Drawing.Size(25, 24);
            this.cmboCam2View.TabIndex = 3;
            this.cmboCam2View.TextChanged += new System.EventHandler(this.cmboCam2View_TextChanged);
            // 
            // gbCamera1
            // 
            this.tlp_Main.SetColumnSpan(this.gbCamera1, 2);
            this.gbCamera1.Controls.Add(this.lblCam1PotentialCracks);
            this.gbCamera1.Controls.Add(this.lblCam1CracksDetected);
            this.gbCamera1.Controls.Add(this.lblCam1Intensity);
            this.gbCamera1.Controls.Add(this.lblCam1Exposure);
            this.gbCamera1.Controls.Add(this.lblCam1IPFPS);
            this.gbCamera1.Controls.Add(this.lblCam1FPS);
            this.gbCamera1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCamera1.Location = new System.Drawing.Point(3, 592);
            this.gbCamera1.Name = "gbCamera1";
            this.gbCamera1.Size = new System.Drawing.Size(56, 104);
            this.gbCamera1.TabIndex = 6;
            this.gbCamera1.TabStop = false;
            this.gbCamera1.Text = "Camera 1 Information";
            // 
            // lblCam1PotentialCracks
            // 
            this.lblCam1PotentialCracks.AutoSize = true;
            this.lblCam1PotentialCracks.Location = new System.Drawing.Point(285, 78);
            this.lblCam1PotentialCracks.Name = "lblCam1PotentialCracks";
            this.lblCam1PotentialCracks.Size = new System.Drawing.Size(138, 17);
            this.lblCam1PotentialCracks.TabIndex = 5;
            this.lblCam1PotentialCracks.Text = "Potential Cracks: n/a";
            this.lblCam1PotentialCracks.Visible = false;
            // 
            // lblCam1CracksDetected
            // 
            this.lblCam1CracksDetected.AutoSize = true;
            this.lblCam1CracksDetected.Location = new System.Drawing.Point(285, 50);
            this.lblCam1CracksDetected.Name = "lblCam1CracksDetected";
            this.lblCam1CracksDetected.Size = new System.Drawing.Size(143, 17);
            this.lblCam1CracksDetected.TabIndex = 4;
            this.lblCam1CracksDetected.Text = "Crack Detected: false";
            // 
            // lblCam1Intensity
            // 
            this.lblCam1Intensity.AutoSize = true;
            this.lblCam1Intensity.Location = new System.Drawing.Point(14, 78);
            this.lblCam1Intensity.Name = "lblCam1Intensity";
            this.lblCam1Intensity.Size = new System.Drawing.Size(118, 17);
            this.lblCam1Intensity.TabIndex = 3;
            this.lblCam1Intensity.Text = "Intensity: n/a LSB";
            // 
            // lblCam1Exposure
            // 
            this.lblCam1Exposure.AutoSize = true;
            this.lblCam1Exposure.Location = new System.Drawing.Point(14, 50);
            this.lblCam1Exposure.Name = "lblCam1Exposure";
            this.lblCam1Exposure.Size = new System.Drawing.Size(117, 17);
            this.lblCam1Exposure.TabIndex = 2;
            this.lblCam1Exposure.Text = "Exposure: n/a ms";
            // 
            // lblCam1IPFPS
            // 
            this.lblCam1IPFPS.AutoSize = true;
            this.lblCam1IPFPS.Location = new System.Drawing.Point(285, 22);
            this.lblCam1IPFPS.Name = "lblCam1IPFPS";
            this.lblCam1IPFPS.Size = new System.Drawing.Size(172, 17);
            this.lblCam1IPFPS.TabIndex = 1;
            this.lblCam1IPFPS.Text = "Image Processor FPS: n/a";
            // 
            // lblCam1FPS
            // 
            this.lblCam1FPS.AutoSize = true;
            this.lblCam1FPS.Location = new System.Drawing.Point(14, 22);
            this.lblCam1FPS.Name = "lblCam1FPS";
            this.lblCam1FPS.Size = new System.Drawing.Size(161, 17);
            this.lblCam1FPS.TabIndex = 0;
            this.lblCam1FPS.Text = "Frames Per Second: n/a";
            // 
            // gbCamera2
            // 
            this.tlp_Main.SetColumnSpan(this.gbCamera2, 2);
            this.gbCamera2.Controls.Add(this.lblCam2PotentialCracks);
            this.gbCamera2.Controls.Add(this.lblCam2CracksDetected);
            this.gbCamera2.Controls.Add(this.lblCam2Intensity);
            this.gbCamera2.Controls.Add(this.lblCam2Exposure);
            this.gbCamera2.Controls.Add(this.lblCam2IPFPS);
            this.gbCamera2.Controls.Add(this.lblCam2FPS);
            this.gbCamera2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbCamera2.Location = new System.Drawing.Point(65, 592);
            this.gbCamera2.Name = "gbCamera2";
            this.gbCamera2.Size = new System.Drawing.Size(56, 104);
            this.gbCamera2.TabIndex = 7;
            this.gbCamera2.TabStop = false;
            this.gbCamera2.Text = "Camera 2 Information";
            // 
            // lblCam2PotentialCracks
            // 
            this.lblCam2PotentialCracks.AutoSize = true;
            this.lblCam2PotentialCracks.Location = new System.Drawing.Point(292, 78);
            this.lblCam2PotentialCracks.Name = "lblCam2PotentialCracks";
            this.lblCam2PotentialCracks.Size = new System.Drawing.Size(138, 17);
            this.lblCam2PotentialCracks.TabIndex = 11;
            this.lblCam2PotentialCracks.Text = "Potential Cracks: n/a";
            this.lblCam2PotentialCracks.Visible = false;
            // 
            // lblCam2CracksDetected
            // 
            this.lblCam2CracksDetected.AutoSize = true;
            this.lblCam2CracksDetected.Location = new System.Drawing.Point(292, 50);
            this.lblCam2CracksDetected.Name = "lblCam2CracksDetected";
            this.lblCam2CracksDetected.Size = new System.Drawing.Size(143, 17);
            this.lblCam2CracksDetected.TabIndex = 10;
            this.lblCam2CracksDetected.Text = "Crack Detected: false";
            // 
            // lblCam2Intensity
            // 
            this.lblCam2Intensity.AutoSize = true;
            this.lblCam2Intensity.Location = new System.Drawing.Point(21, 78);
            this.lblCam2Intensity.Name = "lblCam2Intensity";
            this.lblCam2Intensity.Size = new System.Drawing.Size(118, 17);
            this.lblCam2Intensity.TabIndex = 9;
            this.lblCam2Intensity.Text = "Intensity: n/a LSB";
            // 
            // lblCam2Exposure
            // 
            this.lblCam2Exposure.AutoSize = true;
            this.lblCam2Exposure.Location = new System.Drawing.Point(21, 50);
            this.lblCam2Exposure.Name = "lblCam2Exposure";
            this.lblCam2Exposure.Size = new System.Drawing.Size(117, 17);
            this.lblCam2Exposure.TabIndex = 8;
            this.lblCam2Exposure.Text = "Exposure: n/a ms";
            // 
            // lblCam2IPFPS
            // 
            this.lblCam2IPFPS.AutoSize = true;
            this.lblCam2IPFPS.Location = new System.Drawing.Point(292, 22);
            this.lblCam2IPFPS.Name = "lblCam2IPFPS";
            this.lblCam2IPFPS.Size = new System.Drawing.Size(172, 17);
            this.lblCam2IPFPS.TabIndex = 7;
            this.lblCam2IPFPS.Text = "Image Processor FPS: n/a";
            // 
            // lblCam2FPS
            // 
            this.lblCam2FPS.AutoSize = true;
            this.lblCam2FPS.Location = new System.Drawing.Point(21, 22);
            this.lblCam2FPS.Name = "lblCam2FPS";
            this.lblCam2FPS.Size = new System.Drawing.Size(161, 17);
            this.lblCam2FPS.TabIndex = 6;
            this.lblCam2FPS.Text = "Frames Per Second: n/a";
            // 
            // tlp_ReplayMode
            // 
            this.tlp_ReplayMode.ColumnCount = 2;
            this.tlp_ReplayMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_ReplayMode.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_ReplayMode.Controls.Add(this.groupBox2, 0, 0);
            this.tlp_ReplayMode.Controls.Add(this.groupBox1, 1, 0);
            this.tlp_ReplayMode.Controls.Add(this.groupBox4, 0, 2);
            this.tlp_ReplayMode.Controls.Add(this.groupBox5, 1, 2);
            this.tlp_ReplayMode.Controls.Add(this.zoomImageBox1, 0, 1);
            this.tlp_ReplayMode.Controls.Add(this.zoomImageBox2, 1, 1);
            this.tlp_ReplayMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_ReplayMode.Location = new System.Drawing.Point(127, 3);
            this.tlp_ReplayMode.Name = "tlp_ReplayMode";
            this.tlp_ReplayMode.RowCount = 3;
            this.tlp_Main.SetRowSpan(this.tlp_ReplayMode, 3);
            this.tlp_ReplayMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tlp_ReplayMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_ReplayMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlp_ReplayMode.Size = new System.Drawing.Size(1452, 693);
            this.tlp_ReplayMode.TabIndex = 8;
            // 
            // tsbtn_PreviosFrame
            // 
            this.tsbtn_PreviosFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_PreviosFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_PreviosFrame.Image")));
            this.tsbtn_PreviosFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_PreviosFrame.Name = "tsbtn_PreviosFrame";
            this.tsbtn_PreviosFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_PreviosFrame.Text = "toolStripButton1";
            // 
            // tsbtn_PlayFrame
            // 
            this.tsbtn_PlayFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_PlayFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_PlayFrame.Image")));
            this.tsbtn_PlayFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_PlayFrame.Name = "tsbtn_PlayFrame";
            this.tsbtn_PlayFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_PlayFrame.Text = "toolStripButton2";
            // 
            // tsbtn_StopFrame
            // 
            this.tsbtn_StopFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_StopFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_StopFrame.Image")));
            this.tsbtn_StopFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_StopFrame.Name = "tsbtn_StopFrame";
            this.tsbtn_StopFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_StopFrame.Text = "toolStripButton3";
            // 
            // tsbtn_NextFrame
            // 
            this.tsbtn_NextFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_NextFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_NextFrame.Image")));
            this.tsbtn_NextFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_NextFrame.Name = "tsbtn_NextFrame";
            this.tsbtn_NextFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_NextFrame.Text = "toolStripButton4";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.tlp_CamView);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(720, 129);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Camera View";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tlp_testSettings);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(729, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(720, 129);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Test Settings";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label9);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 546);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(720, 144);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Camera 1 Information";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label10);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(729, 546);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(720, 144);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Camera 2 Information";
            // 
            // tlp_CamView
            // 
            this.tlp_CamView.ColumnCount = 4;
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlp_CamView.Controls.Add(this.trackBar1, 0, 2);
            this.tlp_CamView.Controls.Add(this.label3, 0, 0);
            this.tlp_CamView.Controls.Add(this.label4, 2, 0);
            this.tlp_CamView.Controls.Add(this.comboBox1, 1, 0);
            this.tlp_CamView.Controls.Add(this.comboBox2, 3, 0);
            this.tlp_CamView.Controls.Add(this.label5, 3, 1);
            this.tlp_CamView.Controls.Add(this.label6, 1, 1);
            this.tlp_CamView.Controls.Add(this.textBox1, 2, 1);
            this.tlp_CamView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_CamView.Location = new System.Drawing.Point(3, 18);
            this.tlp_CamView.Name = "tlp_CamView";
            this.tlp_CamView.RowCount = 3;
            this.tlp_CamView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlp_CamView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tlp_CamView.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_CamView.Size = new System.Drawing.Size(714, 108);
            this.tlp_CamView.TabIndex = 0;
            // 
            // trackBar1
            // 
            this.tlp_CamView.SetColumnSpan(this.trackBar1, 4);
            this.trackBar1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trackBar1.Location = new System.Drawing.Point(3, 63);
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(708, 42);
            this.trackBar1.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 30);
            this.label3.TabIndex = 1;
            this.label3.Text = "Video Type: ";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Location = new System.Drawing.Point(359, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(93, 30);
            this.label4.TabIndex = 2;
            this.label4.Text = "Data Type:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // comboBox1
            // 
            this.comboBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(102, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(251, 24);
            this.comboBox1.TabIndex = 3;
            // 
            // comboBox2
            // 
            this.comboBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Location = new System.Drawing.Point(458, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(253, 24);
            this.comboBox2.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Location = new System.Drawing.Point(458, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(253, 30);
            this.label5.TabIndex = 5;
            this.label5.Text = "/ x";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Location = new System.Drawing.Point(102, 30);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(251, 30);
            this.label6.TabIndex = 6;
            this.label6.Text = "Frame: ";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // textBox1
            // 
            this.textBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBox1.Location = new System.Drawing.Point(359, 33);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(93, 22);
            this.textBox1.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 102);
            this.label8.TabIndex = 2;
            this.label8.Text = "Sample Number : \r\nTest Number :\r\nImager Noise :\r\nMinimum Contrast :\r\nTarget Inten" +
    "sity :\r\nMinimum Line Length :";
            // 
            // button1
            // 
            this.button1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.button1.Location = new System.Drawing.Point(538, 34);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(115, 40);
            this.button1.TabIndex = 1;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label7.Location = new System.Drawing.Point(279, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(253, 108);
            this.label7.TabIndex = 0;
            this.label7.Text = "Test Location : C:/temp";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // tlp_testSettings
            // 
            this.tlp_testSettings.ColumnCount = 3;
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.65546F));
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.27451F));
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_testSettings.Controls.Add(this.label8, 0, 0);
            this.tlp_testSettings.Controls.Add(this.label7, 1, 0);
            this.tlp_testSettings.Controls.Add(this.button1, 2, 0);
            this.tlp_testSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_testSettings.Location = new System.Drawing.Point(3, 18);
            this.tlp_testSettings.Name = "tlp_testSettings";
            this.tlp_testSettings.RowCount = 1;
            this.tlp_testSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_testSettings.Size = new System.Drawing.Size(714, 108);
            this.tlp_testSettings.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label9.Location = new System.Drawing.Point(3, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(714, 123);
            this.label9.TabIndex = 0;
            this.label9.Text = "Timestamp :\r\nImage Number :\r\nImage Size :\r\nExposure (s) :\r\nIntensity (lsb) :\r\nPot" +
    "ential Cracks : \r\nContains Crack :";
            // 
            // label10
            // 
            this.label10.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label10.Location = new System.Drawing.Point(3, 18);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(714, 123);
            this.label10.TabIndex = 0;
            this.label10.Text = "Timestamp :\r\nImage Number :\r\nImage Size :\r\nExposure (s) :\r\nIntensity (lsb) :\r\nPot" +
    "ential Cracks : \r\nContains Crack :";
            // 
            // Camera1Display
            // 
            this.tlp_Main.SetColumnSpan(this.Camera1Display, 2);
            this.Camera1Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera1Display.Location = new System.Drawing.Point(3, 43);
            this.Camera1Display.Name = "Camera1Display";
            this.Camera1Display.Size = new System.Drawing.Size(56, 543);
            this.Camera1Display.TabIndex = 4;
            // 
            // Camera2Display
            // 
            this.tlp_Main.SetColumnSpan(this.Camera2Display, 2);
            this.Camera2Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera2Display.Location = new System.Drawing.Point(65, 43);
            this.Camera2Display.Name = "Camera2Display";
            this.Camera2Display.Size = new System.Drawing.Size(56, 543);
            this.Camera2Display.TabIndex = 5;
            // 
            // zoomImageBox1
            // 
            this.zoomImageBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomImageBox1.Location = new System.Drawing.Point(3, 138);
            this.zoomImageBox1.Name = "zoomImageBox1";
            this.zoomImageBox1.Size = new System.Drawing.Size(720, 402);
            this.zoomImageBox1.TabIndex = 5;
            // 
            // zoomImageBox2
            // 
            this.zoomImageBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zoomImageBox2.Location = new System.Drawing.Point(729, 138);
            this.zoomImageBox2.Name = "zoomImageBox2";
            this.zoomImageBox2.Size = new System.Drawing.Size(720, 402);
            this.zoomImageBox2.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 778);
            this.Controls.Add(this.tlp_Main);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "MainForm";
            this.Text = "Optical Failure Detector";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.tlp_Main.ResumeLayout(false);
            this.tlp_Main.PerformLayout();
            this.gbCamera1.ResumeLayout(false);
            this.gbCamera1.PerformLayout();
            this.gbCamera2.ResumeLayout(false);
            this.gbCamera2.PerformLayout();
            this.tlp_ReplayMode.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.tlp_CamView.ResumeLayout(false);
            this.tlp_CamView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.tlp_testSettings.ResumeLayout(false);
            this.tlp_testSettings.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmboCam1View;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmboCam2View;
        private ZoomImageBox Camera1Display;
        private ZoomImageBox Camera2Display;
        private System.Windows.Forms.GroupBox gbCamera1;
        private System.Windows.Forms.GroupBox gbCamera2;
        private System.Windows.Forms.Label lblCam1PotentialCracks;
        private System.Windows.Forms.Label lblCam1CracksDetected;
        private System.Windows.Forms.Label lblCam1Intensity;
        private System.Windows.Forms.Label lblCam1Exposure;
        private System.Windows.Forms.Label lblCam1IPFPS;
        private System.Windows.Forms.Label lblCam1FPS;
        private System.Windows.Forms.Label lblCam2PotentialCracks;
        private System.Windows.Forms.Label lblCam2CracksDetected;
        private System.Windows.Forms.Label lblCam2Intensity;
        private System.Windows.Forms.Label lblCam2Exposure;
        private System.Windows.Forms.Label lblCam2IPFPS;
        private System.Windows.Forms.Label lblCam2FPS;
        private System.Windows.Forms.ToolStripButton tsbtn_ReplayMode;
        private System.Windows.Forms.ToolStripButton tsbtn_CameraMode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbtn_PreviosFrame;
        private System.Windows.Forms.ToolStripButton tsbtn_PlayFrame;
        private System.Windows.Forms.ToolStripButton tsbtn_StopFrame;
        private System.Windows.Forms.ToolStripButton tsbtn_NextFrame;
        private System.Windows.Forms.TableLayoutPanel tlp_ReplayMode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox5;
        private ZoomImageBox zoomImageBox1;
        private ZoomImageBox zoomImageBox2;
        private System.Windows.Forms.TableLayoutPanel tlp_CamView;
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TableLayoutPanel tlp_testSettings;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
    }
}

