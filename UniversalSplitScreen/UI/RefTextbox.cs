using System;
using System.Windows.Forms;
using UniversalSplitScreen.Core;

namespace UniversalSplitScreen.UI
{
	class RefTextbox : TextBox
	{
		private RefType<string> refType = null;
		public RefType<string> RefType
		{
			get => refType;
			set { refType = value; base.Text = refType ?? ""; }
		}

		protected override void OnTextChanged(EventArgs e)
		{
			if (refType != null)
			{
				base.OnTextChanged(e);

				refType.Set(base.Text);
			}
			else if (Program.SplitScreenManager != null)//Program is running, not in form editor
			{
				throw new NullReferenceException("RefType must be set. (See Form1 code)");
			}
		}
	}
}
