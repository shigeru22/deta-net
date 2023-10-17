// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using Xunit.Abstractions;
using Xunit.Sdk;

namespace Deta.Net.Tests;

// reference: https://github.com/xunit/samples.xunit/tree/main/TestOrderExamples/TestCaseOrdering

public class TestPriorityAttribute : ITestCaseOrderer
{
	private static TValue GetOrCreate<TKey, TValue>(IDictionary<TKey, TValue> dictionary, TKey key) where TValue : new()
	{

		if (dictionary.TryGetValue(key, out TValue? result) && result != null)
		{
			return result;
		}

		result = new TValue();
		dictionary[key] = result;

		return result;
	}

	public IEnumerable<TTestCase> OrderTestCases<TTestCase>(IEnumerable<TTestCase> testCases) where TTestCase : ITestCase
	{
		SortedDictionary<int, List<TTestCase>> sortedMethods = new SortedDictionary<int, List<TTestCase>>();

		foreach (TTestCase testCase in testCases)
		{
			int priority = 0;

			foreach (IAttributeInfo attr in testCase.TestMethod.Method.GetCustomAttributes(typeof(PriorityAttribute).AssemblyQualifiedName))
			{
				priority = attr.GetNamedArgument<int>("Priority");
			}

			GetOrCreate(sortedMethods, priority).Add(testCase);
		}

		foreach (List<TTestCase>? list in sortedMethods.Keys.Select(priority => sortedMethods[priority]))
		{
			list.Sort((x, y) => StringComparer.OrdinalIgnoreCase.Compare(x.TestMethod.Method.Name, y.TestMethod.Method.Name));
			foreach (TTestCase testCase in list)
			{
				yield return testCase;
			}
		}
	}
}
