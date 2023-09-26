// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

public readonly struct ListResponse
{
	[JsonPropertyName("names")] public string[] Names { get; init; }
	[JsonPropertyName("paging")] public ListResponsePaging Paging { get; init; }
}

public readonly struct ListResponsePaging
{
	[JsonPropertyName("size")] public int Size { get; init; }
	[JsonPropertyName("last")] public string Last { get; init; }
}
