using System.IO;
using System.IO.Compression;
using static Networking.Constants;

namespace Networking
{
	public static class Compression
	{
		public static byte[] Compress(string input)
		{
			using (var memStream = new MemoryStream())
			{
				using (var gzipStream = new GZipStream(memStream, CompressionMode.Compress))
				{
					using (var deflateStream = new DeflateStream(gzipStream, CompressionMode.Compress))
					{
						using (var streamWriter = new StreamWriter(deflateStream, NetworkEncoding))
						{
							streamWriter.Write(input);
							streamWriter.Flush();
						}
					}
				}

				return memStream.ToArray();
			}
		}

		public static string Decompress(byte[] input)
		{
			using (var memStream = new MemoryStream(input))
			{
				using (var gzipStream = new GZipStream(memStream, CompressionMode.Decompress))
				{
					using (var deflateStream = new DeflateStream(gzipStream, CompressionMode.Decompress))
					{
						using (var streamReader = new StreamReader(deflateStream, NetworkEncoding))
						{
							try
							{
								return streamReader.ReadToEnd();
							}
							catch (IOException)
							{
								return string.Empty;
							}
						}
					}
				}
			}
		}
	}
}
