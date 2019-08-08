using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalSplitScreen.UI
{
	public partial class UpdateForm : Form
	{
		public UpdateForm(string text, bool isThereAnUpdate)
		{
			InitializeComponent();

			TopMost = true;
			WebLinkLabel_DownloadLink.Visible = isThereAnUpdate;
			Label_UpdateText.Text = text;
		}

		private void Button_DisableAutoUpdateCheck_Click(object sender, EventArgs e)
		{
			var cfg = Program.Config;
			if (cfg != null)
			{
				cfg.AutomaticallyCheckForUpdatesOnStartup = false;
				cfg.SaveConfig();
			}

			Program.Form.SetAutomaticallyCheckUpdatesChecked(false);
			Close();
		}

		private void Button_OK_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}
