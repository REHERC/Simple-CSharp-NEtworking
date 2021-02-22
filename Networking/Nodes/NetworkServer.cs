using System.Net.Sockets;
using System.Threading;
//using static System.Console;
using static Networking.Constants;

namespace Networking.Nodes
{
	public sealed class Server : NetworkNode
	{
		public TcpListener ListenerConnexion { get; private set; } = null;

		public TcpClient ClientConnexion { get; private set; } = null;

		public Server(string ip, int port)
		: base (ip, port)
		{
		}

		public override void Start()
		{
			if (Running) return;

			ListenerConnexion = new TcpListener(address, port);
			ListenerConnexion.Start();

			Running = true;

			Thread.Start();
			if (!WaitForStream())
			{
				Stop();
			}
		}

		protected override void Run()
		{
			while (Running)
			{
				//WriteLine($"Waiting for TCP client... ({address}:{port})");
				ClientConnexion = ListenerConnexion.AcceptTcpClient();
				//WriteLine("Connected!");

				Stream = ClientConnexion.GetStream();

				Stream.ReadTimeout = READ_TIMEOUT;
				Stream.WriteTimeout = WRITE_TIMEOUT;

				ClientConnexion.ReceiveTimeout = READ_TIMEOUT;
				ClientConnexion.SendTimeout = WRITE_TIMEOUT;

				while (ClientConnexion.Client.Connected)
				{
					Thread.Sleep(THREAD_SLEEP);
				}
			}
		}

		public override void Stop()
		{
			if (!Running) return;

			ClientConnexion?.Close();
			ListenerConnexion?.Stop();
			ClientConnexion = null;
			ListenerConnexion = null;


			if (Thread != null && Thread.IsAlive)
			{
				Thread.Interrupt();
			}

			Running = false;
		}
	}
}
