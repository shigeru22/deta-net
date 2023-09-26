// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Base;

public struct PutOptions
{
	public long? ExpireIn { get; set; }
	public long? ExpireAt { get; set; }
}
