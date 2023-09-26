// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;
using Deta.Net.Drive;

namespace Deta.Net;

public class Deta
{
	private string apiKey { get; init; }

	public Deta()
	{
		apiKey = Environment.GetEnvironmentVariable("DETA_PROJECT_KEY")
			?? throw new InvalidOperationException("DETA_PROJECT_KEY environment variable not found. Add the variable or use Deta(string) instead.");
	}

	public Deta(string apiKey) => this.apiKey = apiKey;

	public DetaBase GetBase(string baseName, string? host) => new DetaBase(baseName, host);
	public DetaDrive GetDrive(string driveName, string? host) => new DetaDrive(driveName, host);
}
