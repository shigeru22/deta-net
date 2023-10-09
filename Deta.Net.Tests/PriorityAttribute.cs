// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Tests;

// reference: https://github.com/xunit/samples.xunit/tree/main/TestOrderExamples/TestCaseOrdering

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class PriorityAttribute : Attribute
{
	public int Priority { get; private set; }

	public PriorityAttribute(int priority)
	{
		Priority = priority;
	}
}
