// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;
using Deta.Net.Drive;

namespace Deta.Net.Tests;

[TestCaseOrderer("Deta.Net.Tests.TestPriorityAttribute", "Deta.Net.Tests")]
public class InstanceTests
{
	[Fact]
	[Priority(1)]
	public void InstanceCreationTest()
	{
		if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("DETA_PROJECT_KEY")))
		{
			Console.WriteLine("Unable to test instance creation with environment variable. Skipping test.");
			return;
		}

		Deta detaInstance = new Deta();
		Assert.NotNull(detaInstance);
	}

	[Theory]
	[Priority(2)]
	[InlineData("")]
	[InlineData("test_key")]
	public void InstanceCreationTestWithKey(string projectKey)
	{
		if (string.IsNullOrWhiteSpace(projectKey))
		{
			Assert.Throws<InvalidOperationException>(() =>
			{
				Deta detaInstance = new Deta(projectKey);
			});
		}
		else
		{
			Deta detaInstance = new Deta(projectKey);
			Assert.NotNull(detaInstance);
		}
	}

	[Fact]
	[Priority(3)]
	public void BaseCreationTest()
	{
		Deta detaInstance = new Deta("test1_rndKey");
		DetaBase db = detaInstance.GetBase("test_base");
		Assert.NotNull(db);
	}

	[Fact]
	[Priority(4)]
	public void DriveCreationTest()
	{
		Deta detaInstance = new Deta("test1_rndKey");
		DetaDrive drv = detaInstance.GetDrive("test_drive");
		Assert.NotNull(drv);
	}
}
