// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Reflection;
using Deta.Net.Drive;

namespace Deta.Net.Tests;

[TestCaseOrderer("Deta.Net.Tests.TestPriorityAttribute", "Deta.Net.Tests")]
public class DriveTests : IClassFixture<TestDriveFixture>
{
	// note: since these tests involve persistent data store,
	// these cases may need to be run as a whole in order

	private readonly TestDriveFixture context;

	// test cases

	// 1 - put item by file
	private readonly string tc1_path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/1-file.jpg";
	private readonly string tc1_name = "/1-file.jpg";

	// 2 - get item
	private readonly string tc2_path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/1-file.jpg";
	private readonly string tc2_name = "/1-file.jpg";

	// 3 - put item by data chunks
	private readonly string tc3_path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/1-byte.jpg";
	private readonly string tc3_name = "/1-byte.jpg";

	// 4 - list items
	private readonly DriveListOptions tc4 = new DriveListOptions()
	{
		Prefix = "1-",
		Limit = 2
	};

	// 5 - upload item
	private readonly string tc5_path = $"{Path.GetDirectoryName(Assembly.GetEntryAssembly()?.Location)}/Files/2.jpg";
	private readonly string tc5_name = "/2.jpg";

	// 6 - delete single item
	private readonly string tc6_name = "/2.jpg";

	// 7 - delete multiple items
	private readonly string[] tc7_items = new string[] { "/1-byte.jpg", "/1-file.jpg" };

	// expectations (drive contents on each actions)

	// expectations for some cases are their file path's contents,
	// these should not use instance variables as they could occupy system memory.
	// load each file inside each test instead

	// 1 - put item by file (should insert item named from action 1)
	private readonly string exp1 = "/1-file.jpg";

	// 2 - get item (should return item from action 1)

	// 3 - put item by data (should insert item named from action 3)
	private readonly string exp3 = "/1-byte.jpg";

	// 4 - list items (should list items from action 1 and 3)
	private readonly string[] exp4 = new string[] { "1-byte.jpg", "1-file.jpg" };

	// 5 - upload item (should insert item named from action 5)
	private readonly string exp5 = "/2.jpg";

	// 6 - delete single item (should delete item from action 5)
	private readonly string exp6_delName = "2.jpg";
	private readonly string[] exp6_fileList = new string[] { "1-byte.jpg", "1-file.jpg" };

	// 7 - delete multiple items
	private readonly string[] exp7_delNames = new string[] { "1-byte.jpg", "1-file.jpg" };
	// private readonly string[] exp7_fileList = Array.Empty<string>(); // use Assert.Empty() instead

	// test actions:
	// put (file) -> get -> put (data) [*1] -> list -> upload [*1] -> delete (single) [*2] -> delete (multiple) [*2]
	// [*1] = get after each action
	// [*2] = list after each action

	public DriveTests(TestDriveFixture context)
		=> this.context = context;

	[Fact]
	[Priority(1)]
	public async void PutItemTestByFileAsync()
	{
		DrivePutResponse temp = await context.drv.PutAsync(tc1_name, tc1_path);
		Assert.Equal(exp1, temp.Name);
	}

	[Fact]
	[Priority(2)]
	public async void GetItemTestAsync()
	{
		byte[] exp2_data = await File.ReadAllBytesAsync(tc2_path);
		DriveGetResponse temp = await context.drv.GetAsync(tc2_name);
		Assert.Equal(exp2_data, temp.Content);
	}

	[Fact]
	[Priority(3)]
	public async void PutItemTestByDataAsync()
	{
		byte[] data = await File.ReadAllBytesAsync(tc3_path);
		DrivePutResponse temp = await context.drv.PutAsync(tc3_name, data);
		Assert.Equal(exp3, temp.Name);
	}

	[Fact]
	[Priority(4)]
	public async void ListItemsTestAsync()
	{
		DriveListResponse temp = await context.drv.ListAsync(tc4);

		if (temp.Names.Length != exp4.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp4 expectation.");
		}

		AssertAllItems(exp4, temp.Names);
	}

	[Fact]
	[Priority(5)]
	public async void UploadItemTestAsync()
	{
		// also tests StartUploadAsync, UploadPartAsync, and EndUploadAsync
		DriveEndUploadResponse temp = await context.drv.UploadAsync(tc5_name, tc5_path);
		Assert.Equal(exp5, temp.Name);
	}


	[Fact]
	[Priority(6)]
	public async void DeleteSingleItemTestAsync()
	{
		DriveDeleteResponse temp = await context.drv.DeleteAsync(tc6_name); // also tests DetaDrivePayload
		Assert.Equal(exp6_delName, temp.Deleted[0]);

		DriveListResponse filesList = await context.drv.ListAsync();

		if (filesList.Names.Length != exp6_fileList.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp6_fileList expectation.");
		}

		AssertAllItems(exp6_fileList, filesList.Names);
	}

	[Fact]
	[Priority(7)]
	public async void DeleteMultipleItemsTestAsync()
	{
		DriveDeleteResponse temp = await context.drv.DeleteAsync(tc7_items); // also tests DetaDrivePayload
		AssertAllItems(exp7_delNames, temp.Deleted);

		DriveListResponse filesList = await context.drv.ListAsync();
		Assert.Empty(filesList.Names);
	}

	private void AssertAllItems(string[] expected, string[] actual)
	{
		int expLength = expected.Length;
		if (expLength != actual.Length)
		{
			throw new InvalidOperationException("Invalid data count. Check for data length before executing this method.");
		}

		string[] sortExpected = expected.OrderBy(item => item).ToArray();
		string[] sortActual = actual.OrderBy(item => item).ToArray();

		for (int i = 0; i < expLength; i++)
		{
			Assert.Equal(sortExpected[i], sortActual[i]);
		}
	}
}