// Copyright (c) shigeru22. Licensed under the MIT license.
// See LICENSE in the repository root for details.

namespace Deta.Net.Drive;

public class DetaDrive
{
	private const string DETA_DRIVE_URL = ":protocol://:host/:version/:project_id/:drive_name";

	private string baseUrl;
	private string apiKey;

	internal DetaDrive(string apiKey, string driveName)
	{
		if (string.IsNullOrWhiteSpace(driveName))
		{
			throw new InvalidOperationException("driveName must be set or not empty.");
		}

		// hardcode to https://drive.deta.sh/v1 for now
		baseUrl = DETA_DRIVE_URL.Replace(":protocol", "https")
			.Replace(":host", "drive.deta.sh")
			.Replace(":version", "v1")
			.Replace(":project_id", apiKey.Split('_')[0])
			.Replace(":drive_name", driveName);
		this.apiKey = apiKey;

		// Console.WriteLine(baseUrl);
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
