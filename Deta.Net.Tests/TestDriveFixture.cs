// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Drive;

namespace Deta.Net.Tests;

public class TestDriveFixture : IDisposable
{
	internal readonly DetaDrive drv = DetaConfiguration.Instance.DriveTestInstance;

	public TestDriveFixture()
	{
		// pre-run: delete everything
		Task.Run(DeleteAllItemsAsync).Wait();
	}

	public void Dispose()
	{
		// post-run: also delete everything
		Task.Run(DeleteAllItemsAsync).Wait();
		GC.SuppressFinalize(this);
	}

	private async Task DeleteAllItemsAsync()
	{
		_ = await drv.DeleteAsync("/1-file.jpg");
		_ = await drv.DeleteAsync("/1-byte.jpg");
		_ = await drv.DeleteAsync("/2.jpg");
	}
}
