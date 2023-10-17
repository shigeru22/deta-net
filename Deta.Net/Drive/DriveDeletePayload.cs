// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class DriveDeletePayload
{
	[JsonPropertyName("names")]
	[JsonPropertyOrder(0)]
	public string[] Names { get; set; }

	public DriveDeletePayload(string name) => Names = new string[] { name };
	public DriveDeletePayload(string[] names) => Names = names;
}