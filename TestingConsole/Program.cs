using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TestingConsole
{
	class Program
	{
		[DllImport("InjectorCPP.dll", SetLastError = true)]//, EntryPoint = "MyFunc"
		static extern int MyFunc();

		static void Main(string[] args)
		{
			Console.WriteLine($"Hello World = {MyFunc()}");

			Console.ReadKey();
		}
	}
}
