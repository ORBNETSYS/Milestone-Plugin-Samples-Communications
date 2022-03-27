namespace Communications.Client
{
    partial class CommunicationsSidePanelUserControl
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
            this.Button_BroadcastSmartClientInfo = new System.Windows.Forms.Button();
            this.Button_BroadcastComplexData = new System.Windows.Forms.Button();
            this.Btn_PopupCamera = new System.Windows.Forms.Button();
            this.Button_RestartAllSmartClients = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // Button_BroadcastSmartClientInfo
            // 
            this.Button_BroadcastSmartClientInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_BroadcastSmartClientInfo.Location = new System.Drawing.Point(3, 3);
            this.Button_BroadcastSmartClientInfo.Name = "Button_BroadcastSmartClientInfo";
            this.Button_BroadcastSmartClientInfo.Size = new System.Drawing.Size(486, 101);
            this.Button_BroadcastSmartClientInfo.TabIndex = 0;
            this.Button_BroadcastSmartClientInfo.Text = "Broadcast Smart Client Info with Screenshot";
            this.Button_BroadcastSmartClientInfo.UseVisualStyleBackColor = true;
            this.Button_BroadcastSmartClientInfo.Click += new System.EventHandler(this.Button_BroadcastSmartClientInfo_Click);
            // 
            // Button_BroadcastComplexData
            // 
            this.Button_BroadcastComplexData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_BroadcastComplexData.Location = new System.Drawing.Point(3, 110);
            this.Button_BroadcastComplexData.Name = "Button_BroadcastComplexData";
            this.Button_BroadcastComplexData.Size = new System.Drawing.Size(486, 101);
            this.Button_BroadcastComplexData.TabIndex = 0;
            this.Button_BroadcastComplexData.Text = "Broadcast Complex Data";
            this.Button_BroadcastComplexData.UseVisualStyleBackColor = true;
            // 
            // Btn_PopupCamera
            // 
            this.Btn_PopupCamera.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Btn_PopupCamera.Location = new System.Drawing.Point(3, 217);
            this.Btn_PopupCamera.Name = "Btn_PopupCamera";
            this.Btn_PopupCamera.Size = new System.Drawing.Size(486, 101);
            this.Btn_PopupCamera.TabIndex = 0;
            this.Btn_PopupCamera.Text = "Popup Camera on all Smart Clients with Communications Plugin";
            this.Btn_PopupCamera.UseVisualStyleBackColor = true;
            // 
            // Button_RestartAllSmartClients
            // 
            this.Button_RestartAllSmartClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Button_RestartAllSmartClients.Location = new System.Drawing.Point(3, 324);
            this.Button_RestartAllSmartClients.Name = "Button_RestartAllSmartClients";
            this.Button_RestartAllSmartClients.Size = new System.Drawing.Size(486, 101);
            this.Button_RestartAllSmartClients.TabIndex = 0;
            this.Button_RestartAllSmartClients.Text = "Restart All Smart Clients";
            this.Button_RestartAllSmartClients.UseVisualStyleBackColor = true;
            // 
            // CommunicationsSidePanelUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.Button_RestartAllSmartClients);
            this.Controls.Add(this.Btn_PopupCamera);
            this.Controls.Add(this.Button_BroadcastComplexData);
            this.Controls.Add(this.Button_BroadcastSmartClientInfo);
            this.Name = "CommunicationsSidePanelUserControl";
            this.Size = new System.Drawing.Size(492, 611);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button Button_BroadcastSmartClientInfo;
        private System.Windows.Forms.Button Button_BroadcastComplexData;
        private System.Windows.Forms.Button Btn_PopupCamera;
        private System.Windows.Forms.Button Button_RestartAllSmartClients;
    }
}
