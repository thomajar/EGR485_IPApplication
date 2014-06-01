namespace SAF_OpticalFailureDetector
{
    partial class Settings
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
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.gbTestSettings = new System.Windows.Forms.GroupBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtSaveLocation = new System.Windows.Forms.TextBox();
            this.txtTestNumber = new System.Windows.Forms.TextBox();
            this.txtSampleNumber = new System.Windows.Forms.TextBox();
            this.cbEnableDebugSaving = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbRelayController = new System.Windows.Forms.GroupBox();
            this.btnOpenRelay = new System.Windows.Forms.Button();
            this.btnTestRelayOff = new System.Windows.Forms.Button();
            this.btnTestRelayOn = new System.Windows.Forms.Button();
            this.nudPortNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbImageProcessorSettings = new System.Windows.Forms.GroupBox();
            this.nudMinLineLength = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudTargetIntensity = new System.Windows.Forms.NumericUpDown();
            this.nudMinContrast = new System.Windows.Forms.NumericUpDown();
            this.nudImagerNoise = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.gbTestSettings.SuspendLayout();
            this.gbRelayController.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPortNum)).BeginInit();
            this.gbImageProcessorSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinLineLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetIntensity)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinContrast)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImagerNoise)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33334F));
            this.tlpMain.Controls.Add(this.gbTestSettings, 0, 0);
            this.tlpMain.Controls.Add(this.gbRelayController, 0, 1);
            this.tlpMain.Controls.Add(this.gbImageProcessorSettings, 0, 2);
            this.tlpMain.Controls.Add(this.btnSave, 1, 3);
            this.tlpMain.Controls.Add(this.btnClose, 2, 3);
            this.tlpMain.Controls.Add(this.btnClear, 0, 3);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 31.00006F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.00005F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.99981F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.00007F));
            this.tlpMain.Size = new System.Drawing.Size(456, 528);
            this.tlpMain.TabIndex = 1;
            // 
            // gbTestSettings
            // 
            this.tlpMain.SetColumnSpan(this.gbTestSettings, 3);
            this.gbTestSettings.Controls.Add(this.btnBrowse);
            this.gbTestSettings.Controls.Add(this.txtSaveLocation);
            this.gbTestSettings.Controls.Add(this.txtTestNumber);
            this.gbTestSettings.Controls.Add(this.txtSampleNumber);
            this.gbTestSettings.Controls.Add(this.cbEnableDebugSaving);
            this.gbTestSettings.Controls.Add(this.label6);
            this.gbTestSettings.Controls.Add(this.label5);
            this.gbTestSettings.Controls.Add(this.label4);
            this.gbTestSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTestSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbTestSettings.Location = new System.Drawing.Point(3, 3);
            this.gbTestSettings.Name = "gbTestSettings";
            this.gbTestSettings.Size = new System.Drawing.Size(450, 157);
            this.gbTestSettings.TabIndex = 0;
            this.gbTestSettings.TabStop = false;
            this.gbTestSettings.Text = "Test Settings";
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.Location = new System.Drawing.Point(345, 85);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(96, 32);
            this.btnBrowse.TabIndex = 5;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtSaveLocation
            // 
            this.txtSaveLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSaveLocation.Location = new System.Drawing.Point(161, 89);
            this.txtSaveLocation.Name = "txtSaveLocation";
            this.txtSaveLocation.Size = new System.Drawing.Size(175, 26);
            this.txtSaveLocation.TabIndex = 4;
            // 
            // txtTestNumber
            // 
            this.txtTestNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTestNumber.Location = new System.Drawing.Point(161, 57);
            this.txtTestNumber.Name = "txtTestNumber";
            this.txtTestNumber.Size = new System.Drawing.Size(175, 26);
            this.txtTestNumber.TabIndex = 3;
            // 
            // txtSampleNumber
            // 
            this.txtSampleNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSampleNumber.Location = new System.Drawing.Point(161, 23);
            this.txtSampleNumber.Name = "txtSampleNumber";
            this.txtSampleNumber.Size = new System.Drawing.Size(175, 26);
            this.txtSampleNumber.TabIndex = 2;
            // 
            // cbEnableDebugSaving
            // 
            this.cbEnableDebugSaving.AutoSize = true;
            this.cbEnableDebugSaving.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbEnableDebugSaving.Location = new System.Drawing.Point(161, 121);
            this.cbEnableDebugSaving.Name = "cbEnableDebugSaving";
            this.cbEnableDebugSaving.Size = new System.Drawing.Size(191, 24);
            this.cbEnableDebugSaving.TabIndex = 1;
            this.cbEnableDebugSaving.Text = "Enable Debug Saving";
            this.cbEnableDebugSaving.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(10, 93);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(146, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Save Location";
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(10, 59);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(146, 22);
            this.label5.TabIndex = 0;
            this.label5.Text = "Test Number";
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(146, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Sample Number";
            // 
            // gbRelayController
            // 
            this.tlpMain.SetColumnSpan(this.gbRelayController, 3);
            this.gbRelayController.Controls.Add(this.btnOpenRelay);
            this.gbRelayController.Controls.Add(this.btnTestRelayOff);
            this.gbRelayController.Controls.Add(this.btnTestRelayOn);
            this.gbRelayController.Controls.Add(this.nudPortNum);
            this.gbRelayController.Controls.Add(this.label1);
            this.gbRelayController.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbRelayController.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbRelayController.Location = new System.Drawing.Point(3, 166);
            this.gbRelayController.Name = "gbRelayController";
            this.gbRelayController.Size = new System.Drawing.Size(450, 120);
            this.gbRelayController.TabIndex = 1;
            this.gbRelayController.TabStop = false;
            this.gbRelayController.Text = "Relay Controller";
            // 
            // btnOpenRelay
            // 
            this.btnOpenRelay.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenRelay.Location = new System.Drawing.Point(306, 25);
            this.btnOpenRelay.Name = "btnOpenRelay";
            this.btnOpenRelay.Size = new System.Drawing.Size(96, 32);
            this.btnOpenRelay.TabIndex = 4;
            this.btnOpenRelay.Text = "Open";
            this.btnOpenRelay.UseVisualStyleBackColor = true;
            this.btnOpenRelay.Click += new System.EventHandler(this.btnOpenRelay_Click);
            // 
            // btnTestRelayOff
            // 
            this.btnTestRelayOff.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestRelayOff.Location = new System.Drawing.Point(224, 69);
            this.btnTestRelayOff.Name = "btnTestRelayOff";
            this.btnTestRelayOff.Size = new System.Drawing.Size(96, 32);
            this.btnTestRelayOff.TabIndex = 3;
            this.btnTestRelayOff.Text = "Test Off";
            this.btnTestRelayOff.UseVisualStyleBackColor = true;
            this.btnTestRelayOff.Click += new System.EventHandler(this.btnTestRelayOff_Click);
            // 
            // btnTestRelayOn
            // 
            this.btnTestRelayOn.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTestRelayOn.Location = new System.Drawing.Point(86, 69);
            this.btnTestRelayOn.Name = "btnTestRelayOn";
            this.btnTestRelayOn.Size = new System.Drawing.Size(96, 32);
            this.btnTestRelayOn.TabIndex = 2;
            this.btnTestRelayOn.Text = "Test On";
            this.btnTestRelayOn.UseVisualStyleBackColor = true;
            this.btnTestRelayOn.Click += new System.EventHandler(this.btnTestRelayOn_Click);
            // 
            // nudPortNum
            // 
            this.nudPortNum.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudPortNum.Location = new System.Drawing.Point(157, 26);
            this.nudPortNum.Name = "nudPortNum";
            this.nudPortNum.Size = new System.Drawing.Size(100, 26);
            this.nudPortNum.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(10, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port Number";
            // 
            // gbImageProcessorSettings
            // 
            this.tlpMain.SetColumnSpan(this.gbImageProcessorSettings, 3);
            this.gbImageProcessorSettings.Controls.Add(this.nudMinLineLength);
            this.gbImageProcessorSettings.Controls.Add(this.label8);
            this.gbImageProcessorSettings.Controls.Add(this.nudTargetIntensity);
            this.gbImageProcessorSettings.Controls.Add(this.nudMinContrast);
            this.gbImageProcessorSettings.Controls.Add(this.nudImagerNoise);
            this.gbImageProcessorSettings.Controls.Add(this.label7);
            this.gbImageProcessorSettings.Controls.Add(this.label3);
            this.gbImageProcessorSettings.Controls.Add(this.label2);
            this.gbImageProcessorSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbImageProcessorSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbImageProcessorSettings.Location = new System.Drawing.Point(3, 292);
            this.gbImageProcessorSettings.Name = "gbImageProcessorSettings";
            this.gbImageProcessorSettings.Size = new System.Drawing.Size(450, 178);
            this.gbImageProcessorSettings.TabIndex = 2;
            this.gbImageProcessorSettings.TabStop = false;
            this.gbImageProcessorSettings.Text = "Image Processing Settings";
            // 
            // nudMinLineLength
            // 
            this.nudMinLineLength.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinLineLength.Location = new System.Drawing.Point(193, 121);
            this.nudMinLineLength.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudMinLineLength.Name = "nudMinLineLength";
            this.nudMinLineLength.Size = new System.Drawing.Size(100, 26);
            this.nudMinLineLength.TabIndex = 7;
            this.nudMinLineLength.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(10, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(114, 20);
            this.label8.TabIndex = 6;
            this.label8.Text = "Minimum Line";
            this.label8.Visible = false;
            // 
            // nudTargetIntensity
            // 
            this.nudTargetIntensity.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTargetIntensity.Location = new System.Drawing.Point(193, 90);
            this.nudTargetIntensity.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudTargetIntensity.Name = "nudTargetIntensity";
            this.nudTargetIntensity.Size = new System.Drawing.Size(100, 26);
            this.nudTargetIntensity.TabIndex = 5;
            // 
            // nudMinContrast
            // 
            this.nudMinContrast.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinContrast.Location = new System.Drawing.Point(193, 59);
            this.nudMinContrast.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudMinContrast.Name = "nudMinContrast";
            this.nudMinContrast.Size = new System.Drawing.Size(100, 26);
            this.nudMinContrast.TabIndex = 4;
            // 
            // nudImagerNoise
            // 
            this.nudImagerNoise.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudImagerNoise.Location = new System.Drawing.Point(193, 28);
            this.nudImagerNoise.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudImagerNoise.Name = "nudImagerNoise";
            this.nudImagerNoise.Size = new System.Drawing.Size(100, 26);
            this.nudImagerNoise.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(10, 93);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(124, 20);
            this.label7.TabIndex = 2;
            this.label7.Text = "Target Intensity";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(146, 20);
            this.label3.TabIndex = 1;
            this.label3.Text = "Minimum Contrast";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "Imager Noise";
            // 
            // btnSave
            // 
            this.btnSave.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(154, 476);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(146, 49);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(306, 476);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(147, 49);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnClear
            // 
            this.btnClear.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(3, 476);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(145, 49);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(456, 528);
            this.Controls.Add(this.tlpMain);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "Settings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.Settings_Load);
            this.tlpMain.ResumeLayout(false);
            this.gbTestSettings.ResumeLayout(false);
            this.gbTestSettings.PerformLayout();
            this.gbRelayController.ResumeLayout(false);
            this.gbRelayController.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPortNum)).EndInit();
            this.gbImageProcessorSettings.ResumeLayout(false);
            this.gbImageProcessorSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinLineLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTargetIntensity)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinContrast)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudImagerNoise)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbTestSettings;
        private System.Windows.Forms.CheckBox cbEnableDebugSaving;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbRelayController;
        private System.Windows.Forms.Button btnTestRelayOff;
        private System.Windows.Forms.Button btnTestRelayOn;
        private System.Windows.Forms.NumericUpDown nudPortNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbImageProcessorSettings;
        private System.Windows.Forms.NumericUpDown nudTargetIntensity;
        private System.Windows.Forms.NumericUpDown nudMinContrast;
        private System.Windows.Forms.NumericUpDown nudImagerNoise;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnOpenRelay;
        private System.Windows.Forms.NumericUpDown nudMinLineLength;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtSaveLocation;
        private System.Windows.Forms.TextBox txtTestNumber;
        private System.Windows.Forms.TextBox txtSampleNumber;
        private System.Windows.Forms.Button btnBrowse;

    }
}