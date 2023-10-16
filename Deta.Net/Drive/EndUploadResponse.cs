// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Drive;

public class EndUploadResponse
{
	[JsonPropertyName("name")]
	[JsonPropertyOrder(0)]
	public string Name { get; init; }

	[JsonPropertyName("upload_id")]
	[JsonPropertyOrder(1)]
	public string UploadId { get; init; }

	[JsonPropertyName("project_id")]
	[JsonPropertyOrder(2)]
	public string ProjectId { get; init; }

	[JsonPropertyName("drive_name")]
	[JsonPropertyOrder(3)]
	public string DriveName { get; init; }

	public EndUploadResponse(string name, string uploadId, string projectId, string driveName)
		=> (Name, UploadId, ProjectId, DriveName) = (name, uploadId, projectId, driveName);
}
