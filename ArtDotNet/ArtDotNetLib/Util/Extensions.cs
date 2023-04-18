using System;
using System.Security.Cryptography;

namespace ArtDotNet
{
	public static class Extensions
	{
		public static byte[] Block(this byte[] data, int offset, int length)
		{
			var tmp = new byte[length];
			Array.Copy(data, offset, tmp, 0, Math.Min(length, data.Length - offset));
			return tmp;
		}

		public static int GetInt16LE(this byte[] data, int offset)
		{
			return (data[offset + 1] << 8) | (data[offset]);
		}

		public static int GetInt16(this byte[] data, int offset)
		{
			return (data[offset] << 8 | data[offset + 1]);
		}
	}
}

