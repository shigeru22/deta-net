// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class BaseQueryResponse
{
	[JsonPropertyName("paging")]
	[JsonPropertyOrder(0)]
	public BaseQueryPagingResponse Paging { get; init; }

	[JsonPropertyName("items")]
	[JsonPropertyOrder(1)]
	public Dictionary<string, string>[] Items { get; init; }

	public BaseQueryResponse(BaseQueryPagingResponse paging, Dictionary<string, string>[] items)
		=> (Paging, Items) = (paging, items);
}

[Serializable]
public class BaseQueryPagingResponse
{
	[JsonPropertyName("size")]
	[JsonPropertyOrder(0)]
	public int Size { get; init; }

	[JsonPropertyName("last")]
	[JsonPropertyOrder(1)]
	public string Last { get; init; }

	public BaseQueryPagingResponse(int size, string last)
		=> (Size, Last) = (size, last);
}
