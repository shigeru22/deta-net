// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class DeletePayload
{
	[JsonPropertyName("names")]
	[JsonPropertyOrder(0)]
	public string[] Names { get; set; }

	public DeletePayload(string name) => Names = new string[] { name };
	public DeletePayload(string[] names) => Names = names;
}