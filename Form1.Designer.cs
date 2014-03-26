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
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tslbl_Status = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tlp_Main = new System.Windows.Forms.TableLayoutPanel();
            this.camera1ImageBox = new System.Windows.Forms.PictureBox();
            this.process1ImageBox = new System.Windows.Forms.PictureBox();
            this.panel_cntls = new System.Windows.Forms.Panel();
            this.nud_min_contrast = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nud_noise_lvl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_LoadImage = new System.Windows.Forms.Button();
            this.process2ImageBox = new System.Windows.Forms.PictureBox();
            this.camera2ImageBox = new System.Windows.Forms.PictureBox();
            this.toolStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.camera1ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.process1ImageBox)).BeginInit();
            this.panel_cntls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_min_contrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_noise_lvl)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.process2ImageBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera2ImageBox)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(911, 79);
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
            this.tsbtn_Start.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtn_Start.Name = "tsbtn_Start";
            this.tsbtn_Start.Size = new System.Drawing.Size(68, 76);
            this.tsbtn_Start.Text = "Start";
            this.tsbtn_Start.Click += new System.EventHandler(this.tsbtn_Start_Click);
            // 
            // tsbtn_Stop
            // 
            this.tsbtn_Stop.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbtn_Stop.Image = ((System.Drawing.Image)(resources.GetObject("tsbtn_Stop.Image")));
            this.tsbtn_Stop.ImageTransparentColor = System.Drawing.Color.Magenta;
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
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tslbl_Status,
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 717);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 13, 0);
            this.statusStrip1.Size = new System.Drawing.Size(911, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tslbl_Status
            // 
            this.tslbl_Status.Name = "tslbl_Status";
            this.tslbl_Status.Size = new System.Drawing.Size(0, 20);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(151, 20);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            // 
            // tlp_Main
            // 
            this.tlp_Main.ColumnCount = 3;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tlp_Main.Controls.Add(this.camera2ImageBox, 1, 0);
            this.tlp_Main.Controls.Add(this.process2ImageBox, 1, 1);
            this.tlp_Main.Controls.Add(this.camera1ImageBox, 0, 0);
            this.tlp_Main.Controls.Add(this.process1ImageBox, 0, 1);
            this.tlp_Main.Controls.Add(this.panel_cntls, 2, 0);
            this.tlp_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlp_Main.Location = new System.Drawing.Point(0, 79);
            this.tlp_Main.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tlp_Main.Name = "tlp_Main";
            this.tlp_Main.RowCount = 2;
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tlp_Main.Size = new System.Drawing.Size(911, 638);
            this.tlp_Main.TabIndex = 2;
            // 
            // camera1ImageBox
            // 
            this.camera1ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camera1ImageBox.Location = new System.Drawing.Point(3, 2);
            this.camera1ImageBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.camera1ImageBox.Name = "camera1ImageBox";
            this.camera1ImageBox.Size = new System.Drawing.Size(379, 315);
            this.camera1ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.camera1ImageBox.TabIndex = 0;
            this.camera1ImageBox.TabStop = false;
            // 
            // process1ImageBox
            // 
            this.process1ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.process1ImageBox.Location = new System.Drawing.Point(3, 321);
            this.process1ImageBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.process1ImageBox.Name = "process1ImageBox";
            this.process1ImageBox.Size = new System.Drawing.Size(379, 315);
            this.process1ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.process1ImageBox.TabIndex = 1;
            this.process1ImageBox.TabStop = false;
            // 
            // panel_cntls
            // 
            this.panel_cntls.Controls.Add(this.nud_min_contrast);
            this.panel_cntls.Controls.Add(this.label2);
            this.panel_cntls.Controls.Add(this.nud_noise_lvl);
            this.panel_cntls.Controls.Add(this.label1);
            this.panel_cntls.Controls.Add(this.btn_LoadImage);
            this.panel_cntls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_cntls.Location = new System.Drawing.Point(773, 2);
            this.panel_cntls.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_cntls.Name = "panel_cntls";
            this.tlp_Main.SetRowSpan(this.panel_cntls, 2);
            this.panel_cntls.Size = new System.Drawing.Size(135, 634);
            this.panel_cntls.TabIndex = 2;
            // 
            // nud_min_contrast
            // 
            this.nud_min_contrast.Location = new System.Drawing.Point(9, 180);
            this.nud_min_contrast.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nud_min_contrast.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_min_contrast.Name = "nud_min_contrast";
            this.nud_min_contrast.Size = new System.Drawing.Size(121, 22);
            this.nud_min_contrast.TabIndex = 4;
            this.nud_min_contrast.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_min_contrast.ValueChanged += new System.EventHandler(this.nud_min_contrast_ValueChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 159);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 17);
            this.label2.TabIndex = 3;
            this.label2.Text = "Minimum Contrast";
            // 
            // nud_noise_lvl
            // 
            this.nud_noise_lvl.Location = new System.Drawing.Point(9, 107);
            this.nud_noise_lvl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.nud_noise_lvl.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nud_noise_lvl.Name = "nud_noise_lvl";
            this.nud_noise_lvl.Size = new System.Drawing.Size(121, 22);
            this.nud_noise_lvl.TabIndex = 2;
            this.nud_noise_lvl.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nud_noise_lvl.ValueChanged += new System.EventHandler(this.nud_noise_lvl_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 86);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(82, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Noise Level";
            // 
            // btn_LoadImage
            // 
            this.btn_LoadImage.Location = new System.Drawing.Point(773, 2);
            this.btn_LoadImage.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btn_LoadImage.Name = "btn_LoadImage";
            this.btn_LoadImage.Size = new System.Drawing.Size(111, 44);
            this.btn_LoadImage.TabIndex = 0;
            this.btn_LoadImage.Text = "Load Image";
            this.btn_LoadImage.UseVisualStyleBackColor = true;
            // 
            // process2ImageBox
            // 
            this.process2ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.process2ImageBox.Location = new System.Drawing.Point(388, 321);
            this.process2ImageBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.process2ImageBox.Name = "process2ImageBox";
            this.process2ImageBox.Size = new System.Drawing.Size(379, 315);
            this.process2ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.process2ImageBox.TabIndex = 3;
            this.process2ImageBox.TabStop = false;
            // 
            // camera2ImageBox
            // 
            this.camera2ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.camera2ImageBox.Location = new System.Drawing.Point(388, 2);
            this.camera2ImageBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.camera2ImageBox.Name = "camera2ImageBox";
            this.camera2ImageBox.Size = new System.Drawing.Size(379, 315);
            this.camera2ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.camera2ImageBox.TabIndex = 4;
            this.camera2ImageBox.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(911, 742);
            this.Controls.Add(this.tlp_Main);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "Form1";
            this.Text = "Optical Failure Detector";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.tlp_Main.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.camera1ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.process1ImageBox)).EndInit();
            this.panel_cntls.ResumeLayout(false);
            this.panel_cntls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_min_contrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_noise_lvl)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.process2ImageBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.camera2ImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tslbl_Status;
        private System.Windows.Forms.TableLayoutPanel tlp_Main;
        private System.Windows.Forms.PictureBox camera1ImageBox;
        private System.Windows.Forms.PictureBox process1ImageBox;
        private System.Windows.Forms.Panel panel_cntls;
        private System.Windows.Forms.NumericUpDown nud_noise_lvl;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btn_LoadImage;
        private System.Windows.Forms.NumericUpDown nud_min_contrast;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton tsbtn_Settings;
        private System.Windows.Forms.ToolStripButton tsbtn_Start;
        private System.Windows.Forms.ToolStripButton tsbtn_Stop;
        private System.Windows.Forms.ToolStripButton tsbtn_Help;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripButton tsbtn_RefreshCamera;
        private System.Windows.Forms.PictureBox camera2ImageBox;
        private System.Windows.Forms.PictureBox process2ImageBox;
    }
}

