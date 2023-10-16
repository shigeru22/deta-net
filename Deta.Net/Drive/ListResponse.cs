// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class ListResponse
{
	[JsonPropertyName("names")]
	[JsonPropertyOrder(0)]
	public string[] Names { get; init; }

	[JsonPropertyName("paging")]
	[JsonPropertyOrder(1)]
	public ListResponsePaging Paging { get; init; }

	public ListResponse(string[] names, ListResponsePaging paging)
		=> (Names, Paging) = (names, paging);
}

[Serializable]
public class ListResponsePaging
{
	[JsonPropertyName("size")]
	[JsonPropertyOrder(0)]
	public int Size { get; init; }

	[JsonPropertyName("last")]
	[JsonPropertyOrder(1)]
	public string Last { get; init; }

	public ListResponsePaging(int size, string last)
		=> (Size, Last) = (size, last);
}
