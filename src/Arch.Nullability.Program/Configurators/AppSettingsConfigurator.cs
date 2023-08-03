using Arch.Nullability.Program.Infrastructure.Configurators;

namespace Arch.Nullability.Program.Configurators;

public class AppSettingsConfigurator : IConfigurator
{
	public void Configure(WebApplicationBuilder builder)
	{
		builder.Configuration
			.AddJsonFile("AppData/appsettings.json", optional: true, reloadOnChange: false)
			.AddJsonFile(
				$"AppData/appsettings.{builder.Environment.EnvironmentName}.json",
				optional: true,
				reloadOnChange: false
			);
	}

	public void Configure(WebApplication app)
	{
	}
}
