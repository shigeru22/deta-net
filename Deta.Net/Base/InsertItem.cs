// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public abstract class InsertItem
{
	[JsonPropertyName("key")]
	[JsonPropertyOrder(int.MinValue)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Key { get; set; }
}

[Serializable]
internal class InsertItemPayload<T>
{
	[JsonPropertyName("item")] public T Item { get; init; }

	public InsertItemPayload(T item)
	{
		Item = item;
	}
}