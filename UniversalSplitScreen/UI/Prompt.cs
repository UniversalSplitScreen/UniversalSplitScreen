using System.Windows.Forms;

namespace UniversalSplitScreen.UI
{
	static class Prompt
	{
		public static string ShowDialog(string caption)
		{
			Form prompt = new Form()
			{
				Width = 300,
				Height = 105,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};

			TextBox textBox = new TextBox() { Left = 10, Top = 10, Width = 265 };
			textBox.MaxLength = 50;
			prompt.Controls.Add(textBox);

			Button confirmation = new Button() { Text = "OK", Left = 175, Top = 37, Width = 100, DialogResult = DialogResult.OK };
			confirmation.Click += (sender, e) => prompt.Close();
			prompt.Controls.Add(confirmation);
			
			prompt.AcceptButton = confirmation;
			prompt.TopMost = true;

			return prompt.ShowDialog() == DialogResult.OK ? textBox.Text : "";
		}
	}
}
