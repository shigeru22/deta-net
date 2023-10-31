// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;

namespace Deta.Net.Tests;

public class TestGenericBaseFixture : IDisposable
{
	internal readonly DetaBase db = DetaConfiguration.Instance.BaseGenericTestInstance;

	public TestGenericBaseFixture()
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
		await db.DeleteAsync("A001");
		await db.DeleteAsync("A002");
		await db.DeleteAsync("B001");
	}
}
