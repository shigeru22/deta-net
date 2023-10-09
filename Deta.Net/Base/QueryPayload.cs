// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

[Serializable]
public class QueryPayload
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
	[JsonConverter(typeof(QuerySortEnumConverter))]
	[JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
	public QuerySort? Sort { get; set; }

	public QueryPayload(Dictionary<string, string>[]? query = null,
			int? limit = null,
			string? last = null,
			QuerySort? sort = null)
		=> (Query, Limit, Last, Sort) = (query, limit, last, sort);
}
