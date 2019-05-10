using System;
using System.Windows.Forms;
using UniversalSplitScreen.Core;

namespace UniversalSplitScreen.UI
{
	class RefCheckbox : CheckBox
	{
		private RefType<bool> refType = null;
		public RefType<bool> RefType {
			get => refType;
			set { refType = value; base.Checked = refType ?? false; }
		}
		
		protected override void OnCheckedChanged(EventArgs e)
		{
			if (refType != null)
			{
				base.OnCheckedChanged(e);

				Console.WriteLine($"RefCheckbox checked changed, old={refType}, new={base.Checked}");
				refType.Set(base.Checked);
			}
			else
			{
				throw new NullReferenceException("RefType must be set. (See Form1 code)");
			}
		}
	}
}
