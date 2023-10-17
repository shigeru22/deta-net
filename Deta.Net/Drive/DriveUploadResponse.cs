// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

[Serializable]
public class DriveUploadResponse
{
	[JsonPropertyName("name")]
	[JsonPropertyOrder(0)]
	public string Name { get; init; }

	[JsonPropertyName("upload_id")]
	[JsonPropertyOrder(1)]
	public string UploadId { get; init; }

	[JsonPropertyName("part")]
	[JsonPropertyOrder(2)]
	public int Part { get; init; }

	[JsonPropertyName("project_id")]
	[JsonPropertyOrder(3)]
	public string ProjectId { get; init; }

	[JsonPropertyName("drive_name")]
	[JsonPropertyOrder(4)]
	public string DriveName { get; init; }

	public DriveUploadResponse(string name, string uploadId, int part, string projectId, string driveName)
		=> (Name, UploadId, Part, ProjectId, DriveName) = (name, uploadId, part, projectId, driveName);
}
