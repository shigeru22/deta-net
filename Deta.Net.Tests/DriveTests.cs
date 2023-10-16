// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Reflection;
using Deta.Net.Drive;

namespace Deta.Net.Tests;

public class DriveTests
{
	[Fact]
	public async void PutItemTestByFileAsync()
	{
		string filePath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/1-file.jpg";

		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		PutResponse temp = await drv.PutAsync("/1-file.jpg", filePath);

		Assert.Equal("/1-file.jpg", temp.Name);
	}

	[Fact]
	public async void PutItemTestByDataAsync()
	{
		string filePath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/1-byte.jpg";
		byte[] data = await File.ReadAllBytesAsync(filePath);

		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		PutResponse temp = await drv.PutAsync("1-byte.jpg", data);

		Assert.Equal("/1-byte.jpg", temp.Name);
	}

	[Fact]
	public async void UploadItemTestAsync()
	{
		string filePath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/2.jpg";

		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		EndUploadResponse temp = await drv.UploadAsync("/2.jpg", filePath);

		Assert.Equal("/2.jpg", temp.Name);
	}

	[Fact]
	public async void GetItemTestAsync()
	{
		string filePath = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/1-byte.jpg";
		byte[] data = await File.ReadAllBytesAsync(filePath);

		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		GetResponse temp = await drv.GetAsync("/1-file.jpg");

		Assert.Equal(data, temp.Content);
	}

	[Fact]
	public async void DeleteSingleItemTestAsync()
	{
		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		DeleteResponse temp = await drv.DeleteAsync("/1-byte.jpg");

		Assert.Equal("1-byte.jpg", temp.Deleted[0]);
	}

	[Fact]
	public async void DeleteMultipleItemsTestAsync()
	{
		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		DeleteResponse temp = await drv.DeleteAsync(new string[] { "/1-file.jpg", "/1-byte.jpg" });

		// random?
		Assert.Equal("1-file.jpg", temp.Deleted[0]);
		Assert.Equal("1-byte.jpg", temp.Deleted[1]);
	}

	[Fact]
	public async void DeleteByPayloadTestAsync()
	{
		DeletePayload payload = new DeletePayload(new string[] { "/1-file.jpg", "/1-byte.jpg" });

		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		DeleteResponse temp = await drv.DeleteAsync(payload);

		// random?
		Assert.Equal("1-file.jpg", temp.Deleted[0]);
		Assert.Equal("1-byte.jpg", temp.Deleted[1]);
	}

	[Fact]
	public async void ListItemsTestAsync()
	{
		ListOptions options = new ListOptions()
		{
			Prefix = "1-",
			Limit = 1
		};

		DetaDrive drv = DetaConfiguration.Instance.GetDriveInstance();
		ListResponse temp = await drv.ListAsync(options);

		Assert.Equal("1-file.jpg", temp.Names[0]);
		Assert.Equal("1-byte.jpg", temp.Names[1]);
	}
}