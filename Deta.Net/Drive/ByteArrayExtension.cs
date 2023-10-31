// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Drive;

internal static class ByteArrayExtension
{
	internal static IEnumerable<byte[]> GetChunksEnumerator(this byte[] buffer, int size)
	{
		int bufferSize = buffer.Length;
		for (long block = 0; block < bufferSize; block += size)
		{
			byte[] tempBuffer;

			if (block + size < bufferSize)
			{
				tempBuffer = new byte[size];
				Array.Copy(buffer, block, tempBuffer, 0, size);
			}
			else
			{
				long tempSize = bufferSize - block;
				tempBuffer = new byte[tempSize];
				Array.Copy(buffer, block, tempBuffer, 0, tempSize);
			}

			yield return tempBuffer;
		}

		yield break;
	}
}
