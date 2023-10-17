// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base;

internal class BaseInsertResponse
{
	[JsonPropertyName("key")]
	[JsonPropertyOrder(0)]
	public string Key { get; init; }

	public BaseInsertResponse(string key) => Key = key;
}
