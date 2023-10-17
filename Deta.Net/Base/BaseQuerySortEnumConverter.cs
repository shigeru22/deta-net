// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json;
using System.Text.Json.Serialization;

namespace Deta.Net.Base;

internal class BaseQuerySortEnumConverter : JsonConverter<BaseQuerySort>
{
	public override BaseQuerySort Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
		=> reader.GetString() switch
		{
			"asc" => BaseQuerySort.Ascending,
			"desc" => BaseQuerySort.Descending,
			_ => throw new NotSupportedException("Query sort data must be either 'asc' or 'desc'.")
		};

	public override void Write(Utf8JsonWriter writer, BaseQuerySort value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value switch
		{
			BaseQuerySort.Ascending => "asc",
			BaseQuerySort.Descending => "desc",
			_ => throw new NotSupportedException("Query sort value must be in QuerySort enum.")
		});
}
