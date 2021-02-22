using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;

namespace Networking.Communication
{
	public sealed class TCPStream<NETWORK_TYPE> : Stream, IDisposable where NETWORK_TYPE : NetworkBase
	{
		private readonly Thread thread;
		private readonly NetworkBase network;
		private readonly NetworkStream stream;
		public override bool CanRead => stream.CanRead;

		public override bool CanSeek => stream.CanSeek;

		public override bool CanWrite => stream.CanWrite;

		public override long Length => stream.Length;

		public override long Position
		{
			get => stream.Position;
			set => stream.Position = value;
		}

		public TCPStream(string ip, int port)
		{
			network = Activator.CreateInstance(typeof(NETWORK_TYPE), ip, port) as NETWORK_TYPE;
			thread = new Thread(() =>
			{
				network.Run();
			})
			{
				IsBackground = true
			};
			thread.Start();

			while (network.Socket is null || !network.Socket.Connected)
			{
				Thread.Sleep(0);

				if (!thread.IsAlive)
				{
					return;
				}
			}

			stream = new NetworkStream(network);
		}

		public new void Dispose()
		{
			stream?.Dispose();

			if (thread != null && thread.IsAlive)
			{
				thread.Interrupt();
			}
		}

		public override void Flush()
		{
			stream.Flush();
		}

		public override int Read(byte[] buffer, int offset, int count)
		{
			return stream.Read(buffer, offset, count);
		}

		public override long Seek(long offset, SeekOrigin origin)
		{
			return stream.Seek(offset, origin);
		}

		public override void SetLength(long value)
		{
			stream.SetLength(value);
		}

		public override void Write(byte[] buffer, int offset, int count)
		{
			stream.Write(buffer, offset, count);
		}
	}
}
