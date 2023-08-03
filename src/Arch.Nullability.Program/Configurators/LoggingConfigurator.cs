using Arch.Nullability.Program.Infrastructure.Configurators;
using Serilog;

namespace Arch.Nullability.Program.Configurators;

public class LoggingConfigurator : IConfigurator
{
    public void Configure(WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();

        Log.Logger = new LoggerConfiguration()
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        builder.Services.AddLogging(loggingBuilder =>
        {
            loggingBuilder.AddSerilog(dispose: true);
        });
    }

    public void Configure(WebApplication app)
    {
    }
}