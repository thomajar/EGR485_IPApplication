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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.gbRelayController = new System.Windows.Forms.GroupBox();
            this.btnTestRelayOff = new System.Windows.Forms.Button();
            this.btnTestRelayOn = new System.Windows.Forms.Button();
            this.nudPortNum = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.gbImageProcessorSettings = new System.Windows.Forms.GroupBox();
            this.numericUpDown3 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown2 = new System.Windows.Forms.NumericUpDown();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnOpenRelay = new System.Windows.Forms.Button();
            this.tlpMain.SuspendLayout();
            this.gbTestSettings.SuspendLayout();
            this.gbRelayController.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPortNum)).BeginInit();
            this.gbImageProcessorSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.ColumnCount = 3;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tlpMain.Controls.Add(this.gbTestSettings, 0, 0);
            this.tlpMain.Controls.Add(this.gbRelayController, 0, 1);
            this.tlpMain.Controls.Add(this.gbImageProcessorSettings, 0, 2);
            this.tlpMain.Controls.Add(this.btnSave, 1, 3);
            this.tlpMain.Controls.Add(this.btnClose, 2, 3);
            this.tlpMain.Controls.Add(this.btnClear, 0, 3);
            this.tlpMain.Location = new System.Drawing.Point(12, 1);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 4;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00062F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25.00063F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 24.99813F));
            this.tlpMain.Size = new System.Drawing.Size(419, 514);
            this.tlpMain.TabIndex = 1;
            // 
            // gbTestSettings
            // 
            this.tlpMain.SetColumnSpan(this.gbTestSettings, 3);
            this.gbTestSettings.Controls.Add(this.checkBox1);
            this.gbTestSettings.Controls.Add(this.label6);
            this.gbTestSettings.Controls.Add(this.label5);
            this.gbTestSettings.Controls.Add(this.label4);
            this.gbTestSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbTestSettings.Location = new System.Drawing.Point(3, 3);
            this.gbTestSettings.Name = "gbTestSettings";
            this.gbTestSettings.Size = new System.Drawing.Size(413, 122);
            this.gbTestSettings.TabIndex = 0;
            this.gbTestSettings.TabStop = false;
            this.gbTestSettings.Text = "Test Settings";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(12, 95);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(167, 21);
            this.checkBox1.TabIndex = 1;
            this.checkBox1.Text = "Enable Debug Saving";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(9, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "Save Location";
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(9, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "Test Number";
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(9, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 17);
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
            this.gbRelayController.Location = new System.Drawing.Point(3, 131);
            this.gbRelayController.Name = "gbRelayController";
            this.gbRelayController.Size = new System.Drawing.Size(413, 122);
            this.gbRelayController.TabIndex = 1;
            this.gbRelayController.TabStop = false;
            this.gbRelayController.Text = "Relay Controller";
            // 
            // btnTestRelayOff
            // 
            this.btnTestRelayOff.Location = new System.Drawing.Point(118, 64);
            this.btnTestRelayOff.Name = "btnTestRelayOff";
            this.btnTestRelayOff.Size = new System.Drawing.Size(75, 23);
            this.btnTestRelayOff.TabIndex = 3;
            this.btnTestRelayOff.Text = "Test Off";
            this.btnTestRelayOff.UseVisualStyleBackColor = true;
            this.btnTestRelayOff.Click += new System.EventHandler(this.btnTestRelayOff_Click);
            // 
            // btnTestRelayOn
            // 
            this.btnTestRelayOn.Location = new System.Drawing.Point(27, 64);
            this.btnTestRelayOn.Name = "btnTestRelayOn";
            this.btnTestRelayOn.Size = new System.Drawing.Size(75, 23);
            this.btnTestRelayOn.TabIndex = 2;
            this.btnTestRelayOn.Text = "Test On";
            this.btnTestRelayOn.UseVisualStyleBackColor = true;
            this.btnTestRelayOn.Click += new System.EventHandler(this.btnTestRelayOn_Click);
            // 
            // nudPortNum
            // 
            this.nudPortNum.Location = new System.Drawing.Point(157, 30);
            this.nudPortNum.Name = "nudPortNum";
            this.nudPortNum.Size = new System.Drawing.Size(120, 22);
            this.nudPortNum.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Port Number";
            // 
            // gbImageProcessorSettings
            // 
            this.tlpMain.SetColumnSpan(this.gbImageProcessorSettings, 3);
            this.gbImageProcessorSettings.Controls.Add(this.numericUpDown3);
            this.gbImageProcessorSettings.Controls.Add(this.numericUpDown2);
            this.gbImageProcessorSettings.Controls.Add(this.numericUpDown1);
            this.gbImageProcessorSettings.Controls.Add(this.label7);
            this.gbImageProcessorSettings.Controls.Add(this.label3);
            this.gbImageProcessorSettings.Controls.Add(this.label2);
            this.gbImageProcessorSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbImageProcessorSettings.Location = new System.Drawing.Point(3, 259);
            this.gbImageProcessorSettings.Name = "gbImageProcessorSettings";
            this.gbImageProcessorSettings.Size = new System.Drawing.Size(413, 122);
            this.gbImageProcessorSettings.TabIndex = 2;
            this.gbImageProcessorSettings.TabStop = false;
            this.gbImageProcessorSettings.Text = "Image Processing Settings";
            // 
            // numericUpDown3
            // 
            this.numericUpDown3.Location = new System.Drawing.Point(157, 87);
            this.numericUpDown3.Name = "numericUpDown3";
            this.numericUpDown3.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown3.TabIndex = 5;
            // 
            // numericUpDown2
            // 
            this.numericUpDown2.Location = new System.Drawing.Point(157, 59);
            this.numericUpDown2.Name = "numericUpDown2";
            this.numericUpDown2.Size = new System.Drawing.Size(120, 22);
            this.numericUpDown2.TabIndex = 4;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(157, 29);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(122, 22);
            this.numericUpDown1.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(9, 87);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(94, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Minimum Line";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 59);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Minimum Contrast";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Imager Noise";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(142, 387);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(281, 387);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 5;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(3, 387);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(75, 23);
            this.btnClear.TabIndex = 3;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // btnOpenRelay
            // 
            this.btnOpenRelay.Location = new System.Drawing.Point(305, 28);
            this.btnOpenRelay.Name = "btnOpenRelay";
            this.btnOpenRelay.Size = new System.Drawing.Size(75, 23);
            this.btnOpenRelay.TabIndex = 4;
            this.btnOpenRelay.Text = "Open";
            this.btnOpenRelay.UseVisualStyleBackColor = true;
            this.btnOpenRelay.Click += new System.EventHandler(this.btnOpenRelay_Click);
            // 
            // Settings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 528);
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
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.GroupBox gbTestSettings;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox gbRelayController;
        private System.Windows.Forms.Button btnTestRelayOff;
        private System.Windows.Forms.Button btnTestRelayOn;
        private System.Windows.Forms.NumericUpDown nudPortNum;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox gbImageProcessorSettings;
        private System.Windows.Forms.NumericUpDown numericUpDown3;
        private System.Windows.Forms.NumericUpDown numericUpDown2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnOpenRelay;

    }
}