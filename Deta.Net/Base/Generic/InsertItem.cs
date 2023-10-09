// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base.Generic;

[Serializable]
public abstract class InsertItem
{
	[JsonPropertyName("key")]
	[JsonPropertyOrder(int.MinValue)]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Key { get; set; }
}