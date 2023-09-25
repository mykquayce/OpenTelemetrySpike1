namespace OpenTelemetrySpike1.WebApplication1.Services.Concrete;

public class HttpBinClient : IHttpBinClient
{
	private readonly HttpClient _httpClient;

	public HttpBinClient(HttpClient httpClient)
	{
		_httpClient = httpClient;
	}

	public async Task<bool> UnstableAsync(double failureRate = 1, CancellationToken cancellationToken = default)
	{
		var response = await _httpClient.GetAsync($"unstable?failure_rate={failureRate:F2}", cancellationToken);
		return response.IsSuccessStatusCode;
	}
}
