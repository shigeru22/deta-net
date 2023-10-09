// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;
using Deta.Net.Base.Generic;

namespace Deta.Net.Tests;

internal sealed class GetData : GetItem
{
	[JsonPropertyName("id")]
	[JsonPropertyOrder(0)]
	public string? Id { get; set; }

	[JsonPropertyName("firstName")]
	[JsonPropertyOrder(1)]
	public string? FirstName { get; set; }

	[JsonPropertyName("lastName")]
	[JsonPropertyOrder(2)]
	public string? LastName { get; set; }
}
