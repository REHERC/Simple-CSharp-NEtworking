using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using static Networking.Constants;

namespace Networking.Nodes
{
	public abstract class NetworkNode : Stream, IDisposable
	{
		#region Fields and Properties
		protected readonly string ip;
		protected readonly int port;

		protected readonly IPAddress address;

		protected Thread Thread { get; set; }

		public NetworkStream Stream { get; protected set; }

		private bool running_ = false;
		public bool Running
		{
			get
			{
				return running_;
			}
			protected set
			{
				running_ = value;

				if (!value)
				{
					Stream?.Close();
					Stream?.Dispose();
				}
			}
		}
		#endregion

		public NetworkNode(string ip, int port)
		{
			this.ip = ip;
			this.port = port;

			if (!IPAddress.TryParse(ip, out address))
			{
				IPHostEntry host = Dns.GetHostEntry(ip);
				address = host.AddressList[0];
			}

			Thread = new Thread(Run)
			{
				IsBackground = true
			};
		}

		#region Abstarct Methods
		public abstract void Start();

		protected abstract void Run();

		public abstract void Stop();
		#endregion

		#region IO
		public StreamReader CreateReader()
		{
			return new StreamReader(this, NetworkEncoding);
		}

		public BinaryReader CreateBinaryReader()
		{
			return new BinaryReader(this, NetworkEncoding);
		}

		public StreamWriter CreateWriter()
		{
			return new StreamWriter(this, NetworkEncoding);
		}

		public BinaryWriter CreateBinaryWriter()
		{
			return new BinaryWriter(this, NetworkEncoding);
		}
		#endregion

		public bool WaitForStream()
		{
			while (!CanRead || !CanWrite)
			{
				Thread.Sleep(THREAD_SLEEP);

				if (!Running)
				{
					return false;
				}
			}

			return true;
		}

		#region Stream class Encapsulation
		#region Properties
		public override bool CanRead => Stream?.CanRead ?? false;

		public override bool CanSeek => Stream?.CanSeek ?? false;

		public override bool CanWrite => Stream?.CanWrite ?? false;

		public override long Length => -1;
		#endregion

		#region Methods
		public override long Position
		{
			get
			{
				try
				{
					return Stream?.Position ?? 0;
				}
				catch (Exception e)
				{
					switch (e)
					{
						case IOException _:
						case NotSupportedException _:
						case ObjectDisposedException _:
							return 0;
						default:
							throw;
					}
				}
			}
			set
			{
				Stream.Position = value;
			}
		}

		public override void Flush()
		{
			try
			{
				Stream?.Flush();
			}
			catch (Exception e)
			{
				switch (e)
				{
					case IOException _:
					case ObjectDisposedException _:
						return;
					default:
						throw;
				}
			}
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			try
			{
				return Stream?.Seek(offset, origin) ?? 0;
			}
			catch (Exception e)
			{
				switch (e)
				{
					case IOException _:
					case ObjectDisposedException _:
						return 0;
					default:
						throw;
				}
			}
		}

		public override void SetLength(long value)
		{
			try
			{
				Stream?.SetLength(value);
			}
			catch (Exception e)
			{
				switch (e)
				{
					case IOException _:
					case ObjectDisposedException _:
						return;
					default:
						throw;
				}
			}
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			try
			{
				return Stream?.Read(buffer, offset, count) ?? 0;
			}
			catch (Exception e)
			{
				switch (e)
				{
					case IOException _:
					case ObjectDisposedException _:
						return 0;
					default:
						throw;
				}
			}
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			try
			{
				Stream?.Write(buffer, offset, count);
			}
			catch (Exception e)
			{
				switch (e)
				{
					case IOException _:
					case ObjectDisposedException _:
						return;
					default:
						throw;
				}
			}
		}
		#endregion
		#endregion
	}
}