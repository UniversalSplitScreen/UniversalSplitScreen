﻿namespace UniversalSplitScreen.UI
{
	partial class Form1
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.Label_StopWarning = new System.Windows.Forms.Label();
            this.CheckBox_Transparency = new System.Windows.Forms.CheckBox();
            this.startButton = new System.Windows.Forms.Button();
            this.stopButton = new System.Windows.Forms.Button();
            this.resetAllButton = new System.Windows.Forms.Button();
            this.activeWindowPanel = new System.Windows.Forms.Panel();
            this.groupBoxHandleUnlocker = new System.Windows.Forms.GroupBox();
            this.UnlockHandleButton = new System.Windows.Forms.Button();
            this.labelHandleName = new System.Windows.Forms.Label();
            this.textBoxHandleName = new System.Windows.Forms.TextBox();
            this.GroupBox_Utilities = new System.Windows.Forms.GroupBox();
            this.Button_UnlockSourceEngine = new System.Windows.Forms.Button();
            this.Label_CurrentWindowTabInstructions = new System.Windows.Forms.Label();
            this.GamePadGroupBox = new System.Windows.Forms.GroupBox();
            this.ControllerHookNote = new System.Windows.Forms.Label();
            this.ControllerIndexLabel = new System.Windows.Forms.Label();
            this.ControllerIndexComboBox = new System.Windows.Forms.ComboBox();
            this.windowTitleBox = new System.Windows.Forms.GroupBox();
            this.windowTitleLabel = new System.Windows.Forms.Label();
            this.keyboardBox = new System.Windows.Forms.GroupBox();
            this.keyboardHelpLabel = new System.Windows.Forms.Label();
            this.keyboardResetButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel2 = new System.Windows.Forms.FlowLayoutPanel();
            this.keyboardHandleStaticLabel = new System.Windows.Forms.Label();
            this.keyboardHandleLabel = new System.Windows.Forms.Label();
            this.keyboardSetTextbox = new System.Windows.Forms.TextBox();
            this.mouseBox = new System.Windows.Forms.GroupBox();
            this.mouseResetButton = new System.Windows.Forms.Button();
            this.attachMouseButton = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.mouseHandleStaticLabel = new System.Windows.Forms.Label();
            this.mouseHandleLabel = new System.Windows.Forms.Label();
            this.hwndBox = new System.Windows.Forms.GroupBox();
            this.hWndLabel = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.buttonOptions_New = new System.Windows.Forms.Button();
            this.buttonOptions_load = new System.Windows.Forms.Button();
            this.buttonOptions_save = new System.Windows.Forms.Button();
            this.buttonOptions_delete = new System.Windows.Forms.Button();
            this.optionsComboBox = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Label_AutofillHandleName = new System.Windows.Forms.Label();
            this.endButtonSetter = new System.Windows.Forms.Button();
            this.hooksBox = new System.Windows.Forms.GroupBox();
            this.hooksWarningLabel = new System.Windows.Forms.Label();
            this.tabPage9 = new System.Windows.Forms.TabPage();
            this.GroupBox_WindowOptions = new System.Windows.Forms.GroupBox();
            this.Button_ToggleWindowBorders = new System.Windows.Forms.Button();
            this.Button_EnableWindowResize = new System.Windows.Forms.Button();
            this.groupBox_WindowPositionAndOffsets = new System.Windows.Forms.GroupBox();
            this.Panel_Splitscreen2PlayersHorizontal = new System.Windows.Forms.Panel();
            this.Button_SplitscreenBottom = new System.Windows.Forms.Button();
            this.Button_SplitscreenTop = new System.Windows.Forms.Button();
            this.Panel_Splitscreen2PlayersVertical = new System.Windows.Forms.Panel();
            this.Button_SplitscreenRight = new System.Windows.Forms.Button();
            this.Button_SplitscreenLeft = new System.Windows.Forms.Button();
            this.Label_SplitscreenOptions = new System.Windows.Forms.Label();
            this.ComboBox_SplitscreenOptions = new System.Windows.Forms.ComboBox();
            this.Label_LeftOffset = new System.Windows.Forms.Label();
            this.Label_BorderExtraPadding = new System.Windows.Forms.Label();
            this.Label_TopOffset = new System.Windows.Forms.Label();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.tabControl3 = new System.Windows.Forms.TabControl();
            this.tabPage7 = new System.Windows.Forms.TabPage();
            this.panel8 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.StartupHook_MutexTargets = new System.Windows.Forms.TextBox();
            this.CheckBox_StartupHook_FindMutexHook = new System.Windows.Forms.CheckBox();
            this.panel7 = new System.Windows.Forms.Panel();
            this.CheckBox_StartupHook_FindWindow = new System.Windows.Forms.CheckBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel9 = new System.Windows.Forms.Panel();
            this.ComboBox_AppdataSwitch_Selector = new System.Windows.Forms.ComboBox();
            this.CheckBox_StartupHook_UseAppdataSwitch = new System.Windows.Forms.CheckBox();
            this.Button_BrowseFindWindowHookExe = new System.Windows.Forms.Button();
            this.flowLayoutPanel3 = new System.Windows.Forms.FlowLayoutPanel();
            this.Label_FindWindowHookExe = new System.Windows.Forms.Label();
            this.Label_FindWindowHookCmdArgsDescriptor = new System.Windows.Forms.Label();
            this.Button_FindWindowHookLaunch = new System.Windows.Forms.Button();
            this.TextBox_FindWindowHookArgs = new System.Windows.Forms.TextBox();
            this.Checkbox_FindWindowHookIs64 = new System.Windows.Forms.CheckBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.Label_DinputControllerIndex = new System.Windows.Forms.Label();
            this.ComboBox_DinputControllerIndex = new System.Windows.Forms.ComboBox();
            this.CheckBox_StartupHook_Dinput = new System.Windows.Forms.CheckBox();
            this.tabPage8 = new System.Windows.Forms.TabPage();
            this.Button_Goldberg_ID_Set = new System.Windows.Forms.Button();
            this.TextBox_Goldberg_ID = new System.Windows.Forms.TextBox();
            this.Button_Goldberg_Username_Set = new System.Windows.Forms.Button();
            this.TextBox_Goldberg_Username = new System.Windows.Forms.TextBox();
            this.Label_Goldberg_ID = new System.Windows.Forms.Label();
            this.Label_Goldberg_Username = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.Checkbox_AutomaticallyCheckForUpdates = new System.Windows.Forms.CheckBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.Label_CurrentVersion = new System.Windows.Forms.Label();
            this.Button_CheckUpdates = new System.Windows.Forms.Button();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.LabelHandleSearch = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.GroupBoxLicense = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.FileDialog_FindWindowHook = new System.Windows.Forms.OpenFileDialog();
            this.Panel_Splitscreen4Players = new System.Windows.Forms.Panel();
            this.Button_SplitscreenBottomLeft = new System.Windows.Forms.Button();
            this.Button_SplitscreenTopLeft = new System.Windows.Forms.Button();
            this.Button_SplitscreenBottomRight = new System.Windows.Forms.Button();
            this.Button_SplitscreenTopRight = new System.Windows.Forms.Button();
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SystemMouseToMiddle = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefTextbox_AutofillHandleName = new UniversalSplitScreen.UI.RefTextbox();
            this.RefCheckbox_SendScrollwheel = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_DrawMouse = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SendFakeWindowFocusMessages = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SendFakeWindowActivateMessages = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SendNormalKeyboardInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SendNormalMouseInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SendRawKeyboardInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_SendRawMouseInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_GetKeyboardState = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_Dinput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_MouseVisibility = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_UseLegacyInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_XInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_SetCursorPos = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_GetKeyState = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_GetAsyncKeyState = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_GetCursorPos = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_GetForegroundWindow = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_FilterMouseInputMessages = new UniversalSplitScreen.UI.RefCheckbox();
            this.RefCheckbox_Hook_FilterRawInput = new UniversalSplitScreen.UI.RefCheckbox();
            this.refTextbox_LeftOffset = new UniversalSplitScreen.UI.RefTextbox();
            this.refTextbox_TopOffset = new UniversalSplitScreen.UI.RefTextbox();
            this.refTextbox_BorderExtraPadding = new UniversalSplitScreen.UI.RefTextbox();
            this.WebLinkLabel_Goldberg = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkJson = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkIlyaki = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkEasyHook = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkAHKInterop = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkAHK = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkAHKDll = new UniversalSplitScreen.UI.WebLinkLabel();
            this.WebLinkWebsite = new UniversalSplitScreen.UI.WebLinkLabel();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.activeWindowPanel.SuspendLayout();
            this.groupBoxHandleUnlocker.SuspendLayout();
            this.GroupBox_Utilities.SuspendLayout();
            this.GamePadGroupBox.SuspendLayout();
            this.windowTitleBox.SuspendLayout();
            this.keyboardBox.SuspendLayout();
            this.flowLayoutPanel2.SuspendLayout();
            this.mouseBox.SuspendLayout();
            this.flowLayoutPanel1.SuspendLayout();
            this.hwndBox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.hooksBox.SuspendLayout();
            this.tabPage9.SuspendLayout();
            this.GroupBox_WindowOptions.SuspendLayout();
            this.groupBox_WindowPositionAndOffsets.SuspendLayout();
            this.Panel_Splitscreen2PlayersHorizontal.SuspendLayout();
            this.Panel_Splitscreen2PlayersVertical.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl3.SuspendLayout();
            this.tabPage7.SuspendLayout();
            this.panel8.SuspendLayout();
            this.panel7.SuspendLayout();
            this.panel6.SuspendLayout();
            this.panel9.SuspendLayout();
            this.flowLayoutPanel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tabPage8.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage6.SuspendLayout();
            this.GroupBoxLicense.SuspendLayout();
            this.Panel_Splitscreen4Players.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage9);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 470);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.Label_StopWarning);
            this.tabPage1.Controls.Add(this.CheckBox_Transparency);
            this.tabPage1.Controls.Add(this.startButton);
            this.tabPage1.Controls.Add(this.stopButton);
            this.tabPage1.Controls.Add(this.resetAllButton);
            this.tabPage1.Controls.Add(this.activeWindowPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 444);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Current window";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // Label_StopWarning
            // 
            this.Label_StopWarning.AutoSize = true;
            this.Label_StopWarning.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.Label_StopWarning.Location = new System.Drawing.Point(384, 415);
            this.Label_StopWarning.MaximumSize = new System.Drawing.Size(200, 0);
            this.Label_StopWarning.Name = "Label_StopWarning";
            this.Label_StopWarning.Size = new System.Drawing.Size(172, 26);
            this.Label_StopWarning.TabIndex = 11;
            this.Label_StopWarning.Text = "Press the End key (default) on any keyboard to stop split screen";
            // 
            // CheckBox_Transparency
            // 
            this.CheckBox_Transparency.AutoSize = true;
            this.CheckBox_Transparency.Location = new System.Drawing.Point(92, 424);
            this.CheckBox_Transparency.Name = "CheckBox_Transparency";
            this.CheckBox_Transparency.Size = new System.Drawing.Size(121, 17);
            this.CheckBox_Transparency.TabIndex = 11;
            this.CheckBox_Transparency.Text = "Transparency effect";
            this.CheckBox_Transparency.UseVisualStyleBackColor = true;
            this.CheckBox_Transparency.CheckedChanged += new System.EventHandler(this.CheckBox_Transparency_CheckedChanged);
            // 
            // startButton
            // 
            this.startButton.Location = new System.Drawing.Point(562, 418);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(96, 23);
            this.startButton.TabIndex = 10;
            this.startButton.Text = "Start split screen";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // stopButton
            // 
            this.stopButton.Location = new System.Drawing.Point(664, 418);
            this.stopButton.Name = "stopButton";
            this.stopButton.Size = new System.Drawing.Size(101, 23);
            this.stopButton.TabIndex = 9;
            this.stopButton.Text = "Stop split screen";
            this.stopButton.UseVisualStyleBackColor = true;
            this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
            // 
            // resetAllButton
            // 
            this.resetAllButton.Location = new System.Drawing.Point(9, 418);
            this.resetAllButton.Name = "resetAllButton";
            this.resetAllButton.Size = new System.Drawing.Size(75, 23);
            this.resetAllButton.TabIndex = 8;
            this.resetAllButton.Text = "Reset all";
            this.resetAllButton.UseVisualStyleBackColor = true;
            this.resetAllButton.Click += new System.EventHandler(this.resetAllButton_Click);
            // 
            // activeWindowPanel
            // 
            this.activeWindowPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.activeWindowPanel.Controls.Add(this.groupBoxHandleUnlocker);
            this.activeWindowPanel.Controls.Add(this.GroupBox_Utilities);
            this.activeWindowPanel.Controls.Add(this.Label_CurrentWindowTabInstructions);
            this.activeWindowPanel.Controls.Add(this.GamePadGroupBox);
            this.activeWindowPanel.Controls.Add(this.windowTitleBox);
            this.activeWindowPanel.Controls.Add(this.keyboardBox);
            this.activeWindowPanel.Controls.Add(this.mouseBox);
            this.activeWindowPanel.Controls.Add(this.hwndBox);
            this.activeWindowPanel.Location = new System.Drawing.Point(6, 6);
            this.activeWindowPanel.Name = "activeWindowPanel";
            this.activeWindowPanel.Size = new System.Drawing.Size(759, 406);
            this.activeWindowPanel.TabIndex = 7;
            // 
            // groupBoxHandleUnlocker
            // 
            this.groupBoxHandleUnlocker.Controls.Add(this.UnlockHandleButton);
            this.groupBoxHandleUnlocker.Controls.Add(this.labelHandleName);
            this.groupBoxHandleUnlocker.Controls.Add(this.textBoxHandleName);
            this.groupBoxHandleUnlocker.Location = new System.Drawing.Point(209, 261);
            this.groupBoxHandleUnlocker.Name = "groupBoxHandleUnlocker";
            this.groupBoxHandleUnlocker.Size = new System.Drawing.Size(200, 144);
            this.groupBoxHandleUnlocker.TabIndex = 11;
            this.groupBoxHandleUnlocker.TabStop = false;
            this.groupBoxHandleUnlocker.Text = "Handle unlocker";
            // 
            // UnlockHandleButton
            // 
            this.UnlockHandleButton.Location = new System.Drawing.Point(6, 79);
            this.UnlockHandleButton.Name = "UnlockHandleButton";
            this.UnlockHandleButton.Size = new System.Drawing.Size(188, 23);
            this.UnlockHandleButton.TabIndex = 2;
            this.UnlockHandleButton.Text = "Unlock";
            this.toolTip1.SetToolTip(this.UnlockHandleButton, resources.GetString("UnlockHandleButton.ToolTip"));
            this.UnlockHandleButton.UseVisualStyleBackColor = true;
            this.UnlockHandleButton.Click += new System.EventHandler(this.UnlockHandleButton_Click);
            // 
            // labelHandleName
            // 
            this.labelHandleName.AutoSize = true;
            this.labelHandleName.Location = new System.Drawing.Point(6, 29);
            this.labelHandleName.Name = "labelHandleName";
            this.labelHandleName.Size = new System.Drawing.Size(99, 13);
            this.labelHandleName.TabIndex = 1;
            this.labelHandleName.Text = "Enter handle name:";
            // 
            // textBoxHandleName
            // 
            this.textBoxHandleName.Location = new System.Drawing.Point(6, 45);
            this.textBoxHandleName.Name = "textBoxHandleName";
            this.textBoxHandleName.Size = new System.Drawing.Size(188, 20);
            this.textBoxHandleName.TabIndex = 0;
            // 
            // GroupBox_Utilities
            // 
            this.GroupBox_Utilities.Controls.Add(this.Button_UnlockSourceEngine);
            this.GroupBox_Utilities.Location = new System.Drawing.Point(3, 261);
            this.GroupBox_Utilities.Name = "GroupBox_Utilities";
            this.GroupBox_Utilities.Size = new System.Drawing.Size(200, 144);
            this.GroupBox_Utilities.TabIndex = 10;
            this.GroupBox_Utilities.TabStop = false;
            this.GroupBox_Utilities.Text = "Source Engine";
            // 
            // Button_UnlockSourceEngine
            // 
            this.Button_UnlockSourceEngine.Location = new System.Drawing.Point(6, 50);
            this.Button_UnlockSourceEngine.Name = "Button_UnlockSourceEngine";
            this.Button_UnlockSourceEngine.Size = new System.Drawing.Size(188, 40);
            this.Button_UnlockSourceEngine.TabIndex = 8;
            this.Button_UnlockSourceEngine.Text = "Unlock Source engine for a new instance";
            this.toolTip1.SetToolTip(this.Button_UnlockSourceEngine, resources.GetString("Button_UnlockSourceEngine.ToolTip"));
            this.Button_UnlockSourceEngine.UseVisualStyleBackColor = true;
            this.Button_UnlockSourceEngine.Click += new System.EventHandler(this.Button_UnlockSourceEngine_Click);
            // 
            // Label_CurrentWindowTabInstructions
            // 
            this.Label_CurrentWindowTabInstructions.AutoSize = true;
            this.Label_CurrentWindowTabInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_CurrentWindowTabInstructions.Location = new System.Drawing.Point(4, 49);
            this.Label_CurrentWindowTabInstructions.Name = "Label_CurrentWindowTabInstructions";
            this.Label_CurrentWindowTabInstructions.Size = new System.Drawing.Size(429, 15);
            this.Label_CurrentWindowTabInstructions.TabIndex = 9;
            this.Label_CurrentWindowTabInstructions.Text = "Focus a game\'s window (e.g. by alt+tabbing) and set up its input devices here.";
            // 
            // GamePadGroupBox
            // 
            this.GamePadGroupBox.Controls.Add(this.ControllerHookNote);
            this.GamePadGroupBox.Controls.Add(this.ControllerIndexLabel);
            this.GamePadGroupBox.Controls.Add(this.ControllerIndexComboBox);
            this.GamePadGroupBox.Location = new System.Drawing.Point(415, 76);
            this.GamePadGroupBox.Name = "GamePadGroupBox";
            this.GamePadGroupBox.Size = new System.Drawing.Size(210, 144);
            this.GamePadGroupBox.TabIndex = 7;
            this.GamePadGroupBox.TabStop = false;
            this.GamePadGroupBox.Text = "Gamepad/Controller/Joystick";
            // 
            // ControllerHookNote
            // 
            this.ControllerHookNote.AutoSize = true;
            this.ControllerHookNote.Location = new System.Drawing.Point(6, 86);
            this.ControllerHookNote.MaximumSize = new System.Drawing.Size(205, 0);
            this.ControllerHookNote.Name = "ControllerHookNote";
            this.ControllerHookNote.Size = new System.Drawing.Size(203, 52);
            this.ControllerHookNote.TabIndex = 2;
            this.ControllerHookNote.Text = "Note: You need to enable XInput Hook for gamepads to work. You will also need to " +
    "enable DInput to XInput translation to use more than 4 controllers.";
            // 
            // ControllerIndexLabel
            // 
            this.ControllerIndexLabel.AutoSize = true;
            this.ControllerIndexLabel.Location = new System.Drawing.Point(7, 20);
            this.ControllerIndexLabel.Name = "ControllerIndexLabel";
            this.ControllerIndexLabel.Size = new System.Drawing.Size(82, 13);
            this.ControllerIndexLabel.TabIndex = 1;
            this.ControllerIndexLabel.Text = "Controller index:";
            // 
            // ControllerIndexComboBox
            // 
            this.ControllerIndexComboBox.FormattingEnabled = true;
            this.ControllerIndexComboBox.Items.AddRange(new object[] {
            "No controller",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.ControllerIndexComboBox.Location = new System.Drawing.Point(6, 36);
            this.ControllerIndexComboBox.Name = "ControllerIndexComboBox";
            this.ControllerIndexComboBox.Size = new System.Drawing.Size(197, 21);
            this.ControllerIndexComboBox.TabIndex = 0;
            this.ControllerIndexComboBox.SelectedIndexChanged += new System.EventHandler(this.ControllerIndexComboBox_SelectedIndexChanged);
            // 
            // windowTitleBox
            // 
            this.windowTitleBox.Controls.Add(this.windowTitleLabel);
            this.windowTitleBox.Location = new System.Drawing.Point(3, 3);
            this.windowTitleBox.Name = "windowTitleBox";
            this.windowTitleBox.Size = new System.Drawing.Size(622, 39);
            this.windowTitleBox.TabIndex = 2;
            this.windowTitleBox.TabStop = false;
            this.windowTitleBox.Text = "Window title";
            // 
            // windowTitleLabel
            // 
            this.windowTitleLabel.AutoSize = true;
            this.windowTitleLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.windowTitleLabel.Location = new System.Drawing.Point(6, 16);
            this.windowTitleLabel.Name = "windowTitleLabel";
            this.windowTitleLabel.Size = new System.Drawing.Size(0, 13);
            this.windowTitleLabel.TabIndex = 1;
            // 
            // keyboardBox
            // 
            this.keyboardBox.Controls.Add(this.keyboardHelpLabel);
            this.keyboardBox.Controls.Add(this.keyboardResetButton);
            this.keyboardBox.Controls.Add(this.flowLayoutPanel2);
            this.keyboardBox.Controls.Add(this.keyboardSetTextbox);
            this.keyboardBox.Location = new System.Drawing.Point(209, 76);
            this.keyboardBox.Name = "keyboardBox";
            this.keyboardBox.Size = new System.Drawing.Size(200, 144);
            this.keyboardBox.TabIndex = 6;
            this.keyboardBox.TabStop = false;
            this.keyboardBox.Text = "Keyboard";
            // 
            // keyboardHelpLabel
            // 
            this.keyboardHelpLabel.AutoSize = true;
            this.keyboardHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyboardHelpLabel.Location = new System.Drawing.Point(6, 70);
            this.keyboardHelpLabel.Name = "keyboardHelpLabel";
            this.keyboardHelpLabel.Size = new System.Drawing.Size(149, 13);
            this.keyboardHelpLabel.TabIndex = 8;
            this.keyboardHelpLabel.Text = "Type here to set the keyboard";
            // 
            // keyboardResetButton
            // 
            this.keyboardResetButton.Location = new System.Drawing.Point(6, 112);
            this.keyboardResetButton.Name = "keyboardResetButton";
            this.keyboardResetButton.Size = new System.Drawing.Size(188, 26);
            this.keyboardResetButton.TabIndex = 7;
            this.keyboardResetButton.Text = "Reset";
            this.keyboardResetButton.UseVisualStyleBackColor = true;
            this.keyboardResetButton.Click += new System.EventHandler(this.keyboardResetButton_Click);
            // 
            // flowLayoutPanel2
            // 
            this.flowLayoutPanel2.Controls.Add(this.keyboardHandleStaticLabel);
            this.flowLayoutPanel2.Controls.Add(this.keyboardHandleLabel);
            this.flowLayoutPanel2.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutPanel2.Name = "flowLayoutPanel2";
            this.flowLayoutPanel2.Size = new System.Drawing.Size(188, 26);
            this.flowLayoutPanel2.TabIndex = 1;
            // 
            // keyboardHandleStaticLabel
            // 
            this.keyboardHandleStaticLabel.AutoSize = true;
            this.keyboardHandleStaticLabel.Location = new System.Drawing.Point(3, 0);
            this.keyboardHandleStaticLabel.Name = "keyboardHandleStaticLabel";
            this.keyboardHandleStaticLabel.Size = new System.Drawing.Size(41, 13);
            this.keyboardHandleStaticLabel.TabIndex = 7;
            this.keyboardHandleStaticLabel.Text = "Handle";
            // 
            // keyboardHandleLabel
            // 
            this.keyboardHandleLabel.AutoSize = true;
            this.keyboardHandleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.keyboardHandleLabel.Location = new System.Drawing.Point(50, 0);
            this.keyboardHandleLabel.Name = "keyboardHandleLabel";
            this.keyboardHandleLabel.Size = new System.Drawing.Size(15, 15);
            this.keyboardHandleLabel.TabIndex = 8;
            this.keyboardHandleLabel.Text = "0";
            // 
            // keyboardSetTextbox
            // 
            this.keyboardSetTextbox.Location = new System.Drawing.Point(6, 86);
            this.keyboardSetTextbox.Name = "keyboardSetTextbox";
            this.keyboardSetTextbox.Size = new System.Drawing.Size(188, 20);
            this.keyboardSetTextbox.TabIndex = 0;
            this.keyboardSetTextbox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.keyboardSetTextbox_KeyPress);
            // 
            // mouseBox
            // 
            this.mouseBox.Controls.Add(this.mouseResetButton);
            this.mouseBox.Controls.Add(this.attachMouseButton);
            this.mouseBox.Controls.Add(this.flowLayoutPanel1);
            this.mouseBox.Location = new System.Drawing.Point(3, 76);
            this.mouseBox.Name = "mouseBox";
            this.mouseBox.Size = new System.Drawing.Size(200, 144);
            this.mouseBox.TabIndex = 4;
            this.mouseBox.TabStop = false;
            this.mouseBox.Text = "Mouse";
            // 
            // mouseResetButton
            // 
            this.mouseResetButton.Location = new System.Drawing.Point(6, 112);
            this.mouseResetButton.Name = "mouseResetButton";
            this.mouseResetButton.Size = new System.Drawing.Size(188, 26);
            this.mouseResetButton.TabIndex = 7;
            this.mouseResetButton.Text = "Reset";
            this.mouseResetButton.UseVisualStyleBackColor = true;
            this.mouseResetButton.Click += new System.EventHandler(this.mouseResetButton_Click);
            // 
            // attachMouseButton
            // 
            this.attachMouseButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.attachMouseButton.Location = new System.Drawing.Point(6, 58);
            this.attachMouseButton.Name = "attachMouseButton";
            this.attachMouseButton.Size = new System.Drawing.Size(188, 48);
            this.attachMouseButton.TabIndex = 6;
            this.attachMouseButton.Text = "Set mouse";
            this.attachMouseButton.UseVisualStyleBackColor = true;
            this.attachMouseButton.MouseDown += new System.Windows.Forms.MouseEventHandler(this.attachMouseButton_MouseDown);
            this.attachMouseButton.MouseUp += new System.Windows.Forms.MouseEventHandler(this.attachMouseButton_MouseUp);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Controls.Add(this.mouseHandleStaticLabel);
            this.flowLayoutPanel1.Controls.Add(this.mouseHandleLabel);
            this.flowLayoutPanel1.Location = new System.Drawing.Point(6, 19);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(194, 20);
            this.flowLayoutPanel1.TabIndex = 5;
            // 
            // mouseHandleStaticLabel
            // 
            this.mouseHandleStaticLabel.AutoSize = true;
            this.mouseHandleStaticLabel.Location = new System.Drawing.Point(3, 0);
            this.mouseHandleStaticLabel.Name = "mouseHandleStaticLabel";
            this.mouseHandleStaticLabel.Size = new System.Drawing.Size(41, 13);
            this.mouseHandleStaticLabel.TabIndex = 0;
            this.mouseHandleStaticLabel.Text = "Handle";
            // 
            // mouseHandleLabel
            // 
            this.mouseHandleLabel.AutoSize = true;
            this.mouseHandleLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.mouseHandleLabel.Location = new System.Drawing.Point(50, 0);
            this.mouseHandleLabel.Name = "mouseHandleLabel";
            this.mouseHandleLabel.Size = new System.Drawing.Size(15, 15);
            this.mouseHandleLabel.TabIndex = 1;
            this.mouseHandleLabel.Text = "0";
            // 
            // hwndBox
            // 
            this.hwndBox.Controls.Add(this.hWndLabel);
            this.hwndBox.Location = new System.Drawing.Point(631, 4);
            this.hwndBox.Name = "hwndBox";
            this.hwndBox.Size = new System.Drawing.Size(123, 38);
            this.hwndBox.TabIndex = 3;
            this.hwndBox.TabStop = false;
            this.hwndBox.Text = "Window handle";
            // 
            // hWndLabel
            // 
            this.hWndLabel.AutoSize = true;
            this.hWndLabel.Location = new System.Drawing.Point(6, 16);
            this.hWndLabel.Name = "hWndLabel";
            this.hWndLabel.Size = new System.Drawing.Size(36, 13);
            this.hWndLabel.TabIndex = 0;
            this.hWndLabel.Text = "hWnd";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel2);
            this.tabPage2.Controls.Add(this.panel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 444);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Options";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.buttonOptions_New);
            this.panel2.Controls.Add(this.buttonOptions_load);
            this.panel2.Controls.Add(this.buttonOptions_save);
            this.panel2.Controls.Add(this.buttonOptions_delete);
            this.panel2.Controls.Add(this.optionsComboBox);
            this.panel2.Location = new System.Drawing.Point(6, 7);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(756, 30);
            this.panel2.TabIndex = 1;
            // 
            // buttonOptions_New
            // 
            this.buttonOptions_New.Location = new System.Drawing.Point(435, 3);
            this.buttonOptions_New.Name = "buttonOptions_New";
            this.buttonOptions_New.Size = new System.Drawing.Size(75, 23);
            this.buttonOptions_New.TabIndex = 12;
            this.buttonOptions_New.Text = "New";
            this.buttonOptions_New.UseVisualStyleBackColor = true;
            this.buttonOptions_New.Click += new System.EventHandler(this.buttonOptions_New_Click);
            // 
            // buttonOptions_load
            // 
            this.buttonOptions_load.Location = new System.Drawing.Point(516, 4);
            this.buttonOptions_load.Name = "buttonOptions_load";
            this.buttonOptions_load.Size = new System.Drawing.Size(75, 23);
            this.buttonOptions_load.TabIndex = 14;
            this.buttonOptions_load.Text = "Load";
            this.buttonOptions_load.UseVisualStyleBackColor = true;
            this.buttonOptions_load.Click += new System.EventHandler(this.buttonOptions_load_Click);
            // 
            // buttonOptions_save
            // 
            this.buttonOptions_save.Enabled = false;
            this.buttonOptions_save.Location = new System.Drawing.Point(597, 3);
            this.buttonOptions_save.Name = "buttonOptions_save";
            this.buttonOptions_save.Size = new System.Drawing.Size(75, 23);
            this.buttonOptions_save.TabIndex = 13;
            this.buttonOptions_save.Text = "Save";
            this.buttonOptions_save.UseVisualStyleBackColor = true;
            this.buttonOptions_save.Click += new System.EventHandler(this.buttonOptions_save_Click);
            // 
            // buttonOptions_delete
            // 
            this.buttonOptions_delete.Location = new System.Drawing.Point(678, 3);
            this.buttonOptions_delete.Name = "buttonOptions_delete";
            this.buttonOptions_delete.Size = new System.Drawing.Size(75, 23);
            this.buttonOptions_delete.TabIndex = 12;
            this.buttonOptions_delete.Text = "Delete";
            this.buttonOptions_delete.UseVisualStyleBackColor = true;
            this.buttonOptions_delete.Click += new System.EventHandler(this.button_optionsDelete_Click);
            // 
            // optionsComboBox
            // 
            this.optionsComboBox.FormattingEnabled = true;
            this.optionsComboBox.Location = new System.Drawing.Point(4, 4);
            this.optionsComboBox.Name = "optionsComboBox";
            this.optionsComboBox.Size = new System.Drawing.Size(425, 21);
            this.optionsComboBox.TabIndex = 0;
            this.optionsComboBox.SelectedIndexChanged += new System.EventHandler(this.optionsComboBox_SelectedIndexChanged);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart);
            this.panel1.Controls.Add(this.RefCheckbox_SystemMouseToMiddle);
            this.panel1.Controls.Add(this.Label_AutofillHandleName);
            this.panel1.Controls.Add(this.RefTextbox_AutofillHandleName);
            this.panel1.Controls.Add(this.RefCheckbox_SendScrollwheel);
            this.panel1.Controls.Add(this.RefCheckbox_DrawMouse);
            this.panel1.Controls.Add(this.RefCheckbox_RefreshWindowBoundsOnMouseClick);
            this.panel1.Controls.Add(this.RefCheckbox_SendFakeWindowFocusMessages);
            this.panel1.Controls.Add(this.RefCheckbox_SendFakeWindowActivateMessages);
            this.panel1.Controls.Add(this.RefCheckbox_SendNormalKeyboardInput);
            this.panel1.Controls.Add(this.RefCheckbox_SendNormalMouseInput);
            this.panel1.Controls.Add(this.RefCheckbox_SendRawKeyboardInput);
            this.panel1.Controls.Add(this.RefCheckbox_SendRawMouseInput);
            this.panel1.Controls.Add(this.endButtonSetter);
            this.panel1.Controls.Add(this.hooksBox);
            this.panel1.Location = new System.Drawing.Point(6, 43);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(755, 398);
            this.panel1.TabIndex = 0;
            // 
            // Label_AutofillHandleName
            // 
            this.Label_AutofillHandleName.AutoSize = true;
            this.Label_AutofillHandleName.Location = new System.Drawing.Point(1, 327);
            this.Label_AutofillHandleName.Name = "Label_AutofillHandleName";
            this.Label_AutofillHandleName.Size = new System.Drawing.Size(157, 13);
            this.Label_AutofillHandleName.TabIndex = 22;
            this.Label_AutofillHandleName.Text = "Autofill handle name in unlocker";
            // 
            // endButtonSetter
            // 
            this.endButtonSetter.Location = new System.Drawing.Point(3, 372);
            this.endButtonSetter.Name = "endButtonSetter";
            this.endButtonSetter.Size = new System.Drawing.Size(175, 23);
            this.endButtonSetter.TabIndex = 11;
            this.endButtonSetter.Text = "Stop button = End";
            this.toolTip1.SetToolTip(this.endButtonSetter, "Split screen will end when this key is pressed.\r\nChange this from end if you use " +
        "the end key in the game\'s controls.");
            this.endButtonSetter.UseVisualStyleBackColor = true;
            this.endButtonSetter.Click += new System.EventHandler(this.endButtonSetter_Click);
            // 
            // hooksBox
            // 
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_GetKeyboardState);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_Dinput);
            this.hooksBox.Controls.Add(this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_MouseVisibility);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_UseLegacyInput);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_XInput);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_SetCursorPos);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_GetKeyState);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_GetAsyncKeyState);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_GetCursorPos);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_GetForegroundWindow);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_FilterMouseInputMessages);
            this.hooksBox.Controls.Add(this.RefCheckbox_Hook_FilterRawInput);
            this.hooksBox.Controls.Add(this.hooksWarningLabel);
            this.hooksBox.Location = new System.Drawing.Point(472, 3);
            this.hooksBox.Name = "hooksBox";
            this.hooksBox.Size = new System.Drawing.Size(280, 392);
            this.hooksBox.TabIndex = 10;
            this.hooksBox.TabStop = false;
            this.hooksBox.Text = "Hooks";
            // 
            // hooksWarningLabel
            // 
            this.hooksWarningLabel.AutoSize = true;
            this.hooksWarningLabel.ForeColor = System.Drawing.Color.Red;
            this.hooksWarningLabel.Location = new System.Drawing.Point(6, 16);
            this.hooksWarningLabel.MaximumSize = new System.Drawing.Size(280, 0);
            this.hooksWarningLabel.Name = "hooksWarningLabel";
            this.hooksWarningLabel.Size = new System.Drawing.Size(278, 52);
            this.hooksWarningLabel.TabIndex = 0;
            this.hooksWarningLabel.Text = "Warning: Hooks run code in the target game process. This may be detected as a fal" +
    "se positive by an anti-cheat system or anti-virus software. See the documentatio" +
    "n for more info.";
            // 
            // tabPage9
            // 
            this.tabPage9.Controls.Add(this.GroupBox_WindowOptions);
            this.tabPage9.Controls.Add(this.groupBox_WindowPositionAndOffsets);
            this.tabPage9.Location = new System.Drawing.Point(4, 22);
            this.tabPage9.Name = "tabPage9";
            this.tabPage9.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage9.Size = new System.Drawing.Size(768, 444);
            this.tabPage9.TabIndex = 4;
            this.tabPage9.Text = "Window utilities";
            this.tabPage9.UseVisualStyleBackColor = true;
            // 
            // GroupBox_WindowOptions
            // 
            this.GroupBox_WindowOptions.Controls.Add(this.Button_ToggleWindowBorders);
            this.GroupBox_WindowOptions.Controls.Add(this.Button_EnableWindowResize);
            this.GroupBox_WindowOptions.Location = new System.Drawing.Point(325, 5);
            this.GroupBox_WindowOptions.Name = "GroupBox_WindowOptions";
            this.GroupBox_WindowOptions.Size = new System.Drawing.Size(150, 170);
            this.GroupBox_WindowOptions.TabIndex = 40;
            this.GroupBox_WindowOptions.TabStop = false;
            this.GroupBox_WindowOptions.Text = "Window options";
            // 
            // Button_ToggleWindowBorders
            // 
            this.Button_ToggleWindowBorders.Location = new System.Drawing.Point(5, 50);
            this.Button_ToggleWindowBorders.Name = "Button_ToggleWindowBorders";
            this.Button_ToggleWindowBorders.Size = new System.Drawing.Size(140, 25);
            this.Button_ToggleWindowBorders.TabIndex = 34;
            this.Button_ToggleWindowBorders.Text = "Toggle window borders";
            this.Button_ToggleWindowBorders.UseVisualStyleBackColor = true;
            this.Button_ToggleWindowBorders.Click += new System.EventHandler(this.Button_ToggleWindowBorders_Click);
            // 
            // Button_EnableWindowResize
            // 
            this.Button_EnableWindowResize.Location = new System.Drawing.Point(5, 20);
            this.Button_EnableWindowResize.Name = "Button_EnableWindowResize";
            this.Button_EnableWindowResize.Size = new System.Drawing.Size(141, 25);
            this.Button_EnableWindowResize.TabIndex = 33;
            this.Button_EnableWindowResize.Text = "Enable window resizing";
            this.toolTip1.SetToolTip(this.Button_EnableWindowResize, "Allows target window to be resized");
            this.Button_EnableWindowResize.UseVisualStyleBackColor = true;
            this.Button_EnableWindowResize.Click += new System.EventHandler(this.Button_EnableWindowResize_Click);
            // 
            // groupBox_WindowPositionAndOffsets
            // 
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Panel_Splitscreen4Players);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Panel_Splitscreen2PlayersHorizontal);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Panel_Splitscreen2PlayersVertical);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.refTextbox_LeftOffset);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Label_SplitscreenOptions);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.ComboBox_SplitscreenOptions);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.refTextbox_TopOffset);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Label_LeftOffset);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.refTextbox_BorderExtraPadding);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Label_BorderExtraPadding);
            this.groupBox_WindowPositionAndOffsets.Controls.Add(this.Label_TopOffset);
            this.groupBox_WindowPositionAndOffsets.Location = new System.Drawing.Point(5, 5);
            this.groupBox_WindowPositionAndOffsets.Name = "groupBox_WindowPositionAndOffsets";
            this.groupBox_WindowPositionAndOffsets.Size = new System.Drawing.Size(315, 170);
            this.groupBox_WindowPositionAndOffsets.TabIndex = 37;
            this.groupBox_WindowPositionAndOffsets.TabStop = false;
            this.groupBox_WindowPositionAndOffsets.Text = "Window position and offsets";
            // 
            // Panel_Splitscreen2PlayersHorizontal
            // 
            this.Panel_Splitscreen2PlayersHorizontal.Controls.Add(this.Button_SplitscreenBottom);
            this.Panel_Splitscreen2PlayersHorizontal.Controls.Add(this.Button_SplitscreenTop);
            this.Panel_Splitscreen2PlayersHorizontal.Location = new System.Drawing.Point(10, 65);
            this.Panel_Splitscreen2PlayersHorizontal.Name = "Panel_Splitscreen2PlayersHorizontal";
            this.Panel_Splitscreen2PlayersHorizontal.Size = new System.Drawing.Size(150, 85);
            this.Panel_Splitscreen2PlayersHorizontal.TabIndex = 38;
            this.Panel_Splitscreen2PlayersHorizontal.Visible = false;
            // 
            // Button_SplitscreenBottom
            // 
            this.Button_SplitscreenBottom.Location = new System.Drawing.Point(0, 45);
            this.Button_SplitscreenBottom.Name = "Button_SplitscreenBottom";
            this.Button_SplitscreenBottom.Size = new System.Drawing.Size(150, 40);
            this.Button_SplitscreenBottom.TabIndex = 29;
            this.Button_SplitscreenBottom.Text = "Bottom";
            this.Button_SplitscreenBottom.UseVisualStyleBackColor = true;
            this.Button_SplitscreenBottom.Click += new System.EventHandler(this.Button_SplitscreenBottom_Click);
            // 
            // Button_SplitscreenTop
            // 
            this.Button_SplitscreenTop.Location = new System.Drawing.Point(0, 5);
            this.Button_SplitscreenTop.Name = "Button_SplitscreenTop";
            this.Button_SplitscreenTop.Size = new System.Drawing.Size(150, 40);
            this.Button_SplitscreenTop.TabIndex = 28;
            this.Button_SplitscreenTop.Text = "Top";
            this.Button_SplitscreenTop.UseVisualStyleBackColor = true;
            this.Button_SplitscreenTop.Click += new System.EventHandler(this.Button_SplitscreenTop_Click);
            // 
            // Panel_Splitscreen2PlayersVertical
            // 
            this.Panel_Splitscreen2PlayersVertical.Controls.Add(this.Button_SplitscreenRight);
            this.Panel_Splitscreen2PlayersVertical.Controls.Add(this.Button_SplitscreenLeft);
            this.Panel_Splitscreen2PlayersVertical.Location = new System.Drawing.Point(10, 65);
            this.Panel_Splitscreen2PlayersVertical.Name = "Panel_Splitscreen2PlayersVertical";
            this.Panel_Splitscreen2PlayersVertical.Size = new System.Drawing.Size(150, 85);
            this.Panel_Splitscreen2PlayersVertical.TabIndex = 39;
            this.Panel_Splitscreen2PlayersVertical.Visible = false;
            // 
            // Button_SplitscreenRight
            // 
            this.Button_SplitscreenRight.Location = new System.Drawing.Point(75, 5);
            this.Button_SplitscreenRight.Name = "Button_SplitscreenRight";
            this.Button_SplitscreenRight.Size = new System.Drawing.Size(75, 80);
            this.Button_SplitscreenRight.TabIndex = 27;
            this.Button_SplitscreenRight.Text = "Right";
            this.Button_SplitscreenRight.UseVisualStyleBackColor = true;
            this.Button_SplitscreenRight.Click += new System.EventHandler(this.Button_SplitscreenRight_Click);
            // 
            // Button_SplitscreenLeft
            // 
            this.Button_SplitscreenLeft.Location = new System.Drawing.Point(0, 5);
            this.Button_SplitscreenLeft.Name = "Button_SplitscreenLeft";
            this.Button_SplitscreenLeft.Size = new System.Drawing.Size(75, 80);
            this.Button_SplitscreenLeft.TabIndex = 26;
            this.Button_SplitscreenLeft.Text = " Left";
            this.Button_SplitscreenLeft.UseVisualStyleBackColor = true;
            this.Button_SplitscreenLeft.Click += new System.EventHandler(this.Button_SplitscreenLeft_Click);
            // 
            // Label_SplitscreenOptions
            // 
            this.Label_SplitscreenOptions.AutoSize = true;
            this.Label_SplitscreenOptions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_SplitscreenOptions.Location = new System.Drawing.Point(10, 20);
            this.Label_SplitscreenOptions.Name = "Label_SplitscreenOptions";
            this.Label_SplitscreenOptions.Size = new System.Drawing.Size(111, 15);
            this.Label_SplitscreenOptions.TabIndex = 36;
            this.Label_SplitscreenOptions.Text = "Splitscreen options";
            // 
            // ComboBox_SplitscreenOptions
            // 
            this.ComboBox_SplitscreenOptions.FormattingEnabled = true;
            this.ComboBox_SplitscreenOptions.Items.AddRange(new object[] {
            "2PlayersVertical",
            "2PlayersHorizontal",
            "4Players"});
            this.ComboBox_SplitscreenOptions.Location = new System.Drawing.Point(10, 35);
            this.ComboBox_SplitscreenOptions.Name = "ComboBox_SplitscreenOptions";
            this.ComboBox_SplitscreenOptions.Size = new System.Drawing.Size(150, 21);
            this.ComboBox_SplitscreenOptions.TabIndex = 35;
            this.toolTip1.SetToolTip(this.ComboBox_SplitscreenOptions, "List of the most common splitscreen window positions");
            this.ComboBox_SplitscreenOptions.SelectedIndexChanged += new System.EventHandler(this.Refresh_SplitscreenWindow);
            // 
            // Label_LeftOffset
            // 
            this.Label_LeftOffset.AutoSize = true;
            this.Label_LeftOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_LeftOffset.Location = new System.Drawing.Point(230, 70);
            this.Label_LeftOffset.Name = "Label_LeftOffset";
            this.Label_LeftOffset.Size = new System.Drawing.Size(61, 15);
            this.Label_LeftOffset.TabIndex = 25;
            this.Label_LeftOffset.Text = "Left Offset";
            this.toolTip1.SetToolTip(this.Label_LeftOffset, "Useful to hide the window border");
            // 
            // Label_BorderExtraPadding
            // 
            this.Label_BorderExtraPadding.AutoSize = true;
            this.Label_BorderExtraPadding.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_BorderExtraPadding.Location = new System.Drawing.Point(230, 130);
            this.Label_BorderExtraPadding.Name = "Label_BorderExtraPadding";
            this.Label_BorderExtraPadding.Size = new System.Drawing.Size(74, 15);
            this.Label_BorderExtraPadding.TabIndex = 29;
            this.Label_BorderExtraPadding.Text = "Extra Height";
            this.toolTip1.SetToolTip(this.Label_BorderExtraPadding, "Allow to increase the size of the window beyond the calculated height (useful to " +
        "hide title bar if it can\'t be removed safely)");
            // 
            // Label_TopOffset
            // 
            this.Label_TopOffset.AutoSize = true;
            this.Label_TopOffset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_TopOffset.Location = new System.Drawing.Point(230, 100);
            this.Label_TopOffset.Name = "Label_TopOffset";
            this.Label_TopOffset.Size = new System.Drawing.Size(62, 15);
            this.Label_TopOffset.TabIndex = 28;
            this.Label_TopOffset.Text = "Top Offset";
            this.toolTip1.SetToolTip(this.Label_TopOffset, "Useful to hide the title bar if it can\'t be removed");
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.tabControl3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(768, 444);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Utilities";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // tabControl3
            // 
            this.tabControl3.Controls.Add(this.tabPage7);
            this.tabControl3.Controls.Add(this.tabPage8);
            this.tabControl3.Location = new System.Drawing.Point(3, 3);
            this.tabControl3.Name = "tabControl3";
            this.tabControl3.SelectedIndex = 0;
            this.tabControl3.Size = new System.Drawing.Size(762, 435);
            this.tabControl3.TabIndex = 0;
            // 
            // tabPage7
            // 
            this.tabPage7.Controls.Add(this.panel8);
            this.tabPage7.Controls.Add(this.panel7);
            this.tabPage7.Controls.Add(this.panel6);
            this.tabPage7.Controls.Add(this.panel5);
            this.tabPage7.Location = new System.Drawing.Point(4, 22);
            this.tabPage7.Name = "tabPage7";
            this.tabPage7.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage7.Size = new System.Drawing.Size(754, 409);
            this.tabPage7.TabIndex = 0;
            this.tabPage7.Text = "Startup hooks";
            this.tabPage7.UseVisualStyleBackColor = true;
            // 
            // panel8
            // 
            this.panel8.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel8.Controls.Add(this.label2);
            this.panel8.Controls.Add(this.StartupHook_MutexTargets);
            this.panel8.Controls.Add(this.CheckBox_StartupHook_FindMutexHook);
            this.panel8.Location = new System.Drawing.Point(141, 6);
            this.panel8.Name = "panel8";
            this.panel8.Size = new System.Drawing.Size(343, 78);
            this.panel8.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Handle names:";
            // 
            // StartupHook_MutexTargets
            // 
            this.StartupHook_MutexTargets.Location = new System.Drawing.Point(3, 52);
            this.StartupHook_MutexTargets.Name = "StartupHook_MutexTargets";
            this.StartupHook_MutexTargets.Size = new System.Drawing.Size(335, 20);
            this.StartupHook_MutexTargets.TabIndex = 15;
            this.toolTip1.SetToolTip(this.StartupHook_MutexTargets, "Separate multiple names with 5 & symbols, e.g. name1&&&&&name2");
            // 
            // CheckBox_StartupHook_FindMutexHook
            // 
            this.CheckBox_StartupHook_FindMutexHook.AutoSize = true;
            this.CheckBox_StartupHook_FindMutexHook.Location = new System.Drawing.Point(3, 3);
            this.CheckBox_StartupHook_FindMutexHook.Name = "CheckBox_StartupHook_FindMutexHook";
            this.CheckBox_StartupHook_FindMutexHook.Size = new System.Drawing.Size(197, 17);
            this.CheckBox_StartupHook_FindMutexHook.TabIndex = 14;
            this.CheckBox_StartupHook_FindMutexHook.Text = "Find Mutex/Event/Semaphore hook";
            this.toolTip1.SetToolTip(this.CheckBox_StartupHook_FindMutexHook, resources.GetString("CheckBox_StartupHook_FindMutexHook.ToolTip"));
            this.CheckBox_StartupHook_FindMutexHook.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel7.Controls.Add(this.CheckBox_StartupHook_FindWindow);
            this.panel7.Location = new System.Drawing.Point(6, 6);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(129, 23);
            this.panel7.TabIndex = 12;
            // 
            // CheckBox_StartupHook_FindWindow
            // 
            this.CheckBox_StartupHook_FindWindow.AutoSize = true;
            this.CheckBox_StartupHook_FindWindow.Location = new System.Drawing.Point(3, 3);
            this.CheckBox_StartupHook_FindWindow.Name = "CheckBox_StartupHook_FindWindow";
            this.CheckBox_StartupHook_FindWindow.Size = new System.Drawing.Size(112, 17);
            this.CheckBox_StartupHook_FindWindow.TabIndex = 8;
            this.CheckBox_StartupHook_FindWindow.Text = "FindWindow hook";
            this.toolTip1.SetToolTip(this.CheckBox_StartupHook_FindWindow, resources.GetString("CheckBox_StartupHook_FindWindow.ToolTip"));
            this.CheckBox_StartupHook_FindWindow.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel6.Controls.Add(this.panel9);
            this.panel6.Controls.Add(this.Button_BrowseFindWindowHookExe);
            this.panel6.Controls.Add(this.flowLayoutPanel3);
            this.panel6.Controls.Add(this.Label_FindWindowHookCmdArgsDescriptor);
            this.panel6.Controls.Add(this.Button_FindWindowHookLaunch);
            this.panel6.Controls.Add(this.TextBox_FindWindowHookArgs);
            this.panel6.Controls.Add(this.Checkbox_FindWindowHookIs64);
            this.panel6.Location = new System.Drawing.Point(6, 152);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(742, 251);
            this.panel6.TabIndex = 11;
            // 
            // panel9
            // 
            this.panel9.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel9.Controls.Add(this.ComboBox_AppdataSwitch_Selector);
            this.panel9.Controls.Add(this.CheckBox_StartupHook_UseAppdataSwitch);
            this.panel9.Location = new System.Drawing.Point(6, 171);
            this.panel9.Name = "panel9";
            this.panel9.Size = new System.Drawing.Size(205, 75);
            this.panel9.TabIndex = 14;
            // 
            // ComboBox_AppdataSwitch_Selector
            // 
            this.ComboBox_AppdataSwitch_Selector.FormattingEnabled = true;
            this.ComboBox_AppdataSwitch_Selector.Items.AddRange(new object[] {
            "No change",
            "Index 1",
            "Index 2",
            "Index 3",
            "Index 4",
            "Index 5",
            "Index 6",
            "Index 7",
            "Index 8",
            "Index 9",
            "Index 10",
            "Index 11",
            "Index 12",
            "Index 13",
            "Index 14",
            "Index 15",
            "Index 16"});
            this.ComboBox_AppdataSwitch_Selector.Location = new System.Drawing.Point(4, 47);
            this.ComboBox_AppdataSwitch_Selector.Name = "ComboBox_AppdataSwitch_Selector";
            this.ComboBox_AppdataSwitch_Selector.Size = new System.Drawing.Size(195, 21);
            this.ComboBox_AppdataSwitch_Selector.TabIndex = 9;
            // 
            // CheckBox_StartupHook_UseAppdataSwitch
            // 
            this.CheckBox_StartupHook_UseAppdataSwitch.AutoSize = true;
            this.CheckBox_StartupHook_UseAppdataSwitch.Location = new System.Drawing.Point(3, 3);
            this.CheckBox_StartupHook_UseAppdataSwitch.Name = "CheckBox_StartupHook_UseAppdataSwitch";
            this.CheckBox_StartupHook_UseAppdataSwitch.Size = new System.Drawing.Size(196, 17);
            this.CheckBox_StartupHook_UseAppdataSwitch.TabIndex = 8;
            this.CheckBox_StartupHook_UseAppdataSwitch.Text = "Use AppData and user folder switch";
            this.toolTip1.SetToolTip(this.CheckBox_StartupHook_UseAppdataSwitch, resources.GetString("CheckBox_StartupHook_UseAppdataSwitch.ToolTip"));
            this.CheckBox_StartupHook_UseAppdataSwitch.UseVisualStyleBackColor = true;
            // 
            // Button_BrowseFindWindowHookExe
            // 
            this.Button_BrowseFindWindowHookExe.Location = new System.Drawing.Point(6, 3);
            this.Button_BrowseFindWindowHookExe.Name = "Button_BrowseFindWindowHookExe";
            this.Button_BrowseFindWindowHookExe.Size = new System.Drawing.Size(75, 23);
            this.Button_BrowseFindWindowHookExe.TabIndex = 2;
            this.Button_BrowseFindWindowHookExe.Text = "Browse";
            this.Button_BrowseFindWindowHookExe.UseVisualStyleBackColor = true;
            this.Button_BrowseFindWindowHookExe.Click += new System.EventHandler(this.Button_BrowseFindWindowHookExe_Click);
            // 
            // flowLayoutPanel3
            // 
            this.flowLayoutPanel3.Controls.Add(this.Label_FindWindowHookExe);
            this.flowLayoutPanel3.Location = new System.Drawing.Point(3, 32);
            this.flowLayoutPanel3.Name = "flowLayoutPanel3";
            this.flowLayoutPanel3.Size = new System.Drawing.Size(738, 71);
            this.flowLayoutPanel3.TabIndex = 3;
            // 
            // Label_FindWindowHookExe
            // 
            this.Label_FindWindowHookExe.AutoSize = true;
            this.Label_FindWindowHookExe.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Label_FindWindowHookExe.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label_FindWindowHookExe.Location = new System.Drawing.Point(3, 0);
            this.Label_FindWindowHookExe.Name = "Label_FindWindowHookExe";
            this.Label_FindWindowHookExe.Size = new System.Drawing.Size(165, 15);
            this.Label_FindWindowHookExe.TabIndex = 1;
            this.Label_FindWindowHookExe.Text = "Path to game executable...";
            // 
            // Label_FindWindowHookCmdArgsDescriptor
            // 
            this.Label_FindWindowHookCmdArgsDescriptor.AutoSize = true;
            this.Label_FindWindowHookCmdArgsDescriptor.Location = new System.Drawing.Point(3, 106);
            this.Label_FindWindowHookCmdArgsDescriptor.Name = "Label_FindWindowHookCmdArgsDescriptor";
            this.Label_FindWindowHookCmdArgsDescriptor.Size = new System.Drawing.Size(171, 13);
            this.Label_FindWindowHookCmdArgsDescriptor.TabIndex = 5;
            this.Label_FindWindowHookCmdArgsDescriptor.Text = "Command line arguments (optional)";
            // 
            // Button_FindWindowHookLaunch
            // 
            this.Button_FindWindowHookLaunch.Location = new System.Drawing.Point(650, 214);
            this.Button_FindWindowHookLaunch.Name = "Button_FindWindowHookLaunch";
            this.Button_FindWindowHookLaunch.Size = new System.Drawing.Size(87, 32);
            this.Button_FindWindowHookLaunch.TabIndex = 7;
            this.Button_FindWindowHookLaunch.Text = "Launch";
            this.Button_FindWindowHookLaunch.UseVisualStyleBackColor = true;
            this.Button_FindWindowHookLaunch.Click += new System.EventHandler(this.Button_FindWindowHookLaunch_Click);
            // 
            // TextBox_FindWindowHookArgs
            // 
            this.TextBox_FindWindowHookArgs.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextBox_FindWindowHookArgs.Location = new System.Drawing.Point(6, 122);
            this.TextBox_FindWindowHookArgs.Name = "TextBox_FindWindowHookArgs";
            this.TextBox_FindWindowHookArgs.Size = new System.Drawing.Size(731, 23);
            this.TextBox_FindWindowHookArgs.TabIndex = 4;
            // 
            // Checkbox_FindWindowHookIs64
            // 
            this.Checkbox_FindWindowHookIs64.AutoSize = true;
            this.Checkbox_FindWindowHookIs64.Location = new System.Drawing.Point(87, 7);
            this.Checkbox_FindWindowHookIs64.Name = "Checkbox_FindWindowHookIs64";
            this.Checkbox_FindWindowHookIs64.Size = new System.Drawing.Size(63, 17);
            this.Checkbox_FindWindowHookIs64.TabIndex = 6;
            this.Checkbox_FindWindowHookIs64.Text = "Is 64-bit";
            this.Checkbox_FindWindowHookIs64.UseVisualStyleBackColor = true;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.Label_DinputControllerIndex);
            this.panel5.Controls.Add(this.ComboBox_DinputControllerIndex);
            this.panel5.Controls.Add(this.CheckBox_StartupHook_Dinput);
            this.panel5.Location = new System.Drawing.Point(6, 35);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(129, 85);
            this.panel5.TabIndex = 10;
            // 
            // Label_DinputControllerIndex
            // 
            this.Label_DinputControllerIndex.AutoSize = true;
            this.Label_DinputControllerIndex.Location = new System.Drawing.Point(3, 43);
            this.Label_DinputControllerIndex.Name = "Label_DinputControllerIndex";
            this.Label_DinputControllerIndex.Size = new System.Drawing.Size(82, 13);
            this.Label_DinputControllerIndex.TabIndex = 11;
            this.Label_DinputControllerIndex.Text = "Controller index:";
            // 
            // ComboBox_DinputControllerIndex
            // 
            this.ComboBox_DinputControllerIndex.FormattingEnabled = true;
            this.ComboBox_DinputControllerIndex.Items.AddRange(new object[] {
            "No controller",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16"});
            this.ComboBox_DinputControllerIndex.Location = new System.Drawing.Point(3, 59);
            this.ComboBox_DinputControllerIndex.Name = "ComboBox_DinputControllerIndex";
            this.ComboBox_DinputControllerIndex.Size = new System.Drawing.Size(121, 21);
            this.ComboBox_DinputControllerIndex.TabIndex = 10;
            this.ComboBox_DinputControllerIndex.SelectedIndexChanged += new System.EventHandler(this.ComboBox_DinputControllerIndex_SelectedIndexChanged);
            // 
            // CheckBox_StartupHook_Dinput
            // 
            this.CheckBox_StartupHook_Dinput.AutoSize = true;
            this.CheckBox_StartupHook_Dinput.Location = new System.Drawing.Point(3, 3);
            this.CheckBox_StartupHook_Dinput.Name = "CheckBox_StartupHook_Dinput";
            this.CheckBox_StartupHook_Dinput.Size = new System.Drawing.Size(105, 17);
            this.CheckBox_StartupHook_Dinput.TabIndex = 9;
            this.CheckBox_StartupHook_Dinput.Text = "DirectInput hook";
            this.toolTip1.SetToolTip(this.CheckBox_StartupHook_Dinput, resources.GetString("CheckBox_StartupHook_Dinput.ToolTip"));
            this.CheckBox_StartupHook_Dinput.UseVisualStyleBackColor = true;
            // 
            // tabPage8
            // 
            this.tabPage8.Controls.Add(this.Button_Goldberg_ID_Set);
            this.tabPage8.Controls.Add(this.TextBox_Goldberg_ID);
            this.tabPage8.Controls.Add(this.Button_Goldberg_Username_Set);
            this.tabPage8.Controls.Add(this.TextBox_Goldberg_Username);
            this.tabPage8.Controls.Add(this.Label_Goldberg_ID);
            this.tabPage8.Controls.Add(this.Label_Goldberg_Username);
            this.tabPage8.Controls.Add(this.WebLinkLabel_Goldberg);
            this.tabPage8.Location = new System.Drawing.Point(4, 22);
            this.tabPage8.Name = "tabPage8";
            this.tabPage8.Size = new System.Drawing.Size(754, 409);
            this.tabPage8.TabIndex = 1;
            this.tabPage8.Text = "Goldberg";
            this.tabPage8.UseVisualStyleBackColor = true;
            // 
            // Button_Goldberg_ID_Set
            // 
            this.Button_Goldberg_ID_Set.Location = new System.Drawing.Point(334, 157);
            this.Button_Goldberg_ID_Set.Name = "Button_Goldberg_ID_Set";
            this.Button_Goldberg_ID_Set.Size = new System.Drawing.Size(75, 20);
            this.Button_Goldberg_ID_Set.TabIndex = 6;
            this.Button_Goldberg_ID_Set.Text = "Set";
            this.Button_Goldberg_ID_Set.UseVisualStyleBackColor = true;
            this.Button_Goldberg_ID_Set.Click += new System.EventHandler(this.Button_Goldberg_ID_Set_Click);
            // 
            // TextBox_Goldberg_ID
            // 
            this.TextBox_Goldberg_ID.Location = new System.Drawing.Point(7, 157);
            this.TextBox_Goldberg_ID.Name = "TextBox_Goldberg_ID";
            this.TextBox_Goldberg_ID.Size = new System.Drawing.Size(320, 20);
            this.TextBox_Goldberg_ID.TabIndex = 5;
            // 
            // Button_Goldberg_Username_Set
            // 
            this.Button_Goldberg_Username_Set.Location = new System.Drawing.Point(334, 94);
            this.Button_Goldberg_Username_Set.Name = "Button_Goldberg_Username_Set";
            this.Button_Goldberg_Username_Set.Size = new System.Drawing.Size(75, 20);
            this.Button_Goldberg_Username_Set.TabIndex = 4;
            this.Button_Goldberg_Username_Set.Text = "Set";
            this.Button_Goldberg_Username_Set.UseVisualStyleBackColor = true;
            this.Button_Goldberg_Username_Set.Click += new System.EventHandler(this.Button_Goldberg_Username_Set_Click);
            // 
            // TextBox_Goldberg_Username
            // 
            this.TextBox_Goldberg_Username.Location = new System.Drawing.Point(7, 94);
            this.TextBox_Goldberg_Username.Name = "TextBox_Goldberg_Username";
            this.TextBox_Goldberg_Username.Size = new System.Drawing.Size(320, 20);
            this.TextBox_Goldberg_Username.TabIndex = 3;
            // 
            // Label_Goldberg_ID
            // 
            this.Label_Goldberg_ID.AutoSize = true;
            this.Label_Goldberg_ID.Location = new System.Drawing.Point(4, 140);
            this.Label_Goldberg_ID.Name = "Label_Goldberg_ID";
            this.Label_Goldberg_ID.Size = new System.Drawing.Size(51, 13);
            this.Label_Goldberg_ID.TabIndex = 2;
            this.Label_Goldberg_ID.Text = "Steam ID";
            // 
            // Label_Goldberg_Username
            // 
            this.Label_Goldberg_Username.AutoSize = true;
            this.Label_Goldberg_Username.Location = new System.Drawing.Point(4, 77);
            this.Label_Goldberg_Username.Name = "Label_Goldberg_Username";
            this.Label_Goldberg_Username.Size = new System.Drawing.Size(55, 13);
            this.Label_Goldberg_Username.TabIndex = 1;
            this.Label_Goldberg_Username.Text = "Username";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel3);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(768, 444);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "About";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Checkbox_AutomaticallyCheckForUpdates);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Controls.Add(this.Button_CheckUpdates);
            this.panel3.Controls.Add(this.tabControl2);
            this.panel3.Controls.Add(this.WebLinkWebsite);
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(762, 435);
            this.panel3.TabIndex = 3;
            // 
            // Checkbox_AutomaticallyCheckForUpdates
            // 
            this.Checkbox_AutomaticallyCheckForUpdates.AutoSize = true;
            this.Checkbox_AutomaticallyCheckForUpdates.Location = new System.Drawing.Point(578, 59);
            this.Checkbox_AutomaticallyCheckForUpdates.Name = "Checkbox_AutomaticallyCheckForUpdates";
            this.Checkbox_AutomaticallyCheckForUpdates.Size = new System.Drawing.Size(177, 17);
            this.Checkbox_AutomaticallyCheckForUpdates.TabIndex = 8;
            this.Checkbox_AutomaticallyCheckForUpdates.Text = "Automatically check for updates";
            this.Checkbox_AutomaticallyCheckForUpdates.UseVisualStyleBackColor = true;
            this.Checkbox_AutomaticallyCheckForUpdates.CheckedChanged += new System.EventHandler(this.Checkbox_AutomaticallyCheckForUpdates_CheckedChanged);
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.Label_CurrentVersion);
            this.panel4.Location = new System.Drawing.Point(543, 34);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(212, 17);
            this.panel4.TabIndex = 7;
            // 
            // Label_CurrentVersion
            // 
            this.Label_CurrentVersion.AutoSize = true;
            this.Label_CurrentVersion.Dock = System.Windows.Forms.DockStyle.Right;
            this.Label_CurrentVersion.Location = new System.Drawing.Point(95, 0);
            this.Label_CurrentVersion.Name = "Label_CurrentVersion";
            this.Label_CurrentVersion.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Label_CurrentVersion.Size = new System.Drawing.Size(117, 13);
            this.Label_CurrentVersion.TabIndex = 6;
            this.Label_CurrentVersion.Text = "Current version: vX.Y.Z";
            this.Label_CurrentVersion.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Button_CheckUpdates
            // 
            this.Button_CheckUpdates.Location = new System.Drawing.Point(646, 5);
            this.Button_CheckUpdates.Name = "Button_CheckUpdates";
            this.Button_CheckUpdates.Size = new System.Drawing.Size(109, 23);
            this.Button_CheckUpdates.TabIndex = 5;
            this.Button_CheckUpdates.Text = "Check for updates";
            this.Button_CheckUpdates.UseVisualStyleBackColor = true;
            this.Button_CheckUpdates.Click += new System.EventHandler(this.Button_CheckUpdates_Click);
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Location = new System.Drawing.Point(3, 82);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(756, 350);
            this.tabControl2.TabIndex = 4;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.WebLinkJson);
            this.tabPage5.Controls.Add(this.WebLinkIlyaki);
            this.tabPage5.Controls.Add(this.LabelHandleSearch);
            this.tabPage5.Controls.Add(this.WebLinkEasyHook);
            this.tabPage5.Controls.Add(this.WebLinkAHKInterop);
            this.tabPage5.Controls.Add(this.WebLinkAHK);
            this.tabPage5.Controls.Add(this.WebLinkAHKDll);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(748, 324);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Credits";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // LabelHandleSearch
            // 
            this.LabelHandleSearch.AutoSize = true;
            this.LabelHandleSearch.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LabelHandleSearch.Location = new System.Drawing.Point(6, 115);
            this.LabelHandleSearch.Name = "LabelHandleSearch";
            this.LabelHandleSearch.Size = new System.Drawing.Size(581, 15);
            this.LabelHandleSearch.TabIndex = 5;
            this.LabelHandleSearch.Text = "Source engine unlocker handle search: Zoltan Csizmadia, zoltan_csizmadia@yahoo.co" +
    "m";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.GroupBoxLicense);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(748, 324);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "License";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // GroupBoxLicense
            // 
            this.GroupBoxLicense.Controls.Add(this.label1);
            this.GroupBoxLicense.Location = new System.Drawing.Point(6, 6);
            this.GroupBoxLicense.Name = "GroupBoxLicense";
            this.GroupBoxLicense.Size = new System.Drawing.Size(736, 312);
            this.GroupBoxLicense.TabIndex = 6;
            this.GroupBoxLicense.TabStop = false;
            this.GroupBoxLicense.Text = "Universal Split Screen license";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(500, 273);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.ReshowDelay = 100;
            // 
            // FileDialog_FindWindowHook
            // 
            this.FileDialog_FindWindowHook.Filter = "Executable files|*.exe";
            this.FileDialog_FindWindowHook.Title = "Select game executable";
            // 
            // Panel_Splitscreen4Players
            // 
            this.Panel_Splitscreen4Players.Controls.Add(this.Button_SplitscreenBottomRight);
            this.Panel_Splitscreen4Players.Controls.Add(this.Button_SplitscreenTopRight);
            this.Panel_Splitscreen4Players.Controls.Add(this.Button_SplitscreenBottomLeft);
            this.Panel_Splitscreen4Players.Controls.Add(this.Button_SplitscreenTopLeft);
            this.Panel_Splitscreen4Players.Location = new System.Drawing.Point(10, 65);
            this.Panel_Splitscreen4Players.Name = "Panel_Splitscreen4Players";
            this.Panel_Splitscreen4Players.Size = new System.Drawing.Size(150, 85);
            this.Panel_Splitscreen4Players.TabIndex = 39;
            this.Panel_Splitscreen4Players.Visible = false;
            // 
            // Button_SplitscreenBottomLeft
            // 
            this.Button_SplitscreenBottomLeft.Location = new System.Drawing.Point(0, 45);
            this.Button_SplitscreenBottomLeft.Name = "Button_SplitscreenBottomLeft";
            this.Button_SplitscreenBottomLeft.Size = new System.Drawing.Size(75, 40);
            this.Button_SplitscreenBottomLeft.TabIndex = 29;
            this.Button_SplitscreenBottomLeft.Text = "Bottom\r\nLeft";
            this.Button_SplitscreenBottomLeft.UseVisualStyleBackColor = true;
            this.Button_SplitscreenBottomLeft.Click += new System.EventHandler(this.Button_SplitscreenBottomLeft_Click);
            // 
            // Button_SplitscreenTopLeft
            // 
            this.Button_SplitscreenTopLeft.Location = new System.Drawing.Point(0, 5);
            this.Button_SplitscreenTopLeft.Name = "Button_SplitscreenTopLeft";
            this.Button_SplitscreenTopLeft.Size = new System.Drawing.Size(75, 40);
            this.Button_SplitscreenTopLeft.TabIndex = 28;
            this.Button_SplitscreenTopLeft.Text = "Top Left";
            this.Button_SplitscreenTopLeft.UseVisualStyleBackColor = true;
            this.Button_SplitscreenTopLeft.Click += new System.EventHandler(this.Button_SplitscreenTopLeft_Click);
            // 
            // Button_SplitscreenBottomRight
            // 
            this.Button_SplitscreenBottomRight.Location = new System.Drawing.Point(75, 45);
            this.Button_SplitscreenBottomRight.Name = "Button_SplitscreenBottomRight";
            this.Button_SplitscreenBottomRight.Size = new System.Drawing.Size(75, 40);
            this.Button_SplitscreenBottomRight.TabIndex = 31;
            this.Button_SplitscreenBottomRight.Text = "Bottom Right";
            this.Button_SplitscreenBottomRight.UseVisualStyleBackColor = true;
            this.Button_SplitscreenBottomRight.Click += new System.EventHandler(this.Button_SplitscreenBottomRight_Click);
            // 
            // Button_SplitscreenTopRight
            // 
            this.Button_SplitscreenTopRight.Location = new System.Drawing.Point(75, 5);
            this.Button_SplitscreenTopRight.Name = "Button_SplitscreenTopRight";
            this.Button_SplitscreenTopRight.Size = new System.Drawing.Size(75, 40);
            this.Button_SplitscreenTopRight.TabIndex = 30;
            this.Button_SplitscreenTopRight.Text = "Top Right";
            this.Button_SplitscreenTopRight.UseVisualStyleBackColor = true;
            this.Button_SplitscreenTopRight.Click += new System.EventHandler(this.Button_SplitscreenTopRight_Click);
            // 
            // RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart
            // 
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.AutoSize = true;
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.Location = new System.Drawing.Point(214, 97);
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.Name = "RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart";
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.RefType = null;
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.Size = new System.Drawing.Size(82, 17);
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.TabIndex = 24;
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.Text = "Only at start";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart, "Send the activation message only for a brief amount of time when splitscreen star" +
        "ts");
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.UseVisualStyleBackColor = true;
            this.RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart.Visible = false;
            // 
            // RefCheckbox_SystemMouseToMiddle
            // 
            this.RefCheckbox_SystemMouseToMiddle.AutoSize = true;
            this.RefCheckbox_SystemMouseToMiddle.Location = new System.Drawing.Point(3, 212);
            this.RefCheckbox_SystemMouseToMiddle.Name = "RefCheckbox_SystemMouseToMiddle";
            this.RefCheckbox_SystemMouseToMiddle.RefType = null;
            this.RefCheckbox_SystemMouseToMiddle.Size = new System.Drawing.Size(221, 17);
            this.RefCheckbox_SystemMouseToMiddle.TabIndex = 23;
            this.RefCheckbox_SystemMouseToMiddle.Text = "Set system mouse to middle of the screen";
            this.toolTip1.SetToolTip(this.RefCheckbox_SystemMouseToMiddle, "Some games works better when the system mouse is in the middle of the screen (e.g" +
        ". Rift)");
            this.RefCheckbox_SystemMouseToMiddle.UseVisualStyleBackColor = true;
            // 
            // RefTextbox_AutofillHandleName
            // 
            this.RefTextbox_AutofillHandleName.Location = new System.Drawing.Point(4, 343);
            this.RefTextbox_AutofillHandleName.Name = "RefTextbox_AutofillHandleName";
            this.RefTextbox_AutofillHandleName.RefType = null;
            this.RefTextbox_AutofillHandleName.Size = new System.Drawing.Size(175, 20);
            this.RefTextbox_AutofillHandleName.TabIndex = 21;
            this.toolTip1.SetToolTip(this.RefTextbox_AutofillHandleName, "The text in here will automatically be filled in the handle unlocker textbox when" +
        " the preset is loaded.");
            // 
            // RefCheckbox_SendScrollwheel
            // 
            this.RefCheckbox_SendScrollwheel.AutoSize = true;
            this.RefCheckbox_SendScrollwheel.Location = new System.Drawing.Point(3, 166);
            this.RefCheckbox_SendScrollwheel.Name = "RefCheckbox_SendScrollwheel";
            this.RefCheckbox_SendScrollwheel.RefType = null;
            this.RefCheckbox_SendScrollwheel.Size = new System.Drawing.Size(109, 17);
            this.RefCheckbox_SendScrollwheel.TabIndex = 20;
            this.RefCheckbox_SendScrollwheel.Text = "Send scroll wheel";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendScrollwheel, "Sends scroll wheel messages to the game.\r\nThis can conflict with some games.\r\nUse" +
        " this if scroll wheel input is not working.");
            this.RefCheckbox_SendScrollwheel.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_DrawMouse
            // 
            this.RefCheckbox_DrawMouse.AutoSize = true;
            this.RefCheckbox_DrawMouse.Location = new System.Drawing.Point(3, 189);
            this.RefCheckbox_DrawMouse.Name = "RefCheckbox_DrawMouse";
            this.RefCheckbox_DrawMouse.RefType = null;
            this.RefCheckbox_DrawMouse.Size = new System.Drawing.Size(85, 17);
            this.RefCheckbox_DrawMouse.TabIndex = 19;
            this.RefCheckbox_DrawMouse.Text = "Draw mouse";
            this.toolTip1.SetToolTip(this.RefCheckbox_DrawMouse, resources.GetString("RefCheckbox_DrawMouse.ToolTip"));
            this.RefCheckbox_DrawMouse.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_RefreshWindowBoundsOnMouseClick
            // 
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.AutoSize = true;
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.Location = new System.Drawing.Point(3, 143);
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.Name = "RefCheckbox_RefreshWindowBoundsOnMouseClick";
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.RefType = null;
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.Size = new System.Drawing.Size(214, 17);
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.TabIndex = 18;
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.Text = "Refresh window bounds on mouse click";
            this.toolTip1.SetToolTip(this.RefCheckbox_RefreshWindowBoundsOnMouseClick, "This program will bound the fake mouse cursor to the window bounds of the game.\r\n" +
        "If you resize the game window, the window bounds are only refreshed with this op" +
        "tion.");
            this.RefCheckbox_RefreshWindowBoundsOnMouseClick.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_SendFakeWindowFocusMessages
            // 
            this.RefCheckbox_SendFakeWindowFocusMessages.AutoSize = true;
            this.RefCheckbox_SendFakeWindowFocusMessages.Location = new System.Drawing.Point(3, 120);
            this.RefCheckbox_SendFakeWindowFocusMessages.Name = "RefCheckbox_SendFakeWindowFocusMessages";
            this.RefCheckbox_SendFakeWindowFocusMessages.RefType = null;
            this.RefCheckbox_SendFakeWindowFocusMessages.Size = new System.Drawing.Size(193, 17);
            this.RefCheckbox_SendFakeWindowFocusMessages.TabIndex = 17;
            this.RefCheckbox_SendFakeWindowFocusMessages.Text = "Send fake window focus messages";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendFakeWindowFocusMessages, "Similar to send fake window activate messages. \r\nTry this if the other doesn\'t wo" +
        "rk.\r\nVery few games will be affected by this.");
            this.RefCheckbox_SendFakeWindowFocusMessages.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_SendFakeWindowActivateMessages
            // 
            this.RefCheckbox_SendFakeWindowActivateMessages.AutoSize = true;
            this.RefCheckbox_SendFakeWindowActivateMessages.Location = new System.Drawing.Point(3, 97);
            this.RefCheckbox_SendFakeWindowActivateMessages.Name = "RefCheckbox_SendFakeWindowActivateMessages";
            this.RefCheckbox_SendFakeWindowActivateMessages.RefType = null;
            this.RefCheckbox_SendFakeWindowActivateMessages.Size = new System.Drawing.Size(205, 17);
            this.RefCheckbox_SendFakeWindowActivateMessages.TabIndex = 16;
            this.RefCheckbox_SendFakeWindowActivateMessages.Text = "Send fake window activate messages";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendFakeWindowActivateMessages, "Repeatedly sends windows activate messages to the game.\r\nThis can trick the game " +
        "into thinking it is the foreground window,\r\nwhich may mean it will start listeni" +
        "ng for input.");
            this.RefCheckbox_SendFakeWindowActivateMessages.UseVisualStyleBackColor = true;
            this.RefCheckbox_SendFakeWindowActivateMessages.CheckedChanged += new System.EventHandler(this.RefCheckbox_SendFakeWindowActivateMessages_CheckedChanged);
            // 
            // RefCheckbox_SendNormalKeyboardInput
            // 
            this.RefCheckbox_SendNormalKeyboardInput.AutoSize = true;
            this.RefCheckbox_SendNormalKeyboardInput.Location = new System.Drawing.Point(3, 74);
            this.RefCheckbox_SendNormalKeyboardInput.Name = "RefCheckbox_SendNormalKeyboardInput";
            this.RefCheckbox_SendNormalKeyboardInput.RefType = null;
            this.RefCheckbox_SendNormalKeyboardInput.Size = new System.Drawing.Size(158, 17);
            this.RefCheckbox_SendNormalKeyboardInput.TabIndex = 15;
            this.RefCheckbox_SendNormalKeyboardInput.Text = "Send normal keyboard input";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendNormalKeyboardInput, "Sends normal (not raw input) mouse messages to the target game.\r\nRequired for key" +
        "board input to function in most games.");
            this.RefCheckbox_SendNormalKeyboardInput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_SendNormalMouseInput
            // 
            this.RefCheckbox_SendNormalMouseInput.AutoSize = true;
            this.RefCheckbox_SendNormalMouseInput.Location = new System.Drawing.Point(3, 51);
            this.RefCheckbox_SendNormalMouseInput.Name = "RefCheckbox_SendNormalMouseInput";
            this.RefCheckbox_SendNormalMouseInput.RefType = null;
            this.RefCheckbox_SendNormalMouseInput.Size = new System.Drawing.Size(145, 17);
            this.RefCheckbox_SendNormalMouseInput.TabIndex = 14;
            this.RefCheckbox_SendNormalMouseInput.Text = "Send normal mouse input";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendNormalMouseInput, "Sends mouse input messages to the target game.\r\nUse this if a game is not respond" +
        "ing to mouse input in menus.\r\nThis is also required in most 2D games, or any gam" +
        "e that uses the windows mouse cursor.");
            this.RefCheckbox_SendNormalMouseInput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_SendRawKeyboardInput
            // 
            this.RefCheckbox_SendRawKeyboardInput.AutoSize = true;
            this.RefCheckbox_SendRawKeyboardInput.Location = new System.Drawing.Point(3, 28);
            this.RefCheckbox_SendRawKeyboardInput.Name = "RefCheckbox_SendRawKeyboardInput";
            this.RefCheckbox_SendRawKeyboardInput.RefType = null;
            this.RefCheckbox_SendRawKeyboardInput.Size = new System.Drawing.Size(144, 17);
            this.RefCheckbox_SendRawKeyboardInput.TabIndex = 13;
            this.RefCheckbox_SendRawKeyboardInput.Text = "Send raw keyboard input";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendRawKeyboardInput, resources.GetString("RefCheckbox_SendRawKeyboardInput.ToolTip"));
            this.RefCheckbox_SendRawKeyboardInput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_SendRawMouseInput
            // 
            this.RefCheckbox_SendRawMouseInput.AutoSize = true;
            this.RefCheckbox_SendRawMouseInput.Location = new System.Drawing.Point(3, 5);
            this.RefCheckbox_SendRawMouseInput.Name = "RefCheckbox_SendRawMouseInput";
            this.RefCheckbox_SendRawMouseInput.RefType = null;
            this.RefCheckbox_SendRawMouseInput.Size = new System.Drawing.Size(131, 17);
            this.RefCheckbox_SendRawMouseInput.TabIndex = 12;
            this.RefCheckbox_SendRawMouseInput.Text = "Send raw mouse input";
            this.toolTip1.SetToolTip(this.RefCheckbox_SendRawMouseInput, resources.GetString("RefCheckbox_SendRawMouseInput.ToolTip"));
            this.RefCheckbox_SendRawMouseInput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_GetKeyboardState
            // 
            this.RefCheckbox_Hook_GetKeyboardState.AutoSize = true;
            this.RefCheckbox_Hook_GetKeyboardState.Location = new System.Drawing.Point(9, 274);
            this.RefCheckbox_Hook_GetKeyboardState.Name = "RefCheckbox_Hook_GetKeyboardState";
            this.RefCheckbox_Hook_GetKeyboardState.RefType = null;
            this.RefCheckbox_Hook_GetKeyboardState.Size = new System.Drawing.Size(142, 17);
            this.RefCheckbox_Hook_GetKeyboardState.TabIndex = 25;
            this.RefCheckbox_Hook_GetKeyboardState.Text = "Hook GetKeyboardState";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_GetKeyboardState, resources.GetString("RefCheckbox_Hook_GetKeyboardState.ToolTip"));
            this.RefCheckbox_Hook_GetKeyboardState.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_Dinput
            // 
            this.RefCheckbox_Hook_Dinput.AutoSize = true;
            this.RefCheckbox_Hook_Dinput.Location = new System.Drawing.Point(9, 366);
            this.RefCheckbox_Hook_Dinput.Name = "RefCheckbox_Hook_Dinput";
            this.RefCheckbox_Hook_Dinput.RefType = null;
            this.RefCheckbox_Hook_Dinput.Size = new System.Drawing.Size(155, 17);
            this.RefCheckbox_Hook_Dinput.TabIndex = 24;
            this.RefCheckbox_Hook_Dinput.Text = "DInput to XInput translation";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_Dinput, resources.GetString("RefCheckbox_Hook_Dinput.ToolTip"));
            this.RefCheckbox_Hook_Dinput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_UpdateAbsoluteFlagInMouseMessage
            // 
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.AutoSize = true;
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.Location = new System.Drawing.Point(9, 215);
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.Name = "RefCheckbox_UpdateAbsoluteFlagInMouseMessage";
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.RefType = null;
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.Size = new System.Drawing.Size(237, 30);
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.TabIndex = 21;
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.Text = "Update absolute position flag in mouse move\r\n messages (for legacy input)";
            this.toolTip1.SetToolTip(this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage, resources.GetString("RefCheckbox_UpdateAbsoluteFlagInMouseMessage.ToolTip"));
            this.RefCheckbox_UpdateAbsoluteFlagInMouseMessage.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_MouseVisibility
            // 
            this.RefCheckbox_Hook_MouseVisibility.AutoSize = true;
            this.RefCheckbox_Hook_MouseVisibility.Location = new System.Drawing.Point(9, 320);
            this.RefCheckbox_Hook_MouseVisibility.Name = "RefCheckbox_Hook_MouseVisibility";
            this.RefCheckbox_Hook_MouseVisibility.RefType = null;
            this.RefCheckbox_Hook_MouseVisibility.Size = new System.Drawing.Size(124, 17);
            this.RefCheckbox_Hook_MouseVisibility.TabIndex = 23;
            this.RefCheckbox_Hook_MouseVisibility.Text = "Hook mouse visibility";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_MouseVisibility, resources.GetString("RefCheckbox_Hook_MouseVisibility.ToolTip"));
            this.RefCheckbox_Hook_MouseVisibility.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_UseLegacyInput
            // 
            this.RefCheckbox_Hook_UseLegacyInput.AutoSize = true;
            this.RefCheckbox_Hook_UseLegacyInput.Location = new System.Drawing.Point(9, 192);
            this.RefCheckbox_Hook_UseLegacyInput.Name = "RefCheckbox_Hook_UseLegacyInput";
            this.RefCheckbox_Hook_UseLegacyInput.RefType = null;
            this.RefCheckbox_Hook_UseLegacyInput.Size = new System.Drawing.Size(194, 17);
            this.RefCheckbox_Hook_UseLegacyInput.TabIndex = 22;
            this.RefCheckbox_Hook_UseLegacyInput.Text = "Use legacy input (see requirements)";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_UseLegacyInput, resources.GetString("RefCheckbox_Hook_UseLegacyInput.ToolTip"));
            this.RefCheckbox_Hook_UseLegacyInput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_XInput
            // 
            this.RefCheckbox_Hook_XInput.AutoSize = true;
            this.RefCheckbox_Hook_XInput.Location = new System.Drawing.Point(9, 343);
            this.RefCheckbox_Hook_XInput.Name = "RefCheckbox_Hook_XInput";
            this.RefCheckbox_Hook_XInput.RefType = null;
            this.RefCheckbox_Hook_XInput.Size = new System.Drawing.Size(153, 17);
            this.RefCheckbox_Hook_XInput.TabIndex = 21;
            this.RefCheckbox_Hook_XInput.Text = "Hook XInput for gamepads";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_XInput, resources.GetString("RefCheckbox_Hook_XInput.ToolTip"));
            this.RefCheckbox_Hook_XInput.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_SetCursorPos
            // 
            this.RefCheckbox_Hook_SetCursorPos.AutoSize = true;
            this.RefCheckbox_Hook_SetCursorPos.Location = new System.Drawing.Point(9, 169);
            this.RefCheckbox_Hook_SetCursorPos.Name = "RefCheckbox_Hook_SetCursorPos";
            this.RefCheckbox_Hook_SetCursorPos.RefType = null;
            this.RefCheckbox_Hook_SetCursorPos.Size = new System.Drawing.Size(119, 17);
            this.RefCheckbox_Hook_SetCursorPos.TabIndex = 20;
            this.RefCheckbox_Hook_SetCursorPos.Text = "Hook SetCursorPos";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_SetCursorPos, "Overloads the SetCursorPos function, so a game can only move its fake cursor and " +
        "not the real cursor.");
            this.RefCheckbox_Hook_SetCursorPos.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_GetKeyState
            // 
            this.RefCheckbox_Hook_GetKeyState.AutoSize = true;
            this.RefCheckbox_Hook_GetKeyState.Location = new System.Drawing.Point(9, 251);
            this.RefCheckbox_Hook_GetKeyState.Name = "RefCheckbox_Hook_GetKeyState";
            this.RefCheckbox_Hook_GetKeyState.RefType = null;
            this.RefCheckbox_Hook_GetKeyState.Size = new System.Drawing.Size(168, 17);
            this.RefCheckbox_Hook_GetKeyState.TabIndex = 20;
            this.RefCheckbox_Hook_GetKeyState.Text = "Hook GetKeyState for all keys";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_GetKeyState, "This hook will fix a bug in Borderlands 2 where the player would seemingly random" +
        "ly stop moving.");
            this.RefCheckbox_Hook_GetKeyState.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_GetAsyncKeyState
            // 
            this.RefCheckbox_Hook_GetAsyncKeyState.AutoSize = true;
            this.RefCheckbox_Hook_GetAsyncKeyState.Location = new System.Drawing.Point(9, 297);
            this.RefCheckbox_Hook_GetAsyncKeyState.Name = "RefCheckbox_Hook_GetAsyncKeyState";
            this.RefCheckbox_Hook_GetAsyncKeyState.RefType = null;
            this.RefCheckbox_Hook_GetAsyncKeyState.Size = new System.Drawing.Size(144, 17);
            this.RefCheckbox_Hook_GetAsyncKeyState.TabIndex = 20;
            this.RefCheckbox_Hook_GetAsyncKeyState.Text = "Hook GetAsyncKeyState";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_GetAsyncKeyState, resources.GetString("RefCheckbox_Hook_GetAsyncKeyState.ToolTip"));
            this.RefCheckbox_Hook_GetAsyncKeyState.UseVisualStyleBackColor = true;
            this.RefCheckbox_Hook_GetAsyncKeyState.CheckedChanged += new System.EventHandler(this.RefCheckbox_Hook_GetAsyncKeyState_CheckedChanged);
            // 
            // RefCheckbox_Hook_GetCursorPos
            // 
            this.RefCheckbox_Hook_GetCursorPos.AutoSize = true;
            this.RefCheckbox_Hook_GetCursorPos.Location = new System.Drawing.Point(9, 146);
            this.RefCheckbox_Hook_GetCursorPos.Name = "RefCheckbox_Hook_GetCursorPos";
            this.RefCheckbox_Hook_GetCursorPos.RefType = null;
            this.RefCheckbox_Hook_GetCursorPos.Size = new System.Drawing.Size(120, 17);
            this.RefCheckbox_Hook_GetCursorPos.TabIndex = 20;
            this.RefCheckbox_Hook_GetCursorPos.Text = "Hook GetCursorPos";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_GetCursorPos, "Tricks the game into using the fake mouse cursor if it calls GetCursorPos.\r\nUse t" +
        "his if mouse input is not working correctly.");
            this.RefCheckbox_Hook_GetCursorPos.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_GetForegroundWindow
            // 
            this.RefCheckbox_Hook_GetForegroundWindow.AutoSize = true;
            this.RefCheckbox_Hook_GetForegroundWindow.Location = new System.Drawing.Point(9, 123);
            this.RefCheckbox_Hook_GetForegroundWindow.Name = "RefCheckbox_Hook_GetForegroundWindow";
            this.RefCheckbox_Hook_GetForegroundWindow.RefType = null;
            this.RefCheckbox_Hook_GetForegroundWindow.Size = new System.Drawing.Size(165, 17);
            this.RefCheckbox_Hook_GetForegroundWindow.TabIndex = 20;
            this.RefCheckbox_Hook_GetForegroundWindow.Text = "Hook GetForegroundWindow";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_GetForegroundWindow, "Tricks the game into thinking it is the foreground window.\r\nUse this if a game is" +
        " not responding to input.");
            this.RefCheckbox_Hook_GetForegroundWindow.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_FilterMouseInputMessages
            // 
            this.RefCheckbox_Hook_FilterMouseInputMessages.AutoSize = true;
            this.RefCheckbox_Hook_FilterMouseInputMessages.Location = new System.Drawing.Point(9, 100);
            this.RefCheckbox_Hook_FilterMouseInputMessages.Name = "RefCheckbox_Hook_FilterMouseInputMessages";
            this.RefCheckbox_Hook_FilterMouseInputMessages.RefType = null;
            this.RefCheckbox_Hook_FilterMouseInputMessages.Size = new System.Drawing.Size(228, 17);
            this.RefCheckbox_Hook_FilterMouseInputMessages.TabIndex = 20;
            this.RefCheckbox_Hook_FilterMouseInputMessages.Text = "Filter mouse input messages from Windows";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_FilterMouseInputMessages, "Filters out mouse input messages sent by Windows, but not by this program.\r\nUse t" +
        "his if a game is not tricked into using the fake mouse cursor.");
            this.RefCheckbox_Hook_FilterMouseInputMessages.UseVisualStyleBackColor = true;
            // 
            // RefCheckbox_Hook_FilterRawInput
            // 
            this.RefCheckbox_Hook_FilterRawInput.AutoSize = true;
            this.RefCheckbox_Hook_FilterRawInput.Location = new System.Drawing.Point(9, 77);
            this.RefCheckbox_Hook_FilterRawInput.Name = "RefCheckbox_Hook_FilterRawInput";
            this.RefCheckbox_Hook_FilterRawInput.RefType = null;
            this.RefCheckbox_Hook_FilterRawInput.Size = new System.Drawing.Size(214, 17);
            this.RefCheckbox_Hook_FilterRawInput.TabIndex = 20;
            this.RefCheckbox_Hook_FilterRawInput.Text = "Filter raw input messages from Windows";
            this.toolTip1.SetToolTip(this.RefCheckbox_Hook_FilterRawInput, "Filters raw mouse input so only one device is used.\r\nUse this if the game is resp" +
        "onding to input from multiple mice.");
            this.RefCheckbox_Hook_FilterRawInput.UseVisualStyleBackColor = true;
            // 
            // refTextbox_LeftOffset
            // 
            this.refTextbox_LeftOffset.Location = new System.Drawing.Point(190, 70);
            this.refTextbox_LeftOffset.Name = "refTextbox_LeftOffset";
            this.refTextbox_LeftOffset.RefType = null;
            this.refTextbox_LeftOffset.Size = new System.Drawing.Size(35, 20);
            this.refTextbox_LeftOffset.TabIndex = 30;
            // 
            // refTextbox_TopOffset
            // 
            this.refTextbox_TopOffset.Location = new System.Drawing.Point(190, 100);
            this.refTextbox_TopOffset.Name = "refTextbox_TopOffset";
            this.refTextbox_TopOffset.RefType = null;
            this.refTextbox_TopOffset.Size = new System.Drawing.Size(35, 20);
            this.refTextbox_TopOffset.TabIndex = 31;
            // 
            // refTextbox_BorderExtraPadding
            // 
            this.refTextbox_BorderExtraPadding.Location = new System.Drawing.Point(190, 130);
            this.refTextbox_BorderExtraPadding.Name = "refTextbox_BorderExtraPadding";
            this.refTextbox_BorderExtraPadding.RefType = null;
            this.refTextbox_BorderExtraPadding.Size = new System.Drawing.Size(35, 20);
            this.refTextbox_BorderExtraPadding.TabIndex = 32;
            // 
            // WebLinkLabel_Goldberg
            // 
            this.WebLinkLabel_Goldberg.AutoSize = true;
            this.WebLinkLabel_Goldberg.LinkArea = new System.Windows.Forms.LinkArea(82, 4);
            this.WebLinkLabel_Goldberg.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkLabel_Goldberg.Location = new System.Drawing.Point(3, 9);
            this.WebLinkLabel_Goldberg.Name = "WebLinkLabel_Goldberg";
            this.WebLinkLabel_Goldberg.Size = new System.Drawing.Size(416, 30);
            this.WebLinkLabel_Goldberg.TabIndex = 0;
            this.WebLinkLabel_Goldberg.TabStop = true;
            this.WebLinkLabel_Goldberg.Text = "You can use this page to quickly change your Goldberg username and Steam ID.\r\nSee" +
    " here on how to set up the Goldberg Emulator.";
            this.WebLinkLabel_Goldberg.Url = "https://universalsplitscreen.github.io/docs/goldberg/";
            this.WebLinkLabel_Goldberg.UseCompatibleTextRendering = true;
            // 
            // WebLinkJson
            // 
            this.WebLinkJson.AutoSize = true;
            this.WebLinkJson.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkJson.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkJson.Location = new System.Drawing.Point(6, 139);
            this.WebLinkJson.Name = "WebLinkJson";
            this.WebLinkJson.Size = new System.Drawing.Size(63, 15);
            this.WebLinkJson.TabIndex = 6;
            this.WebLinkJson.TabStop = true;
            this.WebLinkJson.Text = "Json.NET";
            this.WebLinkJson.Url = "https://www.newtonsoft.com/json";
            // 
            // WebLinkIlyaki
            // 
            this.WebLinkIlyaki.AutoSize = true;
            this.WebLinkIlyaki.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkIlyaki.LinkArea = new System.Windows.Forms.LinkArea(30, 37);
            this.WebLinkIlyaki.LinkBehavior = System.Windows.Forms.LinkBehavior.NeverUnderline;
            this.WebLinkIlyaki.Location = new System.Drawing.Point(6, 3);
            this.WebLinkIlyaki.Name = "WebLinkIlyaki";
            this.WebLinkIlyaki.Size = new System.Drawing.Size(277, 20);
            this.WebLinkIlyaki.TabIndex = 0;
            this.WebLinkIlyaki.TabStop = true;
            this.WebLinkIlyaki.Text = "Universal Split Screen author: Ilyaki";
            this.WebLinkIlyaki.Url = "https://github.com/Ilyaki";
            this.WebLinkIlyaki.UseCompatibleTextRendering = true;
            // 
            // WebLinkEasyHook
            // 
            this.WebLinkEasyHook.AutoSize = true;
            this.WebLinkEasyHook.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkEasyHook.LinkArea = new System.Windows.Forms.LinkArea(0, 8);
            this.WebLinkEasyHook.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkEasyHook.Location = new System.Drawing.Point(6, 32);
            this.WebLinkEasyHook.Name = "WebLinkEasyHook";
            this.WebLinkEasyHook.Size = new System.Drawing.Size(336, 20);
            this.WebLinkEasyHook.TabIndex = 1;
            this.WebLinkEasyHook.TabStop = true;
            this.WebLinkEasyHook.Text = "EasyHook: Christoph Husse and Justin Stenning";
            this.WebLinkEasyHook.Url = "https://easyhook.github.io/";
            this.WebLinkEasyHook.UseCompatibleTextRendering = true;
            // 
            // WebLinkAHKInterop
            // 
            this.WebLinkAHKInterop.AutoSize = true;
            this.WebLinkAHKInterop.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkAHKInterop.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkAHKInterop.Location = new System.Drawing.Point(6, 91);
            this.WebLinkAHKInterop.Name = "WebLinkAHKInterop";
            this.WebLinkAHKInterop.Size = new System.Drawing.Size(126, 15);
            this.WebLinkAHKInterop.TabIndex = 4;
            this.WebLinkAHKInterop.TabStop = true;
            this.WebLinkAHKInterop.Text = "AutoHotKey.Introp";
            this.WebLinkAHKInterop.Url = "https://github.com/amazing-andrew/AutoHotkey.Interop";
            // 
            // WebLinkAHK
            // 
            this.WebLinkAHK.AutoSize = true;
            this.WebLinkAHK.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkAHK.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkAHK.Location = new System.Drawing.Point(6, 61);
            this.WebLinkAHK.Name = "WebLinkAHK";
            this.WebLinkAHK.Size = new System.Drawing.Size(77, 15);
            this.WebLinkAHK.TabIndex = 2;
            this.WebLinkAHK.TabStop = true;
            this.WebLinkAHK.Text = "AutoHotKey";
            this.WebLinkAHK.Url = "https://github.com/Lexikos/AutoHotkey_L";
            // 
            // WebLinkAHKDll
            // 
            this.WebLinkAHKDll.AutoSize = true;
            this.WebLinkAHKDll.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkAHKDll.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkAHKDll.Location = new System.Drawing.Point(6, 76);
            this.WebLinkAHKDll.Name = "WebLinkAHKDll";
            this.WebLinkAHKDll.Size = new System.Drawing.Size(49, 15);
            this.WebLinkAHKDll.TabIndex = 3;
            this.WebLinkAHKDll.TabStop = true;
            this.WebLinkAHKDll.Text = "ahkdll";
            this.WebLinkAHKDll.Url = "https://github.com/HotKeyIt/ahkdll";
            // 
            // WebLinkWebsite
            // 
            this.WebLinkWebsite.AutoSize = true;
            this.WebLinkWebsite.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.WebLinkWebsite.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline;
            this.WebLinkWebsite.Location = new System.Drawing.Point(3, 9);
            this.WebLinkWebsite.Name = "WebLinkWebsite";
            this.WebLinkWebsite.Size = new System.Drawing.Size(217, 15);
            this.WebLinkWebsite.TabIndex = 2;
            this.WebLinkWebsite.TabStop = true;
            this.WebLinkWebsite.Text = "Universal Split Screen website";
            this.WebLinkWebsite.Url = "https://universalsplitscreen.github.io/";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 484);
            this.Controls.Add(this.tabControl1);
            this.Cursor = System.Windows.Forms.Cursors.Default;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Universal Split Screen";
            this.TopMost = true;
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.activeWindowPanel.ResumeLayout(false);
            this.activeWindowPanel.PerformLayout();
            this.groupBoxHandleUnlocker.ResumeLayout(false);
            this.groupBoxHandleUnlocker.PerformLayout();
            this.GroupBox_Utilities.ResumeLayout(false);
            this.GamePadGroupBox.ResumeLayout(false);
            this.GamePadGroupBox.PerformLayout();
            this.windowTitleBox.ResumeLayout(false);
            this.windowTitleBox.PerformLayout();
            this.keyboardBox.ResumeLayout(false);
            this.keyboardBox.PerformLayout();
            this.flowLayoutPanel2.ResumeLayout(false);
            this.flowLayoutPanel2.PerformLayout();
            this.mouseBox.ResumeLayout(false);
            this.flowLayoutPanel1.ResumeLayout(false);
            this.flowLayoutPanel1.PerformLayout();
            this.hwndBox.ResumeLayout(false);
            this.hwndBox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.hooksBox.ResumeLayout(false);
            this.hooksBox.PerformLayout();
            this.tabPage9.ResumeLayout(false);
            this.GroupBox_WindowOptions.ResumeLayout(false);
            this.groupBox_WindowPositionAndOffsets.ResumeLayout(false);
            this.groupBox_WindowPositionAndOffsets.PerformLayout();
            this.Panel_Splitscreen2PlayersHorizontal.ResumeLayout(false);
            this.Panel_Splitscreen2PlayersVertical.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabControl3.ResumeLayout(false);
            this.tabPage7.ResumeLayout(false);
            this.panel8.ResumeLayout(false);
            this.panel8.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel9.ResumeLayout(false);
            this.panel9.PerformLayout();
            this.flowLayoutPanel3.ResumeLayout(false);
            this.flowLayoutPanel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tabPage8.ResumeLayout(false);
            this.tabPage8.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.tabPage6.ResumeLayout(false);
            this.GroupBoxLicense.ResumeLayout(false);
            this.GroupBoxLicense.PerformLayout();
            this.Panel_Splitscreen4Players.ResumeLayout(false);
            this.ResumeLayout(false);

		}

		#endregion
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.GroupBox windowTitleBox;
		private System.Windows.Forms.Label windowTitleLabel;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.GroupBox mouseBox;
		private System.Windows.Forms.Label mouseHandleLabel;
		private System.Windows.Forms.Label mouseHandleStaticLabel;
		private System.Windows.Forms.GroupBox hwndBox;
		private System.Windows.Forms.Label hWndLabel;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
		private System.Windows.Forms.Button attachMouseButton;
		private System.Windows.Forms.Panel activeWindowPanel;
		private System.Windows.Forms.GroupBox keyboardBox;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel2;
		private System.Windows.Forms.Label keyboardHandleStaticLabel;
		private System.Windows.Forms.Label keyboardHandleLabel;
		private System.Windows.Forms.TextBox keyboardSetTextbox;
		private System.Windows.Forms.Button keyboardResetButton;
		private System.Windows.Forms.Button mouseResetButton;
		private System.Windows.Forms.Label keyboardHelpLabel;
		private System.Windows.Forms.Button startButton;
		private System.Windows.Forms.Button stopButton;
		private System.Windows.Forms.Button resetAllButton;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.GroupBox hooksBox;
		private System.Windows.Forms.Label hooksWarningLabel;
		private System.Windows.Forms.Button endButtonSetter;
		private System.Windows.Forms.Panel panel2;
		private System.Windows.Forms.Button buttonOptions_load;
		private System.Windows.Forms.Button buttonOptions_save;
		private System.Windows.Forms.Button buttonOptions_delete;
		private System.Windows.Forms.ComboBox optionsComboBox;
		private System.Windows.Forms.Button buttonOptions_New;
		private RefCheckbox RefCheckbox_SendRawMouseInput;
		private RefCheckbox RefCheckbox_DrawMouse;
		private RefCheckbox RefCheckbox_RefreshWindowBoundsOnMouseClick;
		private RefCheckbox RefCheckbox_SendFakeWindowFocusMessages;
		private RefCheckbox RefCheckbox_SendFakeWindowActivateMessages;
		private RefCheckbox RefCheckbox_SendNormalKeyboardInput;
		private RefCheckbox RefCheckbox_SendNormalMouseInput;
		private RefCheckbox RefCheckbox_SendRawKeyboardInput;
		private RefCheckbox RefCheckbox_Hook_GetKeyState;
		private RefCheckbox RefCheckbox_Hook_GetAsyncKeyState;
		private RefCheckbox RefCheckbox_Hook_GetCursorPos;
		private RefCheckbox RefCheckbox_Hook_GetForegroundWindow;
		private RefCheckbox RefCheckbox_Hook_FilterMouseInputMessages;
		private RefCheckbox RefCheckbox_Hook_FilterRawInput;
		private RefCheckbox RefCheckbox_Hook_SetCursorPos;
		private System.Windows.Forms.GroupBox GamePadGroupBox;
		private System.Windows.Forms.ComboBox ControllerIndexComboBox;
		private System.Windows.Forms.Label ControllerHookNote;
		private System.Windows.Forms.Label ControllerIndexLabel;
		private RefCheckbox RefCheckbox_Hook_XInput;
		private RefCheckbox RefCheckbox_SendScrollwheel;
		private System.Windows.Forms.Button Button_UnlockSourceEngine;
		private System.Windows.Forms.TabPage tabPage3;
		private WebLinkLabel WebLinkWebsite;
		private System.Windows.Forms.Panel panel3;
		private WebLinkLabel WebLinkEasyHook;
		private WebLinkLabel WebLinkIlyaki;
		private WebLinkLabel WebLinkJson;
		private System.Windows.Forms.Label LabelHandleSearch;
		private WebLinkLabel WebLinkAHKInterop;
		private WebLinkLabel WebLinkAHKDll;
		private WebLinkLabel WebLinkAHK;
		private System.Windows.Forms.TabControl tabControl2;
		private System.Windows.Forms.TabPage tabPage5;
		private System.Windows.Forms.TabPage tabPage6;
		private System.Windows.Forms.GroupBox GroupBoxLicense;
		private System.Windows.Forms.Label label1;
		private RefCheckbox RefCheckbox_Hook_UseLegacyInput;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.GroupBox GroupBox_Utilities;
		private System.Windows.Forms.CheckBox CheckBox_Transparency;
		private System.Windows.Forms.Button Button_CheckUpdates;
		private System.Windows.Forms.Label Label_CurrentVersion;
		private System.Windows.Forms.Panel panel4;
		private RefCheckbox RefCheckbox_Hook_MouseVisibility;
		private System.Windows.Forms.GroupBox groupBoxHandleUnlocker;
		private System.Windows.Forms.Button UnlockHandleButton;
		private System.Windows.Forms.Label labelHandleName;
		private System.Windows.Forms.TextBox textBoxHandleName;
		private RefCheckbox RefCheckbox_UpdateAbsoluteFlagInMouseMessage;
		private System.Windows.Forms.Label Label_StopWarning;
		private RefCheckbox RefCheckbox_Hook_Dinput;
		private System.Windows.Forms.Label Label_AutofillHandleName;
		private RefTextbox RefTextbox_AutofillHandleName;
		private System.Windows.Forms.TabPage tabPage4;
		private System.Windows.Forms.TabControl tabControl3;
		private System.Windows.Forms.TabPage tabPage7;
		private System.Windows.Forms.Button Button_BrowseFindWindowHookExe;
		private System.Windows.Forms.Label Label_FindWindowHookExe;
		private System.Windows.Forms.OpenFileDialog FileDialog_FindWindowHook;
		private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel3;
		private System.Windows.Forms.TextBox TextBox_FindWindowHookArgs;
		private System.Windows.Forms.Label Label_FindWindowHookCmdArgsDescriptor;
		private System.Windows.Forms.Button Button_FindWindowHookLaunch;
		private System.Windows.Forms.CheckBox Checkbox_FindWindowHookIs64;
		private System.Windows.Forms.CheckBox CheckBox_StartupHook_Dinput;
		private System.Windows.Forms.CheckBox CheckBox_StartupHook_FindWindow;
		private System.Windows.Forms.Panel panel5;
		private System.Windows.Forms.Label Label_DinputControllerIndex;
		private System.Windows.Forms.ComboBox ComboBox_DinputControllerIndex;
		private System.Windows.Forms.Panel panel6;
		private System.Windows.Forms.TabPage tabPage8;
		private WebLinkLabel WebLinkLabel_Goldberg;
		private System.Windows.Forms.Button Button_Goldberg_ID_Set;
		private System.Windows.Forms.TextBox TextBox_Goldberg_ID;
		private System.Windows.Forms.Button Button_Goldberg_Username_Set;
		private System.Windows.Forms.TextBox TextBox_Goldberg_Username;
		private System.Windows.Forms.Label Label_Goldberg_ID;
		private System.Windows.Forms.Label Label_Goldberg_Username;
		private RefCheckbox RefCheckbox_Hook_GetKeyboardState;
		private System.Windows.Forms.CheckBox Checkbox_AutomaticallyCheckForUpdates;
		private System.Windows.Forms.CheckBox CheckBox_StartupHook_UseAppdataSwitch;
		private System.Windows.Forms.Panel panel8;
		private System.Windows.Forms.CheckBox CheckBox_StartupHook_FindMutexHook;
		private System.Windows.Forms.Panel panel7;
		private System.Windows.Forms.TextBox StartupHook_MutexTargets;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Panel panel9;
		private System.Windows.Forms.ComboBox ComboBox_AppdataSwitch_Selector;
		private RefCheckbox RefCheckbox_SystemMouseToMiddle;
		private RefCheckbox RefCheckbox_SendFakeWindowActivateMessageOnlyAtStart;
		private System.Windows.Forms.Label Label_CurrentWindowTabInstructions;
		private System.Windows.Forms.TabPage tabPage9;
		private System.Windows.Forms.Button Button_ToggleWindowBorders;
		private System.Windows.Forms.Button Button_EnableWindowResize;
		private RefTextbox refTextbox_BorderExtraPadding;
		private RefTextbox refTextbox_TopOffset;
		private RefTextbox refTextbox_LeftOffset;
		private System.Windows.Forms.Label Label_BorderExtraPadding;
		private System.Windows.Forms.Label Label_LeftOffset;
		private System.Windows.Forms.Label Label_SplitscreenOptions;
		private System.Windows.Forms.ComboBox ComboBox_SplitscreenOptions;
		private System.Windows.Forms.GroupBox groupBox_WindowPositionAndOffsets;
		private System.Windows.Forms.Button Button_SplitscreenLeft;
		private System.Windows.Forms.Button Button_SplitscreenRight;
		private System.Windows.Forms.Label Label_TopOffset;
		private System.Windows.Forms.Panel Panel_Splitscreen2PlayersHorizontal;
		private System.Windows.Forms.Button Button_SplitscreenBottom;
		private System.Windows.Forms.Button Button_SplitscreenTop;
		private System.Windows.Forms.Panel Panel_Splitscreen2PlayersVertical;
		private System.Windows.Forms.GroupBox GroupBox_WindowOptions;
		private System.Windows.Forms.Panel Panel_Splitscreen4Players;
		private System.Windows.Forms.Button Button_SplitscreenBottomLeft;
		private System.Windows.Forms.Button Button_SplitscreenTopLeft;
		private System.Windows.Forms.Button Button_SplitscreenBottomRight;
		private System.Windows.Forms.Button Button_SplitscreenTopRight;
	}
}