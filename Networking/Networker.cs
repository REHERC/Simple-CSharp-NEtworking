using Networking.Nodes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using static Networking.Compression;
using static Networking.Constants;
using static System.Console;

namespace Networking
{
	public sealed class Networker<NODE> where NODE : NetworkNode
	{
		private readonly NetworkNode networkObject;

		private readonly Thread threadSend;

		private readonly Thread threadReceive;

		private readonly Queue<PacketBase> sendQueue;

		private bool started_ = false;
		public bool Started
		{ 
			get
			{
				if (!networkObject.Running)
				{
					Started = false;
				}

				return started_;
			}
			private set
			{
				started_ = value;

				(Started ? OnStarted : OnStopped)?.Invoke(this, EventArgs.Empty);
			}
		}

		public event EventHandler OnStarted;
		public event EventHandler OnStopped;
		public event Action<Networker<NODE>, PacketBase> OnPacketReceived;

		public Networker(string ip, int port)
		{
			networkObject = Activator.CreateInstance(typeof(NODE), new object[] { ip, port }) as NODE;

			sendQueue = new Queue<PacketBase>();

			threadSend = new Thread(ThreadSend)
			{
				IsBackground = true
			};

			threadReceive = new Thread(ThreadReceive)
			{
				IsBackground = true
			};
		}

		public void Start()
		{
			networkObject.Start();

			threadSend.Start();
			threadReceive.Start();

			Started = true;
		}

		public void Stop()
		{
			sendQueue.Clear();

			threadSend.Interrupt();
			threadReceive.Interrupt();

			networkObject.Stop();

			Started = false;
		}

		public void SendPacket(PacketBase packet)
		{
			if (Started)
			{
				sendQueue.Enqueue(packet);
			}
		}

		private void ThreadSend()
		{
			while (Started)
			{
				if (sendQueue.Count > 0)
				{
					PacketBase packet = sendQueue.Dequeue();

					string json = JsonConvert.SerializeObject(packet, Formatting.None);

					byte[] compressed = Compress(json);

					networkObject.Write(compressed, 0, compressed.Length);
					networkObject.Flush();
				}

				Thread.Sleep(THREAD_SLEEP);
			}
		}

		private void ThreadReceive()
		{
			using (StreamReader reader = networkObject.CreateReader())
			{
				while (Started)
				{
					byte[] data;

					using (var memStream = new MemoryStream())
					{
						byte[] buffer = new byte[BUFFER_SIZE];
						int incoming;

						while ((incoming = networkObject.Read(buffer, 0, buffer.Length)) > 0)
						{
							memStream.Write(buffer, 0, incoming);
						}

						data = memStream.ToArray();
					}

					if (data.Length > 0)
					{
						string json = Decompress(data);

						try
						{
							PacketBase packet = JsonConvert.DeserializeObject<PacketBase>(json);

							OnPacketReceived?.Invoke(this, packet);
						}
						catch (NullReferenceException)
						{
							//WriteLine("EXIT " + e.ToString());
						}
					}

					Thread.Sleep(THREAD_SLEEP);
				}
			}
		}
	}
}
