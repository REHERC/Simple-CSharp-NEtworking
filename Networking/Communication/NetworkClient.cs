#pragma warning disable IDE0052
using System.Net.Sockets;
using static Networking.Constants;
using static System.Console;

namespace Networking.Communication
{
	public class NetworkClient : NetworkBase
	{
		private readonly byte[] bytes;

		public NetworkClient(string ip, int port)
		: base(ip, port)
		{
			bytes = new byte[BUFFER_SIZE];
		}

		public override void Run()
		{
			try
			{
				Socket.Connect(endPoint);
			}
			catch (SocketException)
			{
				WriteLine($"Impossible to connect to {ip}:{port}");
				return;
			}

			WriteLine($"Socket connected to {Socket.RemoteEndPoint}");

			WriteString($"This is a test");
			WriteLine("Echoed test = {0}", ReadString());
			WriteLine(ReadString());

			//Close();
		}
	}
}
