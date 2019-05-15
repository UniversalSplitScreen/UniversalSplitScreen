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

		public static DialogResult ShowOkCancelDialog(string caption)
		{
			Form prompt = new Form()
			{
				Width = 300,
				Height = 105,
				FormBorderStyle = FormBorderStyle.FixedDialog,
				Text = caption,
				StartPosition = FormStartPosition.CenterScreen
			};

			Button confirmation = new Button() { Text = "OK", Left = 65, Top = 37, Width = 100, DialogResult = DialogResult.OK };
			confirmation.Click += (sender, e) => prompt.Close();
			prompt.Controls.Add(confirmation);

			Button cancel = new Button() { Text = "Cancel", Left = 175, Top = 37, Width = 100, DialogResult = DialogResult.Cancel };
			cancel.Click += (sender, e) => prompt.Close();
			prompt.Controls.Add(cancel);

			prompt.AcceptButton = confirmation;
			prompt.CancelButton = cancel;
			prompt.TopMost = true;
			
			return prompt.ShowDialog();
		}
	}
}
