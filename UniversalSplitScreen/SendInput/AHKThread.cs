using AutoHotkey.Interop;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalSplitScreen.SendInput
{
	//AutoHotKey eventually gives an AccessViolationException when running in multiple threads. But it is too slow to run on the mouse thread.
	class AHKThread
	{
		private static ConcurrentQueue<string> cmds = new ConcurrentQueue<string>();

		private static AutoHotkeyEngine ahk;

		static AHKThread()
		{
			ahk = AutoHotkeyEngine.Instance;
		}

		public static void AddCommand(string cmd)
		{
			if (cmds.Count == 0)
			{ 
				cmds.Enqueue(cmd);
				Task.Factory.StartNew(() => RunNextCommand());
			}
			else
			{
				cmds.Enqueue(cmd);
			}
		}

		private static void RunNextCommand(string cmd = "")
		{
			if (cmds.Count > 0)
			{
				//ahk.ExecRaw(cmds.Dequeue());
				if(cmds.TryDequeue(out string newCmd))
					ahk.ExecRaw(newCmd);
			}

			if (cmds.Count > 0)
			{
				RunNextCommand();
			}
		}
	}
}
