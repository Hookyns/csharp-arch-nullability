using Arch.Nullability.Program.Infrastructure.Configurators;
using Wolverine;

namespace Arch.Nullability.Program.Configurators;

public class BusConfigurator : IConfigurator
{
	public int Order => int.MinValue;

	public void Configure(WebApplicationBuilder builder)
	{
		builder.Host.UseWolverine(
			options => { options.Discovery.IncludeAssembly(typeof(Application.Application).Assembly); });
	}

	public void Configure(WebApplication app)
	{
	}
}
