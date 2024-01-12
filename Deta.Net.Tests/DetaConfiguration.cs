// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;
using Deta.Net.Drive;
using Microsoft.Extensions.Configuration;

namespace Deta.Net.Tests;

internal class DetaConfiguration
{
	private const string BASE_TEST_NAME = "test-base";
	private const string BASE_GENERIC_TEST_NAME = "test-base-generic";
	private const string DRIVE_TEST_NAME = "test-drive";

	private readonly static DetaConfiguration instance = new DetaConfiguration();
	public static DetaConfiguration Instance => instance;

	private readonly DetaInstance detaInstance;

	private DetaBase? baseTestInstance;
	private DetaBase? baseGenericTestInstance;
	private DetaDrive? driveTestInstance;

	public DetaBase BaseTestInstance
	{
		get
		{
			baseTestInstance ??= detaInstance.GetBase(BASE_TEST_NAME);
			return baseTestInstance;
		}
	}

	public DetaBase BaseGenericTestInstance
	{
		get
		{
			baseGenericTestInstance ??= detaInstance.GetBase(BASE_GENERIC_TEST_NAME);
			return baseGenericTestInstance;
		}
	}

	public DetaDrive DriveTestInstance
	{
		get
		{
			driveTestInstance ??= detaInstance.GetDrive(DRIVE_TEST_NAME);
			return driveTestInstance;
		}
	}

	private DetaConfiguration()
	{
		IConfigurationRoot config = new ConfigurationBuilder()
			.SetBasePath(Directory.GetCurrentDirectory())
			.AddJsonFile("appsettings.json", true, true)
			.Build();

		string apiKey = config["apiKey"]
			?? Environment.GetEnvironmentVariable("DETA_PROJECT_KEY")
				?? throw new InvalidOperationException("Either 'apiKey' value in appsettings.json or DETA_PROJECT_KEY environment variable must be set for this test.");

		detaInstance = new DetaInstance(apiKey);

		baseTestInstance = null;
		baseGenericTestInstance = null;
		driveTestInstance = null;
	}
}
