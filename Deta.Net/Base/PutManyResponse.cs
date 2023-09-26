// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

public readonly struct PutManyResponse
{
	[JsonPropertyName("processed")] public PutManyResponseItems Processed { get; init; }
}

public readonly struct PutManyResponse<T>
{
	[JsonPropertyName("processed")] public PutManyResponseItems<T> Processed { get; init; }
}

public readonly struct PutManyResponseItems
{
	[JsonPropertyName("items")] public object[] Items { get; init; }
}

public readonly struct PutManyResponseItems<T>
{
	[JsonPropertyName("items")] public T[] Items { get; init; }
}
