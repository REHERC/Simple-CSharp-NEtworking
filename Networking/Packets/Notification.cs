using static System.Console;

namespace Networking.Packets
{
	public class Notification : PacketBase
	{
		public override PacketID ID => PacketID.Notification;

		public override void Run()
		{
			WriteLine("Packet delivered");
		}

		public static Notification Create()
		{
			return new Notification();
		}
	}
}
