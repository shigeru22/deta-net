// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Deta.Net.Base;

namespace Deta.Net.Tests;

[TestCaseOrderer("Deta.Net.Tests.TestPriorityAttribute", "Deta.Net.Tests")]
public class BaseTests : IClassFixture<TestBaseFixture>
{
	// note: since these tests involve persistent data store,
	// these cases may need to be run as a whole in order

	private readonly TestBaseFixture context;

	// test cases

	// 1.1 - put, as array
	private readonly Dictionary<string, object>[] tc1_1 = new Dictionary<string, object>[]
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
		})
	};

	// 1.2 - put, without key
	private readonly Dictionary<string, object> tc1_2 = new Dictionary<string, object>(new KeyValuePair<string, object>[]
	{
		new KeyValuePair<string, object>("id", "B001"),
		new KeyValuePair<string, object>("firstName", "Watanabe"),
		new KeyValuePair<string, object>("lastName", "You")
	});

	// 2 - get
	private readonly string tc2 = "A002";

	// 3.1 - query all items (no test case, call QueryAsync() directly)

	// 3.2 - query specific items
	private readonly BaseQueryPayload tc3_2 = new BaseQueryPayload()
	{
		Query = new Dictionary<string, string>[]
		{
			new Dictionary<string, string>(new KeyValuePair<string, string>[]
			{
				new KeyValuePair<string, string>("id?pfx", "B")
			})
		}
	};

	// 4 - insert
	private readonly Dictionary<string, object> tc4 = new Dictionary<string, object>(new KeyValuePair<string, object>[]
	{
		new KeyValuePair<string, object>("key", "B002"),
		new KeyValuePair<string, object>("id", "B002"),
		new KeyValuePair<string, object>("firstName", "Minami"),
		new KeyValuePair<string, object>("lastName", "Kotori")
	});

	// 5 - update (memorize key from action 1.2)
	private readonly Dictionary<string, object> tc5 = new Dictionary<string, object>(new KeyValuePair<string, object>[]
	{
		new KeyValuePair<string, object>("id", "B001"),
		new KeyValuePair<string, object>("firstName", "Kunikida"),
		new KeyValuePair<string, object>("lastName", "Hanamaru")
	});

	// 6 - delete
	private readonly string tc6_key = "A002";

	// expectations (base contents on each actions)

	// 1.1 - put (tc1_1, should insert two records with id "A001" and "A002")
	private readonly Dictionary<string, object>[] exp1_1 = new Dictionary<string, object>[]
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
		})
	};

	// 1.2 - put without key (tc1_2, should insert record with id "B001")
	private readonly Dictionary<string, object> exp1_2 = new Dictionary<string, object>(new KeyValuePair<string, object>[]
	{
		new KeyValuePair<string, object>("id", "B001"),
		new KeyValuePair<string, object>("firstName", "Watanabe"),
		new KeyValuePair<string, object>("lastName", "You")
	});

	// 2 - get (should return this record)
	private readonly Dictionary<string, object> exp2 = new Dictionary<string, object>(new KeyValuePair<string, object>[]
	{
		new KeyValuePair<string, object>("key", "A002"),
		new KeyValuePair<string, object>("id", "A002"),
		new KeyValuePair<string, object>("firstName", "Heanna"),
		new KeyValuePair<string, object>("lastName", "Sumire")
	});

	// 3.1 - query all items (should return all items from action 1.1 and 1.2)
	private readonly Dictionary<string, object>[] exp3_1 = new Dictionary<string, object>[]
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
			new KeyValuePair<string, object>("id", "B001"),
			new KeyValuePair<string, object>("firstName", "Watanabe"),
			new KeyValuePair<string, object>("lastName", "You")
		})
	};

	// 3.2 - query specific items (should return all items where id starts with "B")
	private readonly Dictionary<string, object>[] exp3_2 = new Dictionary<string, object>[]
	{
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("id", "B001"),
			new KeyValuePair<string, object>("firstName", "Watanabe"),
			new KeyValuePair<string, object>("lastName", "You")
		})
	};

	// 4 - insert (should insert record with key "B002")
	private readonly Dictionary<string, object>[] exp4 = new Dictionary<string, object>[]
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
			new KeyValuePair<string, object>("id", "B001"),
			new KeyValuePair<string, object>("firstName", "Watanabe"),
			new KeyValuePair<string, object>("lastName", "You")
		}),
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "B002"),
			new KeyValuePair<string, object>("id", "B002"),
			new KeyValuePair<string, object>("firstName", "Minami"),
			new KeyValuePair<string, object>("lastName", "Kotori")
		})
	};

	// 5 - update (should update record with key "B001", updating "firstName" and "lastName" column)
	private readonly Dictionary<string, object>[] exp5 = new Dictionary<string, object>[]
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
			new KeyValuePair<string, object>("id", "B001"),
			new KeyValuePair<string, object>("firstName", "Kunikida"),
			new KeyValuePair<string, object>("lastName", "Hanamaru")
		}),
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "B002"),
			new KeyValuePair<string, object>("id", "B002"),
			new KeyValuePair<string, object>("firstName", "Minami"),
			new KeyValuePair<string, object>("lastName", "Kotori")
		})
	};

	// 6 - delete (should delete record with key "A002")
	private readonly Dictionary<string, object>[] exp6 = new Dictionary<string, object>[]
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
			new KeyValuePair<string, object>("id", "B001"),
			new KeyValuePair<string, object>("firstName", "Kunikida"),
			new KeyValuePair<string, object>("lastName", "Hanamaru")
		}),
		new Dictionary<string, object>(new KeyValuePair<string, object>[]
		{
			new KeyValuePair<string, object>("key", "B002"),
			new KeyValuePair<string, object>("id", "B002"),
			new KeyValuePair<string, object>("firstName", "Minami"),
			new KeyValuePair<string, object>("lastName", "Kotori")
		})
	};

	// test actions:
	// put -> get -> query -> insert* -> update* -> delete*
	// * = query and compare after action

	public BaseTests(TestBaseFixture context)
		=> this.context = context;

	[Fact]
	[Priority(1)]
	public async void PutItemTestAsync()
	{
		BasePutResponse temp = await context.db.PutAsync(tc1_1);

		// A001
		Assert.Equal(exp1_1[0]["key"].ToString(), temp.Processed.Items[0]["key"].ToString());
		Assert.Equal(exp1_1[0]["id"].ToString(), temp.Processed.Items[0]["id"].ToString());
		Assert.Equal(exp1_1[0]["firstName"].ToString(), temp.Processed.Items[0]["firstName"].ToString());
		Assert.Equal(exp1_1[0]["lastName"].ToString(), temp.Processed.Items[0]["lastName"].ToString());

		// A002
		Assert.Equal(exp1_1[1]["key"].ToString(), temp.Processed.Items[1]["key"].ToString());
		Assert.Equal(exp1_1[1]["id"].ToString(), temp.Processed.Items[1]["id"].ToString());
		Assert.Equal(exp1_1[1]["firstName"].ToString(), temp.Processed.Items[1]["firstName"].ToString());
		Assert.Equal(exp1_1[1]["lastName"].ToString(), temp.Processed.Items[1]["lastName"].ToString());
	}

	[Fact]
	[Priority(2)]
	public async void PutItemWithoutKeyTestAsync()
	{
		BasePutResponse temp = await context.db.PutAsync(new Dictionary<string, object>[] { tc1_2 });

		// B001 (no key)
		Assert.Equal(exp1_2["id"].ToString(), temp.Processed.Items[0]["id"].ToString());
		Assert.Equal(exp1_2["firstName"].ToString(), temp.Processed.Items[0]["firstName"].ToString());
		Assert.Equal(exp1_2["lastName"].ToString(), temp.Processed.Items[0]["lastName"].ToString());

		// memorize key for update test
		context.tempKey = temp.Processed.Items[0]["key"].ToString()
			?? throw new InvalidOperationException("Inserted item key should not be null.");
	}

	[Fact]
	[Priority(3)]
	public async void GetItemTestAsync()
	{
		Dictionary<string, object> temp = await context.db.GetAsync(tc2)
			?? throw new InvalidOperationException($"Item with key '{tc2}' not found in Base.");

		Assert.Equal(exp2["key"].ToString(), temp["key"].ToString());
		Assert.Equal(exp2["id"].ToString(), temp["id"].ToString());
		Assert.Equal(exp2["firstName"].ToString(), temp["firstName"].ToString());
		Assert.Equal(exp2["lastName"].ToString(), temp["lastName"].ToString());
	}

	[Fact]
	[Priority(4)]
	public async void QueryAllItemsTestAsync()
	{
		BaseQueryResponse tempAllItems = await context.db.QueryAsync();

		if (tempAllItems.Items.Length != exp3_1.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp3_1 expectation.");
		}

		AssertAllItems(exp3_1, tempAllItems.Items);
	}

	[Fact]
	[Priority(5)]
	public async void QuerySpecificItemsTestAsync()
	{
		BaseQueryResponse tempAllItems = await context.db.QueryAsync(tc3_2);

		if (tempAllItems.Items.Length != exp3_2.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp3_2 expectation.");
		}

		AssertAllItems(exp3_2, tempAllItems.Items);
	}

	[Fact]
	[Priority(6)]
	public async void InsertItemTestAsync()
	{
		string insertedKey = await context.db.InsertAsync(tc4);
		Assert.Equal(tc4["key"].ToString(), insertedKey);

		// assert all items

		BaseQueryResponse tempAllItems = await context.db.QueryAsync();

		if (tempAllItems.Items.Length != exp4.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp4 expectation.");
		}

		AssertAllItems(exp4, tempAllItems.Items);
	}

	[Fact]
	[Priority(7)]
	public async void UpdateItemTestAsync()
	{
		if (string.IsNullOrWhiteSpace(context.tempKey))
		{
			throw new InvalidOperationException("Temporary key not found. This test requires test case 1.2 to be run and stored.");
		}

		BaseUpdateItem payload = new BaseUpdateItem()
		{
			Set = tc5 // get key from test case 1.2
		};

		BaseUpdateResponse temp = await context.db.UpdateAsync(context.tempKey, payload);

		Assert.Equal(context.tempKey, temp.Key);
		Assert.Equal(payload.Set["id"].ToString(), temp.Set?["id"].ToString());
		Assert.Equal(payload.Set["firstName"].ToString(), temp.Set?["firstName"].ToString());
		Assert.Equal(payload.Set["lastName"].ToString(), temp.Set?["lastName"].ToString());

		// assert all items

		BaseQueryResponse tempAllItems = await context.db.QueryAsync();

		if (tempAllItems.Items.Length != exp5.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp5 expectation.");
		}

		AssertAllItems(exp5, tempAllItems.Items);
	}

	[Fact]
	[Priority(8)]
	public async void DeleteItemTestAsync()
	{
		// BaseQueryResponse before = await context.db.QueryAsync();
		await context.db.DeleteAsync(tc6_key);
		BaseQueryResponse after = await context.db.QueryAsync();

		if (after.Items.Length != exp6.Length)
		{
			throw new InvalidOperationException("Invalid data count. All data must be the same as exp6 expectation.");
		}

		AssertAllItems(exp6, after.Items);
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
			if (sortActual[i]["id"].ToString() == tc1_2["id"].ToString())
			{
				// if current item is inserted item without key (tc1_2), compare with memorized key
				if (!string.IsNullOrWhiteSpace(context.tempKey))
				{
					Assert.Equal(context.tempKey, sortActual[i]["key"].ToString());
				}
			}
			else
			{
				// else compare key directly
				Assert.Equal(sortExpected[i]["key"].ToString(), sortActual[i]["key"].ToString());
			}

			Assert.Equal(sortExpected[i]["id"].ToString(), sortActual[i]["id"].ToString());
			Assert.Equal(sortExpected[i]["firstName"].ToString(), sortActual[i]["firstName"].ToString());
			Assert.Equal(sortExpected[i]["lastName"].ToString(), sortActual[i]["lastName"].ToString());
		}
	}
}
