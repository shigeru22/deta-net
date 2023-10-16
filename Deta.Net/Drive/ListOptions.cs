// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class ListOptions
{
	[JsonPropertyName("recursive")]
	[JsonPropertyOrder(0)]
	public bool? Recursive { get; set; }

	[JsonPropertyName("prefix")]
	[JsonPropertyOrder(1)]
	public string? Prefix { get; set; }

	[JsonPropertyName("limit")]
	[JsonPropertyOrder(2)]
	public int? Limit { get; set; }

	[JsonPropertyName("last")]
	[JsonPropertyOrder(3)]
	public string? Last { get; set; }

	public ListOptions(bool? recursive = null, string? prefix = null, int? limit = null, string? last = null)
		=> (Recursive, Prefix, Limit, Last) = (recursive, prefix, limit, last);
}
