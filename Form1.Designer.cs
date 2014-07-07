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
            this.tsbtn_PreviosFrame = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_PlayFrame = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_StopFrame = new System.Windows.Forms.ToolStripButton();
            this.tsbtn_NextFrame = new System.Windows.Forms.ToolStripButton();
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.tlp_CamView = new System.Windows.Forms.TableLayoutPanel();
            this.sliderFrameNumber = new System.Windows.Forms.TrackBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbo_VideoType = new System.Windows.Forms.ComboBox();
            this.cmbo_DataType = new System.Windows.Forms.ComboBox();
            this.lbl_TotalFrames = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFrameNumber = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tlp_testSettings = new System.Windows.Forms.TableLayoutPanel();
            this.lblTestSettings = new System.Windows.Forms.Label();
            this.lblTestLocation = new System.Windows.Forms.Label();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lblCam1Params = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblCam2Params = new System.Windows.Forms.Label();
            this.Camera1Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera2Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.zibReplayCam1 = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.zibReplayCam2 = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.toolStrip1.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            this.gbCamera1.SuspendLayout();
            this.gbCamera2.SuspendLayout();
            this.tlp_ReplayMode.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tlp_CamView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFrameNumber)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tlp_testSettings.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
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
            this.tsbtn_Help.Visible = false;
            this.tsbtn_Help.Click += new System.EventHandler(this.tsbtn_Help_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 79);
            // 
            // tsbtn_PreviosFrame
            // 
            this.tsbtn_PreviosFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_PreviosFrame.Enabled = false;
            this.tsbtn_PreviosFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_PreviosFrame.Image")));
            this.tsbtn_PreviosFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_PreviosFrame.Name = "tsbtn_PreviosFrame";
            this.tsbtn_PreviosFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_PreviosFrame.Text = "Previous Frame";
            this.tsbtn_PreviosFrame.Click += new System.EventHandler(this.tsbtn_PreviosFrame_Click);
            // 
            // tsbtn_PlayFrame
            // 
            this.tsbtn_PlayFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_PlayFrame.Enabled = false;
            this.tsbtn_PlayFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_PlayFrame.Image")));
            this.tsbtn_PlayFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_PlayFrame.Name = "tsbtn_PlayFrame";
            this.tsbtn_PlayFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_PlayFrame.Text = "Play";
            this.tsbtn_PlayFrame.Click += new System.EventHandler(this.tsbtn_PlayFrame_Click);
            // 
            // tsbtn_StopFrame
            // 
            this.tsbtn_StopFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_StopFrame.Enabled = false;
            this.tsbtn_StopFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_StopFrame.Image")));
            this.tsbtn_StopFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_StopFrame.Name = "tsbtn_StopFrame";
            this.tsbtn_StopFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_StopFrame.Text = "Stop";
            this.tsbtn_StopFrame.Click += new System.EventHandler(this.tsbtn_StopFrame_Click);
            // 
            // tsbtn_NextFrame
            // 
            this.tsbtn_NextFrame.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_NextFrame.Enabled = false;
            this.tsbtn_NextFrame.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_NextFrame.Image")));
            this.tsbtn_NextFrame.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_NextFrame.Name = "tsbtn_NextFrame";
            this.tsbtn_NextFrame.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_NextFrame.Text = "Next Frame";
            this.tsbtn_NextFrame.Click += new System.EventHandler(this.tsbtn_NextFrame_Click);
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
            this.tlp_Main.Size = new System.Drawing.Size(1582, 724);
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
            this.gbCamera1.Location = new System.Drawing.Point(3, 617);
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
            this.gbCamera2.Location = new System.Drawing.Point(65, 617);
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
            this.tlp_ReplayMode.Controls.Add(this.zibReplayCam1, 0, 1);
            this.tlp_ReplayMode.Controls.Add(this.zibReplayCam2, 1, 1);
            this.tlp_ReplayMode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_ReplayMode.Location = new System.Drawing.Point(127, 3);
            this.tlp_ReplayMode.Name = "tlp_ReplayMode";
            this.tlp_ReplayMode.RowCount = 3;
            this.tlp_Main.SetRowSpan(this.tlp_ReplayMode, 3);
            this.tlp_ReplayMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 135F));
            this.tlp_ReplayMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_ReplayMode.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            this.tlp_ReplayMode.Size = new System.Drawing.Size(1452, 718);
            this.tlp_ReplayMode.TabIndex = 8;
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
            // tlp_CamView
            // 
            this.tlp_CamView.ColumnCount = 4;
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14F));
            this.tlp_CamView.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36F));
            this.tlp_CamView.Controls.Add(this.sliderFrameNumber, 0, 2);
            this.tlp_CamView.Controls.Add(this.label3, 0, 0);
            this.tlp_CamView.Controls.Add(this.label4, 2, 0);
            this.tlp_CamView.Controls.Add(this.cmbo_VideoType, 1, 0);
            this.tlp_CamView.Controls.Add(this.cmbo_DataType, 3, 0);
            this.tlp_CamView.Controls.Add(this.lbl_TotalFrames, 3, 1);
            this.tlp_CamView.Controls.Add(this.label6, 1, 1);
            this.tlp_CamView.Controls.Add(this.txtFrameNumber, 2, 1);
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
            // sliderFrameNumber
            // 
            this.tlp_CamView.SetColumnSpan(this.sliderFrameNumber, 4);
            this.sliderFrameNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.sliderFrameNumber.Enabled = false;
            this.sliderFrameNumber.Location = new System.Drawing.Point(3, 63);
            this.sliderFrameNumber.Name = "sliderFrameNumber";
            this.sliderFrameNumber.Size = new System.Drawing.Size(708, 42);
            this.sliderFrameNumber.TabIndex = 0;
            this.sliderFrameNumber.Scroll += new System.EventHandler(this.sliderFrameNumber_Scroll);
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
            // cmbo_VideoType
            // 
            this.cmbo_VideoType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbo_VideoType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbo_VideoType.Enabled = false;
            this.cmbo_VideoType.FormattingEnabled = true;
            this.cmbo_VideoType.Location = new System.Drawing.Point(102, 3);
            this.cmbo_VideoType.Name = "cmbo_VideoType";
            this.cmbo_VideoType.Size = new System.Drawing.Size(251, 24);
            this.cmbo_VideoType.TabIndex = 3;
            // 
            // cmbo_DataType
            // 
            this.cmbo_DataType.Dock = System.Windows.Forms.DockStyle.Fill;
            this.cmbo_DataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbo_DataType.Enabled = false;
            this.cmbo_DataType.FormattingEnabled = true;
            this.cmbo_DataType.Location = new System.Drawing.Point(458, 3);
            this.cmbo_DataType.Name = "cmbo_DataType";
            this.cmbo_DataType.Size = new System.Drawing.Size(253, 24);
            this.cmbo_DataType.TabIndex = 4;
            // 
            // lbl_TotalFrames
            // 
            this.lbl_TotalFrames.AutoSize = true;
            this.lbl_TotalFrames.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbl_TotalFrames.Location = new System.Drawing.Point(458, 30);
            this.lbl_TotalFrames.Name = "lbl_TotalFrames";
            this.lbl_TotalFrames.Size = new System.Drawing.Size(253, 30);
            this.lbl_TotalFrames.TabIndex = 5;
            this.lbl_TotalFrames.Text = "/ x";
            this.lbl_TotalFrames.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
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
            // txtFrameNumber
            // 
            this.txtFrameNumber.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtFrameNumber.Enabled = false;
            this.txtFrameNumber.Location = new System.Drawing.Point(359, 33);
            this.txtFrameNumber.Name = "txtFrameNumber";
            this.txtFrameNumber.ReadOnly = true;
            this.txtFrameNumber.Size = new System.Drawing.Size(93, 22);
            this.txtFrameNumber.TabIndex = 7;
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
            // tlp_testSettings
            // 
            this.tlp_testSettings.ColumnCount = 3;
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 38.65546F));
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 36.27451F));
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tlp_testSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_testSettings.Controls.Add(this.lblTestSettings, 0, 0);
            this.tlp_testSettings.Controls.Add(this.lblTestLocation, 1, 0);
            this.tlp_testSettings.Controls.Add(this.btnBrowse, 2, 0);
            this.tlp_testSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_testSettings.Location = new System.Drawing.Point(3, 18);
            this.tlp_testSettings.Name = "tlp_testSettings";
            this.tlp_testSettings.RowCount = 1;
            this.tlp_testSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlp_testSettings.Size = new System.Drawing.Size(714, 108);
            this.tlp_testSettings.TabIndex = 0;
            // 
            // lblTestSettings
            // 
            this.lblTestSettings.AutoSize = true;
            this.lblTestSettings.Location = new System.Drawing.Point(3, 0);
            this.lblTestSettings.Name = "lblTestSettings";
            this.lblTestSettings.Size = new System.Drawing.Size(150, 102);
            this.lblTestSettings.TabIndex = 2;
            this.lblTestSettings.Text = "Sample Number : \r\nTest Number :\r\nImager Noise :\r\nMinimum Contrast :\r\nTarget Inten" +
    "sity :\r\nMinimum Line Length :";
            // 
            // lblTestLocation
            // 
            this.lblTestLocation.AutoSize = true;
            this.lblTestLocation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblTestLocation.Location = new System.Drawing.Point(279, 0);
            this.lblTestLocation.Name = "lblTestLocation";
            this.lblTestLocation.Size = new System.Drawing.Size(253, 108);
            this.lblTestLocation.TabIndex = 0;
            this.lblTestLocation.Text = "Test Location : C:/temp";
            this.lblTestLocation.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.btnBrowse.Location = new System.Drawing.Point(538, 34);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(115, 40);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.lblCam1Params);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox4.Location = new System.Drawing.Point(3, 571);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(720, 144);
            this.groupBox4.TabIndex = 3;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Camera 1 Information";
            // 
            // lblCam1Params
            // 
            this.lblCam1Params.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCam1Params.Location = new System.Drawing.Point(3, 18);
            this.lblCam1Params.Name = "lblCam1Params";
            this.lblCam1Params.Size = new System.Drawing.Size(714, 123);
            this.lblCam1Params.TabIndex = 0;
            this.lblCam1Params.Text = "Timestamp :\r\nImage Number :\r\nImage Size :\r\nExposure (s) :\r\nIntensity (lsb) :\r\nPot" +
    "ential Cracks : \r\nContains Crack :";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblCam2Params);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox5.Location = new System.Drawing.Point(729, 571);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(720, 144);
            this.groupBox5.TabIndex = 4;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Camera 2 Information";
            // 
            // lblCam2Params
            // 
            this.lblCam2Params.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblCam2Params.Location = new System.Drawing.Point(3, 18);
            this.lblCam2Params.Name = "lblCam2Params";
            this.lblCam2Params.Size = new System.Drawing.Size(714, 123);
            this.lblCam2Params.TabIndex = 0;
            this.lblCam2Params.Text = "Timestamp :\r\nImage Number :\r\nImage Size :\r\nExposure (s) :\r\nIntensity (lsb) :\r\nPot" +
    "ential Cracks : \r\nContains Crack :";
            // 
            // Camera1Display
            // 
            this.tlp_Main.SetColumnSpan(this.Camera1Display, 2);
            this.Camera1Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera1Display.Location = new System.Drawing.Point(3, 43);
            this.Camera1Display.Name = "Camera1Display";
            this.Camera1Display.Size = new System.Drawing.Size(56, 568);
            this.Camera1Display.TabIndex = 4;
            // 
            // Camera2Display
            // 
            this.tlp_Main.SetColumnSpan(this.Camera2Display, 2);
            this.Camera2Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera2Display.Location = new System.Drawing.Point(65, 43);
            this.Camera2Display.Name = "Camera2Display";
            this.Camera2Display.Size = new System.Drawing.Size(56, 568);
            this.Camera2Display.TabIndex = 5;
            // 
            // zibReplayCam1
            // 
            this.zibReplayCam1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zibReplayCam1.Location = new System.Drawing.Point(3, 138);
            this.zibReplayCam1.Name = "zibReplayCam1";
            this.zibReplayCam1.Size = new System.Drawing.Size(720, 427);
            this.zibReplayCam1.TabIndex = 5;
            // 
            // zibReplayCam2
            // 
            this.zibReplayCam2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zibReplayCam2.Location = new System.Drawing.Point(729, 138);
            this.zibReplayCam2.Name = "zibReplayCam2";
            this.zibReplayCam2.Size = new System.Drawing.Size(720, 427);
            this.zibReplayCam2.TabIndex = 6;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1582, 803);
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
            this.tlp_CamView.ResumeLayout(false);
            this.tlp_CamView.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sliderFrameNumber)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tlp_testSettings.ResumeLayout(false);
            this.tlp_testSettings.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
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
        private ZoomImageBox zibReplayCam1;
        private ZoomImageBox zibReplayCam2;
        private System.Windows.Forms.TableLayoutPanel tlp_CamView;
        private System.Windows.Forms.TrackBar sliderFrameNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbo_VideoType;
        private System.Windows.Forms.ComboBox cmbo_DataType;
        private System.Windows.Forms.Label lbl_TotalFrames;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFrameNumber;
        private System.Windows.Forms.TableLayoutPanel tlp_testSettings;
        private System.Windows.Forms.Label lblTestSettings;
        private System.Windows.Forms.Label lblTestLocation;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Label lblCam1Params;
        private System.Windows.Forms.Label lblCam2Params;
    }
}

