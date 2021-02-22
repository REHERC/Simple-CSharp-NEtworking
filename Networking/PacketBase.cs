using JsonSubTypes;
using Networking.Packets;
using Newtonsoft.Json;
using System;

namespace Networking
{
	[Serializable]
	[JsonConverter(typeof(JsonSubtypes), "id")]
	[JsonSubtypes.KnownSubType(typeof(LogMessage), PacketID.LogMessage)]
	public abstract class PacketBase
	{
		[JsonProperty("id")]
		public abstract PacketID ID { get; }

		public abstract void Run();
	}
}
