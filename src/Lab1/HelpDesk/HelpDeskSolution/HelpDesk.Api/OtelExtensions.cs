using OpenTelemetry.Logs;
using OpenTelemetry.Metrics;
using OpenTelemetry.Trace;
using Npgsql;
using OpenTelemetry.Resources;

namespace HelpDesk.Api;

public static class OtelExtensions
{
    public static WebApplicationBuilder AddOtel(this WebApplicationBuilder builder)
    {
        builder.Logging.AddOpenTelemetry(logging =>
        {
           logging.IncludeFormattedMessage = true; // logger.LogInformation("Got a request for {request}", request);
        });
        builder.Services.AddMetrics()
            .AddOpenTelemetry()
            .WithMetrics(provider =>
            {
                provider
                    .AddAspNetCoreInstrumentation()
                    .AddHttpClientInstrumentation()
                   
                    .AddRuntimeInstrumentation();
                provider.ConfigureResource(r =>
                {
                    r.AddService("HelpDesk.Api", "helpdesk.api.employees.problems_created");
                    
                }).AddMeter("HelpDesk.Api");
            }).WithTracing(options =>
            {
                if (builder.Environment.IsDevelopment())
                {
                    options.SetSampler<AlwaysOnSampler>();
                }

                options.AddAspNetCoreInstrumentation()
                .AddNpgsql()
                    .AddHttpClientInstrumentation();
            });
        if (!string.IsNullOrWhiteSpace(builder.Configuration["OTEL_EXPORTER_OTLP_ENDPOINT"]))
        {
            builder.Services.Configure<OpenTelemetryLoggerOptions>(options => options.AddOtlpExporter())
                .ConfigureOpenTelemetryMeterProvider(metrics => metrics.AddOtlpExporter())
                .ConfigureOpenTelemetryTracerProvider(tracing => tracing.AddOtlpExporter());
        }
        return builder;
    }
}
