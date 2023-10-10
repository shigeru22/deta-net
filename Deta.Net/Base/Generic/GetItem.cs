// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Text.Json.Serialization;

namespace Deta.Net.Base.Generic;

public abstract class GetItem
{
	[JsonPropertyName("key")] public string Key { get; set; }

	public GetItem(string key = "") => Key = key; // defaults to empty string, specify it upon instantiating
}