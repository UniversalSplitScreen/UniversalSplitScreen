using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using UniversalSplitScreen.Core;

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

		private string goldbergAccountNamePath;
		private string goldbergSteamIDPath;

		public Form1()
		{
			InitializeComponent();

			startButton.Enabled = true;
			stopButton.Enabled = false;

			ControllerIndexComboBox.SelectedIndex = 0;

			PopulateOptionsRefTypes(Options.CurrentOptions);

			Label_CurrentVersion.Text = $"Current version: {UpdateChecker.currentVersion}";

			LoadGoldbergData();
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
			RefCheckbox_Hook_GetKeyboardState.RefType			= new RefType<bool>("Hook_GetKeyboardState");
			RefCheckbox_Hook_XInput.RefType						= new RefType<bool>("Hook_XInput");
			RefCheckbox_Hook_Dinput.RefType						= new RefType<bool>("Hook_DInput");
			RefCheckbox_Hook_UseLegacyInput.RefType				= new RefType<bool>("Hook_UseLegacyInput");
			RefCheckbox_UpdateAbsoluteFlagInMouseMessage.RefType= new RefType<bool>("UpdateAbsoluteFlagInMouseMessage");
			RefCheckbox_Hook_MouseVisibility.RefType			= new RefType<bool>("Hook_MouseVisibility");

			var handleNameRef = new RefType<string>("AutofillHandleName");
			RefTextbox_AutofillHandleName.RefType = handleNameRef;
			if (!string.IsNullOrEmpty(handleNameRef))
			{
				textBoxHandleName.Text = handleNameRef;
				StartupHook_MutexTargets.Text = handleNameRef;
			}
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
			Program.SplitScreenManager.UnlockHandle();
		}

		private void Button_EnableWindowResize_Click(object sender, EventArgs e)
		{
			Program.SplitScreenManager.AllowWindowResize();
		}

		private void Button_ToggleWindowBorders_Click(object sender, EventArgs e)
		{
			Program.SplitScreenManager.ToggleWindowBorders();
		}

		private void CheckBox_Transparency_CheckedChanged(object sender, EventArgs e)
		{
			Program.Form.Opacity = CheckBox_Transparency.Checked ? 0.90 : 1.00;
		}

		private void Button_CheckUpdates_Click(object sender, EventArgs e)
		{
			UpdateChecker.CheckUpdateDialog(true);
		}

		private void UnlockHandleButton_Click(object sender, EventArgs e)
		{
			if (string.IsNullOrEmpty(textBoxHandleName.Text))
				MessageBox.Show("Handle name cannot be empty.", "Error");
			else
				Program.SplitScreenManager.UnlockHandle(textBoxHandleName.Text);
		}

		private void Button_BrowseFindWindowHookExe_Click(object sender, EventArgs e)
		{
			//DialogResult result = FileDialog_FindWindowHook.ShowDialog();
			//using (OpenFileDialog openFileDialog = new OpenFileDialog())
			{
				/*openFileDialog.InitialDirectory = "c:\\";
				openFileDialog.Filter = "executable files (*.exe)|*.exe";
				openFileDialog.FilterIndex = 1;
				openFileDialog.RestoreDirectory = true;
				openFileDialog.ShowHelp = true;*/
				FileDialog_FindWindowHook.ShowHelp = true;//Whole application freezes otherwise

				if (FileDialog_FindWindowHook.ShowDialog() == DialogResult.OK)
				{
					Label_FindWindowHookExe.Text = FileDialog_FindWindowHook.FileName;
				}
			}
		}

		private void Button_FindWindowHookLaunch_Click(object sender, EventArgs e)
		{
			string fileName = FileDialog_FindWindowHook.FileName;
			string args = TextBox_FindWindowHookArgs.Text;
			bool is64 = Checkbox_FindWindowHookIs64.Checked;
			Logger.WriteLine($"StartupHook Launch button clicked. FileName='{fileName}', arguments='{args}', is64={is64}");

			if (System.IO.File.Exists(fileName))
			{
				Program.SplitScreenManager.CreateAndInjectStartupHook(
					is64, 
					fileName, 
					args,
					CheckBox_StartupHook_UseWaitForIdle.Checked,
					CheckBox_StartupHook_Dinput.Checked,
					CheckBox_StartupHook_FindWindow.Checked,
					(byte) dinputControllerIndex,
					CheckBox_StartupHook_FindMutexHook.Checked,
					StartupHook_MutexTargets.Text
					);
			}
			else
			{
				MessageBox.Show("Executable not found", "Error", MessageBoxButtons.OK);
			}
		}

		private int dinputControllerIndex;
		private void ComboBox_DinputControllerIndex_SelectedIndexChanged(object sender, EventArgs e)
		{
			dinputControllerIndex = ComboBox_DinputControllerIndex.SelectedIndex;
			Logger.WriteLine($"Set dinput controller index = {dinputControllerIndex}");
		}

		public void SetAutomaticallyCheckUpdatesChecked(bool value)
		{
			Checkbox_AutomaticallyCheckForUpdates.Checked = value;
		}

		#region Goldberg
		private void LoadGoldbergData()
		{
			string folder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Goldberg SteamEmu Saves", "settings");
			goldbergAccountNamePath = Path.Combine(folder, "account_name.txt");
			goldbergSteamIDPath = Path.Combine(folder, "user_steam_id.txt");

			string ReadLine(string path)
			{
				try
				{
					return File.ReadLines(path).First();
				}
				catch (Exception)
				{
					return string.Empty;
				}
			}

			TextBox_Goldberg_Username.Text = ReadLine(goldbergAccountNamePath);
			TextBox_Goldberg_ID.Text = ReadLine(goldbergSteamIDPath);
		}

		private void Button_Goldberg_Username_Set_Click(object sender, EventArgs e)
		{
			try
			{
				if (!File.Exists(goldbergAccountNamePath))
					File.CreateText(goldbergAccountNamePath).Dispose();
				File.WriteAllText(goldbergAccountNamePath, TextBox_Goldberg_Username.Text);
			}
			catch (Exception ex)
			{
				string msg = (ex.GetType() == typeof(DirectoryNotFoundException))
					? "Goldberg has not been set up. Try running a game with it at least once.\n"
					: "";
				MessageBox.Show($@"{msg}Error: {ex}", @"Error", MessageBoxButtons.OK);
			}
		}

		private void Button_Goldberg_ID_Set_Click(object sender, EventArgs e)
		{
			try
			{
				if (!File.Exists(goldbergSteamIDPath))
					File.CreateText(goldbergSteamIDPath).Dispose();
				File.WriteAllText(goldbergSteamIDPath, TextBox_Goldberg_ID.Text);
			}
			catch (Exception ex)
			{
				string msg = (ex.GetType() == typeof(DirectoryNotFoundException))
					? "Goldberg has not been set up. Try running a game with it at least once.\n"
					: "";
				MessageBox.Show($@"{msg}Error: {ex}", @"Error", MessageBoxButtons.OK);
			}
		}
		#endregion

		private void Checkbox_AutomaticallyCheckForUpdates_CheckedChanged(object sender, EventArgs e)
		{
			if (Program.Config == null) return;
			Program.Config.AutomaticallyCheckForUpdatesOnStartup = Checkbox_AutomaticallyCheckForUpdates.Checked;
			Program.Config.SaveConfig();
		}
	}
}
