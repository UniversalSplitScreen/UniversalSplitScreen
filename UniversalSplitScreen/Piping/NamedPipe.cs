using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UniversalSplitScreen.Core;

namespace UniversalSplitScreen.Piping
{
	/*	(All messages are outgoing from NamedPipe.cs to HooksCPP.cpp
		Messages:

		* 0x01: add DELTA cursor pos. param1 =x, param2=y

		* 0x02: Set VKEY. 
			param 1 = key. Mouse buttons: 1,2,4,5,6. WASD: 0x41, 0x44, 0x53, 0x47
			param 2 = on off: 0 = off, 1 = on

		* 0x03: close named pipe

		* 0x04: set ABSOLUTE cursor pos. param1 =x, param2=y

		* 0x05: set desktop as foreground window
	*/

	public class NamedPipe
	{
		public readonly string pipeName;

		Thread serverThread;
		NamedPipeServerStream pipeServer;
		bool clientConnected = false;
		IntPtr hWnd;

		int			toSendDeltaX,	toSendDeltaY,	toSendAbsX,	toSendAbsY;
		private ManualResetEvent xyResetEvent = new ManualResetEvent(false);

		public NamedPipe(IntPtr hWnd)
		{
			pipeName = GenerateName();

			this.hWnd = hWnd;

			serverThread = new Thread(Start);
			serverThread.Start();
		}

		private void Start()
		{
			Logger.WriteLine($"Starting pipe {pipeName}");
			pipeServer = new NamedPipeServerStream(pipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous | PipeOptions.WriteThrough);//WriteThrough?
			Logger.WriteLine($"Created pipe {pipeName}");

			try
			{
				pipeServer.WaitForConnection();
			}
			catch(Exception e)
			{
				Logger.WriteLine($"Exception while waiting for pipe client to connect: {e}");
				return;
			}

			clientConnected = true;
			Logger.WriteLine($"Client connected to pipe {pipeName}");

			// If legacy input is enabled, HooksCPP needs to know 
			// - Delta changes in mouse position (since last input). There is no bounding on this as it is used for first person camera movement.
			// - Absolute mouse position (this is bound from 0,0 to width,height). This is used in the menus.
			//If legacy input is disabled, Delta changes aren't used for first person camera movement (e.g. it uses raw input) so it doesn't need to be sent.
						
			bool sendDelta = Options.CurrentOptions.Hook_UseLegacyInput;

			if (sendDelta || Options.CurrentOptions.Hook_GetCursorPos)
			{
				//With this system, input messages are only sent as fast as one thread can manage.
				while (clientConnected)
				{
					xyResetEvent.WaitOne();//Wait for an mouse input message to have altered toSendDeltaX/Y
					if (sendDelta)
					{
						WriteMessageNow(0x01, toSendDeltaX, toSendDeltaY);
						toSendDeltaX = 0;
						toSendDeltaY = 0;
					}
					WriteMessageNow(0x04, toSendAbsX, toSendAbsY);
					xyResetEvent.Reset();//Reset the event (or WaitOne passes immediately)
				}
			}
		}

		/// <summary>
		/// Updates the mouse position to send, and makes the loop know there is data to be sent.
		/// (Doesn't immediately send a message)
		/// </summary>
		/// <param name="deltaX"></param>
		/// <param name="deltaY"></param>
		/// <param name="absoluteX"></param>
		/// <param name="absoluteY"></param>
		public void SendMousePosition(int deltaX, int deltaY, int absoluteX, int absoluteY)
		{
			toSendDeltaX += deltaX;
			toSendDeltaY += deltaY;
			toSendAbsX = absoluteX;
			toSendAbsY = absoluteY;
			xyResetEvent.Set();
		}

		/// <summary>
		/// Queues a send message via the ThreadPool.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="param1"></param>
		/// <param name="param2"></param>
		public void WriteMessage(byte message, int param1, int param2)
		{
			if (clientConnected)
			{
				ThreadPool.QueueUserWorkItem( delegate {
					byte[] bytes = {
						message,
						(byte)(param1 >> 24), (byte)(param1 >> 16), (byte)(param1 >> 8), (byte)param1,
						(byte)(param2 >> 24), (byte)(param2 >> 16), (byte)(param2 >> 8), (byte)param2
					};

					try
					{
						pipeServer?.Write(bytes, 0, 9);
					}
					catch (Exception)
					{
						Program.SplitScreenManager.CheckIfWindowExists(hWnd);
					}
				});
			}
		}

		/// <summary>
		/// Immediately sends a message via the named pipe on the current thread.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="param1"></param>
		/// <param name="param2"></param>
		private void WriteMessageNow(byte message, int param1, int param2)
		{
			byte[] bytes = {
				message,
				(byte)(param1 >> 24), (byte)(param1 >> 16), (byte)(param1 >> 8), (byte)param1,
				(byte)(param2 >> 24), (byte)(param2 >> 16), (byte)(param2 >> 8), (byte)param2
			};

			try
			{
				pipeServer.Write(bytes, 0, 9);
			}
			catch (Exception)
			{
				Program.SplitScreenManager.CheckIfWindowExists(hWnd);
			}
		}

		public void Close()
		{
			Logger.WriteLine($"Closing pipe {pipeName}");
			WriteMessageNow(0x03, 0, 0);//Close pipe message.
			pipeServer?.Dispose();
			pipeServer = null;
			clientConnected = false;
			xyResetEvent.Close();
		}

		//https://github.com/EasyHook/EasyHook/blob/master/EasyHook/RemoteHook.cs
		/// <summary>
		/// Generates a random 30 character long string of numbers and upper/lower case letters.
		/// </summary>
		/// <returns></returns>
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
