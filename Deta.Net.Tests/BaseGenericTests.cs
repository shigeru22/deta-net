// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;

namespace Deta.Net.Tests;

[TestCaseOrderer("Deta.Net.Tests.TestPriorityAttribute", "Deta.Net.Tests")]
public class BaseGenericTests : IClassFixture<TestGenericBaseFixture>
{
	// note: since these tests involve persistent data store,
	// these cases may need to be run as a whole in order

	private readonly TestGenericBaseFixture context;

	// test cases

	// 1 - put, with generic types
	private readonly PutData[] tc1 = new PutData[]
	{
		new PutData() { Key = "A001", Id = "A001", FirstName = "Tennoji", LastName = "Rina" },
		new PutData() { Key = "A002", Id = "A002", FirstName = "Heanna", LastName = "Sumire" }
	};

	// 2 - get, with generic types
	private readonly string tc2 = "A002";

	// 3 - insert, with generic types
	private readonly InsertData tc3 = new InsertData() { Key = "B001", Id = "B001", FirstName = "Kunikida", LastName = "Hanamaru" };

	// expectations (base contents on each actions)

	// 1 - put (tc1, should insert two records with id "A001" and "A002")
	private readonly PutData[] exp1 = new PutData[]
	{
		new PutData() { Key = "A001", Id = "A001", FirstName = "Tennoji", LastName = "Rina" },
		new PutData() { Key = "A002", Id = "A002", FirstName = "Heanna", LastName = "Sumire" }
	};

	// 2 - get (tc2, should return this record)
	private readonly GetData exp2 = new GetData() { Key = "A002", Id = "A002", FirstName = "Heanna", LastName = "Sumire" };

	// 3 - insert (tc3, should return these records after insertion)
	// exp3 uses data from QueryAsync() method, hence the type
	private readonly Dictionary<string, object>[] exp3 = new Dictionary<string, object>[]
	{
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "A001"),
			new KeyValuePair<string, object>("id", "A001"),
			new KeyValuePair<string, object>("firstName", "Tennoji"),
			new KeyValuePair<string, object>("lastName", "Rina")
		}),
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "A002"),
			new KeyValuePair<string, object>("id", "A002"),
			new KeyValuePair<string, object>("firstName", "Heanna"),
			new KeyValuePair<string, object>("lastName", "Sumire")
		}),
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "B001"),
			new KeyValuePair<string, object>("id", "B001"),
			new KeyValuePair<string, object>("firstName", "Kunikida"),
			new KeyValuePair<string, object>("lastName", "Hanamaru")
		})
	};

	// test actions:
	// put -> get -> insert*
	// * = query and compare after action

	public BaseGenericTests(TestGenericBaseFixture context)
		=> this.context = context;

	[Fact]
	[Priority(1)]
	public async void PutItemGenericTestAsync()
	{
		BasePutResponse<PutData> temp = await context.db.PutAsync(tc1);

		Assert.Equal(exp1[0].Key, temp.Processed.Items[0].Key);
		Assert.Equal(exp1[0].Id, temp.Processed.Items[0].Id);
		Assert.Equal(exp1[0].FirstName, temp.Processed.Items[0].FirstName);
		Assert.Equal(exp1[0].LastName, temp.Processed.Items[0].LastName);

		Assert.Equal(exp1[1].Key, temp.Processed.Items[1].Key);
		Assert.Equal(exp1[1].Id, temp.Processed.Items[1].Id);
		Assert.Equal(exp1[1].FirstName, temp.Processed.Items[1].FirstName);
		Assert.Equal(exp1[1].LastName, temp.Processed.Items[1].LastName);
	}

	[Fact]
	[Priority(2)]
	public async void GetItemGenericTestAsync()
	{
		GetData temp = await context.db.GetAsync<GetData>(tc2)
			?? throw new InvalidOperationException($"Item with key '{tc2}' not found in Base.");

		Assert.Equal(exp2.Key, temp.Key);
		Assert.Equal(exp2.Id, temp.Id);
		Assert.Equal(exp2.FirstName, temp.FirstName);
		Assert.Equal(exp2.LastName, temp.LastName);
	}

	[Fact]
	[Priority(3)]
	public async void InsertItemGenericTestAsync()
	{
		string insertedKey = await context.db.InsertAsync(tc3);
		Assert.Equal(tc3.Key, insertedKey);

		BaseQueryResponse tempAllItems = await context.db.QueryAsync();

		if (tempAllItems.Items.Length != exp3.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp3 expectation.");
		}

		AssertAllItems(exp3, tempAllItems.Items);
	}

	private void AssertAllItems(Dictionary<string, object>[] expected, Dictionary<string, object>[] actual)
	{
		int expLength = expected.Length;
		if (expLength != actual.Length)
		{
			throw new InvalidOperationException("Invalid data count. Check for data length before executing this method.");
		}

		Dictionary<string, object>[] sortExpected = expected.OrderBy(item => item["id"].ToString()).ToArray();
		Dictionary<string, object>[] sortActual = actual.OrderBy(item => item["id"].ToString()).ToArray();

		for (int i = 0; i < expLength; i++)
		{
			Assert.Equal(sortExpected[i]["key"].ToString(), sortActual[i]["key"].ToString());
			Assert.Equal(sortExpected[i]["id"].ToString(), sortActual[i]["id"].ToString());
			Assert.Equal(sortExpected[i]["firstName"].ToString(), sortActual[i]["firstName"].ToString());
			Assert.Equal(sortExpected[i]["lastName"].ToString(), sortActual[i]["lastName"].ToString());
		}
	}
}
