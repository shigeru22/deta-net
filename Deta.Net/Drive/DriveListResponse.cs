// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class DriveListResponse
{
	[JsonPropertyName("names")]
	[JsonPropertyOrder(0)]
	public string[] Names { get; init; }

	[JsonPropertyName("paging")]
	[JsonPropertyOrder(1)]
	public DriveListResponsePaging Paging { get; init; }

	public DriveListResponse(string[] names, DriveListResponsePaging paging)
		=> (Names, Paging) = (names, paging);
}

[Serializable]
public class DriveListResponsePaging
{
	[JsonPropertyName("size")]
	[JsonPropertyOrder(0)]
	public int Size { get; init; }

	[JsonPropertyName("last")]
	[JsonPropertyOrder(1)]
	public string Last { get; init; }

	public DriveListResponsePaging(int size, string last)
		=> (Size, Last) = (size, last);
}
