using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static Networking.Constants;

namespace Networking.Communication
{
	public abstract class NetworkBase
	{
		protected readonly string ip;
		protected readonly int port;

		protected readonly IPHostEntry host;
		protected readonly IPAddress ipAddress;
		protected readonly IPEndPoint endPoint;

		public Socket Socket { get; protected set; }

		public NetworkBase(string ip, int port)
		{
			this.ip = ip;
			this.port = port;

			host = Dns.GetHostEntry(ip);
			ipAddress = host.AddressList[0];

			endPoint = new IPEndPoint(ipAddress, port);

			Socket = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
		}

		public abstract void Run();
		public void Close()
		{
			Socket?.Shutdown(SocketShutdown.Both);
			Socket?.Close();
		}

		#region IO
		public void WriteString(string data)
		=> WriteBytes(Encoding.UTF8.GetBytes(data + DATA_END));

		public void WriteBytes(byte[] data)
		{
			Socket?.Send(data);
		}

		public byte[] ReadBytes(out int bytesReceived)
		{
			byte[] bytes = new byte[BUFFER_SIZE];
			bytesReceived = Socket.Receive(bytes);

			return bytes;
		}

		public byte[] ReadBytes()
		=> ReadBytes(out int _);

		public string ReadString()
		{
			string data = string.Empty;

			do
			{
				byte[] bytes = ReadBytes(out int bytesReceived);
				data += Encoding.UTF8.GetString(bytes, 0, bytesReceived);
			}
			while (data.IndexOf(DATA_END) == -1);

			return data.Remove(data.Length - DATA_END.Length, DATA_END.Length);
		}
		#endregion

		#region Conversions
		public static implicit operator Socket(NetworkBase network)
		{
			return network.Socket;
		}

		public static implicit operator IPHostEntry(NetworkBase network)
		{
			return network.host;
		}

		public static implicit operator IPEndPoint(NetworkBase network)
		{
			return network.endPoint;
		}

		public static implicit operator IPAddress(NetworkBase network)
		{
			return network.ipAddress;
		}
		#endregion
	}
}
