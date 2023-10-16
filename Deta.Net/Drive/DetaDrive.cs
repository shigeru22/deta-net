// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

using System.Net;
using System.Text;
using System.Text.Json;

namespace Deta.Net.Drive;

public class DetaDrive
{
	private const string HOST_URL = "https://drive.deta.sh";
	private const int CHUNK_SIZE = 10485760; // 10MB

	private readonly HttpClient httpClient;
	private readonly string driveEndpoint;

	internal DetaDrive(string apiKey, string driveName)
	{
		if (string.IsNullOrWhiteSpace(driveName))
		{
			throw new InvalidOperationException("driveName must be set or not empty.");
		}

		driveEndpoint = $"/v1/{apiKey.Split('_')[0]}/{driveName}";

		// Console.WriteLine(baseUrl);

		httpClient = new HttpClient()
		{
			BaseAddress = new Uri(HOST_URL),
		};
		httpClient.DefaultRequestHeaders.Add("X-API-Key", apiKey);
	}

	public async Task<PutResponse> PutAsync(string name, string filePath, string? contentType = null)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("File with specified filePath not found.");
		}

		if (new FileInfo(filePath).Length > CHUNK_SIZE)
		{
			throw new InvalidOperationException("PutAsync(string, string, string?) only supports files under 10MB. Use UploadAsync(string, string) instead.");
		}

		string targetPath = name.StartsWith("/") ? name : $"/{name}";
		byte[] fileContents = await File.ReadAllBytesAsync(filePath);

		using ByteArrayContent requestBody = new ByteArrayContent(fileContents);
		_ = requestBody.Headers.Remove("Content-Type"); // TODO: test whether Content-Type should always be removed since type could be figured out by name
		if (contentType != null)
		{
			requestBody.Headers.Add("Content-Type", contentType);
		}

		using HttpResponseMessage response = await httpClient.PostAsync($"{driveEndpoint}/files?name={targetPath}", requestBody);
		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode is not HttpStatusCode.Created)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		PutResponse responseData = JsonSerializer.Deserialize<PutResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is 201 (Created) but body is null.");

		return responseData;
	}

	public async Task<PutResponse> PutAsync(string name, byte[] data, string? contentType = null)
	{
		if (data.Length > CHUNK_SIZE)
		{
			throw new InvalidOperationException("PutAsync(string, byte[], string?) only supports files under 10MB. Consider saving the byte array as file and use UploadAsync(string, string) instead.");
		}

		string targetPath = name.StartsWith("/") ? name : $"/{name}";
		using ByteArrayContent requestBody = new ByteArrayContent(data);
		_ = requestBody.Headers.Remove("Content-Type");
		if (contentType != null)
		{
			requestBody.Headers.Add("Content-Type", contentType);
		}

		using HttpResponseMessage response = await httpClient.PostAsync($"{driveEndpoint}/files?name={targetPath}", requestBody);
		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode is not HttpStatusCode.Created)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		PutResponse responseData = JsonSerializer.Deserialize<PutResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is 201 (Created) but body is null.");

		return responseData;
	}

	public async Task<EndUploadResponse> UploadAsync(string name, string filePath)
	{
		if (!File.Exists(filePath))
		{
			throw new FileNotFoundException("File with specified filePath not found.");
		}

		string targetPath = name.StartsWith("/") ? name : $"/{name}";
		byte[] fileContents = await File.ReadAllBytesAsync(filePath);

		StartUploadResponse startResponse = await StartUploadAsync(targetPath);

		int part = 1;
		foreach (byte[] chunk in fileContents.GetChunksEnumerator(CHUNK_SIZE))
		{
			_ = await UploadPartAsync(startResponse.UploadId,
				startResponse.Name,
				part,
				chunk);
		}

		EndUploadResponse endResponse = await EndUploadAsync(startResponse.UploadId, startResponse.Name);
		return endResponse;
	}

	public async Task<StartUploadResponse> StartUploadAsync(string path)
	{
		using HttpResponseMessage response = await httpClient.PostAsync($"{driveEndpoint}/uploads?name={path}", null);

		string responseBody = await response.Content.ReadAsStringAsync();

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode != HttpStatusCode.Accepted)
		{
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		StartUploadResponse responseData = JsonSerializer.Deserialize<StartUploadResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is 202 (Accepted) but body is null.");

		return responseData;
	}

	public async Task<UploadResponse> UploadPartAsync(string uploadId, string path, int part, byte[] chunk)
	{
		using ByteArrayContent requestBody = new ByteArrayContent(chunk);
		using HttpResponseMessage response = await httpClient.PostAsync($"{driveEndpoint}/uploads/{uploadId}/parts?name={path}&part={part}", requestBody);

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

		UploadResponse? responseData = JsonSerializer.Deserialize<UploadResponse>(responseBody)
			?? throw new InvalidOperationException("Response code is 200 (OK) but body is null.");

		return responseData;
	}

	public async Task<EndUploadResponse> EndUploadAsync(string uploadId, string path)
	{
		using HttpResponseMessage response = await httpClient.PatchAsync($"{driveEndpoint}/uploads/{uploadId}?name={path}", null);

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

		EndUploadResponse? responseData = JsonSerializer.Deserialize<EndUploadResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is 200 (OK) but body is null.");

		return responseData;
	}

	public async Task<GetResponse> GetAsync(string name)
	{
		using HttpResponseMessage response = await httpClient.GetAsync($"{driveEndpoint}/files/download?name={name}");

		if (response.StatusCode == HttpStatusCode.Unauthorized)
		{
			throw new HttpRequestException("Unauthorized key. Check API key used and try again.",
				null,
				response.StatusCode);
		}
		else if (response.StatusCode != HttpStatusCode.OK)
		{
			string responseBody = await response.Content.ReadAsStringAsync();
			throw new HttpRequestException($"API returned status code {(int)response.StatusCode} ({response.StatusCode}){(!string.IsNullOrWhiteSpace(responseBody) ? $"\n{responseBody}" : string.Empty)}",
				null,
				response.StatusCode);
		}

		int length = int.Parse(response.Content.Headers.GetValues("Content-Length").FirstOrDefault("0"));
		byte[] content = await response.Content.ReadAsByteArrayAsync();

		GetResponse ret = new GetResponse(name, length, content);
		return ret;
	}

	public async Task<DeleteResponse> DeleteAsync(string name)
	{
		string payload = JsonSerializer.Serialize(new DeletePayload(name));

		using StringContent requestBody = new StringContent(payload, Encoding.UTF8, "application/json");
		using var response = await httpClient.SendAsync(new HttpRequestMessage()
		{
			Method = HttpMethod.Delete,
			RequestUri = new Uri($"{HOST_URL}{driveEndpoint}/files"),
			Content = requestBody
		}); // y u no support DeleteAsync with body?

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

		DeleteResponse responseData = JsonSerializer.Deserialize<DeleteResponse>(responseBody)
			?? throw new InvalidOperationException("Response code is 200 (OK) but body is null.");

		return responseData;
	}

	public async Task<DeleteResponse> DeleteAsync(string[] names)
	{
		string payload = JsonSerializer.Serialize(new DeletePayload(names));

		using StringContent requestBody = new StringContent(payload, Encoding.UTF8, "application/json");
		using var response = await httpClient.SendAsync(new HttpRequestMessage()
		{
			Method = HttpMethod.Delete,
			RequestUri = new Uri($"{HOST_URL}{driveEndpoint}/files"),
			Content = requestBody
		}); // y u no support DeleteAsync with body?

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

		DeleteResponse responseData = JsonSerializer.Deserialize<DeleteResponse>(responseBody)
			?? throw new InvalidOperationException("Response code is 200 (OK) but body is null.");

		return responseData;
	}

	public async Task<DeleteResponse> DeleteAsync(DeletePayload payload)
	{
		string tempPayload = JsonSerializer.Serialize(payload);

		using StringContent requestBody = new StringContent(tempPayload, Encoding.UTF8, "application/json");
		using var response = await httpClient.SendAsync(new HttpRequestMessage()
		{
			Method = HttpMethod.Delete,
			RequestUri = new Uri($"{HOST_URL}{driveEndpoint}/files"),
			Content = requestBody
		}); // y u no support DeleteAsync with body?

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

		DeleteResponse responseData = JsonSerializer.Deserialize<DeleteResponse>(responseBody)
			?? throw new InvalidOperationException("Response code is 200 (OK) but body is null.");

		return responseData;
	}

	public async Task<ListResponse> ListAsync(ListOptions? options)
	{
		List<string> queryParameters = new List<string>();
		if (options.HasValue)
		{
			if (options.Value.Limit.HasValue)
			{
				queryParameters.Add($"limit={options.Value.Limit.Value}");
			}

			if (!string.IsNullOrWhiteSpace(options.Value.Prefix))
			{
				queryParameters.Add($"prefix={options.Value.Prefix}");
			}

			if (!string.IsNullOrWhiteSpace(options.Value.Last))
			{
				queryParameters.Add($"last={options.Value.Last}");
			}
		}

		using HttpResponseMessage response = await httpClient.GetAsync($"{driveEndpoint}/files{(queryParameters.Count > 0 ? $"?{string.Join('&', queryParameters)}" : string.Empty)}");

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

		ListResponse responseData = JsonSerializer.Deserialize<ListResponse>(responseBody)
			?? throw new InvalidOperationException($"Response code is 200 (OK) but body is null.");

		return responseData;
	}
}
