// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net;

public struct PutOptions
{
	[JsonPropertyName("data")] object? Data { get; set; } // check data types (string, byte[], or Buffer)
	[JsonPropertyName("path")] string? Path { get; set; }
	[JsonPropertyName("contentType")] string? ContentType { get; set; }
}
