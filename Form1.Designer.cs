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
            this.panel_cntls = new System.Windows.Forms.Panel();
            this.nud_min_contrast = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nud_noise_lvl = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btn_LoadImage = new System.Windows.Forms.Button();
            this.Camera1Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera1Process = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera2Display = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.Camera2Process = new SAF_OpticalFailureDetector.threading.ZoomImageBox();
            this.toolStrip1.SuspendLayout();
            this.tlp_Main.SuspendLayout();
            this.panel_cntls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_min_contrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_noise_lvl)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(1363, 79);
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
            this.tlp_Main.ColumnCount = 3;
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlp_Main.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 143F));
            this.tlp_Main.Controls.Add(this.panel_cntls, 2, 0);
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
            this.tlp_Main.Size = new System.Drawing.Size(1363, 663);
            this.tlp_Main.TabIndex = 2;
            // 
            // panel_cntls
            // 
            this.panel_cntls.Controls.Add(this.nud_min_contrast);
            this.panel_cntls.Controls.Add(this.label2);
            this.panel_cntls.Controls.Add(this.nud_noise_lvl);
            this.panel_cntls.Controls.Add(this.label1);
            this.panel_cntls.Controls.Add(this.btn_LoadImage);
            this.panel_cntls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel_cntls.Location = new System.Drawing.Point(1223, 2);
            this.panel_cntls.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel_cntls.Name = "panel_cntls";
            this.tlp_Main.SetRowSpan(this.panel_cntls, 2);
            this.panel_cntls.Size = new System.Drawing.Size(137, 659);
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
            this.nud_min_contrast.Size = new System.Drawing.Size(123, 22);
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
            this.label2.Location = new System.Drawing.Point(5, 159);
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
            this.nud_noise_lvl.Size = new System.Drawing.Size(123, 22);
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
            this.label1.Location = new System.Drawing.Point(5, 86);
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
            // Camera1Display
            // 
            this.Camera1Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera1Display.Location = new System.Drawing.Point(3, 3);
            this.Camera1Display.Name = "Camera1Display";
            this.Camera1Display.Size = new System.Drawing.Size(604, 325);
            this.Camera1Display.TabIndex = 9;
            // 
            // Camera1Process
            // 
            this.Camera1Process.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera1Process.Location = new System.Drawing.Point(3, 334);
            this.Camera1Process.Name = "Camera1Process";
            this.Camera1Process.Size = new System.Drawing.Size(604, 326);
            this.Camera1Process.TabIndex = 10;
            // 
            // Camera2Display
            // 
            this.Camera2Display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera2Display.Location = new System.Drawing.Point(613, 3);
            this.Camera2Display.Name = "Camera2Display";
            this.Camera2Display.Size = new System.Drawing.Size(604, 325);
            this.Camera2Display.TabIndex = 11;
            // 
            // Camera2Process
            // 
            this.Camera2Process.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Camera2Process.Location = new System.Drawing.Point(613, 334);
            this.Camera2Process.Name = "Camera2Process";
            this.Camera2Process.Size = new System.Drawing.Size(604, 326);
            this.Camera2Process.TabIndex = 12;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1363, 742);
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
            this.panel_cntls.ResumeLayout(false);
            this.panel_cntls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nud_min_contrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nud_noise_lvl)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TableLayoutPanel tlp_Main;
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
        private System.Windows.Forms.ToolStripButton tsbtn_RefreshCamera;
        private threading.ZoomImageBox Camera1Display;
        private threading.ZoomImageBox Camera1Process;
        private threading.ZoomImageBox Camera2Display;
        private threading.ZoomImageBox Camera2Process;
    }
}

