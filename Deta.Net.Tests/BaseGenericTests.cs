// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;

namespace Deta.Net.Tests;

[TestCaseOrderer("Deta.Net.Tests.TestPriorityAttribute", "Deta.Net.Tests")]
public class BaseGenericTests
{
	[Fact]
	[Priority(1)]
	public async void PutItemGenericTestAsync()
	{
		PutData[] putItems = new PutData[]
		{
			new PutData() { Key = "A001", Id = "A001", FirstName = "John", LastName = "Doe" },
			new PutData() { Key = "A002", Id = "A002", FirstName = "Smitty", LastName = "Werbenjagermanjensen" },
			new PutData() { Key = "A003", Id = "A003", FirstName = "What Zit", LastName = "Tooya" }
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		PutResponse<PutData> temp = await db.PutAsync(putItems);

		Assert.Equal(temp.Processed.Items[0].Key, putItems[0].Key);
		Assert.Equal(temp.Processed.Items[0].Id, putItems[0].Id);
		Assert.Equal(temp.Processed.Items[0].FirstName, putItems[0].FirstName);
		Assert.Equal(temp.Processed.Items[0].LastName, putItems[0].LastName);

		Assert.Equal(temp.Processed.Items[1].Key, putItems[1].Key);
		Assert.Equal(temp.Processed.Items[1].Id, putItems[1].Id);
		Assert.Equal(temp.Processed.Items[1].FirstName, putItems[1].FirstName);
		Assert.Equal(temp.Processed.Items[1].LastName, putItems[1].LastName);

		Assert.Equal(temp.Processed.Items[2].Key, putItems[2].Key);
		Assert.Equal(temp.Processed.Items[2].Id, putItems[2].Id);
		Assert.Equal(temp.Processed.Items[2].FirstName, putItems[2].FirstName);
		Assert.Equal(temp.Processed.Items[2].LastName, putItems[2].LastName);
	}

	[Fact]
	[Priority(2)]
	public async void GetItemGenericTestAsync()
	{
		GetData[] getItems = new GetData[]
		{
			new GetData() { Key = "A001", Id = "A001", FirstName = "John", LastName = "Doe" },
			new GetData() { Key = "A002", Id = "A002", FirstName = "Smitty", LastName = "Werbenjagermanjensen" },
			new GetData() { Key = "A003", Id = "A003", FirstName = "What Zit", LastName = "Tooya" }
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		GetData? temp = await db.GetAsync<GetData>("A001");

		if (temp != null)
		{
			Assert.Equal(temp.Key, getItems[0].Key);
			Assert.Equal(temp.Id, getItems[0].Id);
			Assert.Equal(temp.FirstName, getItems[0].FirstName);
			Assert.Equal(temp.LastName, getItems[0].LastName);
		}
		else
		{
			Console.WriteLine("Data not yet inserted to Base.");
			Assert.Equal("not null", "temp");
		}
	}

	[Fact]
	[Priority(3)]
	public async void InsertItemGenericTestAsync()
	{
		PutData insertItem = new PutData()
		{
			Key = "A004",
			Id = "A004",
			FirstName = "Tennoji",
			LastName = "Rina"
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		string temp = await db.InsertAsync(insertItem);
		Assert.Equal(temp, insertItem.Key);
	}

	[Fact]
	[Priority(4)]
	public async void DeleteItemTestAsync()
	{
		QueryPayload payload = new QueryPayload()
		{
			Query = new Dictionary<string, string>[]
			{
				new Dictionary<string, string>(new KeyValuePair<string, string>[]
				{
					new KeyValuePair<string, string>("key?pfx", "A")
				})
			}
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();

		QueryResponse before = await db.QueryAsync(payload);

		if (before.Items.Length != 4)
		{
			Console.WriteLine("before.Items.Length must be 4. Re-run Put and Insert tests before running this test.");
			Assert.Equal(4, before.Items.Length);
		}

		await db.DeleteAsync("A001");
		await db.DeleteAsync("A002");
		await db.DeleteAsync("A003");
		await db.DeleteAsync("A004");

		QueryResponse after = await db.QueryAsync(payload);

		Assert.Empty(after.Items);
	}
}
