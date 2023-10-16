// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

public class ListResponse
{
	[JsonPropertyName("names")] public string[] Names { get; init; }
	[JsonPropertyName("paging")] public ListResponsePaging Paging { get; init; }

	public ListResponse(string[] names, ListResponsePaging paging)
		=> (Names, Paging) = (names, paging);
}

public class ListResponsePaging
{
	[JsonPropertyName("size")] public int Size { get; init; }
	[JsonPropertyName("last")] public string Last { get; init; }

	public ListResponsePaging(int size, string last)
		=> (Size, Last) = (size, last);
}
