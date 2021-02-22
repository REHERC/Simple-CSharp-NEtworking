using System.Net.Sockets;
using System.Threading;
using static Networking.Constants;

namespace Networking.Nodes
{
	public sealed class Client : NetworkNode
	{
		public TcpClient ClientConnexion { get; private set; } = null;

		public Client(string ip, int port)
		: base(ip, port)
		{
		}

		public override void Start()
		{
			if (Running) return;

			Running = true;

			Thread.Start();
			
			if (!WaitForStream())
			{
				Stop();
			}
		}

		protected override void Run()
		{
			ClientConnexion = new TcpClient();
			ClientConnexion.Connect(address, port);

			Stream = ClientConnexion.GetStream();

			Stream.ReadTimeout = READ_TIMEOUT;
			Stream.WriteTimeout = WRITE_TIMEOUT;

			ClientConnexion.ReceiveTimeout = READ_TIMEOUT;
			ClientConnexion.SendTimeout = WRITE_TIMEOUT;

			while (Running && ClientConnexion.Client.Connected)
			{
				Thread.Sleep(THREAD_SLEEP);
			}

			Stop();
		}

		public override void Stop()
		{
			if (!Running) return;

			ClientConnexion?.Close();
			ClientConnexion = null;

			if (Thread != null && Thread.IsAlive)
			{
				Thread.Interrupt();
			}

			Running = false;
		}
	}
}