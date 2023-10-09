// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class UpdateItem
{
	[JsonPropertyName("set")]
	[JsonPropertyOrder(0)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, object>? Set { get; set; }

	[JsonPropertyName("increment")]
	[JsonPropertyOrder(1)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, int>? Increment { get; set; }

	[JsonPropertyName("append")]
	[JsonPropertyOrder(2)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, object[]>? Append { get; set; }

	[JsonPropertyName("prepend")]
	[JsonPropertyOrder(3)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, object[]>? Prepend { get; set; }

	[JsonPropertyName("delete")]
	[JsonPropertyOrder(4)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string[]? Delete { get; set; }

	public UpdateItem(Dictionary<string, object>? set = null,
			Dictionary<string, int>? increment = null,
			Dictionary<string, object[]>? append = null,
			Dictionary<string, object[]>? prepend = null,
			string[]? delete = null)
		=> (Set, Increment, Append, Prepend, Delete) = (set, increment, append, prepend, delete);
}
