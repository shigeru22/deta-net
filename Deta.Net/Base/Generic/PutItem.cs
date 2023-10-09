// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base.Generic;

[Serializable]
public abstract class PutItem
{
	[JsonPropertyName("key")]
	[JsonPropertyOrder(int.MinValue)]
	public string? Key { get; set; }
}

[Serializable]
internal class PutItems<T>
{
	[JsonPropertyName("items")] public T[] Items { get; set; }

	public PutItems()
	{
		Items = Array.Empty<T>();
	}

	public PutItems(T[] items)
	{
		Items = items;
	}
}
