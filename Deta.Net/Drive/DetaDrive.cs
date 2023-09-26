// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Drive;

public class DetaDrive
{
	private string driveName { get; init; }
	private string? host { get; init; }

	internal DetaDrive(string driveName, string? host)
	{
		if (string.IsNullOrWhiteSpace(driveName))
		{
			throw new InvalidOperationException("driveName must be set or not empty.");
		}

		this.driveName = driveName;
		this.host = host;
	}

	public async Task<string> PutAsync(object data, string? key, PutOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<byte[]?> GetAsync(string name)
	{
		throw new NotImplementedException();
	}

	public async Task<string> DeleteAsync(string key)
	{
		throw new NotImplementedException();
	}

	public async Task<DeleteManyResponse> DeleteManyAsync(string[] names)
	{
		throw new NotImplementedException();
	}

	public async Task<ListResponse> ListAsync(ListOptions? options)
	{
		throw new NotImplementedException();
	}
}
