// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Net;
using System.Text;
using System.Text.Json;
using Deta.Net.Base.Generic;

namespace Deta.Net.Base;

public partial class DetaBase
{
	private const string HOST_URL = "https://database.deta.sh";

	private readonly HttpClient httpClient;
	private readonly string baseEndpoint;

	internal DetaBase(string apiKey, string baseName)
	{
		if (string.IsNullOrWhiteSpace(baseName))
		{
			throw new InvalidOperationException("baseName must be set or not empty.");
		}

		baseEndpoint = $"/v1/{apiKey.Split('_')[0]}/{baseName}";

		// Console.WriteLine(baseUrl);

		httpClient = new HttpClient()
		{
			BaseAddress = new Uri(HOST_URL),
		};
		httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
	}

	public async Task<BasePutResponse> PutAsync(Dictionary<string, object>[] data)
	{
		string payload = JsonSerializer.Serialize(new PutItems<Dictionary<string, object>>(data));

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

		BasePutResponse responseData = JsonSerializer.Deserialize<BasePutResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is {(int)response.StatusCode} ({(response.StatusCode == HttpStatusCode.OK ? "OK" : "Multi-Status")}) but body is null.");

		return responseData;
	}

	public async Task<Dictionary<string, object>?> GetAsync(string key)
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

		Dictionary<string, object>? item = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
		return item;
	}

	public async Task DeleteAsync(string key)
	{
		using HttpResponseMessage response = await httpClient.DeleteAsync($"{baseEndpoint}/items/{key}");

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode != HttpStatusCode.OK)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}
	}

	public async Task<string> InsertAsync(Dictionary<string, object> data)
	{
		string payload = JsonSerializer.Serialize(new BaseInsertItemPayload<Dictionary<string, object>>(data));

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

		Dictionary<string, object>? responseData = JsonSerializer.Deserialize<Dictionary<string, object>>(responseBody);
		object? retObject;

		if (responseData == null)
		{
			throw new InvalidOperationException("Response code is 201 (Created) but body is null.");
		}
		else if (!responseData.TryGetValue("key", out retObject))
		{
			throw new InvalidOperationException("Response code is 201 (Created) but no key is returned.");
		}

		string ret = retObject?.ToString()
			?? throw new InvalidOperationException("Response 'key' value is null.");

		return ret;
	}

	public async Task<BaseUpdateResponse> UpdateAsync(string key, BaseUpdateItem updateParameters)
	{
		string payload = JsonSerializer.Serialize(updateParameters);

		using StringContent requestBody = new StringContent(payload, Encoding.UTF8, "application/json");
		using HttpResponseMessage response = await httpClient.PatchAsync($"{baseEndpoint}/items/{key}", requestBody);

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode == HttpStatusCode.NotFound)
		{
			throw new HttpRequestException("Data with specified key not found.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode == HttpStatusCode.Conflict)
		{
			throw new HttpRequestException("Data with specified key already exists.",
				null,
				response.StatusCode);
		}
		// TODO: handle possible 400 (bad request) issues
		else if (response.StatusCode != HttpStatusCode.OK)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		BaseUpdateResponse? responseData = JsonSerializer.Deserialize<BaseUpdateResponse>(responseBody)
			?? throw new InvalidOperationException("Response code is 200 (OK) but body is null.");

		return responseData;
	}

	public async Task<BaseQueryResponse> QueryAsync(BaseQueryPayload? queryPayload = null)
	{
		string payload = queryPayload != null
			? JsonSerializer.Serialize(queryPayload)
			: string.Empty;

		using StringContent requestBody = new StringContent(payload, Encoding.UTF8, "application/json");
		using HttpResponseMessage response = await httpClient.PostAsync($"{baseEndpoint}/query", requestBody);

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		// TODO: handle possible 400 (bad request) issues
		else if (response.StatusCode != HttpStatusCode.OK)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		BaseQueryResponse responseData = JsonSerializer.Deserialize<BaseQueryResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is {(int)response.StatusCode} ({(response.StatusCode == HttpStatusCode.OK ? "OK" : "Multi-Status")}) but body is null.");

		return responseData;
	}
}
