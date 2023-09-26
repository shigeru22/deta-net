// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

public readonly struct FetchResponse
{
	[JsonPropertyName("count")] public int Count { get; init; }
	[JsonPropertyName("last")] public string Last { get; init; }
	[JsonPropertyName("items")] public object[] Items { get; init; }
}

public readonly struct FetchResponse<T>
{
	[JsonPropertyName("count")] public int Count { get; init; }
	[JsonPropertyName("last")] public string Last { get; init; }
	[JsonPropertyName("items")] public T[] Items { get; init; }
}
