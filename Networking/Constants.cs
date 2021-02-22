using System.Text;

namespace Networking
{
	public static class Constants
	{
		public const int BUFFER_SIZE = 16;

		// Set to at least 1
		// Increases latency the higher it is set
		public const int THREAD_SLEEP = 1;

		public const int READ_TIMEOUT = THREAD_SLEEP + 150;
		public const int WRITE_TIMEOUT = THREAD_SLEEP + 300;

		public static Encoding NetworkEncoding => Encoding.Unicode;
	}
}
