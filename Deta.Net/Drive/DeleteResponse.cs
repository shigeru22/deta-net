// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class DeleteResponse
{
	[JsonPropertyName("deleted")]
	[JsonPropertyOrder(0)]
	public string[] Deleted { get; init; }

	[JsonPropertyName("failed")]
	[JsonPropertyOrder(1)]
	public Dictionary<string, string> Failed { get; init; }

	public DeleteResponse(string[] deleted, Dictionary<string, string> failed)
		=> (Deleted, Failed) = (deleted, failed);
}