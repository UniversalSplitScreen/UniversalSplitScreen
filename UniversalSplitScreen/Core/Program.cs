using AutoHotkey.Interop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using UniversalSplitScreen.Core;
using UniversalSplitScreen.RawInput;
using UniversalSplitScreen.SendInput;

namespace UniversalSplitScreen
{
	class Program
	{
		static Intercept i;

		public static Form1 Form { get; private set; }
		public static IntPtr Form_hWnd { get; private set; }
		public static SplitScreenManager SplitScreenManager { get; private set; }

		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			TaskScheduler.UnobservedTaskException +=
			(object sender, UnobservedTaskExceptionEventArgs excArgs) =>
			{
				Console.WriteLine("Exception occured. Task terminated! + " + excArgs.Exception);
				excArgs.SetObserved();
			};

			

			//var ahk = AutoHotkeyEngine.Instance;
			//ahk.ExecRaw("^m::MsgBox You pressed Ctrl+m.");
			//Console.ReadLine();

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Form = new Form1();
			Form_hWnd = Form.Handle;

			Program.SplitScreenManager = new SplitScreenManager();
			SplitScreenManager.Init();

			i = new Intercept(Form.Handle);
			var x = new RawInputManager(Form_hWnd);
			
			InputDisabler.Init();

			Application.Run(Form);//Not required for RegisterRawInputDevices to work

			SplitScreenManager.DeactivateSplitScreen();
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Console.WriteLine("Error: CurrentDomain_UnhandledException entered.");
			string message = (e.ExceptionObject as Exception).Message;
			Console.WriteLine(message);
			System.Diagnostics.Trace.WriteLine(message, "Unhandled UI Exception");
			Console.WriteLine(message);
		}
	}
}
