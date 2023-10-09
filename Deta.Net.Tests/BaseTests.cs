// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;

namespace Deta.Net.Tests;

[TestCaseOrderer("Deta.Net.Tests.TestPriorityAttribute", "Deta.Net.Tests")]
public class BaseTests
{
	[Fact]
	[Priority(1)]
	public async void PutItemTestAsync()
	{
		Dictionary<string, object>[] putItems = new Dictionary<string, object>[]
		{
			new Dictionary<string, object>(new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("key", "A001"),
				new KeyValuePair<string, object>("id", "A001"),
				new KeyValuePair<string, object>("firstName", "John"),
				new KeyValuePair<string, object>("lastName", "Doe")
			}),
			new Dictionary<string, object>(new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("key", "A002"),
				new KeyValuePair<string, object>("id", "A002"),
				new KeyValuePair<string, object>("firstName", "Smitty"),
				new KeyValuePair<string, object>("lastName", "Werbenjagermanjensen")
			}),
			new Dictionary<string, object>(new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("key", "A003"),
				new KeyValuePair<string, object>("id", "A003"),
				new KeyValuePair<string, object>("firstName", "What Zit"),
				new KeyValuePair<string, object>("lastName", "Tooya")
			})
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		PutResponse temp = await db.PutAsync(putItems);

		Assert.Equal(temp.Processed.Items[0]["key"].ToString(), putItems[0]["key"].ToString());
		Assert.Equal(temp.Processed.Items[0]["id"].ToString(), putItems[0]["id"].ToString());
		Assert.Equal(temp.Processed.Items[0]["firstName"].ToString(), putItems[0]["firstName"].ToString());
		Assert.Equal(temp.Processed.Items[0]["lastName"].ToString(), putItems[0]["lastName"].ToString());

		Assert.Equal(temp.Processed.Items[1]["key"].ToString(), putItems[1]["key"].ToString());
		Assert.Equal(temp.Processed.Items[1]["id"].ToString(), putItems[1]["id"].ToString());
		Assert.Equal(temp.Processed.Items[1]["firstName"].ToString(), putItems[1]["firstName"].ToString());
		Assert.Equal(temp.Processed.Items[1]["lastName"].ToString(), putItems[1]["lastName"].ToString());

		Assert.Equal(temp.Processed.Items[2]["key"].ToString(), putItems[2]["key"].ToString());
		Assert.Equal(temp.Processed.Items[2]["id"].ToString(), putItems[2]["id"].ToString());
		Assert.Equal(temp.Processed.Items[2]["firstName"].ToString(), putItems[2]["firstName"].ToString());
		Assert.Equal(temp.Processed.Items[2]["lastName"].ToString(), putItems[2]["lastName"].ToString());
	}

	[Fact]
	[Priority(2)]
	public async void GetItemTestAsync()
	{
		Dictionary<string, object>[] getItems = new Dictionary<string, object>[]
		{
			new Dictionary<string, object>(new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("key", "A001"),
				new KeyValuePair<string, object>("id", "A001"),
				new KeyValuePair<string, object>("firstName", "John"),
				new KeyValuePair<string, object>("lastName", "Doe")
			})
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		Dictionary<string, object>? temp = await db.GetAsync("A001");

		if (temp != null)
		{
			Assert.Equal(temp["key"].ToString(), getItems[0]["key"].ToString());
			Assert.Equal(temp["id"].ToString(), getItems[0]["id"].ToString());
			Assert.Equal(temp["firstName"].ToString(), getItems[0]["firstName"].ToString());
			Assert.Equal(temp["lastName"].ToString(), getItems[0]["lastName"].ToString());
		}
		else
		{
			Console.WriteLine("Data not yet inserted to Base.");
			Assert.Equal("not null", "temp");
		}
	}

	[Fact]
	[Priority(3)]
	public async void InsertItemTestAsync()
	{
		Dictionary<string, object> insertItem = new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "A004"),
			new KeyValuePair<string, object>("id", "A004"),
			new KeyValuePair<string, object>("firstName", "Tennoji"),
			new KeyValuePair<string, object>("lastName", "Rina")
		});

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();

		string temp = await db.InsertAsync(insertItem);
		Assert.Equal(temp, insertItem["key"].ToString());

		_ = await Assert.ThrowsAsync<HttpRequestException>(async () =>
		{
			string temp = await db.InsertAsync(insertItem);
		});
	}

	[Fact]
	[Priority(4)]
	public async void UpdateItemTestAsync()
	{
		UpdateItem test = new UpdateItem()
		{
			Set = new Dictionary<string, object>(new KeyValuePair<string, object>[]
			{
				new KeyValuePair<string, object>("firstName", "What Zit")
			}),
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		UpdateResponse temp = await db.UpdateAsync("A003", test);

		Assert.Equal("A003", temp.Key);
		Assert.Equal(test.Set["firstName"].ToString(),
			temp.Set?["firstName"].ToString()
				?? throw new InvalidOperationException("Response 'set.firstName' value must not be null."));
	}

	[Fact]
	[Priority(5)]
	public async void QueryItemsTestAsync()
	{
		QueryPayload test = new QueryPayload()
		{
			Query = new Dictionary<string, string>[]
			{
				new Dictionary<string, string>(new KeyValuePair<string, string>[]
				{
					new KeyValuePair<string, string>("firstName", "John")
				})
			},
			Sort = QuerySort.Descending
		};

		DetaBase db = DetaConfiguration.Instance.GetBaseInstance();
		QueryResponse temp = await db.QueryAsync(test);

		Assert.Equal("A001", temp.Items[0]["key"].ToString());
		Assert.Equal("John", temp.Items[0]["firstName"].ToString());
		Assert.Equal("Doe", temp.Items[0]["lastName"].ToString());
	}

	[Fact]
	[Priority(6)]
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
