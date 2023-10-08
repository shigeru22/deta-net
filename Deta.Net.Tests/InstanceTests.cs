// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;
using Deta.Net.Drive;

namespace Deta.Net.Tests;

public class InstanceTests
{
	[Fact]
	public void InstanceCreationTest()
	{
		Deta detaInstance;
		try
		{
			detaInstance = new Deta();
		}
		catch (InvalidOperationException)
		{
			Console.WriteLine("DETA_PROJECT_KEY environment variable not set. Unable to test initialization without key.");
			return;
		}

		Assert.NotNull(detaInstance);
	}

	[Theory]
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
	public void BaseCreationTest()
	{
		Deta detaInstance = new Deta("test1_rndKey");
		DetaBase db = detaInstance.GetBase("test_base");
		Assert.NotNull(db);
	}

	[Fact]
	public void DriveCreationTest()
	{
		Deta detaInstance = new Deta("test1_rndKey");
		DetaDrive drv = detaInstance.GetDrive("test_drive");
		Assert.NotNull(drv);
	}
}
