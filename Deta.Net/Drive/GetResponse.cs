// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Drive;

public class GetResponse
{
	public string FileName { get; init; }
	public int Length { get; init; }
	public byte[] Content { get; init; }

	public GetResponse(string fileName, int length, byte[] content)
		=> (FileName, Length, Content) = (fileName, length, content);
}