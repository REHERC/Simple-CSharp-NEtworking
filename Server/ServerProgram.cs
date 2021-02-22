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
			if (packet is LogMessage)
			{
				server.SendPacket(packet);
			}
			else
			{
				packet.Run();
			}
		};
		
		server.Start();

		ReadLine();
	}
}