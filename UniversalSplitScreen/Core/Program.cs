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
using UniversalSplitScreen.UI;

namespace UniversalSplitScreen
{
	class Program
	{
		static Intercept i;

		public static Form1 Form { get; private set; }
		public static IntPtr Form_hWnd { get; private set; }
		public static SplitScreenManager SplitScreenManager { get; private set; }
		public static MessageProcessor MessageProcessor { get; private set; }
		public static OptionsStructure Options { get; private set; }

		static void Main(string[] args)
		{
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			TaskScheduler.UnobservedTaskException +=
			(object sender, UnobservedTaskExceptionEventArgs excArgs) =>
			{
				Logger.WriteLine("Exception occured. Task terminated! + " + excArgs.Exception);
				excArgs.SetObserved();
			};

			//ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
			//Logger.WriteLine($"ThreadPool: max worker threads = {workerThreads}, completion port threads = {completionPortThreads}");
			//bool smt = ThreadPool.SetMaxThreads(100 * workerThreads, 100 * completionPortThreads);
			//Console.WriteLine($"SetMaxTheads*100 success = {smt}");

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);

			Form = new Form1();
			Form_hWnd = Form.Handle;
			
			Options = new OptionsStructure();

			Core.Options.LoadOptions();
			
			SplitScreenManager = new SplitScreenManager();
			SplitScreenManager.Init();

			MessageProcessor = new MessageProcessor();

			i = new Intercept();
			var x = new RawInputManager(Form_hWnd);
			
			InputDisabler.Init();

			Application.Run(Form);//Not required for RegisterRawInputDevices to work

			SplitScreenManager.DeactivateSplitScreen();

			Logger.WriteLine("Exiting application");
			Environment.Exit(0);
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			Logger.WriteLine("Error: CurrentDomain_UnhandledException entered.");
			string message = (e.ExceptionObject as Exception).Message;
			Logger.WriteLine(message);
			System.Diagnostics.Trace.WriteLine(message, "Unhandled UI Exception");
			Logger.WriteLine(message);
		}
	}
}
