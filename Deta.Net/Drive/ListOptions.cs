// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

public struct ListOptions
{
	[JsonPropertyName("recursive")] public bool? Recursive { get; set; }
	[JsonPropertyName("prefix")] public string? Prefix { get; set; }
	[JsonPropertyName("limit")] public int? Limit { get; set; }
	[JsonPropertyName("last")] public string? Last { get; set; }
}
