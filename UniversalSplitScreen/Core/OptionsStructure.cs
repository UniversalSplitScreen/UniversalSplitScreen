using Newtonsoft.Json;

namespace UniversalSplitScreen.Core
{
	//Do not rename these fields without changing Form1.PopulateOptionsRefTypes
	public class OptionsStructure
	{
		public string OptionsName					{ get; set; } = "Default";
		
		public bool SendRawMouseInput				{ get; set; } = false;
		public bool SendRawKeyboardInput			{ get; set; } = false;
		public bool SendNormalMouseInput			{ get; set; } = true;
		public bool SendNormalKeyboardInput			{ get; set; } = true;
		public bool SendScrollwheel					{ get; set; } = false;

		public bool SendWM_ACTIVATE					{ get; set; } = true;
		public bool SendWM_SETFOCUS					{ get; set; } = false;

		public bool RefreshWindowBoundsOnMouseClick	{ get; set; } = false;

		public bool DrawMouse						{ get; set; } = true;

		public string AutofillHandleName			{ get; set; } = string.Empty;

		public bool Hook_FilterRawInput				{ get; set; } = false;
		public bool Hook_FilterWindowsMouseInput	{ get; set; } = false;
		public bool Hook_GetForegroundWindow		{ get; set; } = false;
		public bool Hook_GetCursorPos				{ get; set; } = false;
		public bool Hook_SetCursorPos				{ get; set; } = false;
		public bool Hook_GetAsyncKeyState			{ get; set; } = false;
		public bool Hook_GetKeyState				{ get; set; } = false;
		public bool Hook_XInput						{ get; set; } = false;
		public bool Hook_DInput						{ get; set; } = false;
		public bool Hook_UseLegacyInput				{ get; set; } = false;
		public bool UpdateAbsoluteFlagInMouseMessage { get; set; } = true; //Only does something if legacy input is on
		public bool Hook_MouseVisibility			{ get; set; } = false;

		public ushort EndVKey						{ get; set; } = 0x23;

		public override string ToString() => OptionsName;

		public OptionsStructure Clone() => JsonConvert.DeserializeObject<OptionsStructure>(JsonConvert.SerializeObject(this));
	}
}
