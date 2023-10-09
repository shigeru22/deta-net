// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class UpdateResponse
{
	[JsonPropertyName("key")]
	[JsonPropertyOrder(int.MinValue)]
	public string Key { get; init; }

	[JsonPropertyName("set")]
	[JsonPropertyOrder(0)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, object>? Set { get; init; }

	[JsonPropertyName("delete")]
	[JsonPropertyOrder(1)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string[]? Delete { get; init; }

	public UpdateResponse(string key, Dictionary<string, object>? set = null, string[]? delete = null)
		=> (Key, Set, Delete) = (key, set, delete);
}
