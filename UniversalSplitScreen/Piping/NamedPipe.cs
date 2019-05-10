using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UniversalSplitScreen.Piping
{
	public class NamedPipe
	{
		public readonly string pipeName;

		Thread serverThread;
		NamedPipeServerStream pipeServer;
		bool clientConnected = false;

		public NamedPipe()
		{
			pipeName = GenerateName();

			serverThread = new Thread(Start);
			serverThread.Start();
		}

		private void Start()
		{
			Console.WriteLine($"Starting pipe {pipeName}");
			pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.InOut, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);
			Console.WriteLine($"Created pipe {pipeName}");

			pipeServer.WaitForConnection();
			clientConnected = true;
			Console.WriteLine($"Client connected to pipe {pipeName}");
		}

		public void AddMessage(byte message, int param1, int param2)
		{
			if (clientConnected)
			{
				byte[] bytes = {
						message,
						(byte)(param1 >> 24), (byte)(param1 >> 16), (byte)(param1 >> 8), (byte)param1,
						(byte)(param2 >> 24), (byte)(param2 >> 16), (byte)(param2 >> 8), (byte)param2
					};

				pipeServer?.Write(bytes, 0, 9);
			}
		}

		public void Close()
		{
			Console.WriteLine($"Closing pipe {pipeName}");
			pipeServer?.Dispose();
			pipeServer = null;
		}

		//https://github.com/EasyHook/EasyHook/blob/master/EasyHook/RemoteHook.cs
		private string GenerateName()
		{
			byte[] data = new byte[30];
			var random = new RNGCryptoServiceProvider();
			random.GetBytes(data);

			StringBuilder sb = new StringBuilder();

			for (int i = 0; i < 30; i++)
			{
				byte b = (byte)(data[i] % 62);

				if (b <= 9)
					sb.Append((Char)('0' + b));
				else if (b <= 35)
					sb.Append((Char)('A' + (b - 10)));
				else
					sb.Append((Char)('a' + (b - 36)));
			}

			return sb.ToString();
		}
	}
}
