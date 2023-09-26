// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Base;

public class DetaBase
{
	private string baseName { get; init; }
	private string? host { get; init; }

	internal DetaBase(string baseName, string? host)
	{
		if (string.IsNullOrWhiteSpace(baseName))
		{
			throw new InvalidOperationException("baseName must be set or not empty.");
		}

		this.baseName = baseName;
		this.host = host;
	}

	public async Task<object?> PutAsync(object data, string key, PutOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<T?> PutAsync<T>(T data, string? key, PutOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<object?> GetAsync(string key)
	{
		throw new NotImplementedException();
	}

	public async Task<T?> GetAsync<T>(string key)
	{
		throw new NotImplementedException();
	}

	public async Task DeleteAsync(string key)
	{
		throw new NotImplementedException();
	}

	public async Task<object> InsertAsync(object data, string? key, PutOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<T> InsertAsync<T>(T data, string? key, PutOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<PutManyResponse> PutManyAsync(object[] data, PutManyOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<PutManyResponse<T>> PutManyAsync<T>(T[] data, PutManyOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task UpdateAsync(object updates, string key, UpdateOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<FetchResponse> FetchAsync(object? query, FetchOptions? options)
	{
		throw new NotImplementedException();
	}

	public async Task<FetchResponse<T>> FetchAsync<T>(object? query, FetchOptions? options)
	{
		throw new NotImplementedException();
	}
}
