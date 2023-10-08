// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Base;

public class DetaBase
{
	private const string DETA_BASE_URL = ":protocol://:host/:version/:project_id/:base_name";

	private string baseUrl { get; init; }
	private string apiKey { get; init; }

	internal DetaBase(string apiKey, string baseName)
	{
		if (string.IsNullOrWhiteSpace(baseName))
		{
			throw new InvalidOperationException("baseName must be set or not empty.");
		}

		// hardcode to https://database.deta.sh/v1 for now
		baseUrl = DETA_BASE_URL.Replace(":protocol", "https")
			.Replace(":host", "database.deta.sh")
			.Replace(":version", "v1")
			.Replace(":project_id", apiKey.Split('_')[0])
			.Replace(":base_name", baseName);
		this.apiKey = apiKey;

		// Console.WriteLine(baseUrl);
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
