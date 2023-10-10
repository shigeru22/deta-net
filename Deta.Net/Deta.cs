// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;
using Deta.Net.Drive;

namespace Deta.Net;

public class Deta
{
	private readonly string apiKey;
	private readonly string projectId;

	public Deta()
	{
		apiKey = Environment.GetEnvironmentVariable("DETA_PROJECT_KEY")
			?? throw new InvalidOperationException("DETA_PROJECT_KEY environment variable not found. Add the variable or use Deta(string) instead.");
		apiKey = apiKey.Trim();

		if (string.IsNullOrWhiteSpace(apiKey))
		{
			throw new InvalidOperationException("DETA_PROJECT_KEY environment variable must not be empty.");
		}

		projectId = apiKey.Split('_')[0];
	}

	public Deta(string apiKey)
	{
		this.apiKey = apiKey;
		this.apiKey = this.apiKey.Trim();

		if (string.IsNullOrWhiteSpace(this.apiKey))
		{
			throw new InvalidOperationException("apiKey must not be empty.");
		}

		projectId = apiKey.Split('_')[0];
	}

	public DetaBase GetBase(string baseName) => new DetaBase(apiKey, baseName);
	public DetaDrive GetDrive(string driveName) => new DetaDrive(apiKey, driveName);
}
