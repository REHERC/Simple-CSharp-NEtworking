using System.Text;

namespace Networking
{
	public static class Constants
	{
		public const int BUFFER_SIZE = 16;
		public const string DATA_END = "\0\0\0\0\0\0\0\0";

		public const int THREAD_SLEEP = 10;

		public const int READ_TIMEOUT = THREAD_SLEEP + 40;
		public const int WRITE_TIMEOUT = THREAD_SLEEP + 140;

		public static Encoding NetworkEncoding => Encoding.Unicode;
	}
}
