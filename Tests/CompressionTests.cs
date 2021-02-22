using NUnit.Framework;
using System;
using System.Text;
using static Networking.Compression;
//using static Networking.Constants;

namespace Unit
{
	public class CompressionTests
	{
		[Test]
		public void ReadBackTest()
		{
			StringBuilder sb = new StringBuilder();
			for (short i = short.MinValue; i < short.MaxValue; ++i)
			{
				sb.Append(Guid.NewGuid().ToString());
			}

			string message = sb.ToString();

			byte[] compressed = Compress(message);

			string decompressed = Decompress(compressed);

			//Assert.Fail($"Message length:\t {message.Length} chars\nCompressed:\t\t {compressed.Length} bytes\nRaw:\t\t\t {NetworkEncoding.GetBytes(message).Length} bytes");
			
			Assert.AreEqual(decompressed, message);
		}
	}
}