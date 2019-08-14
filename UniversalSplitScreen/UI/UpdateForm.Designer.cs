namespace UniversalSplitScreen.UI
{
	partial class UpdateForm
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
			this.Label_UpdateText = new System.Windows.Forms.Label();
			this.Button_OK = new System.Windows.Forms.Button();
			this.Button_DisableAutoUpdateCheck = new System.Windows.Forms.Button();
			this.WebLinkLabel_DownloadLink = new UniversalSplitScreen.UI.WebLinkLabel();
			this.SuspendLayout();
			// 
			// Label_UpdateText
			// 
			this.Label_UpdateText.AutoSize = true;
			this.Label_UpdateText.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Label_UpdateText.Location = new System.Drawing.Point(12, 9);
			this.Label_UpdateText.Name = "Label_UpdateText";
			this.Label_UpdateText.Size = new System.Drawing.Size(182, 16);
			this.Label_UpdateText.TabIndex = 1;
			this.Label_UpdateText.Text = "Found new version: XX.YY.ZZ";
			// 
			// Button_OK
			// 
			this.Button_OK.Location = new System.Drawing.Point(202, 61);
			this.Button_OK.Name = "Button_OK";
			this.Button_OK.Size = new System.Drawing.Size(75, 23);
			this.Button_OK.TabIndex = 2;
			this.Button_OK.Text = "OK";
			this.Button_OK.UseVisualStyleBackColor = true;
			this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
			// 
			// Button_DisableAutoUpdateCheck
			// 
			this.Button_DisableAutoUpdateCheck.Location = new System.Drawing.Point(11, 61);
			this.Button_DisableAutoUpdateCheck.Name = "Button_DisableAutoUpdateCheck";
			this.Button_DisableAutoUpdateCheck.Size = new System.Drawing.Size(185, 23);
			this.Button_DisableAutoUpdateCheck.TabIndex = 3;
			this.Button_DisableAutoUpdateCheck.Text = "Disable automatic update checking";
			this.Button_DisableAutoUpdateCheck.UseVisualStyleBackColor = true;
			this.Button_DisableAutoUpdateCheck.Click += new System.EventHandler(this.Button_DisableAutoUpdateCheck_Click);
			// 
			// WebLinkLabel_DownloadLink
			// 
			this.WebLinkLabel_DownloadLink.AutoSize = true;
			this.WebLinkLabel_DownloadLink.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.WebLinkLabel_DownloadLink.LinkArea = new System.Windows.Forms.LinkArea(28, 4);
			this.WebLinkLabel_DownloadLink.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
			this.WebLinkLabel_DownloadLink.Location = new System.Drawing.Point(12, 38);
			this.WebLinkLabel_DownloadLink.Name = "WebLinkLabel_DownloadLink";
			this.WebLinkLabel_DownloadLink.Size = new System.Drawing.Size(203, 20);
			this.WebLinkLabel_DownloadLink.TabIndex = 0;
			this.WebLinkLabel_DownloadLink.TabStop = true;
			this.WebLinkLabel_DownloadLink.Text = "Download the latest version here.";
			this.WebLinkLabel_DownloadLink.Url = "https://github.com/UniversalSplitScreen/UniversalSplitScreen/releases/latest";
			this.WebLinkLabel_DownloadLink.UseCompatibleTextRendering = true;
			// 
			// UpdateForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(289, 91);
			this.Controls.Add(this.Button_DisableAutoUpdateCheck);
			this.Controls.Add(this.Button_OK);
			this.Controls.Add(this.Label_UpdateText);
			this.Controls.Add(this.WebLinkLabel_DownloadLink);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "UpdateForm";
			this.Text = "Update checker";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private WebLinkLabel WebLinkLabel_DownloadLink;
		private System.Windows.Forms.Label Label_UpdateText;
		private System.Windows.Forms.Button Button_OK;
		private System.Windows.Forms.Button Button_DisableAutoUpdateCheck;
	}
}