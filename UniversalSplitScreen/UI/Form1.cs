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

namespace UniversalSplitScreen.UI
{
	public partial class Form1 : Form
	{
		public bool ButtonPressed { get; private set; } = false;

		public string WindowTitleText { get => windowTitleLabel.Text; set => windowTitleLabel.Text = value; }
		public string WindowHandleText { get => hWndLabel.Text; set => hWndLabel.Text = value; }
		public string MouseHandleText { get => mouseHandleLabel.Text; set => mouseHandleLabel.Text = value; }
		public string KeyboardHandleText { get => keyboardHandleLabel.Text; set => keyboardHandleLabel.Text = value; }

		public ComboBox OptionsComboBox => optionsComboBox;

		public Form1()
		{
			InitializeComponent();

			startButton.Enabled = true;
			stopButton.Enabled = false;

			SetupOptionsPage();
		}

		public void SetupOptionsPage()
		{
			sendRawMouseCheckbox.Checked = Core.Options.CurrentOptions.SendRawMouseInput;
			sendRawKeyboardCheckbox.Checked = Core.Options.CurrentOptions.SendRawKeyboardInput;
			sendNormalMouseCheckbox.Checked = Core.Options.CurrentOptions.SendNormalMouseInput;
			sendNormalKeyboardCheckbox.Checked = Core.Options.CurrentOptions.SendNormalKeyboardInput;
			send_WM_ACTIVATE_checkbox.Checked = Core.Options.CurrentOptions.SendWM_ACTIVATE;
			send_WM_FOCUS_checkbox.Checked = Core.Options.CurrentOptions.SendWM_SETFOCUS;
			refreshWindowBoundsOnLMBCheckbox.Checked = Core.Options.CurrentOptions.RefreshWindowBoundsOnMouseClick;
			drawMouseCheckbox.Checked = Core.Options.CurrentOptions.DrawMouse;
			drawMouseEveryXmsField.Value = Core.Options.CurrentOptions.DrawMouseEveryXMilliseconds;
		}

		protected override void WndProc(ref Message msg)
		{
			Program.MessageProcessor?.WndProc(ref msg);

			base.WndProc(ref msg);
		}

		#region Main page events
		private void attachMouseButton_MouseDown(object sender, MouseEventArgs e) => ButtonPressed = true;

		private void attachMouseButton_MouseUp(object sender, MouseEventArgs e) => ButtonPressed = false;

		private void keyboardSetTextbox_KeyPress(object sender, KeyPressEventArgs e)
		{
			keyboardSetTextbox.Clear();
			Console.WriteLine($"Set keyboard, pointer = {Program.MessageProcessor.LastKeyboardPressed}");
			Program.SplitScreenManager.SetKeyboardPointer(Program.MessageProcessor.LastKeyboardPressed);
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
			Core.Options.CurrentOptions.SendRawMouseInput = sendRawMouseCheckbox.Checked;
		}

		private void sendRawKeyboardCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.SendRawKeyboardInput = sendRawKeyboardCheckbox.Checked;
		}

		private void sendNormalMouseCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.SendNormalMouseInput = sendNormalMouseCheckbox.Checked;
		}

		private void sendNormalKeyboardCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.SendNormalKeyboardInput = sendNormalKeyboardCheckbox.Checked;
		}

		private void send_WM_ACTIVATE_checkbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.SendWM_ACTIVATE = send_WM_ACTIVATE_checkbox.Checked;
		}

		private void send_WM_FOCUS_checkbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.SendWM_SETFOCUS = send_WM_FOCUS_checkbox.Checked;
		}

		private void refreshWindowBoundsOnLMBCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.RefreshWindowBoundsOnMouseClick = refreshWindowBoundsOnLMBCheckbox.Checked;
		}

		private void drawMouseEveryXmsField_ValueChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.DrawMouseEveryXMilliseconds = (int)drawMouseEveryXmsField.Value;
		}

		private void drawMouseCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.DrawMouse = drawMouseCheckbox.Checked;
		}

		private void checkBoxHook_filterWindowsRawInput_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.Hook_FilterRawInput = checkBoxHook_filterWindowsRawInput.Checked;
		}

		private void checkBoxHook_filterCallWndProc_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.Hook_FilterWindowsMouseInput = checkBoxHook_filterCallWndProc.Checked;
		}

		private void checkBoxHook_getForegroundWindow_CheckedChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.Hook_GetForegroundWindow = checkBoxHook_getForegroundWindow.Checked;
		}
		#endregion

		#region Public methods
		public void SetEndButtonText(string text)
		{
			endButtonSetter.Text = text;
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

		private void endButtonSetter_Click(object sender, EventArgs e)
		{
			if (!Program.SplitScreenManager.IsRunningInSplitScreen)
			{
				Program.MessageProcessor.WaitToSetEndKey();
			}
		}

		private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
		{
			Program.MessageProcessor.StopWaitingToSetEndKey();
		}

		private void buttonOptions_load_Click(object sender, EventArgs e)
		{
			Core.Options.LoadButtonClicked();
		}

		private void buttonOptions_save_Click(object sender, EventArgs e)
		{
			Core.Options.SaveButtonClicked();
		}

		private void button_optionsDelete_Click(object sender, EventArgs e)
		{
			Core.Options.DeleteButtonClicked();
		}

		private void optionsComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			bool b = optionsComboBox.SelectedItem?.ToString() != "Default";
			buttonOptions_delete.Enabled = b;
			buttonOptions_save.Enabled = b;
		}

		private void buttonOptions_New_Click(object sender, EventArgs e)
		{
			string name = Prompt.ShowDialog("Enter preset name");
			if (!string.IsNullOrWhiteSpace(name))
			{
				Core.Options.NewButtonClicked(name);
			}
		}
	}
}
