using Microsoft.Extensions.Hosting;

namespace Arch.Nullability.Core.ApplicationLayers;

public interface ILayer
{
	void Configure(IHostBuilder builder);
	void Configure(IHost app);
}
