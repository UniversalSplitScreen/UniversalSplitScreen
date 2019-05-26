using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UniversalSplitScreen.UI
{
	class WebLinkLabel : LinkLabel
	{
		public virtual string Url { get; set; }

		public WebLinkLabel() : base ()
		{
			LinkBehavior = LinkBehavior.HoverUnderline;
		}
		

		protected override void OnMouseClick(MouseEventArgs e)
		{
			System.Diagnostics.Process.Start(Url);

			base.OnMouseClick(e);
		}
	}
}
