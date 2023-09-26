// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

public struct FetchOptions
{
	[JsonPropertyName("limit")] public int? Limit { get; set; }
	[JsonPropertyName("last")] public string? Last { get; set; }
	[JsonPropertyName("desc")] public bool? Desc { get; set; }
}
