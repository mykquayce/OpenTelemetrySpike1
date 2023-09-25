using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);

var otel = builder.Services.AddOpenTelemetry();

// Configure OpenTelemetry Resources with the application name
otel.ConfigureResource(resource => resource
	.AddService(serviceName: builder.Environment.ApplicationName));

// Add Metrics for ASP.NET Core and our custom metrics and export to Prometheus
otel.WithMetrics(metrics => metrics
	// Metrics provider from OpenTelemetry
	.AddAspNetCoreInstrumentation()
	.AddHttpClientInstrumentation()
	// Metrics provides by ASP.NET Core in .NET 8
	.AddMeter("Microsoft.AspNetCore.Hosting")
	.AddMeter("Microsoft.AspNetCore.Server.Kestrel")
	.AddPrometheusExporter());

builder.Services.AddHttpClient<OpenTelemetrySpike1.WebApplication1.Services.IHttpBinClient, OpenTelemetrySpike1.WebApplication1.Services.Concrete.HttpBinClient>(client =>
{
	client.BaseAddress = new Uri("https://httpbingo.org/");
});


var app = builder.Build();

app.MapGet("/", SendGreeting);

// Configure the Prometheus scraping endpoint
app.MapPrometheusScrapingEndpoint();

app.Run();

static async Task<string> SendGreeting(ILogger<Program> logger, OpenTelemetrySpike1.WebApplication1.Services.IHttpBinClient httpBinClient, CancellationToken cancellationToken)
{
	bool success = await httpBinClient.UnstableAsync(.5, cancellationToken);

	return "Hello World! " + success;
}
