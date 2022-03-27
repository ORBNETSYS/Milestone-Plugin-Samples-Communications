namespace Communications.Client
{
    partial class PopupSmartClientInfo
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
            this.PictureBoxScreenCap = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBox_SmartClientInfo = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxScreenCap)).BeginInit();
            this.SuspendLayout();
            // 
            // PictureBoxScreenCap
            // 
            this.PictureBoxScreenCap.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PictureBoxScreenCap.Location = new System.Drawing.Point(420, 65);
            this.PictureBoxScreenCap.Name = "PictureBoxScreenCap";
            this.PictureBoxScreenCap.Size = new System.Drawing.Size(787, 550);
            this.PictureBoxScreenCap.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.PictureBoxScreenCap.TabIndex = 0;
            this.PictureBoxScreenCap.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(262, 36);
            this.label1.TabIndex = 1;
            this.label1.Text = "Smart Client Info:";
            // 
            // TextBox_SmartClientInfo
            // 
            this.TextBox_SmartClientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.TextBox_SmartClientInfo.Location = new System.Drawing.Point(18, 65);
            this.TextBox_SmartClientInfo.Multiline = true;
            this.TextBox_SmartClientInfo.Name = "TextBox_SmartClientInfo";
            this.TextBox_SmartClientInfo.Size = new System.Drawing.Size(396, 550);
            this.TextBox_SmartClientInfo.TabIndex = 2;
            // 
            // PopupSmartClientInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 636);
            this.Controls.Add(this.TextBox_SmartClientInfo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.PictureBoxScreenCap);
            this.Name = "PopupSmartClientInfo";
            this.Text = "PopupSmartClientInfo";
            ((System.ComponentModel.ISupportInitialize)(this.PictureBoxScreenCap)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox PictureBoxScreenCap;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBox_SmartClientInfo;
    }
}