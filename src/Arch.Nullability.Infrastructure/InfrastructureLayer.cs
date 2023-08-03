using Arch.Nullability.Core.ApplicationLayers;
using Microsoft.Extensions.Hosting;

namespace Arch.Nullability.Infrastructure;

public class InfrastructureLayer : ILayer
{
	public void Configure(IHostBuilder builder)
	{
	}

	public void Configure(IHost app)
	{
	}
}
