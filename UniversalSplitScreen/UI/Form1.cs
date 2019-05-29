using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.Core;
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
		public int ControllerSelectedIndex { get => ControllerIndexComboBox.SelectedIndex; set => ControllerIndexComboBox.SelectedIndex = value; }

		public ComboBox OptionsComboBox => optionsComboBox;

		public Form1()
		{
			InitializeComponent();

			startButton.Enabled = true;
			stopButton.Enabled = false;

			ControllerIndexComboBox.SelectedIndex = 0;

			PopulateOptionsRefTypes(Options.CurrentOptions);
		}

		public void PopulateOptionsRefTypes(OptionsStructure options)
		{
			RefCheckbox_SendRawMouseInput.RefType				= new RefType<bool>("SendRawMouseInput");

			RefCheckbox_SendRawKeyboardInput.RefType			= new RefType<bool>("SendRawKeyboardInput");
			RefCheckbox_SendNormalMouseInput.RefType			= new RefType<bool>("SendNormalMouseInput");
			RefCheckbox_SendNormalKeyboardInput.RefType			= new RefType<bool>("SendNormalKeyboardInput");
			RefCheckbox_SendScrollwheel.RefType					= new RefType<bool>("SendScrollwheel");
			RefCheckbox_SendFakeWindowActivateMessages.RefType	= new RefType<bool>("SendWM_ACTIVATE");
			RefCheckbox_SendFakeWindowFocusMessages.RefType		= new RefType<bool>("SendWM_SETFOCUS");

			RefCheckbox_RefreshWindowBoundsOnMouseClick.RefType	= new RefType<bool>("RefreshWindowBoundsOnMouseClick");
			RefCheckbox_DrawMouse.RefType						= new RefType<bool>("DrawMouse");
			
			RefCheckbox_Hook_FilterRawInput.RefType				= new RefType<bool>("Hook_FilterRawInput");
			RefCheckbox_Hook_FilterMouseInputMessages.RefType	= new RefType<bool>("Hook_FilterWindowsMouseInput");
			RefCheckbox_Hook_GetForegroundWindow.RefType		= new RefType<bool>("Hook_GetForegroundWindow");
			RefCheckbox_Hook_GetCursorPos.RefType				= new RefType<bool>("Hook_GetCursorPos");
			RefCheckbox_Hook_SetCursorPos.RefType				= new RefType<bool>("Hook_SetCursorPos");
			RefCheckbox_Hook_GetAsyncKeyState.RefType			= new RefType<bool>("Hook_GetAsyncKeyState");
			RefCheckbox_Hook_GetKeyState.RefType				= new RefType<bool>("Hook_GetKeyState");
			RefCheckbox_Hook_XInput.RefType						= new RefType<bool>("Hook_XInput");
			RefCheckbox_Hook_UseLegacyInput.RefType				= new RefType<bool>("Hook_UseLegacyInput");

			drawMouseEveryXmsField.Value						= Options.CurrentOptions.DrawMouseEveryXMilliseconds;
		}
		

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == 0x00FF)//WM_INPUT
			{
				Program.MessageProcessor?.WndProc(ref msg);
			}
			else
			{
				base.WndProc(ref msg);
			}
		}

		#region Main page events
		private void attachMouseButton_MouseDown(object sender, MouseEventArgs e) => ButtonPressed = true;

		private void attachMouseButton_MouseUp(object sender, MouseEventArgs e) => ButtonPressed = false;

		private void keyboardSetTextbox_KeyPress(object sender, KeyPressEventArgs e)
		{
			keyboardSetTextbox.Clear();
			Logger.WriteLine($"Set keyboard, pointer = {Program.MessageProcessor.LastKeyboardPressed}");
			Program.SplitScreenManager.SetKeyboardHandle(Program.MessageProcessor.LastKeyboardPressed);
		}

		private void mouseResetButton_Click(object sender, EventArgs e)
		{
			Logger.WriteLine("Resetting mouse pointer");
			Program.SplitScreenManager.SetMouseHandle(new IntPtr(0));
		}

		private void keyboardResetButton_Click(object sender, EventArgs e)
		{
			Logger.WriteLine("Resetting keyboard pointer");
			Program.SplitScreenManager.SetKeyboardHandle(new IntPtr(0));
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

		private void ControllerIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
		{
			int index = ControllerIndexComboBox.SelectedIndex;
			Logger.WriteLine($"Set controller index = {index}");
			Program.SplitScreenManager?.SetControllerIndex(index);
		}
		#endregion

		#region Option page events
		private void drawMouseEveryXmsField_ValueChanged(object sender, EventArgs e)
		{
			Core.Options.CurrentOptions.DrawMouseEveryXMilliseconds = (int)drawMouseEveryXmsField.Value;
		}
		
		public void SetEndButtonText(string text)
		{
			endButtonSetter.Text = text;
		}
		
		public void OnSplitScreenStart()
		{
			startButton.Enabled = false;
			stopButton.Enabled = true;
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
		#endregion

		public void OnSplitScreenEnd()
		{
			startButton.Enabled = true;
			stopButton.Enabled = false;
		}
		
		private void RefCheckbox_Hook_GetAsyncKeyState_CheckedChanged(object sender, EventArgs e)
		{

		}

		private void Button_UnlockSourceEngine_Click(object sender, EventArgs e)
		{
			Program.SplitScreenManager.UnlockSourceEngine();
		}
	}
}
