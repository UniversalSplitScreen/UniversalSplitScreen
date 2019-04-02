namespace UniversalSplitScreen
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
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.startButton = new System.Windows.Forms.Button();
			this.stopButton = new System.Windows.Forms.Button();
			this.resetAllButton = new System.Windows.Forms.Button();
			this.activeWindowPanel = new System.Windows.Forms.Panel();
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
			this.panel1 = new System.Windows.Forms.Panel();
			this.hooksBox = new System.Windows.Forms.GroupBox();
			this.checkBoxHook_getForegroundWindow = new System.Windows.Forms.CheckBox();
			this.checkBoxHook_filterCallWndProc = new System.Windows.Forms.CheckBox();
			this.checkBoxHook_filterWindowsRawInput = new System.Windows.Forms.CheckBox();
			this.hooksWarningLabel = new System.Windows.Forms.Label();
			this.drawMouseCheckbox = new System.Windows.Forms.CheckBox();
			this.drawMouseEveryXmsLabel = new System.Windows.Forms.Label();
			this.drawMouseEveryXmsField = new System.Windows.Forms.NumericUpDown();
			this.refreshWindowBoundsOnLMBCheckbox = new System.Windows.Forms.CheckBox();
			this.send_WM_FOCUS_checkbox = new System.Windows.Forms.CheckBox();
			this.send_WM_ACTIVATE_checkbox = new System.Windows.Forms.CheckBox();
			this.sendNormalKeyboardCheckbox = new System.Windows.Forms.CheckBox();
			this.sendNormalMouseCheckbox = new System.Windows.Forms.CheckBox();
			this.sendRawKeyboardCheckbox = new System.Windows.Forms.CheckBox();
			this.sendRawMouseCheckbox = new System.Windows.Forms.CheckBox();
			this.endButtonSetter = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.activeWindowPanel.SuspendLayout();
			this.windowTitleBox.SuspendLayout();
			this.keyboardBox.SuspendLayout();
			this.flowLayoutPanel2.SuspendLayout();
			this.mouseBox.SuspendLayout();
			this.flowLayoutPanel1.SuspendLayout();
			this.hwndBox.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.panel1.SuspendLayout();
			this.hooksBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.drawMouseEveryXmsField)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.tabPage2);
			this.tabControl1.Location = new System.Drawing.Point(12, 12);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(776, 426);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.startButton);
			this.tabPage1.Controls.Add(this.stopButton);
			this.tabPage1.Controls.Add(this.resetAllButton);
			this.tabPage1.Controls.Add(this.activeWindowPanel);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(768, 400);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "Current window";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// startButton
			// 
			this.startButton.Location = new System.Drawing.Point(559, 371);
			this.startButton.Name = "startButton";
			this.startButton.Size = new System.Drawing.Size(96, 23);
			this.startButton.TabIndex = 10;
			this.startButton.Text = "Start split screen";
			this.startButton.UseVisualStyleBackColor = true;
			this.startButton.Click += new System.EventHandler(this.startButton_Click);
			// 
			// stopButton
			// 
			this.stopButton.Location = new System.Drawing.Point(661, 371);
			this.stopButton.Name = "stopButton";
			this.stopButton.Size = new System.Drawing.Size(101, 23);
			this.stopButton.TabIndex = 9;
			this.stopButton.Text = "Stop split screen";
			this.stopButton.UseVisualStyleBackColor = true;
			this.stopButton.Click += new System.EventHandler(this.stopButton_Click);
			// 
			// resetAllButton
			// 
			this.resetAllButton.Location = new System.Drawing.Point(7, 371);
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
			this.activeWindowPanel.Controls.Add(this.windowTitleBox);
			this.activeWindowPanel.Controls.Add(this.keyboardBox);
			this.activeWindowPanel.Controls.Add(this.mouseBox);
			this.activeWindowPanel.Controls.Add(this.hwndBox);
			this.activeWindowPanel.Location = new System.Drawing.Point(6, 6);
			this.activeWindowPanel.Name = "activeWindowPanel";
			this.activeWindowPanel.Size = new System.Drawing.Size(759, 359);
			this.activeWindowPanel.TabIndex = 7;
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
			this.keyboardBox.Location = new System.Drawing.Point(209, 48);
			this.keyboardBox.Name = "keyboardBox";
			this.keyboardBox.Size = new System.Drawing.Size(200, 132);
			this.keyboardBox.TabIndex = 6;
			this.keyboardBox.TabStop = false;
			this.keyboardBox.Text = "Keyboard";
			// 
			// keyboardHelpLabel
			// 
			this.keyboardHelpLabel.AutoSize = true;
			this.keyboardHelpLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.keyboardHelpLabel.Location = new System.Drawing.Point(7, 55);
			this.keyboardHelpLabel.Name = "keyboardHelpLabel";
			this.keyboardHelpLabel.Size = new System.Drawing.Size(149, 13);
			this.keyboardHelpLabel.TabIndex = 8;
			this.keyboardHelpLabel.Text = "Type here to set the keyboard";
			// 
			// keyboardResetButton
			// 
			this.keyboardResetButton.Location = new System.Drawing.Point(6, 100);
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
			this.keyboardSetTextbox.Location = new System.Drawing.Point(6, 74);
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
			this.mouseBox.Location = new System.Drawing.Point(3, 48);
			this.mouseBox.Name = "mouseBox";
			this.mouseBox.Size = new System.Drawing.Size(200, 132);
			this.mouseBox.TabIndex = 4;
			this.mouseBox.TabStop = false;
			this.mouseBox.Text = "Mouse";
			// 
			// mouseResetButton
			// 
			this.mouseResetButton.Location = new System.Drawing.Point(6, 100);
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
			this.attachMouseButton.Location = new System.Drawing.Point(6, 46);
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
			this.tabPage2.Controls.Add(this.panel1);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(768, 400);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "Options";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// panel1
			// 
			this.panel1.AutoScroll = true;
			this.panel1.Controls.Add(this.endButtonSetter);
			this.panel1.Controls.Add(this.hooksBox);
			this.panel1.Controls.Add(this.drawMouseCheckbox);
			this.panel1.Controls.Add(this.drawMouseEveryXmsLabel);
			this.panel1.Controls.Add(this.drawMouseEveryXmsField);
			this.panel1.Controls.Add(this.refreshWindowBoundsOnLMBCheckbox);
			this.panel1.Controls.Add(this.send_WM_FOCUS_checkbox);
			this.panel1.Controls.Add(this.send_WM_ACTIVATE_checkbox);
			this.panel1.Controls.Add(this.sendNormalKeyboardCheckbox);
			this.panel1.Controls.Add(this.sendNormalMouseCheckbox);
			this.panel1.Controls.Add(this.sendRawKeyboardCheckbox);
			this.panel1.Controls.Add(this.sendRawMouseCheckbox);
			this.panel1.Location = new System.Drawing.Point(7, 7);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(755, 390);
			this.panel1.TabIndex = 0;
			// 
			// hooksBox
			// 
			this.hooksBox.Controls.Add(this.checkBoxHook_getForegroundWindow);
			this.hooksBox.Controls.Add(this.checkBoxHook_filterCallWndProc);
			this.hooksBox.Controls.Add(this.checkBoxHook_filterWindowsRawInput);
			this.hooksBox.Controls.Add(this.hooksWarningLabel);
			this.hooksBox.Location = new System.Drawing.Point(472, 3);
			this.hooksBox.Name = "hooksBox";
			this.hooksBox.Size = new System.Drawing.Size(280, 384);
			this.hooksBox.TabIndex = 10;
			this.hooksBox.TabStop = false;
			this.hooksBox.Text = "Hooks";
			// 
			// checkBoxHook_getForegroundWindow
			// 
			this.checkBoxHook_getForegroundWindow.AutoSize = true;
			this.checkBoxHook_getForegroundWindow.Location = new System.Drawing.Point(9, 121);
			this.checkBoxHook_getForegroundWindow.Name = "checkBoxHook_getForegroundWindow";
			this.checkBoxHook_getForegroundWindow.Size = new System.Drawing.Size(165, 17);
			this.checkBoxHook_getForegroundWindow.TabIndex = 11;
			this.checkBoxHook_getForegroundWindow.Text = "Hook GetForegroundWindow";
			this.checkBoxHook_getForegroundWindow.UseVisualStyleBackColor = true;
			this.checkBoxHook_getForegroundWindow.CheckedChanged += new System.EventHandler(this.checkBoxHook_getForegroundWindow_CheckedChanged);
			// 
			// checkBoxHook_filterCallWndProc
			// 
			this.checkBoxHook_filterCallWndProc.AutoSize = true;
			this.checkBoxHook_filterCallWndProc.Location = new System.Drawing.Point(9, 97);
			this.checkBoxHook_filterCallWndProc.Name = "checkBoxHook_filterCallWndProc";
			this.checkBoxHook_filterCallWndProc.Size = new System.Drawing.Size(228, 17);
			this.checkBoxHook_filterCallWndProc.TabIndex = 10;
			this.checkBoxHook_filterCallWndProc.Text = "Filter mouse input messages from Windows";
			this.checkBoxHook_filterCallWndProc.UseVisualStyleBackColor = true;
			this.checkBoxHook_filterCallWndProc.CheckedChanged += new System.EventHandler(this.checkBoxHook_filterCallWndProc_CheckedChanged);
			// 
			// checkBoxHook_filterWindowsRawInput
			// 
			this.checkBoxHook_filterWindowsRawInput.AutoSize = true;
			this.checkBoxHook_filterWindowsRawInput.Location = new System.Drawing.Point(9, 73);
			this.checkBoxHook_filterWindowsRawInput.Name = "checkBoxHook_filterWindowsRawInput";
			this.checkBoxHook_filterWindowsRawInput.Size = new System.Drawing.Size(214, 17);
			this.checkBoxHook_filterWindowsRawInput.TabIndex = 9;
			this.checkBoxHook_filterWindowsRawInput.Text = "Filter raw input messages from Windows";
			this.checkBoxHook_filterWindowsRawInput.UseVisualStyleBackColor = true;
			this.checkBoxHook_filterWindowsRawInput.CheckedChanged += new System.EventHandler(this.checkBoxHook_filterWindowsRawInput_CheckedChanged);
			// 
			// hooksWarningLabel
			// 
			this.hooksWarningLabel.AutoSize = true;
			this.hooksWarningLabel.ForeColor = System.Drawing.Color.Red;
			this.hooksWarningLabel.Location = new System.Drawing.Point(6, 16);
			this.hooksWarningLabel.MaximumSize = new System.Drawing.Size(280, 0);
			this.hooksWarningLabel.Name = "hooksWarningLabel";
			this.hooksWarningLabel.Size = new System.Drawing.Size(265, 39);
			this.hooksWarningLabel.TabIndex = 0;
			this.hooksWarningLabel.Text = "Warning: Hooks inject code into the target game. This may be detected by an anti-" +
    "cheat system or anti-virus software. See the documentation for more info.";
			// 
			// drawMouseCheckbox
			// 
			this.drawMouseCheckbox.AutoSize = true;
			this.drawMouseCheckbox.Location = new System.Drawing.Point(4, 171);
			this.drawMouseCheckbox.Name = "drawMouseCheckbox";
			this.drawMouseCheckbox.Size = new System.Drawing.Size(85, 17);
			this.drawMouseCheckbox.TabIndex = 7;
			this.drawMouseCheckbox.Text = "Draw mouse";
			this.drawMouseCheckbox.UseVisualStyleBackColor = true;
			this.drawMouseCheckbox.CheckedChanged += new System.EventHandler(this.drawMouseCheckbox_CheckedChanged);
			// 
			// drawMouseEveryXmsLabel
			// 
			this.drawMouseEveryXmsLabel.AutoSize = true;
			this.drawMouseEveryXmsLabel.Location = new System.Drawing.Point(3, 193);
			this.drawMouseEveryXmsLabel.Name = "drawMouseEveryXmsLabel";
			this.drawMouseEveryXmsLabel.Size = new System.Drawing.Size(164, 13);
			this.drawMouseEveryXmsLabel.TabIndex = 8;
			this.drawMouseEveryXmsLabel.Text = "Draw mouse every X milliseconds";
			// 
			// drawMouseEveryXmsField
			// 
			this.drawMouseEveryXmsField.Location = new System.Drawing.Point(173, 191);
			this.drawMouseEveryXmsField.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
			this.drawMouseEveryXmsField.Name = "drawMouseEveryXmsField";
			this.drawMouseEveryXmsField.Size = new System.Drawing.Size(120, 20);
			this.drawMouseEveryXmsField.TabIndex = 8;
			this.drawMouseEveryXmsField.Tag = "";
			this.drawMouseEveryXmsField.ValueChanged += new System.EventHandler(this.drawMouseEveryXmsField_ValueChanged);
			// 
			// refreshWindowBoundsOnLMBCheckbox
			// 
			this.refreshWindowBoundsOnLMBCheckbox.AutoSize = true;
			this.refreshWindowBoundsOnLMBCheckbox.Location = new System.Drawing.Point(4, 148);
			this.refreshWindowBoundsOnLMBCheckbox.Name = "refreshWindowBoundsOnLMBCheckbox";
			this.refreshWindowBoundsOnLMBCheckbox.Size = new System.Drawing.Size(214, 17);
			this.refreshWindowBoundsOnLMBCheckbox.TabIndex = 6;
			this.refreshWindowBoundsOnLMBCheckbox.Text = "Refresh window bounds on mouse click";
			this.refreshWindowBoundsOnLMBCheckbox.UseVisualStyleBackColor = true;
			this.refreshWindowBoundsOnLMBCheckbox.CheckedChanged += new System.EventHandler(this.refreshWindowBoundsOnLMBCheckbox_CheckedChanged);
			// 
			// send_WM_FOCUS_checkbox
			// 
			this.send_WM_FOCUS_checkbox.AutoSize = true;
			this.send_WM_FOCUS_checkbox.Location = new System.Drawing.Point(4, 124);
			this.send_WM_FOCUS_checkbox.Name = "send_WM_FOCUS_checkbox";
			this.send_WM_FOCUS_checkbox.Size = new System.Drawing.Size(193, 17);
			this.send_WM_FOCUS_checkbox.TabIndex = 5;
			this.send_WM_FOCUS_checkbox.Text = "Send fake window focus messages";
			this.send_WM_FOCUS_checkbox.UseVisualStyleBackColor = true;
			this.send_WM_FOCUS_checkbox.CheckedChanged += new System.EventHandler(this.send_WM_FOCUS_checkbox_CheckedChanged);
			// 
			// send_WM_ACTIVATE_checkbox
			// 
			this.send_WM_ACTIVATE_checkbox.AutoSize = true;
			this.send_WM_ACTIVATE_checkbox.Location = new System.Drawing.Point(4, 100);
			this.send_WM_ACTIVATE_checkbox.Name = "send_WM_ACTIVATE_checkbox";
			this.send_WM_ACTIVATE_checkbox.Size = new System.Drawing.Size(205, 17);
			this.send_WM_ACTIVATE_checkbox.TabIndex = 4;
			this.send_WM_ACTIVATE_checkbox.Text = "Send fake window activate messages";
			this.send_WM_ACTIVATE_checkbox.UseVisualStyleBackColor = true;
			this.send_WM_ACTIVATE_checkbox.CheckedChanged += new System.EventHandler(this.send_WM_ACTIVATE_checkbox_CheckedChanged);
			// 
			// sendNormalKeyboardCheckbox
			// 
			this.sendNormalKeyboardCheckbox.AutoSize = true;
			this.sendNormalKeyboardCheckbox.Location = new System.Drawing.Point(4, 76);
			this.sendNormalKeyboardCheckbox.Name = "sendNormalKeyboardCheckbox";
			this.sendNormalKeyboardCheckbox.Size = new System.Drawing.Size(158, 17);
			this.sendNormalKeyboardCheckbox.TabIndex = 3;
			this.sendNormalKeyboardCheckbox.Text = "Send normal keyboard input";
			this.sendNormalKeyboardCheckbox.UseVisualStyleBackColor = true;
			this.sendNormalKeyboardCheckbox.CheckedChanged += new System.EventHandler(this.sendNormalKeyboardCheckbox_CheckedChanged);
			// 
			// sendNormalMouseCheckbox
			// 
			this.sendNormalMouseCheckbox.AutoSize = true;
			this.sendNormalMouseCheckbox.Location = new System.Drawing.Point(4, 52);
			this.sendNormalMouseCheckbox.Name = "sendNormalMouseCheckbox";
			this.sendNormalMouseCheckbox.Size = new System.Drawing.Size(145, 17);
			this.sendNormalMouseCheckbox.TabIndex = 2;
			this.sendNormalMouseCheckbox.Text = "Send normal mouse input";
			this.sendNormalMouseCheckbox.UseVisualStyleBackColor = true;
			this.sendNormalMouseCheckbox.CheckedChanged += new System.EventHandler(this.sendNormalMouseCheckbox_CheckedChanged);
			// 
			// sendRawKeyboardCheckbox
			// 
			this.sendRawKeyboardCheckbox.AutoSize = true;
			this.sendRawKeyboardCheckbox.Location = new System.Drawing.Point(4, 28);
			this.sendRawKeyboardCheckbox.Name = "sendRawKeyboardCheckbox";
			this.sendRawKeyboardCheckbox.Size = new System.Drawing.Size(144, 17);
			this.sendRawKeyboardCheckbox.TabIndex = 1;
			this.sendRawKeyboardCheckbox.Text = "Send raw keyboard input";
			this.sendRawKeyboardCheckbox.UseVisualStyleBackColor = true;
			this.sendRawKeyboardCheckbox.CheckedChanged += new System.EventHandler(this.sendRawKeyboardCheckbox_CheckedChanged);
			// 
			// sendRawMouseCheckbox
			// 
			this.sendRawMouseCheckbox.AutoSize = true;
			this.sendRawMouseCheckbox.Location = new System.Drawing.Point(4, 4);
			this.sendRawMouseCheckbox.Name = "sendRawMouseCheckbox";
			this.sendRawMouseCheckbox.Size = new System.Drawing.Size(131, 17);
			this.sendRawMouseCheckbox.TabIndex = 0;
			this.sendRawMouseCheckbox.Text = "Send raw mouse input";
			this.sendRawMouseCheckbox.UseVisualStyleBackColor = true;
			this.sendRawMouseCheckbox.CheckedChanged += new System.EventHandler(this.sendRawMouseCheckbox_CheckedChanged);
			// 
			// endButtonSetter
			// 
			this.endButtonSetter.Location = new System.Drawing.Point(4, 363);
			this.endButtonSetter.Name = "endButtonSetter";
			this.endButtonSetter.Size = new System.Drawing.Size(175, 23);
			this.endButtonSetter.TabIndex = 11;
			this.endButtonSetter.Text = "Stop button = End";
			this.endButtonSetter.UseVisualStyleBackColor = true;
			this.endButtonSetter.Click += new System.EventHandler(this.endButtonSetter_Click);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(800, 450);
			this.Controls.Add(this.tabControl1);
			this.Cursor = System.Windows.Forms.Cursors.Default;
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MaximizeBox = false;
			this.Name = "Form1";
			this.Text = "Form1";
			this.TopMost = true;
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.activeWindowPanel.ResumeLayout(false);
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
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.hooksBox.ResumeLayout(false);
			this.hooksBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.drawMouseEveryXmsField)).EndInit();
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
		private System.Windows.Forms.NumericUpDown drawMouseEveryXmsField;
		private System.Windows.Forms.CheckBox refreshWindowBoundsOnLMBCheckbox;
		private System.Windows.Forms.CheckBox send_WM_FOCUS_checkbox;
		private System.Windows.Forms.CheckBox send_WM_ACTIVATE_checkbox;
		private System.Windows.Forms.CheckBox sendNormalKeyboardCheckbox;
		private System.Windows.Forms.CheckBox sendNormalMouseCheckbox;
		private System.Windows.Forms.CheckBox sendRawKeyboardCheckbox;
		private System.Windows.Forms.CheckBox sendRawMouseCheckbox;
		private System.Windows.Forms.Label drawMouseEveryXmsLabel;
		private System.Windows.Forms.CheckBox drawMouseCheckbox;
		private System.Windows.Forms.GroupBox hooksBox;
		private System.Windows.Forms.Label hooksWarningLabel;
		private System.Windows.Forms.CheckBox checkBoxHook_filterWindowsRawInput;
		private System.Windows.Forms.CheckBox checkBoxHook_filterCallWndProc;
		private System.Windows.Forms.CheckBox checkBoxHook_getForegroundWindow;
		private System.Windows.Forms.Button endButtonSetter;
	}
}