// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class PutResponse
{
	[JsonPropertyName("processed")] public PutResponseItems Processed { get; init; }
	[JsonPropertyName("failed")] public PutResponseItems Failed { get; init; }

	public PutResponse(PutResponseItems processed, PutResponseItems failed)
		=> (Processed, Failed) = (processed, failed);
}

[Serializable]
public class PutResponse<T>
{
	[JsonPropertyName("processed")] public PutResponseItems<T> Processed { get; init; }
	[JsonPropertyName("failed")] public PutResponseItems<T> Failed { get; init; }

	public PutResponse(PutResponseItems<T> processed, PutResponseItems<T> failed)
		=> (Processed, Failed) = (processed, failed);
}

[Serializable]
public class PutResponseItems
{
	[JsonPropertyName("items")] public Dictionary<string, object>[] Items { get; init; }

	public PutResponseItems(Dictionary<string, object>[] items) => Items = items;
}

[Serializable]
public class PutResponseItems<T>
{
	[JsonPropertyName("items")] public T[] Items { get; init; }

	public PutResponseItems(T[] items) => Items = items;
}
