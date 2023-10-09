// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;
using Microsoft.Extensions.Configuration;

namespace Deta.Net.Tests;

internal class DetaConfiguration
{
	private readonly static DetaConfiguration instance = new DetaConfiguration();
	public static DetaConfiguration Instance => instance;

	private readonly IConfigurationRoot config;

	private readonly string apiKey;

	private DetaConfiguration()
	{
		config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", true, true)
			.Build();

		apiKey = config["apiKey"]
			?? Environment.GetEnvironmentVariable("DETA_PROJECT_KEY")
				?? throw new InvalidOperationException("Either 'apiKey' value in appsettings.json or DETA_PROJECT_KEY environment variable must be set for this test.");
	}

	public DetaBase GetBaseInstance() => new Deta(apiKey).GetBase("test-base");
}
