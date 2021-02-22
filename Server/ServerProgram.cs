using Networking;
using Networking.Nodes;
using Networking.Packets;
using static System.Console;

internal class ServerProgram
{
	static void Main()
	{
		Title = "TCP Server";

		var server = new Networker<Server>("127.0.0.1", 5000);
		server.OnPacketReceived += (sender, packet) =>
		{
			packet?.Run();
		};
		
		server.Start();

		while (server.Started)
		{
			server.SendPacket(LogMessage.Create(ReadLine()));
		}


		ReadLine();
	}

	public static void Nothing()
	{
		Title = "TCP Server";

		var server = new Server("127.0.0.1", 5000);
		server.Start();

		using (var reader = server.CreateReader())
		{
			while (server.Running)
			{
				if (!reader.EndOfStream)
				{
					WriteLine(reader.ReadLine());
				}
			}
		}

		ReadLine();
	}
}