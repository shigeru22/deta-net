// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class BaseQueryPayload
{
	[JsonPropertyName("query")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public Dictionary<string, string>[]? Query { get; set; }

	[JsonPropertyName("limit")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public int? Limit { get; set; }

	[JsonPropertyName("last")]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public string? Last { get; set; }

	[JsonPropertyName("sort")]
	[JsonConverter(typeof(BaseQuerySortEnumConverter))]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public BaseQuerySort? Sort { get; set; }

	public BaseQueryPayload(Dictionary<string, string>[]? query = null,
			int? limit = null,
			string? last = null,
			BaseQuerySort? sort = null)
		=> (Query, Limit, Last, Sort) = (query, limit, last, sort);
}
