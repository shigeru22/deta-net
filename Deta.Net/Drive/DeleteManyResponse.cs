// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

public readonly struct DeleteManyResponse
{
	[JsonPropertyName("deleted")] public string[] Deleted { get; init; }
	[JsonPropertyName("failed")] public Dictionary<string, string> Failed { get; init; } // TODO: test json serialization result
}
