namespace Arch.Nullability.Program.Infrastructure.Configurators;

public interface IConfigurator
{
	int Order => 0;

	void Configure(WebApplicationBuilder builder);

	void Configure(WebApplication app);
}
