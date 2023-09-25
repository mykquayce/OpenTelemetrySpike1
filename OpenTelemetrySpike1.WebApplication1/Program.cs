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
	// Metrics provides by ASP.NET Core in .NET 8
	.AddMeter("Microsoft.AspNetCore.Hosting")
	.AddMeter("Microsoft.AspNetCore.Server.Kestrel")
	.AddPrometheusExporter());

var app = builder.Build();

app.MapGet("/", SendGreeting);

// Configure the Prometheus scraping endpoint
app.MapPrometheusScrapingEndpoint();

app.Run();

string SendGreeting(ILogger<Program> logger)
{
	// Log a message
	logger.LogInformation("Sending greeting");

	return "Hello World!";
}
