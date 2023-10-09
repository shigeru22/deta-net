// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

internal class InsertResponse
{
	[JsonPropertyName("key")] public string Key { get; init; }

	public InsertResponse(string key) => Key = key;
}
