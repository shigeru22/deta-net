// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class QueryResponse
{
	[JsonPropertyName("paging")] public QueryPagingResponse Paging { get; init; }
	[JsonPropertyName("items")] public Dictionary<string, string>[] Items { get; init; }

	public QueryResponse(QueryPagingResponse paging, Dictionary<string, string>[] items)
		=> (Paging, Items) = (paging, items);
}

[Serializable]
public class QueryPagingResponse
{
	[JsonPropertyName("size")] public int Size { get; init; }
	[JsonPropertyName("last")] public string Last { get; init; }

	public QueryPagingResponse(int size, string last)
		=> (Size, Last) = (size, last);
}
