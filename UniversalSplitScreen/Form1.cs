using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.RawInput;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen
{
	public partial class Form1 : Form
	{
		public bool ButtonPressed { get; private set; } = false;

		public string WindowTitleText { get => windowTitleLabel.Text; set => windowTitleLabel.Text = value; }
		public string WindowHandleText { get => hWndLabel.Text; set => hWndLabel.Text = value; }
		public string MouseHandleText { get => mouseHandleLabel.Text; set => mouseHandleLabel.Text = value; }
		public string KeyboardHandleText { get => keyboardHandleLabel.Text; set => keyboardHandleLabel.Text = value; }

		public Form1()
		{
			InitializeComponent();

			startButton.Enabled = true;
			stopButton.Enabled = false;

			SetupOptionsPage();
		}

		private void SetupOptionsPage()
		{
			sendRawMouseCheckbox.Checked = Core.Options.SendRawMouseInput;
			sendRawKeyboardCheckbox.Checked = Core.Options.SendRawKeyboardInput;
			sendNormalMouseCheckbox.Checked = Core.Options.SendNormalMouseInput;
			sendNormalKeyboardCheckbox.Checked = Core.Options.SendNormalKeyboardInput;
			send_WM_ACTIVATE_checkbox.Checked = Core.Options.SendWM_ACTIVATE;
			send_WM_FOCUS_checkbox.Checked = Core.Options.SendWM_SETFOCUS;
			refreshWindowBoundsOnLMBCheckbox.Checked = Core.Options.RefreshWindowBoundsOnMouseClick;
			drawMouseCheckbox.Checked = Core.Options.DrawMouse;
			drawMouseEveryXmsField.Value = Core.Options.DrawMouseEveryXMilliseconds;
		}

		protected override void WndProc(ref Message msg)
		{
			MessageProcessor.WndProc(ref msg);

			base.WndProc(ref msg);
		}

		#region Main page events
		private void attachMouseButton_MouseDown(object sender, MouseEventArgs e) => ButtonPressed = true;

		private void attachMouseButton_MouseUp(object sender, MouseEventArgs e) => ButtonPressed = false;

		private void keyboardSetTextbox_KeyPress(object sender, KeyPressEventArgs e)
		{
			keyboardSetTextbox.Clear();
			Console.WriteLine($"Set keyboard, pointer = {MessageProcessor.LastKeyboardPressed}");
			Program.SplitScreenManager.SetKeyboardPointer(MessageProcessor.LastKeyboardPressed);
		}

		private void mouseResetButton_Click(object sender, EventArgs e)
		{
			Console.WriteLine("Resetting mouse pointer");
			Program.SplitScreenManager.SetMousePointer(new IntPtr(0));
		}

		private void keyboardResetButton_Click(object sender, EventArgs e)
		{
			Console.WriteLine("Resetting keyboard pointer");
			Program.SplitScreenManager.SetKeyboardPointer(new IntPtr(0));
		}

		private void resetAllButton_Click(object sender, EventArgs e)
		{
			Program.SplitScreenManager.ResetAllHandles();
		}

		private void startButton_Click(object sender, EventArgs e)
		{
			Program.SplitScreenManager.ActivateSplitScreen();
		}

		private void stopButton_Click(object sender, EventArgs e)
		{
			Program.SplitScreenManager.DeactivateSplitScreen();
		}
		#endregion

		#region Options page value change events
		private void sendRawMouseCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.SendRawMouseInput = sendRawMouseCheckbox.Checked;
		}

		private void sendRawKeyboardCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.SendRawKeyboardInput = sendRawKeyboardCheckbox.Checked;
		}

		private void sendNormalMouseCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.SendNormalMouseInput = sendNormalMouseCheckbox.Checked;
		}

		private void sendNormalKeyboardCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.SendNormalKeyboardInput = sendNormalKeyboardCheckbox.Checked;
		}

		private void send_WM_ACTIVATE_checkbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.SendWM_ACTIVATE = send_WM_ACTIVATE_checkbox.Checked;
		}

		private void send_WM_FOCUS_checkbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.SendWM_SETFOCUS = send_WM_FOCUS_checkbox.Checked;
		}

		private void refreshWindowBoundsOnLMBCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.RefreshWindowBoundsOnMouseClick = refreshWindowBoundsOnLMBCheckbox.Checked;
		}

		private void drawMouseEveryXmsField_ValueChanged(object sender, EventArgs e)
		{
			Core.Options.DrawMouseEveryXMilliseconds = (int)drawMouseEveryXmsField.Value;
		}

		private void drawMouseCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.DrawMouse = drawMouseCheckbox.Checked;
		}
		#endregion

		//TODO: when ss starts, disable the start button (and vice cersa

		public void OnSplitScreenStart()
		{
			startButton.Enabled = false;
			stopButton.Enabled = true;
		}

		public void OnSplitScreenEnd()
		{
			startButton.Enabled = true;
			stopButton.Enabled = false;
		}
	}
}
