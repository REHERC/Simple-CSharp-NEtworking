using System;
using System.Net.Sockets;
using static System.Console;

namespace Networking.Communication
{
	public class NetworkServer : NetworkBase
	{
		public NetworkServer(string ip, int port)
        : base(ip, port)
		{ }

        public override void Run()
        {
            Socket.Bind(endPoint);

            while (true)
			{
                Socket.Listen(1);

                WriteLine("Waiting for a connection...");
                
                try
				{
					Socket = Socket.Accept();

					WriteLine($"Accepted socket {Socket.RemoteEndPoint}");

					string received = ReadString();

					WriteLine("Received string");
					//WriteLine("Text received : {0}", received);
					WriteLine(received);
					WriteString(received);
				}
				catch (Exception e)
				{
					WriteLine(e.ToString());
				}
            }
        }
	}
}
