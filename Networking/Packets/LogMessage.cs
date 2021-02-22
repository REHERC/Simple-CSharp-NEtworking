using Newtonsoft.Json;
using static System.Console;

namespace Networking.Packets
{
	public class LogMessage : PacketBase
	{
		public override PacketID ID => PacketID.LogMessage;

		[JsonProperty("message")]
		public string Message { get; set; }

		public override void Run()
		{
			WriteLine(Message);
		}

		public static LogMessage Create(string message)
		{
			return new LogMessage()
			{
				Message = message
			};
		}
	}
}
