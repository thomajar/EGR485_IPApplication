namespace SAF_OpticalFailureDetector.threading
{
    partial class ZoomImageBox
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.DisplayPanel = new System.Windows.Forms.Panel();
            this.DisplayText = new System.Windows.Forms.Label();
            this.DisplayImageBox = new System.Windows.Forms.PictureBox();
            this.DisplayPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DisplayImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // DisplayPanel
            // 
            this.DisplayPanel.Controls.Add(this.DisplayText);
            this.DisplayPanel.Controls.Add(this.DisplayImageBox);
            this.DisplayPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayPanel.Location = new System.Drawing.Point(0, 0);
            this.DisplayPanel.Name = "DisplayPanel";
            this.DisplayPanel.Size = new System.Drawing.Size(581, 490);
            this.DisplayPanel.TabIndex = 0;
            // 
            // DisplayText
            // 
            this.DisplayText.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayText.Location = new System.Drawing.Point(0, 0);
            this.DisplayText.Name = "DisplayText";
            this.DisplayText.Size = new System.Drawing.Size(581, 490);
            this.DisplayText.TabIndex = 2;
            this.DisplayText.Text = "ZoomImageBoxText";
            this.DisplayText.MouseDown += new System.Windows.Forms.MouseEventHandler(this.DisplayImageBox_MouseDown);
            this.DisplayText.MouseEnter += new System.EventHandler(this.DisplayText_MouseEnter);
            this.DisplayText.MouseLeave += new System.EventHandler(this.DisplayImageBox_MouseLeave);
            this.DisplayText.MouseUp += new System.Windows.Forms.MouseEventHandler(this.DisplayImageBox_MouseUp);
            // 
            // DisplayImageBox
            // 
            this.DisplayImageBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DisplayImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DisplayImageBox.Location = new System.Drawing.Point(0, 0);
            this.DisplayImageBox.Name = "DisplayImageBox";
            this.DisplayImageBox.Size = new System.Drawing.Size(581, 490);
            this.DisplayImageBox.TabIndex = 1;
            this.DisplayImageBox.TabStop = false;
            // 
            // ZoomImageBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.DisplayPanel);
            this.Name = "ZoomImageBox";
            this.Size = new System.Drawing.Size(581, 490);
            this.DisplayPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DisplayImageBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel DisplayPanel;
        private System.Windows.Forms.PictureBox DisplayImageBox;
        private System.Windows.Forms.Label DisplayText;
    }
}
