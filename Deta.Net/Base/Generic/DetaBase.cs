// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Net;
using System.Text;
using System.Text.Json;
using Deta.Net.Base.Generic;

namespace Deta.Net.Base;

public partial class DetaBase
{
	public async Task<BasePutResponse<T>> PutAsync<T>(T[] data) where T : PutItem
	{
		string payload = JsonSerializer.Serialize(new PutItems<T>(data));

		using StringContent requestBody = new StringContent(payload, Encoding.UTF8, "application/json");
		using HttpResponseMessage response = await httpClient.PutAsync($"{baseEndpoint}/items", requestBody);

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode is not HttpStatusCode.OK and not HttpStatusCode.MultiStatus)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		BasePutResponse<T>? responseData = JsonSerializer.Deserialize<BasePutResponse<T>>(responseBody)
			?? throw new InvalidOperationException($"Response code is {(int)response.StatusCode} ({(response.StatusCode == HttpStatusCode.OK ? "OK" : "Multi-Status")}) but body is null.");

		return responseData;
	}

	public async Task<T?> GetAsync<T>(string key) where T : GetItem
	{
		using HttpResponseMessage response = await httpClient.GetAsync($"{baseEndpoint}/items/{key}");

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode == HttpStatusCode.NotFound)
		{
			return default;
		}
		else if (response.StatusCode != HttpStatusCode.OK)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		T? item = JsonSerializer.Deserialize<T>(responseBody);
		return item;
	}

	public async Task<string> InsertAsync<T>(T data)
	{
		string payload = JsonSerializer.Serialize(new BaseInsertItemPayload<T>(data));

		using StringContent requestBody = new StringContent(payload, Encoding.UTF8, "application/json");
		using HttpResponseMessage response = await httpClient.PostAsync($"{baseEndpoint}/items", requestBody);

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode == HttpStatusCode.Conflict)
		{
			throw new HttpRequestException("Data with specified key already exists.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode != HttpStatusCode.Created)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		BaseInsertResponse? responseData = JsonSerializer.Deserialize<BaseInsertResponse>(responseBody);

		if (responseData == null)
		{
			throw new InvalidOperationException("Response code is 201 (Created) but body is null.");
		}
		else if (string.IsNullOrWhiteSpace(responseData.Key))
		{
			throw new InvalidOperationException("Response code is 201 (Created) but no key is returned.");
		}

		return responseData.Key;
	}
}