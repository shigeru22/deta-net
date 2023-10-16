// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class PutResponse
{
	[JsonPropertyName("name")]
	[JsonPropertyOrder(0)]
	public string Name { get; init; }

	[JsonPropertyName("project_id")]
	[JsonPropertyOrder(1)]
	public string ProjectId { get; init; }

	[JsonPropertyName("drive_name")]
	[JsonPropertyOrder(2)]
	public string DriveName { get; init; }

	public PutResponse(string name, string projectId, string driveName)
		=> (Name, ProjectId, DriveName) = (name, projectId, driveName);
}
